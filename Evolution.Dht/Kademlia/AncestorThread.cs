using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Evolution.Dht.Util;

namespace Evolution.Dht.Kademlia
{
    public abstract class AncestorThread
    {
        public delegate void FinishHandler(AncestorThread thread);
        public event FinishHandler Finish;
 
        private Client client;
        private PeerId keyId;
        protected Thread thread;
        protected DateTime startTime; 

        public Client Client
        {
            get { return client; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
        }

        public PeerId KeyId
        {
            get { return keyId; }
        }

        public AncestorThread()
        {
            thread = new Thread(Run);
        }

        public AncestorThread(Client client)
        {
            thread = new Thread(Run);
            this.client = client;
        }

        public AncestorThread(Client client, PeerId keyId)
        {
            thread = new Thread(Run);
            this.client = client;
            this.keyId = keyId;
        }
        
        // costruttore aggiunto temporaneamente per eliminare il ricalcolo dell'hash della key ogni volta
        public AncestorThread(Client client, string key)
        {
            thread = new Thread(Run);
            this.client = client;
            this.keyId = new PeerId();
            keyId.Id = HexEncoding.GetBytes(key);
        }

        public void Start()
        {
            thread.Start();
        }

        public virtual void Abort()
        {
            thread.Abort();
        }

        public void Join()
        {
            thread.Join();
        }

        protected virtual Message CreateFindRequest()
        {
            return null;
        }

        public virtual void ProcessMessage(IMessage message)
        {

        }

        // Per ora non ci interessa avere una funzione di callback EndInvoke
        // passiamo pertanto null
        protected virtual void Run()
        {
            if (Finish != null)
            {
                Finish.BeginInvoke(this, null, null);
            }
        }
    }
}
