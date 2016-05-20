using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Evolution.Dht.Kademlia
{
    [Serializable]
    
    public enum Status
    {
        ToRemove = 0,   // non e' mai in linea
        JustCreated = 1,    //non contattato
        ToUpdate = 2,     // meno di 1 ora     
        AlwaysOn = 3   // ricontattato e c'e' ancora
    }

    public class PeerInfo
    {
        private PeerId id = new PeerId();
        //TODO: Capirne il proposito del nome dei peer
        private string name = string.Empty;
        private IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
        private int rtt = -1;
        private DateTime lastSeen = DateTime.MinValue;
        private DateTime lastSend = DateTime.MinValue;
        private DateTime createdOn = DateTime.MinValue;
        private Status status = Status.JustCreated;

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }

        public int Retries
        {
            get;
            set;
        }

        public PeerId Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public IPEndPoint EndPoint
        {
            get { return endpoint; }
            set { endpoint = value; }
        }

        public DateTime LastSeen
        {
            get { return lastSeen; }
            set { lastSeen = value; }
        }

        public DateTime LastSend
        {
            get { return lastSend; }
            set { lastSend = value; }
        }
        
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        
        public void Update(PeerInfo peer)
        {
            endpoint = peer.endpoint;
            rtt = peer.rtt;
            lastSeen = DateTime.Now;
        }

        public bool Equals(PeerInfo peer)
        {
            return (this.id == peer.id || this.EndPoint == peer.EndPoint);
        }
    }
}
