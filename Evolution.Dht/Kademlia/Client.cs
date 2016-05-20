using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using NLog;
using System.Reflection;

namespace Evolution.Dht.Kademlia
{
    public class Client : Evolution.Dht.Client
    {
        internal List<PeerInfo> bootstrapPeers = new List<PeerInfo>();
        internal BucketList buckets;
        internal Dictionary<string, Storage> publications = new Dictionary<string, Storage>();
        internal Dictionary<string, Storage> storages = new Dictionary<string, Storage>();
        internal List<AncestorThread> threads = new List<AncestorThread>();
        private PeerInfo me = new PeerInfo();
        private Transport transport;
        private bool running = false;
        private MaintenanceThread mntThread;

        // Log purpose
        Logger logger = LogManager.GetLogger("Client");

        public int K
        {
            get { return Settings.BUCKETS_MAX_PEERS; }
        }

        public bool Running
        {
            get { return running; }
        }

        public int Alfa
        {
            get { return Settings.THETA; }
        }

        public List<PeerInfo> BootstrapPeers
        {
            get { return bootstrapPeers; }
        }

        public PeerInfo Me
        {
            get { return me; }
            set { me = value; }
        }

        public Transport Transport
        {
            get { return transport; }
        }

        public Client()
        {
            logger.Info("Client Class initialized");
            buckets = new BucketList(this);
            transport = CreateTransport();
        }

        public string GetVersion()
        {
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // Non sono sicuro di cosa useremo per versionare, tengo il codice qui per utilita' futura
            // TODO: forse sarebbe meglio istaniare la versione come var privata all'avvio?
            //string assemblyVersion = Assembly.LoadFile('your assembly file').GetName().Version.ToString();
            //string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            //string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            return assemblyVersion;
        }

        public override void Start()
        {
            me.Id.Create();
            transport.Message += new Transport.MessageHandler(transport_Message);
            if (me.EndPoint.Port == 0)
            {
                me.EndPoint.Port = 4401;
            }
            if (me.EndPoint.Address == IPAddress.Any)
            {
                me.EndPoint.Address = IPAddress.Loopback;
            }

            try
            {
                logger.Info("In ascolto sulla porta {0}", me.EndPoint.Port.ToString());
                transport.Listen(me.EndPoint.Port);
            }
            catch (Exception e)
            {
                logger.Fatal("E' stata lanciata l'eccezione: {0}", e.Message);
            }
            //Serialize
            buckets.Load();

            UpdatePeer(Me);

            running = true;
            mntThread = new MaintenanceThread(this);
            mntThread.Start();
        }


        public override void Abort()
        {

            // TODO: Forse sarebbe meglio salvare periodicamente la lista in caso di crash
            //Saving nodes in kad.xml
            buckets.Store();

            if (running)
            {
                running = false;
                // Questa riga bloccava la chiusura. 
                // TODO: Indagare sul motivo. Era necessario il join? 
                //thread.Join();
            }

            //TODO: Attenzione! Se non sono tutti FindNodeThread crasha!!
            foreach (FindNodeThread thread in threads)
            {
                thread.Abort();
            }

            transport.Stop();
        }

        public override void Join()
        {
            //Serialize
        }

        public override void BeginStore(string key, string val)
        {
            lock (threads)
            {
                key = PeerId.CalculateId(key).ToString();
                Storage storage = new Storage();
                storage.Key = key;
                storage.Val = val;
                storage.Expiration = DateTime.Now + new TimeSpan(0, 20, 0);
                publications[key] = storage;
                StoreThread thread = new StoreThread(this, key, val);
                thread.Finish += new AncestorThread.FinishHandler(thread_Finish);
                threads.Add(thread);
                thread.Start();
            }
        }

        void thread_Finish(AncestorThread thread)
        {
            lock (threads)
            {
                threads.Remove(thread);
            }
        }

        public override void BeginLookup(string key)
        {
            lock (threads)
            {
                FindValueThread thread = new FindValueThread(this, PeerId.CalculateId(key).ToString());
                thread.Finish += new AncestorThread.FinishHandler(thread_Finish);
                threads.Add(thread);
                thread.Start();
            }
        }

        public void FireLookupResult(string key, string val)
        {
            RaiseLookupEvent(key, val);
        }

        // Un giorno il thread transport non mandera' anche i messaggi ma sara' solo un listener ora fa entrambe le cose
        public void Send(Message message, PeerInfo peer = null)
        {
            logger.Info("SendMessage " + Enum.GetName(typeof(Opcode), message.FinalMessage[1]) + " (" + message.DestPeer.EndPoint.ToString() + ")");

            lock (buckets)
            {
                PeerInfo bucketPeer = buckets.GetPeer(peer.Id);
                if (bucketPeer != null)
                {
                    bucketPeer.LastSend = DateTime.Now;
                }
            }
            // non serve piu'?
            //  message.Peer = Me;

            transport.Send(message, peer);
            //            transport.Send(message, peer);
            //            transport.Send(message, peer);
        }

