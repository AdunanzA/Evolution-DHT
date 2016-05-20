using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Evolution.Dht.Util;

namespace Evolution.Dht.Kademlia
{
    public class FindNodeThread : AncestorThread
    {
        private PeerId keyId;
        
        public FindNodeThread(Client client, PeerId keyId)
        {
            base.thread.Name = "FindNodeThread";
            this.keyId = keyId;
        }
        
        // costruttore aggiunto temporaneamente per eliminare il ricalcolo dell'hash della key ogni volta
        public FindNodeThread(Client client, string key)
        {
            base.thread.Name = "FindNodeThread";
            this.keyId = new PeerId();
            keyId.Id = HexEncoding.GetBytes(key);
        }

        public override void ProcessMessage(IMessage message)
        {
            if (message.opcode == Opcode.FindnodeResponse)
            {
                foreach (var peer in message.peers)
                {
                    // Cosa avra' voluto fare?? (cit.)
                    if (peer.Id.ToString() == KeyId.ToString())
                    {
                    }
                }
            }
        }

        protected override Message CreateFindRequest()
        {
            var message = new Message(Client.Me);
            message.CreateFindNodeRequest(KeyId);
            return message;
        }


        protected override void Run()
        {
            /// Questa parte potrebbe essere definita come ricerca iterativa
            Message message = CreateFindRequest();

            DateTime lastSend = DateTime.Now;

            List<PeerInfo> peers = Client.GetCloserPeers(KeyId, Client.Alfa);
            lock (Client)
            {
                foreach (PeerInfo peer in peers)
                {
                    lastSend = DateTime.Now;
                    Client.Send(message, peer);
                }
            }
            /// fine iterativa

            //WaitForNeearerPeers
            // Aspetto 10ms e poi controllo se ci sono nuovi peer che sono piu' vicini alla chiave di quelli che avevo precedentemente contattato (nella parte "iterativa")
            // Se ne trovo allora li contatto con una FindNodeRequest
            
            // Algorithm 3 Loose concurrent node lookup
            //http://www.tlc-networks.polito.it/oldsite/mellia/corsi/07-08/Laboratorio/MasterThesisBrunner.pdf
            // In realta' non e' concurrent al momento perche' i nodi sono contattati in sequenza per 2 sec
            while ((DateTime.Now - lastSend).TotalMilliseconds < 2*1000)
            {
                Thread.Sleep(10);
                List<PeerInfo> newPeers = Client.GetCloserPeers(KeyId, Client.Alfa);
                foreach (PeerInfo newPeer in newPeers)
                {
                    bool nearer = true;
                    foreach (PeerInfo oldPeer in peers)
                    {
                        if (PeerId.CalculateDistance(oldPeer.Id, KeyId) <= PeerId.CalculateDistance(newPeer.Id, KeyId))
                        {
                            nearer = false;
                        }
                    }
                    if (nearer)
                    {
                        lastSend = DateTime.Now;
                        peers.Add(newPeer);
                        Client.Send(message, newPeer);
                    }
                }
            }
            base.Run();

        }
    }
}
