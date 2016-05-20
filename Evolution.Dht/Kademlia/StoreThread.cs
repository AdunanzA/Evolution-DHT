using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Evolution.Dht.Kademlia
{
    public class StoreThread : AncestorThread
    {
        private string val;
        private string key;

        public StoreThread(Client client, string key, string val) :
            base(client, key)
        {
            base.thread.Name = "StoreThread";
            this.key = key;
            this.val = val;
        }

        protected override void Run()
        {
            // 2 è il numero dei nodi su cui replicare l'informazione durante lo store
            // (il numero massimo di nodi restituiti da GetClosedpeers()
            List<PeerInfo> peers = Client.GetCloserPeers(KeyId, Settings.STORE_MAX_REPLICATION_PEERS);

            foreach(PeerInfo peer in peers)
            {
                Message message = new Message(peer);
                message.CreateStoreRequest(key, val);
                Client.Send(message, peer);
            }
            base.Run();
        }
    }
}
