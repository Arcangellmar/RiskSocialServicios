using System.Security.Cryptography;

namespace RiskSocialLogin.Utils
{
    public class Encry
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convierte el hash a una representación de cadena hexadecimal
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
        public static bool VerifyPassword(string password, string storedHash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }
    }
}
