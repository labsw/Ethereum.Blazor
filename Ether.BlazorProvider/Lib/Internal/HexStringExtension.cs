using System.Text;

namespace Ether.BlazorProvider.Internal
{
    internal static class HexStringExtension
    {
        public static string ToHexUTF8(this string value)
        {
            string hexStr = "0x" + ToHex(Encoding.UTF8.GetBytes(value));
            return hexStr;
        }

        //-- 

        private static string ToHex(byte[] value)
        {
            return string.Concat(value.Select(b => b.ToString("x2")).ToArray());
        }

    }
}
