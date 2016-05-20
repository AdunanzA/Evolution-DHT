using System;

namespace HashLib.Crypto.BuildIn
{
    internal class RIPEMD160Managed : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public RIPEMD160Managed()
            : base(new System.Security.Cryptography.RIPEMD160Managed(), 64)
        {
        }

        public override System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACRIPEMD160();
        }
    }
}
