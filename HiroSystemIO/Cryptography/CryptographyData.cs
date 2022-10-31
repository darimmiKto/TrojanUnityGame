using System.Text;
using System.Security.Cryptography;
using System;

namespace HiroSystemIO.Cryptography
{
    public class CryptographyData
    {
        private const string key_encrypto = "SDj72!K017akdqu12,;adp123u13c5rs";
        private const string iv_encrypto = "ocEDj2371c.3G0sa";

        private static CryptographyData cryp_instance = null;
        public static CryptographyData instance
        {
            get
            {
                if (cryp_instance == null)
                {
                    cryp_instance = new CryptographyData();
                }
                return cryp_instance;
            }
        }

        public string Encrypt(string text)
        {

            byte[] plain_text_bytes = ASCIIEncoding.UTF8.GetBytes(text);
            AesCryptoServiceProvider aes_crypto = new AesCryptoServiceProvider();
            aes_crypto.BlockSize = 128;
            aes_crypto.KeySize = 256;
            aes_crypto.Key = ASCIIEncoding.UTF8.GetBytes(key_encrypto);
            aes_crypto.IV = ASCIIEncoding.UTF8.GetBytes(iv_encrypto);
            aes_crypto.Padding = PaddingMode.PKCS7;
            aes_crypto.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes_crypto.CreateEncryptor(aes_crypto.Key, aes_crypto.IV);
            byte[] encrypted = crypto.TransformFinalBlock(plain_text_bytes, 0, plain_text_bytes.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encrypted)
        {
            byte[] encrypted_bytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider aes_crypto = new AesCryptoServiceProvider();
            aes_crypto.BlockSize = 128;
            aes_crypto.KeySize = 256;
            aes_crypto.Key = ASCIIEncoding.UTF8.GetBytes(key_encrypto);
            aes_crypto.IV = ASCIIEncoding.UTF8.GetBytes(iv_encrypto);
            aes_crypto.Padding = PaddingMode.PKCS7;
            aes_crypto.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes_crypto.CreateDecryptor(aes_crypto.Key, aes_crypto.IV);
            byte[] secret = crypto.TransformFinalBlock(encrypted_bytes, 0, encrypted_bytes.Length);
            crypto.Dispose();
            return ASCIIEncoding.UTF8.GetString(secret);
        }
    }
}
