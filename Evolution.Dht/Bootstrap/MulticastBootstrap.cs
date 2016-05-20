using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Evolution.Dht.Bootstrap
{
    public class MulticastBootstrap : IDisposable
    {
        private UdpClient udp;
        private Thread thread;
        private bool running;

        public MulticastBootstrap()
        {
        }

        public void Start()
        {
            udp = new UdpClient(4405);
            udp.Connect(IPAddress.Parse("224.5.6.7"), 4405);
            udp.JoinMulticastGroup(IPAddress.Parse("224.5.6.7"));

            thread = new Thread(Run);
            running = true;
            thread.Start();
        }

        public void Abort()
        {
            if (running)
            {
                running = false;
                udp.DropMulticastGroup(IPAddress.Parse("224.5.6.7"));
                udp.Close();
            }
        }

        public void Join()
        {
            if (running)
            {
                thread.Join();
            }
        }

        public void Send(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Flush();

            udp.Send(stream.GetBuffer(), (int)stream.Position);
        }

        private void Run()
        {
            while (running)
            {
                IPEndPoint endpoint = null;
                byte[] buffer = udp.Receive(ref endpoint);

                MemoryStream stream = new MemoryStream(buffer);
                BinaryFormatter formatter = new BinaryFormatter();
                object obj = formatter.Deserialize(stream);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Abort();
        }

        #endregion
    }
}
