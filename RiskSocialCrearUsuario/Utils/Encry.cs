using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using System.Security.Cryptography;
using System.Text;

namespace RiskSocialCrearUsuario.Utils
{
    public class Encry
    {
        public static string HashPassword(string password)
        {
            //using (var sha256 = SHA256.Create())
            //{
            //    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            //    byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            //    // Convierte el hash a una representación de cadena hexadecimal
            //    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            //    return hash;
            //}

            // Reemplaza 'your-key-id' con el ID de tu clave maestra de KMS
            var keyId = "fc7880c2-18b4-45e4-860d-daeb0a191870";

            // Crea el cliente de KMS
            using (var kmsClient = new AmazonKeyManagementServiceClient())
            {
                // Cifrar el texto
                var cifradoRequest = new EncryptRequest
                {
                    KeyId = keyId,
                    Plaintext = new MemoryStream(Encoding.UTF8.GetBytes(password))
                };

                var cifradoResponse = kmsClient.EncryptAsync(cifradoRequest).Result;
                var textoCifrado = Convert.ToBase64String(cifradoResponse.CiphertextBlob.ToArray());

                return textoCifrado;
            }
        }
        public static bool VerifyPassword(string password, string storedHash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }
    }
}
