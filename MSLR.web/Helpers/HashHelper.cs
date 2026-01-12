using System.Security.Cryptography;
using System.Text;

namespace MSLR.web.Helpers
{
    public static class HashHelper
    {
        public static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
