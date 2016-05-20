using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using NLog;
using Evolution.Dht;

namespace Evolution.Dht.Kademlia
{
    public class BucketList : IEnumerable<PeerInfo>
    {
        Client client;

        // Log purpose
        Logger logger = LogManager.GetLogger("BucketList");

        //TODO: Perché proprio 160? eMule quanti ne usa??
        private Bucket[] buckets = new Bucket[Settings.BUCKETS_NUM];

        public int Lenght
        {
            get { return this.Lenght; }
        }
        /// <summary>
        /// Costruttore. Instanzia 160 nuovi buckets
        /// </summary>
        /// <param name="client">Tiene riferimento del client che l'ha instanziato</param>
        public BucketList(Client client)
        {
            this.client = client;
            for (int i = 0; i < Settings.BUCKETS_NUM; i++)
            {
                buckets[i] = new Bucket();
            }
        }

        public void Load()
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load("kad.xml");
                foreach (XmlElement element in document.DocumentElement.ChildNodes)
                {
                    PeerInfo peer = new PeerInfo();
                    peer.Id = PeerId.LoadFromString(element.GetAttribute("id"));
                    string strAddress = element.GetAttribute("address");
                    IPAddress address = IPAddress.Parse(strAddress);
                    string strPort = element.GetAttribute("port");
                    int port = 4401;
                    Int32.TryParse(strPort, out port);
                    peer.EndPoint = new IPEndPoint(address, port);
                    UpdatePeer(peer);
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        public void Store()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml("<peers></peers>");
            foreach (PeerInfo peer in this)
            {
                XmlElement element = document.CreateElement("peer");
                element.SetAttribute("id", peer.Id.ToString());
                element.SetAttribute("address", peer.EndPoint.Address.ToString());
                element.SetAttribute("port", peer.EndPoint.Port.ToString());
                document.DocumentElement.AppendChild(element);
            }
            using (XmlWriter writer = new XmlTextWriter("kad.xml", Encoding.UTF8))
            {
                document.Save(writer);
            }
        }

        public PeerInfo GetPeer(PeerId peerId)
        {
            foreach (PeerInfo peer in this)
            {
                if (peer.Id == peerId)
                {
                    return peer;
                }
            }
            return null;
        }

        public PeerInfo GetPeer(IPEndPoint ipe)
        {
            foreach (PeerInfo peer in this)
            {
                if (peer.EndPoint.Equals(ipe))
                {
                    return peer;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPeer"></param>
        public void UpdatePeer(PeerInfo newPeer)
        {
            // Perché calcola sempre la distanza?
            // Perché altrimenti non sapremmo in che bucket inserire il peer
            PeerId distance = PeerId.CalculateDistance(client.Me.Id, newPeer.Id);

            // Ricerca all'interno dei bucktes il nuovo peer da aggiornare
            foreach (Bucket bucket in buckets)
            {
                foreach (PeerInfo peer in bucket)
                {
                    // Se esite gia' il peer nella bucketlist aggiorniamo solamente
                    // Ed impediamo di inserire piu' volte nella bucketlist un peer con stesso ID o Endpoint
                    // TODO: da rivedere quando avremo riscritto la classe Peer
                    if (peer.Id == newPeer.Id || peer.EndPoint.Equals(newPeer.EndPoint))
                    {
                        logger.Info("Updated peer: " + peer.EndPoint.ToString());
                        peer.Update(newPeer);
                        bucket.Remove(peer);
                        bucket.Insert(0, peer);
                        client.RaiseUpdatedPeerEvent(peer);
                        return; //Se lo aggiorno non lo aggiungo
                    }
                }
            }

            int index = distance.GetBucketIndex();

            //newPeer.LastSeen = DateTime.Now;

            //TODO: criterio di scarto peers
            // In teoria non dovremo scartare a priori i nuovi peer se siamo pieni
            // ma bisognerebbe controllare che il peer che vogliamo aggiornare sia
            // più affidabile di quelli che già abbiamo (cfr. Sybil attack)
            if (buckets[index].Count < client.K)
            {
                // TODO: aggiunta peer inesistenti
                // Attenzione lo sto aggiungendo senza assicurarmi che esista
                //es. quando richiamo bootstrap dalla gui
                buckets[index].Add(newPeer);
                logger.Info("New peer added: " + newPeer.EndPoint.ToString());
                client.RaiseAddedPeerEvent(newPeer);
            }
        }

        public void Remove(PeerId id)
        {
            foreach (Bucket bucket in buckets)
            {
                foreach (PeerInfo peer in bucket)
                {
                    if (peer.Id == id)
                    {
                        bucket.Remove(peer);
                        return;
                    }
                }
            }
        }

        #region IEnumerable<Bucket> Members

        public IEnumerator<PeerInfo> GetEnumerator()
        {
            foreach (Bucket bucket in buckets)
            {
                foreach (PeerInfo peer in bucket)
                {
                    // La parola chiave yield segnala al compilatore che il metodo è un blocco iterator
                    yield return peer;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public string Dump()
        {
            string dump = string.Empty;
            int i = 0;
            foreach (Bucket bucket in buckets)
            {
                dump += "Bucket " + (i++) + "\r\n";
                foreach (PeerInfo peer in bucket)
                {
                    dump += "\tPeer " + peer.Id.ToString() + " (" + peer.LastSeen + ")" + "\r\n";
                }
            }
            return dump;
        }
    }
}
