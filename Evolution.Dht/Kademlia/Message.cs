using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Evolution.Dht.Util;

namespace Evolution.Dht.Kademlia
{
    public enum Header : byte
    {
        EvolutionDHT = 0xA0 // EvolutionDHT UDP Header 
    }

    public enum Opcode : byte
    {
        Ping = 0x01,
        Pong = 0x02,
        StoreRequest = 0xb1,
        StoreResponse = 0xb2,
        FindnodeRequest = 0xc1,
        FindnodeResponse = 0xc2,
        FindvalueRequest = 0xd1,
        FindvalueResponse = 0xd2,
        BoostrapRequest = 0xe1,
        BoostrapResponse = 0xe2,
        GODValue = 0xFF //GOD MODE
    }

    public class Message : IMessage
    {

        //(header)[1](opcode)[1](payloadlen)[2](data)[payloadlen]
        
        private byte[] PayloadLen = new byte[2]; // Max 2 bytes

        private byte[] Payload;
        private byte[] finalMessage;
        // potrebbe servire piu' avanti
        private int ttl = 10;

        // da parte per stasera
        private PeerInfo destPeer; // Assomiglia alla ben nota minchiata della generazione di ID random        
        private List<String> parameters = new List<String>();

        public PeerInfo DestPeer
        {
            get { return destPeer; }
        }

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

        public byte[] FinalMessage 
        {
            get { return finalMessage; }
        }

        public Message(PeerInfo dPeer)
        {
            destPeer = dPeer;
        }

        // Ma....dobbiamo inviare tutto un peerInfo nel pacchetto?
        // O ci basta sapere il source endpoint quando lo riceviamo?
        // Non ci servirebbe anche l'id del peer che mi ha inviato il messaggio?!
        public Message(IPEndPoint ipEndPoint)
        {
            destPeer = new PeerInfo();
            destPeer.EndPoint = ipEndPoint;
        }

        /// <summary>
        /// Crea l'intestazione dei pacchetti
        /// </summary>
        /// <param name="hdr">Header da inserire nell'intestazione (1 byte)</param>
        /// <param name="payloadLen"></param>
        private void CreateHeader(Header hdr, int payloadLen) 
        {
            // Per ora reputiamo sufficienti 65535 byte massimi di dati nei pacchetti
            // Usiamo infatti 2 soli byte 0xFF-0xFF per definire la lunghezza dei dati
            if (payloadLen > 65535)
            {
                throw new Exception("Malformed Packet: PayloadLen is more than 65535!");
            }

            finalMessage = new byte[1 + 1 + 2 + payloadLen];
            finalMessage[0] = (byte)hdr;

            byte[] ba =  BitConverter.GetBytes(payloadLen);
            
            // decine in posizione 2, unita' in posizione 3 (piu' a destra)
            finalMessage[2] = ba.Length > 1 ? ba[1] : (byte)0x00;
            finalMessage[3] = ba[0];
        }

        public void CreatePing()
        {
            CreateHeader(Header.EvolutionDHT, 0);
            finalMessage[1] = (byte)Opcode.Ping;
        }

        public void CreatePong()
        {
            CreateHeader(Header.EvolutionDHT, 0);
            finalMessage[1] = (byte)Opcode.Pong;
        }

        // E' ufficiale: la key passata è del tipo "pippo" e non un HASH
        public void CreateStoreRequest(string key, string val)
        {
            // Calcolo lunghezza dei dati
            byte[] bkey = HexEncoding.GetBytes(key);
            // byte[] bFakeKey = new byte[Settings.ID_LENGTH];
            //Array.Copy(bkey, bFakey, bkey.Length);
            byte[] bval = UTF8Encoding.ASCII.GetBytes(val);

            CreateHeader(Header.EvolutionDHT, Settings.ID_LENGTH + bval.Length);
            finalMessage[1] = (byte)Opcode.StoreRequest;
            
            // Dati
            //bFakeKey.CopyTo(finalMessage, 4);
            bkey.CopyTo(finalMessage, 4);
            bval.CopyTo(finalMessage, Settings.ID_LENGTH + 4);
        }

        // TODO: CreateStoreResponse non serve proprio a niente??

        public void CreateFindValueRequest(string key)
        {
            // Calcolo lunghezza dei dati
            byte[] bkey = HexEncoding.GetBytes(key);
            //byte[] bFakeKey = new byte[Settings.ID_LENGTH];
            //Array.Copy(bkey, bFakeKey, bkey.Length);

            CreateHeader(Header.EvolutionDHT, Settings.ID_LENGTH);
            finalMessage[1] = (byte)Opcode.FindvalueRequest;

            // Dati
            //bFakeKey.CopyTo(finalMessage, 4);
            bkey.CopyTo(finalMessage, 4);
        }

        public void CreateFindValueResponse(string key, string val)
        {
            // Calcolo lunghezza dei dati
            byte[] bkey = HexEncoding.GetBytes(key);
            //byte[] bFakeKey = new byte[Settings.ID_LENGTH];
            //Array.Copy(bkey, bFakeKey, bkey.Length);
            byte[] bval = UTF8Encoding.ASCII.GetBytes(val);

            CreateHeader(Header.EvolutionDHT, Settings.ID_LENGTH + bval.Length);
            finalMessage[1] = (byte)Opcode.FindvalueResponse;

            // Dati
            //bFakeKey.CopyTo(finalMessage, 4);
            bkey.CopyTo(finalMessage, 4);
            bval.CopyTo(finalMessage, Settings.ID_LENGTH + 4);
        }

        public void CreateFindNodeRequest(PeerId id)
        {
            // magari dobbiamo controllare se ID e' valido, forse anche dalle altre parti...
            CreateHeader(Header.EvolutionDHT, Settings.ID_LENGTH);
            finalMessage[1] = (byte)Opcode.FindnodeRequest;

            // Dati
            id.Id.CopyTo(finalMessage, 4);
        }

        public void CreateFindNodeResponse(List<PeerInfo> peers)
        {
            // dobbiamo implementare l'ipv6 quindi questo pacchetto deve tenerne conto
            // piu' avanti ...
            int payloadlen = Settings.ID_LENGTH
                + 4     // IP 4 byte
                + 2     // Port always 2 byte
                + 2;    // TODO: Mancano due byte... ???

            CreateHeader(Header.EvolutionDHT, peers.Count * payloadlen);
            finalMessage[1] = (byte)Opcode.FindnodeResponse;


            // dobbiamo finire di costruire il pacchetto, l'offeset 4 non va bene, solo la prima volta
            // vedi class arraysegment .net 4.5
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bwriter = new BinaryWriter(ms))
                {
                    foreach (PeerInfo peer in peers)
                    {
                        bwriter.Write(peer.Id.Id);
                        bwriter.Write(peer.EndPoint.Address.GetAddressBytes());
                        bwriter.Write(peer.EndPoint.Port);
                    }
                    ms.ToArray().CopyTo(finalMessage,4);
                }
            }
        }

        public void CreateBoostrap(PeerId id,Opcode opc)
        {
            // magari dobbiamo controllare se ID e' valido, forse anche dalle altre parti...
            CreateHeader(Header.EvolutionDHT, Settings.ID_LENGTH);
            finalMessage[1] = (byte)opc;

            // Dati
            id.Id.CopyTo(finalMessage, 4);
        }
    }
}
