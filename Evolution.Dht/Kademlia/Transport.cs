using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using NLog;
using Evolution.Dht.Util;
using Evolution.Dht;

namespace Evolution.Dht.Kademlia
{
    public class Transport
    {
        public delegate void MessageHandler(Transport transport, IMessage message);
        public event MessageHandler Message;

        private UdpClient udp;
        private Thread th;

        // Log purpose
        Logger logger = LogManager.GetLogger("Transport");

        public void Listen(int port)
        {
            udp = new UdpClient(port);
            th = new Thread(Run);
            th.Name = "TransportThread";
            th.Start();
        }

        public void Stop()
        {
            if (udp != null)
            {
                udp.Close();
                th.Abort();
                th.Join();
            }
        }

        // Peer non serve piu' perche' lo mettiamo nella classe message il destinatario
        public void Send(Message message, PeerInfo peer = null)
        {
            udp.Send(message.FinalMessage, message.FinalMessage.Length, message.DestPeer.EndPoint);
        }

        private void Run()
        {
            try
            {
                byte[] buffer;
                IPEndPoint remoteEP = null;
                while (true)
                {
                    remoteEP = null;
                    buffer = udp.Receive(ref remoteEP);
                    var receivedMessage = DeserializeMessage(buffer, remoteEP);

                    if (Message != null && receivedMessage != null)
                    {
                        Message.BeginInvoke(this, receivedMessage,null,null);
                    }
                }
            }
            catch (SocketException)
            {
            }
            catch (ThreadAbortException)
            {
            }
        }

        private IMessage DeserializeMessage(byte[] memStream, IPEndPoint srcIPEndpoint)
        {
            if (memStream.Length == 0) return null;
            
            var rcvMex = new MessageReceived();
            
            // Scartiamo tutto il traffico non di EvolutionDHT
            Header hd;
            if (!Enum.TryParse<Header>(memStream[0].ToString(), out  hd)) return null;
            if (Header.EvolutionDHT.Equals(rcvMex.header)) return null;

            rcvMex.opcode = (Opcode)Enum.Parse(typeof(Opcode), memStream[1].ToString());
            var payloadLength = int.Parse(memStream[2].ToString() + memStream[3].ToString());

            byte[] tmpKey = new byte[Settings.ID_LENGTH];

            switch (rcvMex.opcode)
            {
                case Opcode.Ping:
                    break;
                case Opcode.Pong:
                    break;
                case Opcode.StoreRequest:
                    Array.Copy(memStream, 4, tmpKey, 0, Settings.ID_LENGTH);
                    rcvMex.key = HexEncoding.ToString(tmpKey);
                    rcvMex.val = UTF8Encoding.ASCII.GetString(memStream, 4 + Settings.ID_LENGTH, payloadLength - Settings.ID_LENGTH);
                    break;
                case Opcode.StoreResponse:
                    break;
                case Opcode.BoostrapRequest:
                case Opcode.BoostrapResponse:
                case Opcode.FindnodeRequest:
                    rcvMex.peerID = new PeerId();
                    Array.Copy(memStream, 4, rcvMex.peerID.Id, 0, Settings.ID_LENGTH);
                    break;
                case Opcode.FindnodeResponse:
                    // il 4 di questo offset potebbe diventare 16 quando implementeremo l'IPV6
                    int offset = 4 + 4 + Settings.ID_LENGTH;
                    var rcvPeerCount = payloadLength / (offset);

                    byte[] address = new byte[4];
                    byte[] port = new byte[4];

                    // Inizializzo lista peers
                    rcvMex.peers = new List<PeerInfo>();

                    for (int i = 0; i < rcvPeerCount; i++)
                    {
                        var tmpPeer = new PeerInfo();

                        Array.Copy(memStream, 4 + i * offset, tmpPeer.Id.Id, 0, Settings.ID_LENGTH);
                        Array.Copy(memStream, (4 + Settings.ID_LENGTH) + i * offset, address, 0, 4);
                        Array.Copy(memStream, (4 + 4 + Settings.ID_LENGTH) + i * offset, port, 0, 4);

                        // facciamo casino nella deserializzazione di ip e porta presumibilmente leggiamo shiftato di 2 byte
                        tmpPeer.EndPoint = new IPEndPoint(new IPAddress(address), BitConverter.ToInt32(port, 0));
                        rcvMex.peers.Add(tmpPeer);
                    }
                    break;
                case Opcode.FindvalueRequest:
                    Array.Copy(memStream, 4, tmpKey, 0, Settings.ID_LENGTH);
                    rcvMex.key = HexEncoding.ToString(tmpKey);
                    break;
                case Opcode.FindvalueResponse:
                    Array.Copy(memStream, 4, tmpKey, 0, Settings.ID_LENGTH);
                    rcvMex.key = HexEncoding.ToString(tmpKey);
                    rcvMex.val = UTF8Encoding.ASCII.GetString(memStream, 4 + Settings.ID_LENGTH, payloadLength - Settings.ID_LENGTH);
                    break;
                case Opcode.GODValue:
                    // aggiungere format del pc al destPeer :P
                    break;
                default:
                    break;
            }
            rcvMex.SourceEndPoint = srcIPEndpoint;
            return rcvMex;
        }

    }
}
