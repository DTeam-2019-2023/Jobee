using System.Security.Cryptography;
using System.Text;

namespace Jobee_API.Tools
{
    public class HashPassword
    {
        public static string hashPassword(string pass)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(pass);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }
    }
}
