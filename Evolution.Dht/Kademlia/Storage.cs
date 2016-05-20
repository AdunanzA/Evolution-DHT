using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Dht.Kademlia
{
    public class Storage
    {
        private DateTime expiration = DateTime.MinValue;

        public string Key { get; set; }
        public string Val { get; set; }

        public DateTime Expiration
        {
            get { return expiration; }
            set { expiration = value; }
        }
        

    }
}
