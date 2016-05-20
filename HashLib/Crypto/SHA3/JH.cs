
using System;

namespace HashLib.Crypto.SHA3
{
    internal class JH224 : JHBase
    {
        public JH224() :
            base(HashLib.HashSize.HashSize224)
        {
        }
    }

    internal class JH256 : JHBase
    {
        public JH256() :
            base(HashLib.HashSize.HashSize256)
        {
        }
    }

    internal class JH384 : JHBase
    {
        public JH384() :
            base(HashLib.HashSize.HashSize384)
        {
        }
    }

    internal class JH512 : JHBase
    {
        public JH512() :
            base(HashLib.HashSize.HashSize512)
        {
        }
    }

    internal abstract class JHBase : HashCryptoNotBuildIn
    {
        #region Consts
        private static readonly ulong[] s_bitslices =
        {
            0x67F815DFA2DED572, 0x571523B70A15847B, 0xF6875A4D90D6AB81, 0x402BD1C3C54F9F4E,
            0x9CFA455CE03A98EA, 0x9A99B26699D2C503, 0x8A53BBF2B4960266, 0x31A2DB881A1456B5,
            0xDB0E199A5C5AA303, 0x1044C1870AB23F40, 0x1D959E848019051C, 0xDCCDE75EADEB336F,
            0x416BBF029213BA10, 0xD027BBF7156578DC, 0x5078AA3739812C0A, 0xD3910041D2BF1A3F,
            0x907ECCF60D5A2D42, 0xCE97C0929C9F62DD, 0xAC442BC70BA75C18, 0x23FCC663D665DFD1,
            0x1AB8E09E036C6E97, 0xA8EC6C447E450521, 0xFA618E5DBB03F1EE, 0x97818394B29796FD,
            0x2F3003DB37858E4A, 0x956A9FFB2D8D672A, 0x6C69B8F88173FE8A, 0x14427FC04672C78A,
            0xC45EC7BD8F15F4C5, 0x80BB118FA76F4475, 0xBC88E4AEB775DE52, 0xF4A3A6981E00B882,
            0x1563A3A9338FF48E, 0x89F9B7D524565FAA, 0xFDE05A7C20EDF1B6, 0x362C42065AE9CA36,
            0x3D98FE4E433529CE, 0xA74B9A7374F93A53, 0x86814E6F591FF5D0, 0x9F5AD8AF81AD9D0E,
            0x6A6234EE670605A7, 0x2717B96EBE280B8B, 0x3F1080C626077447, 0x7B487EC66F7EA0E0,
            0xC0A4F84AA50A550D, 0x9EF18E979FE7E391, 0xD48D605081727686, 0x62B0E5F3415A9E7E,
            0x7A205440EC1F9FFC, 0x84C9F4CE001AE4E3, 0xD895FA9DF594D74F, 0xA554C324117E2E55,
            0x286EFEBD2872DF5B, 0xB2C4A50FE27FF578, 0x2ED349EEEF7C8905, 0x7F5928EB85937E44,
            0x4A3124B337695F70, 0x65E4D61DF128865E, 0xE720B95104771BC7, 0x8A87D423E843FE74,
            0xF2947692A3E8297D, 0xC1D9309B097ACBDD, 0xE01BDC5BFB301B1D, 0xBF829CF24F4924DA,
            0xFFBF70B431BAE7A4, 0x48BCF8DE0544320D, 0x39D3BB5332FCAE3B, 0xA08B29E0C1C39F45,
            0x0F09AEF7FD05C9E5, 0x34F1904212347094, 0x95ED44E301B771A2, 0x4A982F4F368E3BE9,
            0x15F66CA0631D4088, 0xFFAF52874B44C147, 0x30C60AE2F14ABB7E, 0xE68C6ECCC5B67046,
            0x00CA4FBD56A4D5A4, 0xAE183EC84B849DDA, 0xADD1643045CE5773, 0x67255C1468CEA6E8,
            0x16E10ECBF28CDAA3, 0x9A99949A5806E933, 0x7B846FC220B2601F, 0x1885D1A07FACCED1,
            0xD319DD8DA15B5932, 0x46B4A5AAC01C9A50, 0xBA6B04E467633D9F, 0x7EEE560BAB19CAF6,
            0x742128A9EA79B11F, 0xEE51363B35F7BDE9, 0x76D350755AAC571D, 0x01707DA3FEC2463A,
            0x42D8A498AFC135F7, 0x79676B9E20ECED78, 0xA8DB3AEA15638341, 0x832C83324D3BC3FA,
            0xF347271C1F3B40A7, 0x9A762DB734F04059, 0xFD4F21D26C4E3EE7, 0xEF5957DC398DFDB8,
            0xDAEB492B490C9B8D, 0x0D70F36849D7A25B, 0x84558D7AD0AE3B7D, 0x658EF8E4F0E9A5F5,
            0x533B1036F4A2B8A0, 0x5AEC3E759E07A80C, 0x4F88E85692946891, 0x4CBCBAF8555CB05B,
            0x7B9487F3993BBBE3, 0x5D1C6B72D6F4DA75, 0x6DB334DC28ACAE64, 0x71DB28B850A5346C,
            0x2A518D10F2E261F8, 0xFC75DD593364DBE3, 0xA23FCE43F1BCAC1C, 0xB043E8023CD1BB67,
            0x75A12988CA5B0A33, 0x5C5316B44D19347F, 0x1E4D790EC3943B92, 0x3FAFEEB6D7757479,
            0x21391ABEF7D4A8EA, 0x5127234C097EF45C, 0xD23C32BA5324A326, 0xADD5A66D4A17A344,
            0x08C9F2AFA63E1DB5, 0x563C6B91983D5983, 0x4D608672A17CF84C, 0xF6C76E08CC3EE246,
            0x5E76BCB1B333982F, 0x2AE6C4EFA566D62B, 0x36D4C1BEE8B6F406, 0x6321EFBC1582EE74,
            0x69C953F40D4EC1FD, 0x26585806C45A7DA7, 0x16FAE0061614C17E, 0x3F9D63283DAF907E,
            0x0CD29B00E3F2C9D2, 0x300CD4B730CEAA5F, 0x9832E0F216512A74, 0x9AF8CEE3D830EB0D,
            0x9279F1B57B9EC54B, 0xD36886046EE651FF, 0x316796E6574D239B, 0x05750A17F3A6E6CC 
        };

