using System;

namespace HashLib.Crypto.BuildIn
{
    internal class SHA512Managed : HashCryptoBuildIn, IHasHMACBuildIn
    {
        public SHA512Managed()
            : base(new System.Security.Cryptography.SHA512Managed(), 128)
        {
        }

        public override System.Security.Cryptography.HMAC GetBuildHMAC()
        {
            return new System.Security.Cryptography.HMACSHA512();
        }
    }
}
