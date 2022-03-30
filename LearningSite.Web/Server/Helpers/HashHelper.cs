using System.Security.Cryptography;
using System.Text;

namespace LearningSite.Web.Server.Helpers
{
    public static class HashHelper
    {
        public static string GenerateHash(string input, string salt)
        {
            using var alg = new HMACSHA256(GetBytes(salt));
            var result = alg.ComputeHash(GetBytes(input));
            return Convert.ToBase64String(result);
        }

        public static string GenerateSalt(int size = 32)
        {
            byte[] buff = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(buff);
        }

        private static byte[] GetBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
