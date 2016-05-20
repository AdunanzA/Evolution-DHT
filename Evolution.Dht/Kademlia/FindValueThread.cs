using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Evolution.Dht.Kademlia
{
    class FindValueThread : AncestorThread
    {
        private string key;
        private Client client;

        public FindValueThread(Client client, string key) : 
            base(client, PeerId.CalculateId(key))
        {
            base.thread.Name = "FindValueThread";
            this.key = key;
            this.client = client;
        }

        public override void Abort()
        {
            client.FireLookupResult(key, "Nessun risultato!");
            base.Abort();
        }

        // Cerca anche in locale perchè nella bucketlist c'è anche il mio nodo locale
        // nel caso decidessimo di eliminare il nodo fare attenzione
        protected override void Run()
        {
            DateTime lastSend = DateTime.Now;
            Message message = null;
            List<PeerInfo> peers = client.GetCloserPeers(KeyId, client.Alfa);
            lock (client)
            {
                foreach (PeerInfo peer in peers)
                {
                    lastSend = DateTime.Now;
                    message = new Message(peer);
                    message.CreateFindValueRequest(key);
                    client.Send(message, peer);
                }
            }
            base.startTime = DateTime.Now;
        }

        public override void ProcessMessage(IMessage message)
        {

            if (message.opcode == Opcode.FindvalueResponse)
            {
                if (!string.IsNullOrEmpty(message.key))
                {
                    if (message.key == key)
                    {
                        client.FireLookupResult(key, message.val);
                        //Termina il Thread se trova la chiave
                        base.Run();
                    }
                }
            }
        }
    }
}
