using System;
using System.Diagnostics;

namespace HashLib.Hash64
{
    internal class Murmur2_64 : MultipleTransformNonBlock, IHash64, INonBlockHash
    {
        protected const ulong M = 0xc6a4a7935bd1e995;
        protected const int R = 47;
        protected const uint SEED = 0xc58f1a7b;

        public Murmur2_64()
            : base(8, 8)
        {
        }

        public override HashResult ComputeBytes(byte[] a_data)
        {
            int length = a_data.Length;

            if (length == 0)
                return new HashResult((ulong)0);

            ulong h = SEED ^ (ulong)length;
            int currentIndex = 0;


            while (length >= 8)
            {
                ulong k = (ulong)a_data[currentIndex++] | (ulong)a_data[currentIndex++] << 8 | (ulong)a_data[currentIndex++] << 16 |
                          (ulong)a_data[currentIndex++] << 24 | (ulong)a_data[currentIndex++] << 32 | (ulong)a_data[currentIndex++] << 40 |
                          (ulong)a_data[currentIndex++] << 48 | (ulong)a_data[currentIndex++] << 56;

                k *= M;
                k ^= k >> R;
                k *= M;

                h ^= k;
                h *= M;

                length -= 8;
            }

            switch (length)
            {
                case 7:
                    h ^= (ulong)a_data[currentIndex++] << 48 | (ulong)a_data[currentIndex++] << 40 | (ulong)a_data[currentIndex++] << 32 |
                         (ulong)a_data[currentIndex++] << 24 | (ulong)a_data[currentIndex++] << 16 | (ulong)a_data[currentIndex++] << 8 |
                         (ulong)a_data[currentIndex++];
                    h *= M;
                    break;
                case 6:
                    h ^= (ulong)a_data[currentIndex++] << 40 | (ulong)a_data[currentIndex++] << 32 | (ulong)a_data[currentIndex++] << 24 |
                         (ulong)a_data[currentIndex++] << 16 | (ulong)a_data[currentIndex++] << 8 | (ulong)a_data[currentIndex++];
                    h *= M;
                    break;
                case 5:
                    h ^= (ulong)a_data[currentIndex++] << 32 | (ulong)a_data[currentIndex++] << 24 | (ulong)a_data[currentIndex++] << 16 |
                         (ulong)a_data[currentIndex++] << 8 | (ulong)a_data[currentIndex++];
                    h *= M;
                    break;
                case 4:
                    h ^= (ulong)a_data[currentIndex++] << 24 | (ulong)a_data[currentIndex++] << 16 | (ulong)a_data[currentIndex++] << 8 |
                         (ulong)a_data[currentIndex++];
                    h *= M;
                    break;
                case 3:
                    h ^= (ulong)a_data[currentIndex++] << 16 | (ulong)a_data[currentIndex++] << 8 | (ulong)a_data[currentIndex++];
                    h *= M;
                    break;
                case 2:
                    h ^= (ulong)a_data[currentIndex++] << 8 | (ulong)a_data[currentIndex++];
                    h *= M;
                    break;
                case 1:
                    h ^= (ulong)a_data[currentIndex++];
                    h *= M;
                    break;
            };

            h ^= h >> R;
            h *= M;
            h ^= h >> R;

            return new HashResult(h);
        }
    }
}
