using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Evolution.Dht.Util;
using System.Net.NetworkInformation;
using System.Linq;
using Evolution.Dht;
using HashLib;

namespace Evolution.Dht.Kademlia
{
    public class PeerId
    {
        private byte[] buffer = new byte[Settings.ID_LENGTH];
        private static HashSize hashSize;
        public byte[] Id
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public PeerId()
        {
            // conversione ID_LENGTH to enum usato dalla HASHLIB
            Enum.TryParse<HashSize>(Settings.ID_LENGTH.ToString(), out hashSize); 
        }

        public static explicit operator PeerId(string id) 
        {
            if (id.Length != Settings.ID_LENGTH)
                return null;

            var pid = new PeerId();
            pid.Id = UTF8Encoding.ASCII.GetBytes(id);
            return pid;
        }

        private string GetMacAddress()
        {
            return 
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
        
        }

        /// <summary>
        /// Generazione dell'id basato su unicita' del MacAddress + Versione dell'OS + Nome dell'account utente
        /// </summary>
        public void Create()
        {

            IHash hasher;
            hasher = HashFactory.Crypto.SHA3.CreateShabal(hashSize);
            string signature = GetMacAddress() + Environment.OSVersion.VersionString + Environment.UserName;
            buffer = hasher.ComputeBytes(Encoding.UTF8.GetBytes(signature)).GetBytes();
        }

        // Codice delirante tutt'ora ignoto
        public int GetBucketIndex()
        {
            int i;
            for (i = 159; i >= 0; i--)
            {
                if (IsBit(i))
                {
                    return i;
                }
            }
            return 0;
        }

        public bool IsBit(int index)
        {
            int byteIndex = index / 8;
            int bitIndex = index % 8;
            byte mask = (byte)(0x01 << bitIndex);

            return (buffer[19-byteIndex] & mask) != 0;
        }

        public void SetByte(int index, byte val)
        {
            buffer[19 - index] = val;
        }

        #region OPERATORI

        // TODO: capire perche' alcuni peer sono nulli e crashiano l'operatore == sui peerid
        public static bool operator == (PeerId id1, PeerId id2)
        {
            if (object.Equals(id1, null) || object.Equals(id2, null)) { return object.Equals(id1, id2); }
            return id1.ToString() == id2.ToString();
        }

        public static bool operator !=(PeerId id1, PeerId id2)
        {
            if (object.Equals(id1, null) || object.Equals(id2, null)) { return !object.Equals(id1, id2); }
            return id1.buffer != id2.buffer;
        }

        public static bool operator >(PeerId id1, PeerId id2)
        {
            if (object.Equals(id1, null) || object.Equals(id2, null)) { return false; }
            for (int i = 0; i < 160; i++)
            {
                if (id1.IsBit(160 - 1 - i) && !id2.IsBit(160 - 1 - i))
                {
                    return true;
                }
                if (!id1.IsBit(160 - 1 - i) && id2.IsBit(160 - 1 - i))
                {
                    return false;
                }
            }
            return false;
        }

        public static bool operator <(PeerId id1, PeerId id2)
        {
            if (object.Equals(id1, null) || object.Equals(id2, null)) { return false; }
            for (int i = 0; i < 160; i++)
            {
                if (id2.IsBit(160 - 1 - i) && !id1.IsBit(160 - 1 - i))
                {
                    return true;
                }
                if (!id2.IsBit(160 - 1 - i) && id1.IsBit(160 - 1 - i))
                {
                    return false;
                }
            }
            return false;
        }

        public static bool operator >=(PeerId id1, PeerId id2)
        {
            return id1 == id2 || id1 > id2;
        }

        public static bool operator <=(PeerId id1, PeerId id2)
        {
            return id1 == id2 || id1 < id2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return object.Equals(this, null); }
            return this.ToString() == obj.ToString();
        }

        public override int  GetHashCode()
        {
 	         return buffer.GetHashCode();
        }

        public override string ToString()
        {
            // Si potrebbero usare una codifica a 64bit
            return HexEncoding.ToString(buffer);
        }

        #endregion

        public static PeerId CalculateDistance(PeerId id1, PeerId id2)
        {
            // TODO: gestire il caso di ritorno nullo
            if (id1 == null || id2.Equals(null)) { return null; }
            
            PeerId id = new PeerId();
            int i;
            for (i = 0; i < 20; i++)
            {
                id.buffer[i] = (byte)(id1.buffer[i] ^ id2.buffer[i]);
            }
            return id;
        }

        public static PeerId CalculateId(string key)
        {
            PeerId id = new PeerId();
            var hash = HashFactory.Crypto.SHA3.CreateShabal(hashSize);
            byte[] input = System.Text.Encoding.ASCII.GetBytes(key);
            id.Id = hash.ComputeBytes(input).GetBytes();            
            return id;
        }

        // TODO: Chiamare key il parametro non è un po' fuorviante?!
        public static PeerId LoadFromString(string key)
        {
            PeerId id = new PeerId();
            //int discarded;
            id.buffer = HexEncoding.GetBytes(key);
            return id;
        }
    }
}
