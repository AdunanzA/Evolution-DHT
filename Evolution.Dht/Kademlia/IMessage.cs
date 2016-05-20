using System;
using System.Collections.Generic;
using System.Net;

namespace Evolution.Dht.Kademlia
{
    public interface IMessage
    {
        Header          header          { get; set; }
        Opcode          opcode          { get; set; }
        IPEndPoint      SourceEndPoint   { get; set; }
        string          key             { get; set; }
        string          val             { get; set; }
        PeerId          peerID          { get; set; }
        List<PeerInfo>  peers           { get; set; }

    }
}
