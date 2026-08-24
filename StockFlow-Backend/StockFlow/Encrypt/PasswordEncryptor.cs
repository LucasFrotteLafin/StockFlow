using System.Security.Cryptography;
using System.Text;

namespace FocusSpace.Encrypt
{
    public class PasswordEncryptor
    {
        public static string Encrypt(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public static bool Verify(string password, string hash)
        {
            var encrypted = Encrypt(password);
            return encrypted.Equals(hash);
        }
    }
}