        private static readonly ulong[][] initial_states = 
        {
            new ulong [] 
            { 
                0x3002ed0Be070c282, 0xb134ce319e3a0C8d, 0x87cd46ba2f940C8f, 0xc47179fc0Ad8c41e, 
                0x7b2d9669bb1ae061, 0x97863de13d8971af, 0x94c0c9f7600452d2, 0x9c79a53dca4963c7, 
                0x9febbcbd1f558bfd, 0xbff842b45bbd3408, 0x9e99c7b9355c51ba, 0xb313cc71624ea455, 
                0x25f785c193577285, 0xd2255000696b3645, 0xdf1edd27dbeb9033, 0xe93d607ee1adbacc 
            }, 
            new ulong [] 
            { 
                0x6e593ac5e2b868c9, 0xe5e67a1def457e42, 0x7a1f7106d9b74561, 0x0122a9067861c72f, 
                0xe22919b9c191297b, 0xd6a2c58ce14c2bc4, 0xdf5d1b90cabe2062, 0x5faca78e6305b2d3, 
                0x0431316dba8c3e14, 0x717252905400e7b0, 0x10e55d071e32ce4c, 0x785102e2ec00a81b, 
                0xa504d15f7972579f, 0x38b2f52534b6b8f0, 0x177f905f3efa7016, 0x90ac69e764c08fe2 
            }, 
            new ulong [] 
            { 
                0x402dab64ab239c07, 0x8de9de47e41cb58c, 0x6952c27e62b19b8d, 0x80fc2f002b2db6ba, 
                0x3a178c30efbcafcb, 0x31401931aaa36fad, 0xe34c6f3a42778989, 0x7ddb0D442b732ebf, 
                0xe5543aa6ca3ec4f2, 0xc52244fc0Ab8378a, 0x91e0e904bcc397a3, 0xfa6048e15304a837, 
                0xa6bed45f3ad33171, 0x128533f4f84adadc, 0xd05849c8f4f8c76e, 0xa9b69546a3949e8b 
            }, 
            new ulong [] 
            { 
                0xcc4209c65860ab50, 0x1bdcb9bd4ca5e74c, 0x245ea1d1fb7a2eaf, 0xa1c0d5c4ab4ef4e5, 
                0x7320560C6643f24c, 0x183d8b9aea819399, 0xc7b640a9fcd965cf, 0x663bfebe7312839e, 
                0xe0d8320A7e2f9a0F, 0x40130B8e5591d417, 0x3f5f4ec4dee4b505, 0x321dfd98ee5abc8c, 
                0xc4e66ce4251c0814, 0xdb43bde1bc954b1b, 0x1480b643c29e227f, 0x03033c3309b9330A 
            }
        };
        #endregion

