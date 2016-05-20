using System;
using System.Diagnostics;

namespace HashLib
{
    internal class HMACNotBuildInAdapter : HMACBase
    {
        private byte[] m_opad;
        private byte[] m_ipad;
        private IHash m_hash;

        internal HMACNotBuildInAdapter(IHash a_underlyingHash)
            : base(a_underlyingHash.HashSize, a_underlyingHash.BlockSize)
        {
            m_hash = a_underlyingHash;

            m_ipad = new byte[m_hash.BlockSize];
            m_opad = new byte[m_hash.BlockSize];  
        }

        private void UpdatePads()
        {
            byte[] key;
            if (Key.Length > m_hash.BlockSize)
                key = m_hash.ComputeBytes(Key).GetBytes();
            else
                key = Key;

            for (int i = 0; i < m_hash.BlockSize; i++)
            {
                m_ipad[i] = 0x36;
                m_opad[i] = 0x5c;
            }

            for (int i = 0; i < key.Length; i++)
            {
                m_ipad[i] ^= key[i];
                m_opad[i] ^= key[i];
            }
        }

        public override void Initialize()
        {
            m_hash.Initialize();
            m_bTransforming = false;
        }

        public override HashResult TransformFinal()
        {
            if (!m_bTransforming)
            {
                UpdatePads();
                m_hash.TransformBytes(m_ipad);
                m_bTransforming = true;
            }

            HashResult h = m_hash.TransformFinal();
            Initialize();
            m_hash.TransformBytes(m_opad);
            m_hash.TransformBytes(h.GetBytes());
            h = m_hash.TransformFinal();
            Initialize();
            return h;
        }

        public override void TransformBytes(byte[] a_data, int a_index, int a_length)
        {
            Debug.Assert(a_index >= 0);
            Debug.Assert(a_length >= 0);
            Debug.Assert(a_index + a_length <= a_data.Length);

            if (!m_bTransforming)
            {
                UpdatePads();
                m_hash.TransformBytes(m_ipad);
                m_bTransforming = true;
            }

            m_hash.TransformBytes(a_data, a_index, a_length);
        }

        public override string Name
        {
            get
            {
                return String.Format("{0}({1})", GetType().Name, m_hash.GetType().Name);
            }
        }
    }
}
