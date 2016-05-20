using System;
using System.Diagnostics;

namespace HashLib
{
    internal abstract class HashCrypto : Hash, ICrypto, IBlockHash
    {
        protected HashCrypto(int a_hashSize, int a_blockSize)
            : base(a_hashSize, a_blockSize)
        {
        }
    }
}
