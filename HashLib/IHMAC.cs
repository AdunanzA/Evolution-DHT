using System;

namespace HashLib
{
    public interface IHMAC : IHash
    {
        byte[] Key { get; set; }
    }
}