        protected readonly ulong[] m_state = new ulong[16];

        public JHBase(HashSize a_hashSize) :
            base((int)a_hashSize, 128, 64)
        {
            Initialize();
        }

        protected override void TransformBlock(byte[] a_data, int a_index)
        {
            ulong[] data = new ulong[8];
            Converters.ConvertBytesToULongs(a_data, a_index, 64, data);

            const ulong c_2_1 = 0x5555555555555555;
            const ulong c_2_2 = 0xaaaaaaaaaaaaaaaa;
            const ulong c_4_1 = 0x3333333333333333;
            const ulong c_4_2 = 0xcccccccccccccccc;
            const ulong c_8_1 = 0x0f0f0f0f0f0f0f0f;
            const ulong c_8_2 = 0xf0f0f0f0f0f0f0f0;
            const ulong c_16_1 = 0x00ff00ff00ff00ff;
            const ulong c_16_2 = 0xff00ff00ff00ff00;
            const ulong c_32_1 = 0x0000ffff0000ffff;
            const ulong c_32_2 = 0xffff0000ffff0000;

            ulong m0 = m_state[0] ^ data[0];
            ulong m1 = m_state[1] ^ data[1];
            ulong m2 = m_state[2] ^ data[2];
            ulong m3 = m_state[3] ^ data[3];
            ulong m4 = m_state[4] ^ data[4];
            ulong m5 = m_state[5] ^ data[5];
            ulong m6 = m_state[6] ^ data[6];
            ulong m7 = m_state[7] ^ data[7];

            ulong m8 = m_state[8];
            ulong m9 = m_state[9];
            ulong m10 = m_state[10];
            ulong m11 = m_state[11];
            ulong m12 = m_state[12];
            ulong m13 = m_state[13];
            ulong m14 = m_state[14];
            ulong m15 = m_state[15];

            for (int r = 0; r < 140; r += 28) 
            {
                int r1 = r;
                int r2 = r + 2;

                m12 = ~m12;
                m14 = ~m14;
                m0 ^= ~m8 & s_bitslices[r1];
                m2 ^= ~m10 & s_bitslices[r2];
                ulong temp0 = s_bitslices[r1++] ^ (m0 & m4);
                ulong temp1 = s_bitslices[r2++] ^ (m2 & m6);
                m0 ^= m8 & m12;
                m2 ^= m10 & m14;
                m12 ^= ~m4 & m8;
                m14 ^= ~m6 & m10;
                m4 ^= m0 & m8;
                m6 ^= m2 & m10;
                m8 ^= m0 & ~m12;
                m10 ^= m2 & ~m14;
                m0 ^= m4 | m12;
                m2 ^= m6 | m14;
                m12 ^= m4 & m8;
                m14 ^= m6 & m10;
                m4 ^= temp0 & m0;
                m6 ^= temp1 & m2;
                m8 ^= temp0;
                m10 ^= temp1;

                m2 ^= m4;
                m6 ^= m8;
                m10 ^= m0 ^ m12;
                m14 ^= m0;
                m0 ^= m6;
                m4 ^= m10;
                m8 ^= m2 ^ m14;
                m12 ^= m2;

                m2 = ((m2 & c_2_1) << 1) | ((m2 & c_2_2) >> 1);
                m6 = ((m6 & c_2_1) << 1) | ((m6 & c_2_2) >> 1);
                m10 = ((m10 & c_2_1) << 1) | ((m10 & c_2_2) >> 1);
                m14 = ((m14 & c_2_1) << 1) | ((m14 & c_2_2) >> 1);

                m13 = ~m13;
                m15 = ~m15;
                m1 ^= ~m9 & s_bitslices[r1];
                m3 ^= ~m11 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m1 & m5);
                temp1 = s_bitslices[r2++] ^ (m3 & m7);
                m1 ^= m9 & m13;
                m3 ^= m11 & m15;
                m13 ^= ~m5 & m9;
                m15 ^= ~m7 & m11;
                m5 ^= m1 & m9;
                m7 ^= m3 & m11;
                m9 ^= m1 & ~m13;
                m11 ^= m3 & ~m15;
                m1 ^= m5 | m13;
                m3 ^= m7 | m15;
                m13 ^= m5 & m9;
                m15 ^= m7 & m11;
                m5 ^= temp0 & m1;
                m7 ^= temp1 & m3;
                m9 ^= temp0;
                m11 ^= temp1;

                m3 ^= m5;
                m7 ^= m9;
                m11 ^= m1 ^ m13;
                m15 ^= m1;
                m1 ^= m7;
                m5 ^= m11;
                m9 ^= m3 ^ m15;
                m13 ^= m3;

                m3 = ((m3 & c_2_1) << 1) | ((m3 & c_2_2) >> 1);
                m7 = ((m7 & c_2_1) << 1) | ((m7 & c_2_2) >> 1);
                m11 = ((m11 & c_2_1) << 1) | ((m11 & c_2_2) >> 1);
                m15 = ((m15 & c_2_1) << 1) | ((m15 & c_2_2) >> 1);

                r1 += 2;
                r2 += 2;

                m12 = ~m12;
                m14 = ~m14;
                m0 ^= ~m8 & s_bitslices[r1];
                m2 ^= ~m10 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m0 & m4);
                temp1 = s_bitslices[r2++] ^ (m2 & m6);
                m0 ^= m8 & m12;
                m2 ^= m10 & m14;
                m12 ^= ~m4 & m8;
                m14 ^= ~m6 & m10;
                m4 ^= m0 & m8;
                m6 ^= m2 & m10;
                m8 ^= m0 & ~m12;
                m10 ^= m2 & ~m14;
                m0 ^= m4 | m12;
                m2 ^= m6 | m14;
                m12 ^= m4 & m8;
                m14 ^= m6 & m10;
                m4 ^= temp0 & m0;
                m6 ^= temp1 & m2;
                m8 ^= temp0;
                m10 ^= temp1;

