using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Evolution.Dht.Kademlia
{
    class MessageReceived : IMessage
    {
        public Header header { get; set; }

        public Opcode opcode { get; set; }

        public IPEndPoint SourceEndPoint
        { get; set; }

        public string key
        { get; set; }

        public string val
        { get; set; }

        public PeerId peerID
        { get; set; }

        public List<PeerInfo> peers
        { get; set; }
    }
}
