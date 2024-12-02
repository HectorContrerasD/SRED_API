using System.Security.Cryptography;
using System.Text;

namespace SRED_API.Helpers
{
    public class EncryptionHelper
    {
        public static string StringToSHA512(string s)
        {
            using (var sha512 = SHA512.Create())
            {
                var arreglo = Encoding.UTF8.GetBytes(s);

                var hash = sha512.ComputeHash(arreglo);

                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
