using System;
using System.Diagnostics;

namespace HashLib
{
    internal abstract class HMACBase : Hash, IHMAC
    {
        private byte[] m_key;
        protected bool m_bTransforming = false;

        public HMACBase(int a_hashSize, int a_blockSize) 
            : base(a_hashSize, a_blockSize)
        {
            m_key = new byte[0];
        }

        public virtual byte[] Key
        {
            get
            {
                return m_key;
            }
            set
            {
                m_key = value; 
            }
        }
    }
}
