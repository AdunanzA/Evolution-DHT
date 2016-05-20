using System;
using System.Diagnostics;
using System.Text;

namespace HashLib.Hash32
{
    internal class DotNet : MultipleTransformNonBlock, IHash32, IFastHashCodes, INonBlockHash
    {
        public DotNet()
            : base(4, 8)
        {
        }

        public override HashResult ComputeBytes(byte[] a_data)
        {
            return new HashResult(Convert.ToBase64String(a_data).GetHashCode());
        }

        public override HashResult ComputeByte(byte a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeChar(char a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeShort(short a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeUShort(ushort a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeInt(int a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeUInt(uint a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeLong(long a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeULong(ulong a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeFloat(float a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeDouble(double a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeString(string a_data)
        {
            return new HashResult(a_data.GetHashCode());
        }

        public override HashResult ComputeChars(char[] a_data)
        {
            return ComputeBytes(Converters.ConvertCharsToBytes(a_data));
        }

        public override HashResult ComputeShorts(short[] a_data)
        {
            return ComputeBytes(Converters.ConvertShortsToBytes(a_data));
        }

        public override HashResult ComputeUShorts(ushort[] a_data)
        {
            return ComputeBytes(Converters.ConvertUShortsToBytes(a_data));
        }

        public override HashResult ComputeInts(int[] a_data)
        {
            return ComputeBytes(Converters.ConvertIntsToBytes(a_data));
        }

        public override HashResult ComputeUInts(uint[] a_data)
        {
            return ComputeBytes(Converters.ConvertUIntsToBytes(a_data));
        }

        public override HashResult ComputeLongs(long[] a_data)
        {
            return ComputeBytes(Converters.ConvertLongsToBytes(a_data));
        }

        public override HashResult ComputeULongs(ulong[] a_data)
        {
            return ComputeBytes(Converters.ConvertULongsToBytes(a_data));
        }

        public override HashResult ComputeDoubles(double[] a_data)
        {
            return ComputeBytes(Converters.ConvertDoublesToBytes(a_data));
        }

        public override HashResult ComputeFloats(float[] a_data)
        {
            return ComputeBytes(Converters.ConvertFloatsToBytes(a_data));
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
