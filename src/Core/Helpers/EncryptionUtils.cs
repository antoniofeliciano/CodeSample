using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers
{
    public static class EncryptionUtils
    {
        public static string EncryptString(string plainText, string key)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.GenerateIV();
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new(csEncrypt);
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
        }

        public static string DecryptString(string encryptedText, string key)
        {
            byte[] cipherText = Convert.FromBase64String(encryptedText);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = cipherText.Take(16).ToArray();
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(cipherText.Skip(16).ToArray());
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        public static string GenerateSecureKey(int keySizeInBytes)
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] keyData = new byte[keySizeInBytes];
            rng.GetBytes(keyData);
            return BitConverter.ToString(keyData).Replace("-", "");
        }
        public static string GenerateTempSecureKey(DateTime? date = null)
        {
            var d = date?.ToString("yyyyyMM") ?? DateTime.Now.ToString("yyyyyMM");
            var key = $"{d[1]}9839{d[6]}0612C{d[3]}B8364{d[2]}BC18B{d[4]}ECA3ED{d[0]}{d[5]}";
            return key;
        }
    }
}