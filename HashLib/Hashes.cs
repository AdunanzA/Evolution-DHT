using System;
using System.Reflection;
using System.Linq;
using System.Collections.ObjectModel;

namespace HashLib
{
    public static class Hashes
    {
        public readonly static ReadOnlyCollection<Type> All;
        public readonly static ReadOnlyCollection<Type> AllUnique;
        public readonly static ReadOnlyCollection<Type> Hash32;
        public readonly static ReadOnlyCollection<Type> Hash64;
        public readonly static ReadOnlyCollection<Type> CryptoAll;
        public readonly static ReadOnlyCollection<Type> CryptoNotBuildIn;
        public readonly static ReadOnlyCollection<Type> CryptoBuildIn;
        public readonly static ReadOnlyCollection<Type> HMACCryptoBuildIn;

        public readonly static ReadOnlyCollection<Type> NonBlock;
        public readonly static ReadOnlyCollection<Type> FastComputes;
        public readonly static ReadOnlyCollection<Type> Checksums;

        static Hashes()
        {
            All = (from hf in Assembly.GetAssembly(typeof(IHash)).GetTypes()
                   where hf.IsClass
                   where !hf.IsAbstract
                   where hf != typeof(HMACNotBuildInAdapter)
                   where hf != typeof(HashCryptoBuildIn)
                   where hf != typeof(HMACBuildInAdapter)
                   where hf != typeof(HashLib.Checksum.CRC32)
                   where hf != typeof(HashLib.Checksum.CRC64)
                   where hf.IsImplementingInterface(typeof(IHash))
                   where !hf.IsNested
                   select hf).ToList().AsReadOnly();

            All = (from hf in All
                         orderby hf.Name
                         select hf).ToList().AsReadOnly();

            var x2 = new Type[] 
            { 
                typeof(HashLib.Crypto.BuildIn.SHA1Cng), 
                typeof(HashLib.Crypto.BuildIn.SHA1Managed), 
                typeof(HashLib.Crypto.BuildIn.SHA256Cng), 
                typeof(HashLib.Crypto.BuildIn.SHA256Managed), 
                typeof(HashLib.Crypto.BuildIn.SHA384Cng), 
                typeof(HashLib.Crypto.BuildIn.SHA384Managed), 
                typeof(HashLib.Crypto.BuildIn.SHA512Cng), 
                typeof(HashLib.Crypto.BuildIn.SHA512Managed), 
                typeof(HashLib.Crypto.MD5),
                typeof(HashLib.Crypto.RIPEMD160),
                typeof(HashLib.Crypto.SHA1),
                typeof(HashLib.Crypto.SHA256),
                typeof(HashLib.Crypto.SHA384),
                typeof(HashLib.Crypto.SHA512),
            };

            AllUnique = (from hf in All
                         where !(hf.IsDerivedFrom(typeof(HashLib.Hash32.DotNet)))
                         where !x2.Contains(hf)
                         where !hf.IsNested
                         select hf).ToList().AsReadOnly();

            Hash32 = (from hf in All
                      where hf.IsImplementingInterface(typeof(IHash32))
                      select hf).ToList().AsReadOnly();

            Hash64 = (from hf in All
                      where hf.IsImplementingInterface(typeof(IHash64))
                      select hf).ToList().AsReadOnly();

            Checksums = (from hf in All
                         where hf.IsImplementingInterface(typeof(IChecksum))
                         select hf).ToList().AsReadOnly();

            FastComputes = (from hf in All
                            where hf.IsImplementingInterface(typeof(IFastHashCodes))
                            select hf).ToList().AsReadOnly();

            NonBlock = (from hf in All
                        where hf.IsImplementingInterface(typeof(INonBlockHash))
                        select hf).ToList().AsReadOnly();

            CryptoAll = (from hf in All
                         where hf.IsImplementingInterface(typeof(ICrypto))
                         select hf).ToList().AsReadOnly();

            CryptoNotBuildIn = (from hf in CryptoAll
                                where hf.IsImplementingInterface(typeof(ICryptoNotBuildIn))
                                select hf).ToList().AsReadOnly();

            CryptoBuildIn = (from hf in CryptoAll
                             where hf.IsImplementingInterface(typeof(ICryptoBuildIn))
                             select hf).ToList().AsReadOnly();

            HMACCryptoBuildIn = (from hf in CryptoBuildIn
                                 where hf.IsImplementingInterface(typeof(IHasHMACBuildIn))
                                 select hf).ToList().AsReadOnly();
        }
    }
}
