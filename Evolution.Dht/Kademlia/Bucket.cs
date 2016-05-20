using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Dht.Kademlia
{
    public class Bucket : List<PeerInfo>
    {
        public void Reorder(int k)
        {
            List<PeerInfo> newList = new List<PeerInfo>();
            while (Count > k)
                RemoveAt(Count - 1);
        }
    }
}