                m2 ^= m4;
                m6 ^= m8;
                m10 ^= m0 ^ m12;
                m14 ^= m0;
                m0 ^= m6;
                m4 ^= m10;
                m8 ^= m2 ^ m14;
                m12 ^= m2;

                m2 = ((m2 & c_4_1) << 2) | ((m2 & c_4_2) >> 2);
                m6 = ((m6 & c_4_1) << 2) | ((m6 & c_4_2) >> 2);
                m10 = ((m10 & c_4_1) << 2) | ((m10 & c_4_2) >> 2);
                m14 = ((m14 & c_4_1) << 2) | ((m14 & c_4_2) >> 2);

                m13 = ~m13;
                m15 = ~m15;
                m1 ^= ~m9 & s_bitslices[r1];
                m3 ^= ~m11 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m1 & m5);
                temp1 = s_bitslices[r2++] ^ (m3 & m7);
                m1 ^= m9 & m13;
                m3 ^= m11 & m15;
                m13 ^= ~m5 & m9;
                m15 ^= ~m7 & m11;
                m5 ^= m1 & m9;
                m7 ^= m3 & m11;
                m9 ^= m1 & ~m13;
                m11 ^= m3 & ~m15;
                m1 ^= m5 | m13;
                m3 ^= m7 | m15;
                m13 ^= m5 & m9;
                m15 ^= m7 & m11;
                m5 ^= temp0 & m1;
                m7 ^= temp1 & m3;
                m9 ^= temp0;
                m11 ^= temp1;

                m3 ^= m5;
                m7 ^= m9;
                m11 ^= m1 ^ m13;
                m15 ^= m1;
                m1 ^= m7;
                m5 ^= m11;
                m9 ^= m3 ^ m15;
                m13 ^= m3;

                m3 = ((m3 & c_4_1) << 2) | ((m3 & c_4_2) >> 2);
                m7 = ((m7 & c_4_1) << 2) | ((m7 & c_4_2) >> 2);
                m11 = ((m11 & c_4_1) << 2) | ((m11 & c_4_2) >> 2);
                m15 = ((m15 & c_4_1) << 2) | ((m15 & c_4_2) >> 2);

                r1 += 2;
                r2 += 2;

