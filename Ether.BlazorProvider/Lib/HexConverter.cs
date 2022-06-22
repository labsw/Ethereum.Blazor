using System.Numerics;

namespace Ether.BlazorProvider
{
    /// <summary>
    /// A helper class for converting hex strings into numerical values
    /// </summary>
    public static class HexConverter
    {
        /// <summary>
        /// Converts a hex string into a long.
        /// If the hex string starts with "0x" this will be stripped before the convertion happens.
        /// Exceptions can be thrown if the hex value cannnot be converted into a long.
        /// </summary>
        public static long HexToLong(string hex)
        {
            if (hex.StartsWith("0x"))
                hex = hex[2..];

            return long.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        /// <summary>
        /// Converts a hex string into a BigInteger.
        /// If the hex string starts with "0x" this will be stripped before the convertion happens.
        /// Exceptions can be thrown if the hex value cannnot be converted into a BigInteger.
        /// </summary>
        public static BigInteger HexToBigInteger(string hex)
        {
            if (hex.StartsWith("0x"))
                hex = hex[2..];

            return BigInteger.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
        }

    }
}
