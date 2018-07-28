using System;

namespace Stratis.SmartContracts.Tools.Sct
{
    // Borrowed from x42.SmartContracts.Util
    public static class Extensions
    {
        public static string ToHexString(this byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }        
    }
}