                m12 = ~m12;
                m14 = ~m14;
                m0 ^= ~m8 & s_bitslices[r1];
                m2 ^= ~m10 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m0 & m4);
                temp1 = s_bitslices[r2++] ^ (m2 & m6);
                m0 ^= m8 & m12;
                m2 ^= m10 & m14;
                m12 ^= ~m4 & m8;
                m14 ^= ~m6 & m10;
                m4 ^= m0 & m8;
                m6 ^= m2 & m10;
                m8 ^= m0 & ~m12;
                m10 ^= m2 & ~m14;
                m0 ^= m4 | m12;
                m2 ^= m6 | m14;
                m12 ^= m4 & m8;
                m14 ^= m6 & m10;
                m4 ^= temp0 & m0;
                m6 ^= temp1 & m2;
                m8 ^= temp0;
                m10 ^= temp1;

                m2 ^= m4;
                m6 ^= m8;
                m10 ^= m0 ^ m12;
                m14 ^= m0;
                m0 ^= m6;
                m4 ^= m10;
                m8 ^= m2 ^ m14;
                m12 ^= m2;

                m2 = ((m2 & c_8_1) << 4) | ((m2 & c_8_2) >> 4);
                m6 = ((m6 & c_8_1) << 4) | ((m6 & c_8_2) >> 4);
                m10 = ((m10 & c_8_1) << 4) | ((m10 & c_8_2) >> 4);
                m14 = ((m14 & c_8_1) << 4) | ((m14 & c_8_2) >> 4);

                m13 = ~m13;
                m15 = ~m15;
                m1 ^= ~m9 & s_bitslices[r1];
                m3 ^= ~m11 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m1 & m5);
                temp1 = s_bitslices[r2++] ^ (m3 & m7);
                m1 ^= m9 & m13;
                m3 ^= m11 & m15;
                m13 ^= ~m5 & m9;
                m15 ^= ~m7 & m11;
                m5 ^= m1 & m9;
                m7 ^= m3 & m11;
                m9 ^= m1 & ~m13;
                m11 ^= m3 & ~m15;
                m1 ^= m5 | m13;
                m3 ^= m7 | m15;
                m13 ^= m5 & m9;
                m15 ^= m7 & m11;
                m5 ^= temp0 & m1;
                m7 ^= temp1 & m3;
                m9 ^= temp0;
                m11 ^= temp1;

                m3 ^= m5;
                m7 ^= m9;
                m11 ^= m1 ^ m13;
                m15 ^= m1;
                m1 ^= m7;
                m5 ^= m11;
                m9 ^= m3 ^ m15;
                m13 ^= m3;

                m3 = ((m3 & c_8_1) << 4) | ((m3 & c_8_2) >> 4);
                m7 = ((m7 & c_8_1) << 4) | ((m7 & c_8_2) >> 4);
                m11 = ((m11 & c_8_1) << 4) | ((m11 & c_8_2) >> 4);
                m15 = ((m15 & c_8_1) << 4) | ((m15 & c_8_2) >> 4);

                r1 += 2;
                r2 += 2;

                m12 = ~m12;
                m14 = ~m14;
                m0 ^= ~m8 & s_bitslices[r1];
                m2 ^= ~m10 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m0 & m4);
                temp1 = s_bitslices[r2++] ^ (m2 & m6);
                m0 ^= m8 & m12;
                m2 ^= m10 & m14;
                m12 ^= ~m4 & m8;
                m14 ^= ~m6 & m10;
                m4 ^= m0 & m8;
                m6 ^= m2 & m10;
                m8 ^= m0 & ~m12;
                m10 ^= m2 & ~m14;
                m0 ^= m4 | m12;
                m2 ^= m6 | m14;
                m12 ^= m4 & m8;
                m14 ^= m6 & m10;
                m4 ^= temp0 & m0;
                m6 ^= temp1 & m2;
                m8 ^= temp0;
                m10 ^= temp1;

                m2 ^= m4;
                m6 ^= m8;
                m10 ^= m0 ^ m12;
                m14 ^= m0;
                m0 ^= m6;
                m4 ^= m10;
                m8 ^= m2 ^ m14;
                m12 ^= m2;

                m2 = ((m2 & c_16_1) << 8) | ((m2 & c_16_2) >> 8);
                m6 = ((m6 & c_16_1) << 8) | ((m6 & c_16_2) >> 8);
                m10 = ((m10 & c_16_1) << 8) | ((m10 & c_16_2) >> 8);
                m14 = ((m14 & c_16_1) << 8) | ((m14 & c_16_2) >> 8);

                m13 = ~m13;
                m15 = ~m15;
                m1 ^= ~m9 & s_bitslices[r1];
                m3 ^= ~m11 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m1 & m5);
                temp1 = s_bitslices[r2++] ^ (m3 & m7);
                m1 ^= m9 & m13;
                m3 ^= m11 & m15;
                m13 ^= ~m5 & m9;
                m15 ^= ~m7 & m11;
                m5 ^= m1 & m9;
                m7 ^= m3 & m11;
                m9 ^= m1 & ~m13;
                m11 ^= m3 & ~m15;
                m1 ^= m5 | m13;
                m3 ^= m7 | m15;
                m13 ^= m5 & m9;
                m15 ^= m7 & m11;
                m5 ^= temp0 & m1;
                m7 ^= temp1 & m3;
                m9 ^= temp0;
                m11 ^= temp1;

                m3 ^= m5;
                m7 ^= m9;
                m11 ^= m1 ^ m13;
                m15 ^= m1;
                m1 ^= m7;
                m5 ^= m11;
                m9 ^= m3 ^ m15;
                m13 ^= m3;

                m3 = ((m3 & c_16_1) << 8) | ((m3 & c_16_2) >> 8);
                m7 = ((m7 & c_16_1) << 8) | ((m7 & c_16_2) >> 8);
                m11 = ((m11 & c_16_1) << 8) | ((m11 & c_16_2) >> 8);
                m15 = ((m15 & c_16_1) << 8) | ((m15 & c_16_2) >> 8);

                r1 += 2;
                r2 += 2;

                m12 = ~m12;
                m14 = ~m14;
                m0 ^= ~m8 & s_bitslices[r1];
                m2 ^= ~m10 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m0 & m4);
                temp1 = s_bitslices[r2++] ^ (m2 & m6);
                m0 ^= m8 & m12;
                m2 ^= m10 & m14;
                m12 ^= ~m4 & m8;
                m14 ^= ~m6 & m10;
                m4 ^= m0 & m8;
                m6 ^= m2 & m10;
                m8 ^= m0 & ~m12;
                m10 ^= m2 & ~m14;
                m0 ^= m4 | m12;
                m2 ^= m6 | m14;
                m12 ^= m4 & m8;
                m14 ^= m6 & m10;
                m4 ^= temp0 & m0;
                m6 ^= temp1 & m2;
                m8 ^= temp0;
                m10 ^= temp1;

                m2 ^= m4;
                m6 ^= m8;
                m10 ^= m0 ^ m12;
                m14 ^= m0;
                m0 ^= m6;
                m4 ^= m10;
                m8 ^= m2 ^ m14;
                m12 ^= m2;

                m2 = ((m2 & c_32_1) << 16) | ((m2 & c_32_2) >> 16);
                m6 = ((m6 & c_32_1) << 16) | ((m6 & c_32_2) >> 16);
                m10 = ((m10 & c_32_1) << 16) | ((m10 & c_32_2) >> 16);
                m14 = ((m14 & c_32_1) << 16) | ((m14 & c_32_2) >> 16);

                m13 = ~m13;
                m15 = ~m15;
                m1 ^= ~m9 & s_bitslices[r1];
                m3 ^= ~m11 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m1 & m5);
                temp1 = s_bitslices[r2++] ^ (m3 & m7);
                m1 ^= m9 & m13;
                m3 ^= m11 & m15;
                m13 ^= ~m5 & m9;
                m15 ^= ~m7 & m11;
                m5 ^= m1 & m9;
                m7 ^= m3 & m11;
                m9 ^= m1 & ~m13;
                m11 ^= m3 & ~m15;
                m1 ^= m5 | m13;
                m3 ^= m7 | m15;
                m13 ^= m5 & m9;
                m15 ^= m7 & m11;
                m5 ^= temp0 & m1;
                m7 ^= temp1 & m3;
                m9 ^= temp0;
                m11 ^= temp1;

                m3 ^= m5;
                m7 ^= m9;
                m11 ^= m1 ^ m13;
                m15 ^= m1;
                m1 ^= m7;
                m5 ^= m11;
                m9 ^= m3 ^ m15;
                m13 ^= m3;

                m3 = ((m3 & c_32_1) << 16) | ((m3 & c_32_2) >> 16);
                m7 = ((m7 & c_32_1) << 16) | ((m7 & c_32_2) >> 16);
                m11 = ((m11 & c_32_1) << 16) | ((m11 & c_32_2) >> 16);
                m15 = ((m15 & c_32_1) << 16) | ((m15 & c_32_2) >> 16);

                r1 += 2;
                r2 += 2;

                m12 = ~m12;
                m14 = ~m14;
                m0 ^= ~m8 & s_bitslices[r1];
                m2 ^= ~m10 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m0 & m4);
                temp1 = s_bitslices[r2++] ^ (m2 & m6);
                m0 ^= m8 & m12;
                m2 ^= m10 & m14;
                m12 ^= ~m4 & m8;
                m14 ^= ~m6 & m10;
                m4 ^= m0 & m8;
                m6 ^= m2 & m10;
                m8 ^= m0 & ~m12;
                m10 ^= m2 & ~m14;
                m0 ^= m4 | m12;
                m2 ^= m6 | m14;
                m12 ^= m4 & m8;
                m14 ^= m6 & m10;
                m4 ^= temp0 & m0;
                m6 ^= temp1 & m2;
                m8 ^= temp0;
                m10 ^= temp1;

                m2 ^= m4;
                m6 ^= m8;
                m10 ^= m0 ^ m12;
                m14 ^= m0;
                m0 ^= m6;
                m4 ^= m10;
                m8 ^= m2 ^ m14;
                m12 ^= m2;

                m2 = (m2 << 32) | (m2 >> 32);
                m6 = (m6 << 32) | (m6 >> 32);
                m10 = (m10 << 32) | (m10 >> 32);
                m14 = (m14 << 32) | (m14 >> 32);

                m13 = ~m13;
                m15 = ~m15;
                m1 ^= ~m9 & s_bitslices[r1];
                m3 ^= ~m11 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m1 & m5);
                temp1 = s_bitslices[r2++] ^ (m3 & m7);
                m1 ^= m9 & m13;
                m3 ^= m11 & m15;
                m13 ^= ~m5 & m9;
                m15 ^= ~m7 & m11;
                m5 ^= m1 & m9;
                m7 ^= m3 & m11;
                m9 ^= m1 & ~m13;
                m11 ^= m3 & ~m15;
                m1 ^= m5 | m13;
                m3 ^= m7 | m15;
                m13 ^= m5 & m9;
                m15 ^= m7 & m11;
                m5 ^= temp0 & m1;
                m7 ^= temp1 & m3;
                m9 ^= temp0;
                m11 ^= temp1;

                m3 ^= m5;
                m7 ^= m9;
                m11 ^= m1 ^ m13;
                m15 ^= m1;
                m1 ^= m7;
                m5 ^= m11;
                m9 ^= m3 ^ m15;
                m13 ^= m3;

                m3 = (m3 << 32) | (m3 >> 32);
                m7 = (m7 << 32) | (m7 >> 32);
                m11 = (m11 << 32) | (m11 >> 32);
                m15 = (m15 << 32) | (m15 >> 32);

                r1 += 2;
                r2 += 2;

                m12 = ~m12;
                m14 = ~m14;
                m0 ^= ~m8 & s_bitslices[r1];
                m2 ^= ~m10 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m0 & m4);
                temp1 = s_bitslices[r2++] ^ (m2 & m6);
                m0 ^= m8 & m12;
                m2 ^= m10 & m14;
                m12 ^= ~m4 & m8;
                m14 ^= ~m6 & m10;
                m4 ^= m0 & m8;
                m6 ^= m2 & m10;
                m8 ^= m0 & ~m12;
                m10 ^= m2 & ~m14;
                m0 ^= m4 | m12;
                m2 ^= m6 | m14;
                m12 ^= m4 & m8;
                m14 ^= m6 & m10;
                m4 ^= temp0 & m0;
                m6 ^= temp1 & m2;
                m8 ^= temp0;
                m10 ^= temp1;

                m2 ^= m4;
                m6 ^= m8;
                m10 ^= m0 ^ m12;
                m14 ^= m0;
                m0 ^= m6;
                m4 ^= m10;
                m8 ^= m2 ^ m14;
                m12 ^= m2;

                m13 = ~m13;
                m15 = ~m15;
                m1 ^= ~m9 & s_bitslices[r1];
                m3 ^= ~m11 & s_bitslices[r2];
                temp0 = s_bitslices[r1++] ^ (m1 & m5);
                temp1 = s_bitslices[r2++] ^ (m3 & m7);
                m1 ^= m9 & m13;
                m3 ^= m11 & m15;
                m13 ^= ~m5 & m9;
                m15 ^= ~m7 & m11;
                m5 ^= m1 & m9;
                m7 ^= m3 & m11;
                m9 ^= m1 & ~m13;
                m11 ^= m3 & ~m15;
                m1 ^= m5 | m13;
                m3 ^= m7 | m15;
                m13 ^= m5 & m9;
                m15 ^= m7 & m11;
                m5 ^= temp0 & m1;
                m7 ^= temp1 & m3;
                m9 ^= temp0;
                m11 ^= temp1;

                m3 ^= m5;
                m7 ^= m9;
                m11 ^= m1 ^ m13;
                m15 ^= m1;
                m1 ^= m7;
                m5 ^= m11;
                m9 ^= m3 ^ m15;
                m13 ^= m3;

                temp0 = m2;
                m2 = m3;
                m3 = temp0;

                temp0 = m6;
                m6 = m7;
                m7 = temp0;

                temp0 = m10;
                m10 = m11;
                m11 = temp0;

                temp0 = m14;
                m14 = m15;
                m15 = temp0; 
            }

            m12 = ~m12;
            m14 = ~m14;
            m0 ^= ~m8 & s_bitslices[140];
            m2 ^= ~m10 & s_bitslices[142];
            ulong temp2 = s_bitslices[140] ^ (m0 & m4);
            ulong temp3 = s_bitslices[142] ^ (m2 & m6);
            m0 ^= m8 & m12;
            m2 ^= m10 & m14;
            m12 ^= ~m4 & m8;
            m14 ^= ~m6 & m10;
            m4 ^= m0 & m8;
            m6 ^= m2 & m10;
            m8 ^= m0 & ~m12;
            m10 ^= m2 & ~m14;
            m0 ^= m4 | m12;
            m2 ^= m6 | m14;
            m_state[12] = m12 ^ (m4 & m8) ^ data[4];
            m_state[14] = m14 ^ (m6 & m10) ^ data[6];
            m_state[4] = m4 ^ (temp2 & m0);

            m_state[6] = m6 ^ (temp3 & m2);
            m_state[8] = m8 ^ temp2 ^ data[0];
            m_state[10] = m10 ^ temp3 ^ data[2];

            m13 = ~m13;
            m15 = ~m15;
            m1 ^= ~m9 & s_bitslices[141];
            m3 ^= ~m11 & s_bitslices[143];
            temp2 = s_bitslices[141] ^ (m1 & m5);
            temp3 = s_bitslices[143] ^ (m3 & m7);
            m1 ^= m9 & m13;
            m3 ^= m11 & m15;
            m13 ^= ~m5 & m9;
            m15 ^= ~m7 & m11;
            m5 ^= m1 & m9;
            m7 ^= m3 & m11;
            m9 ^= m1 & ~m13;
            m11 ^= m3 & ~m15;
            m1 ^= m5 | m13;
            m3 ^= m7 | m15;
            m_state[13] = m13 ^ (m5 & m9) ^ data[5];
            m_state[15] = m15 ^ (m7 & m11) ^ data[7];
            m_state[5] = m5 ^ (temp2 & m1);
            m_state[7] = m7 ^ (temp3 & m3);
            m_state[9] = m9 ^ temp2 ^ data[1];
            m_state[11] = m11 ^ temp3 ^ data[3];

            m_state[0] = m0;
            m_state[1] = m1;
            m_state[2] = m2;
            m_state[3] = m3;
        }

        protected override void Finish()
        {
            ulong bits = m_processedBytes * 8;

            int padindex = 56;

            if (m_processedBytes % 64 != 0)
                padindex += m_buffer.Length - m_buffer.Pos;

            byte[] pad = new byte[padindex + 8];

            pad[0] = 0x80;

            Converters.ConvertULongToBytesSwapOrder(bits, pad, padindex);
            padindex += 8;

            TransformBytes(pad, 0, padindex);
        }

        protected override byte[] GetResult() 
        {
            return Converters.ConvertULongsToBytes(m_state, 8, 8).SubArray(64 - HashSize, HashSize);
        }

        public override void Initialize() 
        {
            switch (HashSize)
            {
                case 28: Array.Copy(initial_states[0], m_state, m_state.Length); break;
                case 32: Array.Copy(initial_states[1], m_state, m_state.Length); break;
                case 48: Array.Copy(initial_states[2], m_state, m_state.Length); break;
                case 64: Array.Copy(initial_states[3], m_state, m_state.Length); break;
            }

            base.Initialize();
        }
    }
}
