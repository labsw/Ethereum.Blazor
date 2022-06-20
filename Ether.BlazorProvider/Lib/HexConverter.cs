using System.Numerics;

namespace Ether.BlazorProvider
{
    public static class HexConverter
    {
        public static long HexToLong(string hex)
        {
            if (hex.StartsWith("0x"))
                hex = hex[2..];

            return long.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        public static BigInteger HexToBigInteger(string hex)
        {
            if (hex.StartsWith("0x"))
                hex = hex[2..];

            return BigInteger.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

    }
}
