using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Evolution.Dht.Kademlia;
using Evolution.Dht;

namespace Evolution.Dht
{
    public abstract class Client
    {
        public delegate void LookupHandler(string key, string val);
        public event LookupHandler Lookup;

        public delegate void PeerAddedHandler(PeerInfo peer);
        public event PeerAddedHandler PeerAdded;

        public delegate void PeerDeletedHandler(PeerInfo peer);
        public event PeerDeletedHandler PeerDeleted;

        public delegate void PeerUpdatedHandler(PeerInfo peer);
        public event PeerUpdatedHandler PeerUpdated;

        public void RaiseLookupEvent(string k, string v)
        {
            LookupHandler evt = Lookup;
            if (evt != null)
            {
                evt(k, v);
            }
        }

        public void RaiseAddedPeerEvent(PeerInfo peer)
        {
            PeerAddedHandler evt = PeerAdded;
            if (evt != null)
            {
                evt.BeginInvoke(peer, null, null);
            }
        }

        public void RaiseDeletedPeerEvent(PeerInfo peer)
        {
            PeerDeletedHandler evt = PeerDeleted;
            if (evt != null)
            {
                evt.BeginInvoke(peer, null, null);
            }
        }

        public void RaiseUpdatedPeerEvent(PeerInfo peer)
        {
            PeerUpdatedHandler evt = PeerUpdated;
            if (evt != null)
            {
                evt.BeginInvoke(peer, null, null);
            }
        }

        public abstract void Start();
        public abstract void Abort();
        public abstract void Join();
        public abstract void BeginStore(string key, string val);
        public abstract void BeginLookup(string key);

    }
}
