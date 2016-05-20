using System;
using System.Diagnostics;
using System.Text;

namespace HashLib.Hash32
{
    internal class Murmur2 : MultipleTransformNonBlock, IHash32, IFastHashCodes, INonBlockHash
    {
        private const uint M = 0x5bd1e995;
        private const int R = 24;
        private const uint SEED = 0xc58f1a7b;

        public Murmur2()
            : base(4, 4)
        {
        }

        public override HashResult ComputeBytes(byte[] a_data)
        {
            int length = a_data.Length;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k = (uint)(a_data[currentIndex++] | a_data[currentIndex++] << 8 | a_data[currentIndex++] << 16 | a_data[currentIndex++] << 24);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }

            switch (length)
            {
                case 3:
                    h ^= (uint)(a_data[currentIndex++] | a_data[currentIndex++] << 8);
                    h ^= (uint)(a_data[currentIndex] << 16);
                    h *= M;
                    break;
                case 2:
                    h ^= (uint)(a_data[currentIndex++] | a_data[currentIndex] << 8);
                    h *= M;
                    break;
                case 1:
                    h ^= a_data[currentIndex];
                    h *= M;
                    break;
                default:
                    break;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeByte(byte a_data)
        {
            uint h = SEED ^ 1;

            h ^= a_data;
            h *= M;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeChar(char a_data)
        {
            uint h = SEED ^ 2;

            h ^= (ushort)a_data;
            h *= M;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeShort(short a_data)
        {
            uint h = SEED ^ 2;

            h ^= (ushort)a_data;
            h *= M;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeUShort(ushort a_data)
        {
            uint h = SEED ^ 2;

            h ^= (ushort)a_data;
            h *= M;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeInt(int a_data)
        {
            uint h = SEED ^ 4;

            uint k = (uint)a_data;
            k *= M;
            k ^= k >> R;
            k *= M;

            h *= M;
            h ^= k;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeUInt(uint a_data)
        {
            uint h = SEED ^ 4;

            uint k = (uint)a_data;
            k *= M;
            k ^= k >> R;
            k *= M;

            h *= M;
            h ^= k;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeLong(long a_data)
        {
            uint h = SEED ^ 8;

            uint k = (uint)a_data;
            k *= M;
            k ^= k >> R;
            k *= M;

            h *= M;
            h ^= k;

            k = (uint)(a_data >> 32);
            k *= M;
            k ^= k >> R;
            k *= M;

            h *= M;
            h ^= k;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeULong(ulong a_data)
        {
            uint h = SEED ^ 8;

            uint k = (uint)a_data;
            k *= M;
            k ^= k >> R;
            k *= M;

            h *= M;
            h ^= k;

            k = (uint)(a_data >> 32);
            k *= M;
            k ^= k >> R;
            k *= M;

            h *= M;
            h ^= k;

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeFloat(float a_data)
        {
            return ComputeBytes(Converters.ConvertFloatToBytes(a_data));
        }

        public override HashResult ComputeDouble(double a_data)
        {
            return ComputeBytes(Converters.ConvertDoubleToBytes(a_data));
        }

        public override HashResult ComputeString(string a_data)
        {
            int length = a_data.Length * 2;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k = (uint)a_data[currentIndex++] |
                         ((uint)a_data[currentIndex++] << 16);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }

            if (length == 2)
            {
                h ^= (uint)a_data[currentIndex];
                h *= M;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeChars(char[] a_data)
        {
            int length = a_data.Length * 2;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k = (uint)a_data[currentIndex++] | ((uint)a_data[currentIndex++] << 16);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }

            if (length == 2)
            {
                h ^= (uint)a_data[currentIndex];
                h *= M;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeShorts(short[] a_data)
        {
            int length = a_data.Length * 2;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k = (uint)((ushort)a_data[currentIndex++] | (ushort)a_data[currentIndex++] << 16);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }

            if (length == 2)
            {
                h ^= (ushort)a_data[currentIndex++];
                h *= M;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeUShorts(ushort[] a_data)
        {
            int length = a_data.Length * 2;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k = (uint)((ushort)a_data[currentIndex++] | (ushort)a_data[currentIndex++] << 16);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }

            if (length == 2)
            {
                h ^= (uint)(ushort)a_data[currentIndex++];
                h *= M;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeInts(int[] a_data)
        {
            int length = a_data.Length * 4;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k = (uint)a_data[currentIndex++];
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeUInts(uint[] a_data)
        {
            int length = a_data.Length * 4;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k = a_data[currentIndex++];
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;
                length -= 4;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeLongs(long[] a_data)
        {
            int length = a_data.Length * 8;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 8)
            {
                uint k = (uint)(a_data[currentIndex]);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;

                k = (uint)(a_data[currentIndex++] >> 32);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;

                length -= 8;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeULongs(ulong[] a_data)
        {
            int length = a_data.Length * 8;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 8)
            {
                uint k = (uint)(a_data[currentIndex]);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;

                k = (uint)(a_data[currentIndex++] >> 32);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;

                length -= 8;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeDoubles(double[] a_data)
        {
            int length = a_data.Length * 8;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 8)
            {
                long v = BitConverter.ToInt64(Converters.ConvertDoubleToBytes(a_data[currentIndex++]), 0);

                uint k = (uint)v;
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;

                k = (uint)(v >> 32);
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;

                length -= 8;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeFloats(float[] a_data)
        {
            int length = a_data.Length * 4;

            if (length == 0)
                return new HashResult((uint)0);

            uint h = SEED ^ (uint)length;
            int currentIndex = 0;

            while (length >= 4)
            {
                int v = BitConverter.ToInt32(Converters.ConvertFloatToBytes(a_data[currentIndex++]), 0);

                uint k = (uint)v;
                k *= M;
                k ^= k >> R;
                k *= M;

                h *= M;
                h ^= k;

                length -= 4;
            }

            h ^= h >> 13;
            h *= M;
            h ^= h >> 15;

            return new HashResult(h);
        }

        public override HashResult ComputeChar(char a_data, Encoding a_encoding)
        {
            return ComputeBytes(a_encoding.GetBytes(new char[] { a_data }));
        }

        public override HashResult ComputeString(string a_data, Encoding a_encoding)
        {
            return ComputeBytes(Converters.ConvertStringToBytes(a_data, a_encoding));
        }

        public override HashResult ComputeChars(char[] a_data, Encoding a_encoding)
        {
            return ComputeBytes(Converters.ConvertCharsToBytes(a_data, a_encoding));
        }
    }
}