        protected virtual Transport CreateTransport()
        {
            return new Transport();
        }

        // Risulta arcano il motivo per cui questo delegato e' in questa classe invece che nella classe transport.cs
        // Oltretutto andrebbe fatto in un thread separato essendo lo smistamento dei pacchetti
        // Teoria del pigro che non vuole paasare le variabili (i.e. threads) 
        void transport_Message(Transport transport, IMessage message)
        {
            logger.Info("ReceivedMessage " + message.opcode + "(" + message.SourceEndPoint.ToString() + ")");

            // Questo:
            //UpdatePeer(message.Peer);
            // ci sta come i cavoli a merenda ovvero
            // sembra troppo pesante per essere richiamata qui...

            switch (message.opcode)
            {
                case Opcode.Ping:
                    ProcessPingRequest(message);
                    break;
                case Opcode.Pong:
                    ProcessPongResponse(message);
                    break;
                case Opcode.FindnodeRequest:
                    ProcessFindNodeRequest(message);
                    break;
                case Opcode.FindnodeResponse:
                    ProcessFindNodeResponse(message);
                    break;
                case Opcode.FindvalueRequest:
                    ProcessFindValueRequest(message);
                    break;
                case Opcode.StoreRequest:
                    ProcessStoreRequest(message);
                    break;
                case Opcode.BoostrapRequest:
                    ProcessBoostrapRequest(message);
                    break;
                case Opcode.BoostrapResponse:
                    ProcessBoostrapResponse(message);
                    break;
            }

            lock (threads)
            {
                // Oltre a processare il messaggio nello switchcase lo da' anche in pasto a tutti i thread presenti nella lista di thread (this.threads)
                // lista che viene riempita nella beginlookup() e beginStore(), dove viene creato un thread (findValueThread o findNodethread) 
                // che viene inserito in tale lista
                foreach (AncestorThread thread in threads)
                {
                    thread.ProcessMessage(message);
                }
            }
        }

        void ProcessFindNodeRequest(IMessage message)
        {
            List<PeerInfo> peers = GetCloserPeers(message.peerID, K);
            var response = new Message(message.SourceEndPoint);
            response.CreateFindNodeResponse(peers);
            Send(response, response.DestPeer);
        }

        // TODO: questa lista potrebbe contenere potenzialmente peers che sono
        //          vicini alla chiave che sto cercando. Sarebbe bene interrogarli e chiederglielo
        //          invece di pingarli solamente. (If (searchInProgress) { ... } ) magari con un mutex
        void ProcessFindNodeResponse(IMessage message)
        {
            foreach (var peer in message.peers)
            {
                UpdatePeer(peer);
                Message mex = new Message(peer);
                mex.CreatePing();
                Send(mex, peer);
            }
        }

        void ProcessFindValueRequest(IMessage message)
        {
            string key = message.key;
            Storage storage = null;
            lock (storages)
            {
                storages.TryGetValue(key, out storage);
            }
            // se conosco il valore per quella determinata chiave allora rispondo con il valore
            if (storage != null)
            {
                Message response = new Message(message.SourceEndPoint);
                response.CreateFindValueResponse(key, storage.Val);
                Send(response, response.DestPeer);
            }
            // se non ho la chiave cercata allora mando i nodi più vicini
            else
            {
                List<PeerInfo> peers = GetCloserPeers((PeerId)message.key, K);
                var response = new Message(message.SourceEndPoint);
                response.CreateFindNodeResponse(peers);
                Send(response, response.DestPeer);
            }
        }

        /// <summary>
        /// TODO: ci sono possibili attacchi:
        /// * overflow di storerequest su un nodo (possiamo evitarlo imponendo un limite superato il quale la richiesta viene ignorata)
        /// * se ho in locale la parola chiave non cerco sugli altri nodi, quindi un nodo remoto puo' sovrascrivere/accecare una determinata
        /// parola chiave
        /// </summary>
        /// <param name="message"></param>
        void ProcessStoreRequest(IMessage message)
        {
            Storage storage = new Storage();
            storage.Key = message.key;
            storage.Val = message.val;
            storage.Expiration = DateTime.Now + new TimeSpan(0, 20, 0);
            storages[message.key] = storage;
        }

        void ProcessPingRequest(IMessage message)
        {
            var mex = new Message(message.SourceEndPoint);
            mex.CreatePong();
            Send(mex, mex.DestPeer);
        }

        void ProcessPongResponse(IMessage message)
      {
            PeerInfo peer = buckets.GetPeer(message.SourceEndPoint);
            if (peer != null)
            {
                UpdatePeer(peer);
            }
        }

