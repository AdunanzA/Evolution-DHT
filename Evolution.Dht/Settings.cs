using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Dht
{
    static class Settings
    {
        public const int ID_LENGTH = 28;
        public const int BUCKETS_NUM = 160;
        public const int BUCKETS_MAX_PEERS = 20;
        
        /// <summary> Numero massimo di ricorsioni? da vedere nel paper</summary>
        public const int THETA = 10;

        /// <summary> Numero massimo di nodi contattati contemporanemente nella ricerca iterativa FindNode (concurrent lookup) </summary>
        public const int ALPHA = 3;

        /// <summary> 10 min </summary>
        public const int TIMING_PEER_REMOVE = 10 * 30 * 1000;
        /// <summary> 5 min </summary>
        public const int TIMING_PEER_UPDATE = 5 * 30 * 1000;
        /// <summary> 2 hrs </summary>
        public const int TIMING_PEER_ALWAYS_ON = 2 *60 * 30 * 1000;

        /// <summary> 10 min </summary>
        public const int TIMING_PUBLICATION_EXPIRATION = 10 * 30 * 1000;

        /// <summary> 1 min </summary>
        public const int TIMING_REFRESH_CLIENTRUN = 1 * 10 * 1000;

        /// <summary> 2 Tentativi prima di cancellare un Peer  </summary>
        public const int PEER_REMOVE_RETRIES = 2;

        /// <summary> Base address of the site containing the list of active nodes </summary>
        public const string HIVE_URL_BASE = "http://evolution.adunanza.net/dht/";
        /// <summary> URL of the page to call for getting the list of active nodes </summary>
        public const string HIVE_URL_NODES = "nodes.php";
        /// <summary> URL of the page to call for announce this nodes to the hive server</summary>
        public const string HIVE_URL_ANNOUNCE = "announce.php?port=";

        /// <summary> Numero Massimo di peer verso cui replicare l'informazione</summary>
        public const int STORE_MAX_REPLICATION_PEERS = 2;

        /// <summary> 45 Sec </summary>
        public const int FINDVALUE_MAX_TIMESPAN = 5;
        

    }
}
