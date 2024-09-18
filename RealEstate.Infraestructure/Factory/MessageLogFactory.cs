using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace RealEstate.Infraestructure.Factory
{
    public class MessageLogFactory
    {
        private const string V = " != null";
        private const string Format = "x2";

        public static string HashCode(string s)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(s);
            using SHA256 sHA256 = SHA256.Create();
            Debug.Assert(sHA256 != null, nameof(sHA256) + V);
            var hashBytes = sHA256.ComputeHash(messageBytes);
            var sb = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString(Format));
            }

            return sb.ToString();
        }
    }
}