        void ProcessBoostrapRequest(IMessage message)
        {
            var response = new Message(message.SourceEndPoint);
            response.CreateBoostrap(me.Id, Opcode.BoostrapResponse);
            Send(response, response.DestPeer);

            PeerInfo pi = new PeerInfo();
            pi.Id = message.peerID;
            pi.EndPoint = message.SourceEndPoint;
            UpdatePeer(pi);
        }

        void ProcessBoostrapResponse(IMessage message)
        {
            PeerInfo pi = new PeerInfo();
            pi.Id = message.peerID;
            pi.EndPoint = message.SourceEndPoint;
            UpdatePeer(pi);
        }

        public List<PeerInfo> GetCloserPeers(PeerId id, int count)
        {
            List<PeerInfo> closerPeers = new List<PeerInfo>();
            lock (buckets)
            {
                foreach (PeerInfo peer in buckets)
                {
                    bool inserted = false;
                    int i;
                    for (i = 0; i < closerPeers.Count; i++)
                    {
                        PeerInfo closerPeer = closerPeers[i];
                        try
                        {
                            if (PeerId.CalculateDistance(closerPeer.Id, id) > PeerId.CalculateDistance(peer.Id, id))
                            {
                                closerPeers.Insert(i, peer);
                                inserted = true;
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            // TODO: Gestire in modo più responsabile perché ricevo un id nullo
                            logger.Error("Mannaggia! Mi è stato passato un id nullo :S");
                        }
                    }
                    if (!inserted)
                    {
                        closerPeers.Add(peer);
                    }
                }
            }
            if (count < closerPeers.Count)
            {
                closerPeers.RemoveRange(count, closerPeers.Count - count);
            }
            return closerPeers;
        }

        public PeerInfo GetPeer(PeerId peerId)
        {
            lock (buckets)
            {
                return buckets.GetPeer(peerId);
            }
        }

        public void UpdatePeer(PeerInfo newPeer)
        {
            lock (buckets)
            {
                buckets.UpdatePeer(newPeer);
            }

            //TODO: Check storage for replication
            // Controlla che il nuovo peer sia adatto alla replica delle "nostre" keys
            // (statement da verificare)
            foreach (string key in storages.Keys)
            {
                PeerId keyId = PeerId.CalculateId(key);
                if (PeerId.CalculateDistance(Me.Id, keyId) > PeerId.CalculateDistance(newPeer.Id, keyId))
                {
                    var message = new Message(newPeer);
                    message.CreateStoreRequest(key, storages[key].Val);
                    Send(message, newPeer);
                }
            }
        }

        public string Dump()
        {
            string dump = string.Empty;

            lock (buckets)
            {
                dump = buckets.Dump();
            }

            foreach (Storage storage in storages.Values)
            {
                dump += "Storage " + storage.Key + " = " + storage.Val + "\r\n";
            }
            return dump;
        }

        public void DownloadNodes(string hiveUrl = Settings.HIVE_URL_BASE, string hiveUrlNodes = Settings.HIVE_URL_NODES)
        {

            WebClient wc = new WebClient();
            //wc.Headers.Set(HttpRequestHeader.UserAgent,"EvolutionDHT v" + GetVersion() );
            wc.Headers.Set(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.32");
            wc.Proxy = null; // TODO: Let client specify proxy

            try
            {
                string list = wc.DownloadString(hiveUrl + hiveUrlNodes); // Get master list

                string[] hosts = list.Split('\n');

                foreach (string host in hosts)
                {
                    // Each line is <ip> <port>
                    string[] parts = host.Split(':');
                    if (parts.Length == 2)
                    {
                        try
                        {
                            IPEndPoint node = new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));
                            // Provo a fare bootstrap sui nodi scaricati per aggiungerli in lista
                            if (node != null)
                            {
                                // Non mi piace aggiungere i peer cosi'
                                PeerInfo peer = new PeerInfo();
                                peer.EndPoint = node;
                                this.bootstrapPeers.Add(peer);
                                logger.Info("Added node: " + node);
                            }
                            else
                            {
                                logger.Warn("Adding node: " + node + "Failed.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Bad entry from hive server!" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Download della lista nodi fallito!" + ex.Message);
            }


        }

        public void AnnounceToHive(string port, string hiveUrlBase = Settings.HIVE_URL_BASE, string hiveUrlAnnounce = Settings.HIVE_URL_ANNOUNCE)
        {
            WebClient downloader = new WebClient();
            try
            {
                string response = downloader.DownloadString(hiveUrlBase + hiveUrlAnnounce + port);
                if (response.Contains("succeded"))
                {
                    logger.Info("Announcing to hive server succeded.");
                }
                else
                {
                    logger.Error("Annuncio al server Hive centrale fallito!");
                }
            }
            catch (WebException ex)
            {
                logger.Error("Annuncio al server Hive centrale fallito!");
                logger.Error("L'indirizzo specificato " + hiveUrlBase + hiveUrlAnnounce + port + " non e' valido" + ex.Message);
            }

        }
    }
}
