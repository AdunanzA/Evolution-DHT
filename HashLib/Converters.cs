﻿using System;
using System.Diagnostics;
using System.Text;

namespace HashLib
{
    public static class Converters
    {
        public static byte[] ConvertToBytes(object a_in)
        {
            if (a_in is byte)
                return new byte[] {(byte)a_in };
            else if (a_in is short)
                return BitConverter.GetBytes((short)a_in);
            else if (a_in is ushort)
                return BitConverter.GetBytes((ushort)a_in);
            else if (a_in is char)
                return BitConverter.GetBytes((char)a_in);
            else if (a_in is int)
                return BitConverter.GetBytes((int)a_in);
            else if (a_in is uint)
                return BitConverter.GetBytes((uint)a_in);
            else if (a_in is long)
                return BitConverter.GetBytes((long)a_in);
            else if (a_in is ulong)
                return BitConverter.GetBytes((ulong)a_in);
            else if (a_in is float)
                return Converters.ConvertFloatToBytes((float)a_in);
            else if (a_in is double)
                return BitConverter.GetBytes((double)a_in);
            else if (a_in is string)
                return ConvertStringToBytes((string)a_in);
            else if (a_in is byte[])
                return (byte[])((byte[])a_in).Clone();
            else if (a_in is short[])
                return ConvertShortsToBytes((short[])a_in);
            else if (a_in is ushort[])
                return ConvertUShortsToBytes((ushort[])a_in);
            else if (a_in is char[])
                return ConvertCharsToBytes((char[])a_in);
            else if (a_in is int[])
                return ConvertIntsToBytes((int[])a_in);
            else if (a_in is uint[])
                return ConvertUIntsToBytes((uint[])a_in);
            else if (a_in is long[])
                return ConvertLongsToBytes((long[])a_in);
            else if (a_in is ulong[])
                return ConvertULongsToBytes((ulong[])a_in);
            else if (a_in is float[])
                return ConvertFloatsToBytes((float[])a_in);
            else if (a_in is double[])
                return ConvertDoublesToBytes((double[])a_in);
            else
                throw new ArgumentException();
        }

        public static uint[] ConvertBytesToUInts(byte[] a_in)
        {
            Check(a_in, 1, 4);

            uint[] result = new uint[a_in.Length / 4];
            ConvertBytesToUInts(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToUInts(byte[] a_in, uint[] a_result)
        {
            Check(a_in, 1, a_result, 4);

            ConvertBytesToUInts(a_in, 0, a_in.Length, a_result);
        } 

        public static uint[] ConvertBytesToUInts(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 4, a_index, a_length);

            uint[] result = new uint[a_length / 4];
            ConvertBytesToUInts(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToUInts(byte[] a_in, int a_index, int a_length, uint[] a_result)
        {
            Check(a_in, 1, a_result, 4, a_index, a_length);

            ConvertBytesToUInts(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToUInts(byte[] a_in, int a_index_in, int a_length, uint[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 4, a_index_in, a_length, a_index_out);

            Buffer.BlockCopy(a_in, a_index_in, a_result, a_index_out * 4, a_length);
        }

        public static int[] ConvertBytesToInts(byte[] a_in)
        {
            Check(a_in, 1, 4);

            int[] result = new int[a_in.Length / 4];
            ConvertBytesToInts(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToInts(byte[] a_in, int[] a_result)
        {
            Check(a_in, 1, a_result, 4);

            ConvertBytesToInts(a_in, 0, a_in.Length, a_result);
        }

        public static int[] ConvertBytesToInts(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 4, a_index, a_length);

            int[] result = new int[a_length / 4];
            ConvertBytesToInts(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToInts(byte[] a_in, int a_index, int a_length, int[] a_result)
        {
            Check(a_in, 1, a_result, 4, a_index, a_length);

            ConvertBytesToInts(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToInts(byte[] a_in, int a_index_in, int a_length, int[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 4, a_index_in, a_length, a_index_out);

            Buffer.BlockCopy(a_in, a_index_in, a_result, a_index_out * 4, a_length);
        }

        public static ulong[] ConvertBytesToULongs(byte[] a_in)
        {
            Check(a_in, 1, 8);

            ulong[] result = new ulong[a_in.Length / 8];
            ConvertBytesToULongs(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToULongs(byte[] a_in, ulong[] a_result)
        {
            Check(a_in, 1, a_result, 8);

            ConvertBytesToULongs(a_in, 0, a_in.Length, a_result);
        }

        public static ulong[] ConvertBytesToULongs(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 4, a_index, a_length);

            ulong[] result = new ulong[a_length / 8];
            ConvertBytesToULongs(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToULongs(byte[] a_in, int a_index, int a_length, ulong[] a_result)
        {
            Check(a_in, 1, a_result, 8, a_index, a_length);

            ConvertBytesToULongs(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToULongs(byte[] a_in, int a_index_in, int a_length, ulong[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 8, a_index_in, a_length, a_index_out);

            Buffer.BlockCopy(a_in, a_index_in, a_result, a_index_out * 8, a_length);
        }

        public static long[] ConvertBytesToLongs(byte[] a_in)
        {
            Check(a_in, 1, 8);

            long[] result = new long[a_in.Length / 8];
            ConvertBytesToLongs(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToLongs(byte[] a_in, long[] a_result)
        {
            Check(a_in, 1, a_result, 8);

            ConvertBytesToLongs(a_in, 0, a_in.Length, a_result);
        }

        public static long[] ConvertBytesToLongs(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 8, a_index, a_length);

            long[] result = new long[a_length / 8];
            ConvertBytesToLongs(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToLongs(byte[] a_in, int a_index, int a_length, long[] a_result)
        {
            Check(a_in, 1, a_result, 8, a_index, a_length);

            ConvertBytesToLongs(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToLongs(byte[] a_in, int a_index_in, int a_length, long[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 8, a_index_in, a_length, a_index_out);

            Buffer.BlockCopy(a_in, a_index_in, a_result, a_index_out * 8, a_length);
        }

        public static uint[] ConvertBytesToUIntsSwapOrder(byte[] a_in)
        {
            Check(a_in, 1, 4);

            uint[] result = new uint[a_in.Length / 4];
            ConvertBytesToUIntsSwapOrder(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToUIntsSwapOrder(byte[] a_in, uint[] a_result)
        {
            Check(a_in, 1, a_result, 4);

            ConvertBytesToUIntsSwapOrder(a_in, 0, a_in.Length, a_result);
        }

        public static uint[] ConvertBytesToUIntsSwapOrder(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 4, a_index, a_length);

            uint[] result = new uint[a_length / 4];
            ConvertBytesToUIntsSwapOrder(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToUIntsSwapOrder(byte[] a_in, int a_index, int a_length, uint[] a_result)
        {
            Check(a_in, 1, a_result, 4, a_index, a_length);

            ConvertBytesToUIntsSwapOrder(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToUIntsSwapOrder(byte[] a_in, int a_index, int a_length, uint[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 4, a_index, a_length, a_index_out);

            for (int i = a_index_out; a_length > 0; a_length -= 4)
            {
                a_result[i++] =
                    ((uint)a_in[a_index++] << 24) |
                    ((uint)a_in[a_index++] << 16) |
                    ((uint)a_in[a_index++] << 8) |
                    a_in[a_index++];
            }
        }

        public static int[] ConvertBytesToIntsSwapOrder(byte[] a_in)
        {
            Check(a_in, 1, 4);

            int[] result = new int[a_in.Length / 4];
            ConvertBytesToIntsSwapOrder(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToIntsSwapOrder(byte[] a_in, int[] a_result)
        {
            Check(a_in, 1, a_result, 4);

            ConvertBytesToIntsSwapOrder(a_in, 0, a_in.Length, a_result);
        }

        public static int[] ConvertBytesToIntsSwapOrder(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 4, a_index, a_length);

            int[] result = new int[a_length / 4];
            ConvertBytesToIntsSwapOrder(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToIntsSwapOrder(byte[] a_in, int a_index, int a_length, int[] a_result)
        {
            Check(a_in, 1, a_result, 4, a_index, a_length);

            ConvertBytesToIntsSwapOrder(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToIntsSwapOrder(byte[] a_in, int a_index, int a_length, int[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 4, a_index, a_length, a_index_out);

            for (int i = a_index_out; a_length > 0; a_length -= 4)
            {
                a_result[i++] =
                    ((int)a_in[a_index++] << 24) |
                    ((int)a_in[a_index++] << 16) |
                    ((int)a_in[a_index++] << 8) |
                    a_in[a_index++];
            }
        }

        public static ulong[] ConvertBytesToULongsSwapOrder(byte[] a_in)
        {
            Check(a_in, 1, 8);

            ulong[] result = new ulong[a_in.Length / 8];
            ConvertBytesToULongsSwapOrder(a_in, 0, a_in.Length, result);
            return result;
        }

        public static ulong ConvertBytesToULongSwapOrder(byte[] a_in, int a_index)
        {
            Debug.Assert(a_index >= 0);
            Debug.Assert(a_index + 8 <= a_in.Length);

            return ((ulong)a_in[a_index++] << 56) |
                   ((ulong)a_in[a_index++] << 48) |
                   ((ulong)a_in[a_index++] << 40) |
                   ((ulong)a_in[a_index++] << 32) |
                   ((ulong)a_in[a_index++] << 24) |
                   ((ulong)a_in[a_index++] << 16) |
                   ((ulong)a_in[a_index++] << 8) |
                   a_in[a_index];
        }

        public static ulong ConvertBytesToULong(byte[] a_in, int a_index)
        {
            Debug.Assert(a_index >= 0);
            Debug.Assert(a_index + 8 <= a_in.Length);

            return BitConverter.ToUInt64(a_in, a_index);
        }

        public static uint ConvertBytesToUIntSwapOrder(byte[] a_in, int a_index)
        {
            Debug.Assert(a_index >= 0);
            Debug.Assert(a_index + 4 <= a_in.Length);

            return ((uint)a_in[a_index++] << 24) | 
                   ((uint)a_in[a_index++] << 16) | 
                   ((uint)a_in[a_index++] << 8) | 
                   a_in[a_index];
        }

        public static uint ConvertBytesToUInt(byte[] a_in, int a_index)
        {
            Debug.Assert(a_index >= 0);
            Debug.Assert(a_index + 4 <= a_in.Length);

            return (uint)a_in[a_index++] |
                   ((uint)a_in[a_index++] << 8) |
                   ((uint)a_in[a_index++] << 16) |
                   ((uint)a_in[a_index] << 24);
        }

        public static void ConvertBytesToULongsSwapOrder(byte[] a_in, ulong[] a_result)
        {
            Check(a_in, 1, a_result, 8);

            ConvertBytesToULongsSwapOrder(a_in, 0, a_in.Length, a_result);
        }

        public static ulong[] ConvertBytesToULongsSwapOrder(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 8, a_index, a_length);

            ulong[] result = new ulong[a_length / 8];
            ConvertBytesToULongsSwapOrder(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToULongsSwapOrder(byte[] a_in, int a_index, int a_length, ulong[] a_result)
        {
            Check(a_in, 1, a_result, 8, a_index, a_length);

            ConvertBytesToULongsSwapOrder(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToULongsSwapOrder(byte[] a_in, int a_index_in, int a_length, ulong[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 8, a_index_in, a_length, a_index_out);

            for (int i = a_index_out; a_length > 0; a_length -= 8)
            {
                a_result[i++] =
                    (((ulong)a_in[a_index_in++] << 56) |
                    ((ulong)a_in[a_index_in++] << 48) |
                    ((ulong)a_in[a_index_in++] << 40) |
                    ((ulong)a_in[a_index_in++] << 32) |
                    ((ulong)a_in[a_index_in++] << 24) |
                    ((ulong)a_in[a_index_in++] << 16) |
                    ((ulong)a_in[a_index_in++] << 8) |
                    ((ulong)a_in[a_index_in++]));
            }
        }

        public static long[] ConvertBytesToLongsSwapOrder(byte[] a_in)
        {
            Check(a_in, 1, 8);

            long[] result = new long[a_in.Length / 8];
            ConvertBytesToLongsSwapOrder(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToLongsSwapOrder(byte[] a_in, long[] a_result)
        {
            Check(a_in, 1, a_result, 8);

            ConvertBytesToLongsSwapOrder(a_in, 0, a_in.Length, a_result);
        }

        public static long[] ConvertBytesToLongsSwapOrder(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 8, a_index, a_length);

            long[] result = new long[a_length / 8];
            ConvertBytesToLongsSwapOrder(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToLongsSwapOrder(byte[] a_in, int a_index, int a_length, long[] a_result)
        {
            Check(a_in, 1, a_result, 8, a_index, a_length);

            ConvertBytesToLongsSwapOrder(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToLongsSwapOrder(byte[] a_in, int a_index_in, int a_length, long[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 8, a_index_in, a_length, a_index_out);

            for (int i = a_index_out; a_length > 0; a_length -= 8)
            {
                a_result[i++] =
                    (((long)a_in[a_index_in++] << 56) |
                    ((long)a_in[a_index_in++] << 48) |
                    ((long)a_in[a_index_in++] << 40) |
                    ((long)a_in[a_index_in++] << 32) |
                    ((long)a_in[a_index_in++] << 24) |
                    ((long)a_in[a_index_in++] << 16) |
                    ((long)a_in[a_index_in++] << 8) |
                    ((long)a_in[a_index_in++]));
            }
        }

        public static byte[] ConvertStringToBytes(string a_in)
        {
            return ConvertStringToBytes(a_in, Encoding.Unicode);
        }

        public static byte[] ConvertStringToBytes(string a_in, Encoding a_encoding)
        {
            return a_encoding.GetBytes(a_in);
        }

        public static byte[] ConvertCharsToBytes(char[] a_in)
        {
            Check(a_in, 2, 1);

            byte[] result = new byte[a_in.Length * 2];
            Buffer.BlockCopy(a_in, 0, result, 0, result.Length);
            return result;
        }

        public static byte[] ConvertCharsToBytes(char[] a_in, Encoding a_encoding)
        {
            Check(a_in, 2, 1);

            return a_encoding.GetBytes(a_in);
        }

        public static byte[] ConvertShortsToBytes(short[] a_in)
        {
            Check(a_in, 2, 1);

            byte[] result = new byte[a_in.Length * 2];
            Buffer.BlockCopy(a_in, 0, result, 0, result.Length);
            return result;
        }

        public static byte[] ConvertUShortsToBytes(ushort[] a_in)
        {
            Check(a_in, 2, 1);

            byte[] result = new byte[a_in.Length * 2];
            Buffer.BlockCopy(a_in, 0, result, 0, result.Length);
            return result;
        }

        public static byte[] ConvertIntsToBytes(int[] a_in)
        {
            Check(a_in, 4, 1);

            return ConvertIntsToBytes(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertIntsToBytes(int[] a_in, int a_index, int a_length)
        {
            Check(a_in, 4, 1, a_index, a_length);

            byte[] result = new byte[a_length * 4];
            Buffer.BlockCopy(a_in, a_index * 4, result, 0, a_length * 4);
            return result;
        }

        public static byte[] ConvertIntsToBytesSwapOrder(int[] a_in)
        {
            Check(a_in, 4, 1);

            return ConvertIntsToBytesSwapOrder(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertIntsToBytesSwapOrder(int[] a_in, int a_index, int a_length)
        {
            Check(a_in, 4, 1, a_index, a_length);

            byte[] result = new byte[a_length * 4];

            for (int j = 0; a_length > 0; a_length--, a_index++)
            {
                result[j++] = (byte)(a_in[a_index] >> 24);
                result[j++] = (byte)(a_in[a_index] >> 16);
                result[j++] = (byte)(a_in[a_index] >> 8);
                result[j++] = (byte)a_in[a_index];
            }

            return result;
        }

        public static byte[] ConvertUIntsToBytes(uint[] a_in)
        {
            Check(a_in, 4, 1);

            return ConvertUIntsToBytes(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertUIntsToBytes(uint[] a_in, int a_index, int a_length)
        {
            Check(a_in, 4, 1, a_index, a_length);

            byte[] result = new byte[a_length * 4];
            Buffer.BlockCopy(a_in, a_index * 4, result, 0, a_length * 4);
            return result;
        }

        public static byte[] ConvertUIntsToBytesSwapOrder(uint[] a_in)
        {
            Check(a_in, 4, 1);

            return ConvertUIntsToBytesSwapOrder(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertUIntsToBytesSwapOrder(uint[] a_in, int a_index, int a_length)
        {
            Check(a_in, 4, 1, a_index, a_length);

            byte[] result = new byte[a_length * 4];

            for (int j=0; a_length > 0; a_length--, a_index++)
            {
                result[j++] = (byte)(a_in[a_index] >> 24);
                result[j++] = (byte)(a_in[a_index] >> 16);
                result[j++] = (byte)(a_in[a_index] >> 8);
                result[j++] = (byte)a_in[a_index];
            }

            return result;
        }

        public static byte[] ConvertLongsToBytes(long[] a_in)
        {
            Check(a_in, 8, 1);

            return ConvertLongsToBytes(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertLongsToBytes(long[] a_in, int a_index, int a_length)
        {
            Check(a_in, 8, 1, a_index, a_length);

            byte[] result = new byte[a_length * 8];
            Buffer.BlockCopy(a_in, a_index * 8, result, 0, a_length * 8);
            return result;
        }

        public static byte[] ConvertLongsToBytesSwapOrder(long[] a_in)
        {
            Check(a_in, 8, 1);

            return ConvertLongsToBytesSwapOrder(a_in, 0, a_in.Length);
        }

        public static ulong SwapOrder(ulong a_ulong)
        {
            return
                (a_ulong >> 56) |
                ((a_ulong & 0x00FF000000000000) >> 40) |
                ((a_ulong & 0x0000FF0000000000) >> 24) |
                ((a_ulong & 0x000000FF00000000) >> 8) |
                ((a_ulong & 0x00000000FF000000) << 8) |
                ((a_ulong & 0x0000000000FF0000) << 24) |
                ((a_ulong & 0x000000000000FF00) << 40) |
                (a_ulong << 56);
        }

        public static long SwapOrder(long a_long)
        {
            return (long)SwapOrder((ulong)a_long);
        }

        public static uint SwapOrder(uint a_int)
        {
            return
                (a_int >> 24) |
                ((a_int & 0x00FF0000) >> 8) |
                ((a_int & 0x0000FF00) << 8) |
                (a_int << 24);
        }

        public static int SwapOrder(int a_int)
        {
            return (int)SwapOrder((uint)a_int);
        }

        public static byte[] ConvertLongsToBytesSwapOrder(long[] a_in, int a_index, int a_length)
        {
            Check(a_in, 8, 1, a_index, a_length);

            byte[] result = new byte[a_length * 8];

            for (int j = 0; a_length > 0; a_length--, a_index++)
            {
                result[j++] = (byte)(a_in[a_index] >> 56);
                result[j++] = (byte)(a_in[a_index] >> 48);
                result[j++] = (byte)(a_in[a_index] >> 40);
                result[j++] = (byte)(a_in[a_index] >> 32);
                result[j++] = (byte)(a_in[a_index] >> 24);
                result[j++] = (byte)(a_in[a_index] >> 16);
                result[j++] = (byte)(a_in[a_index] >> 8);
                result[j++] = (byte)a_in[a_index];
            }

            return result;
        }

        public static byte[] ConvertULongsToBytes(ulong[] a_in)
        {
            Check(a_in, 8, 1);

            return ConvertULongsToBytes(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertULongsToBytes(ulong[] a_in, int a_index, int a_length)
        {
            Check(a_in, 8, 1, a_index, a_length);

            byte[] result = new byte[a_length * 8];
            ConvertULongsToBytes(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertULongsToBytes(ulong[] a_in, int a_index, int a_length, byte[] a_result)
        {
            Check(a_in, 8, a_result, 1, a_index, a_length);

            Buffer.BlockCopy(a_in, a_index * 8, a_result, 0, a_length * 8);
        }

        public static byte[] ConvertULongsToBytesSwapOrder(ulong[] a_in)
        {
            Check(a_in, 8, 1);
            return ConvertULongsToBytesSwapOrder(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertULongsToBytesSwapOrder(ulong[] a_in, int a_index, int a_length)
        {
            Check(a_in, 8, 1, a_index, a_length);

            byte[] result = new byte[a_length * 8];

            for (int j = 0; a_length > 0; a_length--, a_index++)
            {
                result[j++] = (byte)(a_in[a_index] >> 56);
                result[j++] = (byte)(a_in[a_index] >> 48);
                result[j++] = (byte)(a_in[a_index] >> 40);
                result[j++] = (byte)(a_in[a_index] >> 32);
                result[j++] = (byte)(a_in[a_index] >> 24);
                result[j++] = (byte)(a_in[a_index] >> 16);
                result[j++] = (byte)(a_in[a_index] >> 8);
                result[j++] = (byte)a_in[a_index];
            }

            return result;
        }

        public static byte[] ConvertDoublesToBytes(double[] a_in)
        {
            Check(a_in, 8, 1);

            return ConvertDoublesToBytes(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertDoublesToBytes(double[] a_in, int a_index, int a_length)
        {
            Check(a_in, 8, 1, a_index, a_length);

            byte[] result = new byte[a_length * 8];
            Buffer.BlockCopy(a_in, a_index * 8, result, 0, a_length * 8);
            return result;
        }

        public static byte[] ConvertFloatsToBytes(float[] a_in)
        {
            Check(a_in, 4, 1);

            return ConvertFloatsToBytes(a_in, 0, a_in.Length);
        }

        public static byte[] ConvertFloatsToBytes(float[] a_in, int a_index, int a_length)
        {
            Check(a_in, 4, 1, a_index, a_length);

            byte[] result = new byte[a_length * 4];
            Buffer.BlockCopy(a_in, a_index * 4, result, 0, a_length * 4);
            return result;
        }

        public static string ConvertBytesToHexString(byte[] a_in, bool a_group = true)
        {
            Check(a_in, 1, 4);

            string hex = BitConverter.ToString(a_in).ToUpper();

            if (a_group)
            {
                string[] ar = BitConverter.ToString(a_in).ToUpper().Split(new char[] { '-' });

                hex = "";

                for (int i = 0; i < ar.Length / 4; i++)
                {
                    if (i != 0)
                        hex += "-";
                    hex += ar[i * 4] + ar[i * 4 + 1] + ar[i * 4 + 2] + ar[i * 4 + 3];
                }
            }
            else
                hex = hex.Replace("-", "");

            return hex;
        }

        public static byte[] ConvertHexStringToBytes(string a_in)
        {
            a_in = a_in.Replace("-", "");

            Debug.Assert(a_in.Length % 2 == 0);

            byte[] result = new byte[a_in.Length / 2];

            for (int i = 0; i < result.Length; i++)
                result[i] = Byte.Parse(a_in.Substring(i*2, 2), System.Globalization.NumberStyles.HexNumber);

            return result;
        }

        public static char[] ConvertBytesToChars(byte[] a_in)
        {
            Check(a_in, 1, 2);

            char[] result = new char[a_in.Length / 2];
            Buffer.BlockCopy(a_in, 0, result, 0, a_in.Length);
            return result;
        }

        public static short[] ConvertBytesToShorts(byte[] a_in)
        {
            Check(a_in, 1, 2);

            short[] result = new short[a_in.Length / 2];
            Buffer.BlockCopy(a_in, 0, result, 0, a_in.Length);
            return result;
        }

        public static ushort[] ConvertBytesToUShorts(byte[] a_in)
        {
            Check(a_in, 1, 2);

            ushort[] result = new ushort[a_in.Length / 2];
            Buffer.BlockCopy(a_in, 0, result, 0, a_in.Length);
            return result;
        }

        public static float[] ConvertBytesToFloats(byte[] a_in)
        {
            Check(a_in, 1, 4);

            float[] result = new float[a_in.Length / 4];
            ConvertBytesToFloats(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToFloats(byte[] a_in, float[] a_result)
        {
            Check(a_in, 1, a_result, 4);

            ConvertBytesToFloats(a_in, 0, a_in.Length, a_result);
        }

        public static float[] ConvertBytesToFloats(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 4, a_index, a_length);

            float[] result = new float[a_length / 4];
            ConvertBytesToFloats(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToFloats(byte[] a_in, int a_index, int a_length, float[] a_result)
        {
            Check(a_in, 1, a_result, 4, a_index, a_length);

            ConvertBytesToFloats(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToFloats(byte[] a_in, int a_index_in, int a_length, float[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 4, a_index_in, a_length, a_index_out);

            Buffer.BlockCopy(a_in, a_index_in, a_result, a_index_out * 4, a_length);
        }

        public static double ConvertBytesToDouble(byte[] a_in, int a_index = 0)
        {
            Debug.Assert(a_index + 8 <= a_in.Length);

            return BitConverter.ToDouble(a_in, a_index);
        }

        public static double[] ConvertBytesToDoubles(byte[] a_in)
        {
            Check(a_in, 1, 8);

            double[] result = new double[a_in.Length / 8];
            ConvertBytesToDoubles(a_in, 0, a_in.Length, result);
            return result;
        }

        public static void ConvertBytesToDoubles(byte[] a_in, double[] a_result)
        {
            Check(a_in, 1, a_result, 8);

            ConvertBytesToDoubles(a_in, 0, a_in.Length, a_result);
        }

        public static double[] ConvertBytesToDoubles(byte[] a_in, int a_index, int a_length)
        {
            Check(a_in, 1, 8, a_index, a_length);

            double[] result = new double[a_length / 8];
            ConvertBytesToDoubles(a_in, a_index, a_length, result);
            return result;
        }

        public static void ConvertBytesToDoubles(byte[] a_in, int a_index, int a_length, double[] a_result)
        {
            Check(a_in, 1, a_result, 8, a_index, a_length);

            ConvertBytesToDoubles(a_in, a_index, a_length, a_result, 0);
        }

        public static void ConvertBytesToDoubles(byte[] a_in, int a_index_in, int a_length, double[] a_result, int a_index_out)
        {
            Check(a_in, 1, a_result, 8, a_index_in, a_length, a_index_out);

            Buffer.BlockCopy(a_in, a_index_in, a_result, a_index_out * 8, a_length);
        }

        [Conditional("DEBUG")]
        private static void Check<I>(I[] a_in, int a_in_size, int a_out_size)
        {
            Debug.Assert((a_in.Length % a_out_size) == 0);
        }

        [Conditional("DEBUG")]
        private static void Check<I, O>(I[] a_in, int a_in_size, O[] a_result, int a_out_size)
        {
            Debug.Assert((a_in.Length % a_out_size) == 0);
            Debug.Assert(a_result.Length >= (a_in.Length / a_out_size)); 
        }

        [Conditional("DEBUG")]
        private static void Check<I>(I[] a_in, int a_in_size, int a_out_size, int a_index, int a_length)
        {
            Debug.Assert((a_length % a_out_size) == 0);
            Debug.Assert(a_index >= 0);
            if (a_length > 0)
                Debug.Assert(a_index < a_in.Length);
            Debug.Assert(a_length >= 0);
            Debug.Assert(a_index + a_length <= a_in.Length);
        }

        [Conditional("DEBUG")]
        private static void Check<I, O>(I[] a_in, int a_in_size, O[] a_result, int a_out_size, int a_index, int a_length)
        {
            Debug.Assert(a_index >= 0);
            if (a_length > 0)
                Debug.Assert(a_index < a_in.Length);
            Debug.Assert(a_length >= 0);
            Debug.Assert(a_index + a_length <= a_in.Length);
            Debug.Assert((a_length % a_out_size) == 0);
            Debug.Assert(a_result.Length >= (a_length / a_out_size)); 
        }

        [Conditional("DEBUG")]
        private static void Check<I, O>(I[] a_in, int a_in_size, O[] a_result, int a_out_size, int a_index_in, int a_length, int a_index_out)
        {
            Debug.Assert(a_index_in >= 0);
            if (a_length > 0)
                Debug.Assert(a_index_in < a_in.Length);
            Debug.Assert(a_length >= 0);
            Debug.Assert(a_index_in + a_length <= a_in.Length);
            Debug.Assert((a_length % a_out_size) == 0);
            Debug.Assert(a_index_out + a_result.Length >= (a_length / a_out_size));
        }

        public static float ConvertBytesToFloat(byte[] a_in, int a_index = 0)
        {
            Debug.Assert(a_index + 4 <= a_in.Length);

            return BitConverter.ToSingle(a_in, a_index);
        }

        public static byte[] ConvertFloatToBytes(float a_in)
        {
            return BitConverter.GetBytes(a_in);
        }

        public static void ConvertFloatToBytes(float a_in, byte[] a_result, int a_index)
        {
            Debug.Assert(a_index + 4 <= a_result.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_result, a_index, 4);
        }

        public static void ConvertCharToBytes(char a_in, byte[] a_result, int a_index)
        {
            Debug.Assert(a_index + 2 <= a_result.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_result, a_index, 2);
        }

        public static void ConvertShortToBytes(short a_in, byte[] a_result, int a_index)
        {
            Debug.Assert(a_index + 2 <= a_result.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_result, a_index, 2);
        }

        public static void ConvertUShortToBytes(ushort a_in, byte[] a_result, int a_index)
        {
            Debug.Assert(a_index + 2 <= a_result.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_result, a_index, 2);
        }

        public static void ConvertIntToBytes(int a_in, byte[] a_result, int a_index)
        {
            Debug.Assert(a_index + 4 <= a_result.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_result, a_index, 4);
        }

        public static void ConvertUIntToBytes(uint a_in, byte[] a_result, int a_index)
        {
            Debug.Assert(a_index + 4 <= a_result.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_result, a_index, 4);
        }

        public static void ConvertLongToBytes(long a_in, byte[] a_out, int a_index)
        {
            Debug.Assert(a_index + 8 <= a_out.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_out, a_index, 8);
        }

        public static void ConvertULongToBytes(ulong a_in, byte[] a_out, int a_index)
        {
            Debug.Assert(a_index + 8 <= a_out.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_out, a_index, 8);
        }

        public static void ConvertULongToBytesSwapOrder(ulong a_in, byte[] a_out, int a_index)
        {
            Debug.Assert(a_index + 8 <= a_out.Length);

            a_out[a_index++] = (byte)(a_in >> 56);
            a_out[a_index++] = (byte)(a_in >> 48);
            a_out[a_index++] = (byte)(a_in >> 40);
            a_out[a_index++] = (byte)(a_in >> 32);
            a_out[a_index++] = (byte)(a_in >> 24);
            a_out[a_index++] = (byte)(a_in >> 16);
            a_out[a_index++] = (byte)(a_in >> 8);
            a_out[a_index++] = (byte)a_in;
        }

        public static byte[] ConvertDoubleToBytes(double a_in)
        {
            return BitConverter.GetBytes(a_in);
        }

        public static void ConvertDoubleToBytes(double a_in, byte[] a_result, int a_index)
        {
            Debug.Assert(a_index + 8 <= a_result.Length);

            Array.Copy(BitConverter.GetBytes(a_in), 0, a_result, a_index, 8);
        }
    }
}
