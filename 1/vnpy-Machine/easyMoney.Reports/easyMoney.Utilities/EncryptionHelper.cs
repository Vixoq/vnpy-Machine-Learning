using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace easyMoney.Utilities
{
    public class EncryptionHelper
    {

        #region Class members and properties

        /// <summary>
        /// salt for cryptography key based on password
        /// </summary>
        private readonly byte[] salt = { 0xFF, 0x12, 0x65, 0x88, 0x23, 0xE1, 0x07, 0x89, 0x1B, 0xAE, 0x01, 0xE2 };

        private AesManaged aes = new AesManaged();

        private String password = String.Empty;

        public String Password
        {
            get { return password; }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    password = String.Empty;
                }
                else
                {
                    password = value.Trim();
                }
                setEncyptionParameters();
            }
        }

        #endregion

        #region Init

        protected EncryptionHelper()
        {
        }

        private void setEncyptionParameters()
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
        }

        #endregion

        #region Singleton implementation

        private static readonly Lazy<EncryptionHelper> instance = new Lazy<EncryptionHelper>(() => new EncryptionHelper());

        public static EncryptionHelper Instance
        {
            get { return instance.Value; }
        }

        #endregion

        #region Main routines

        public String Decrypt(String encrypted)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);
            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
            cs.FlushFinalBlock();
            byte[] dec = ms.ToArray();
            cs.Close();
            return Encoding.UTF8.GetString(dec);
        }

        public String Encrypt(String plain)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plain);
            cs.Write(plainBytes, 0, plainBytes.Length);
            cs.FlushFinalBlock();
            byte[] enc = ms.ToArray();
            cs.Close();
            
            return Convert.ToBase64String(enc);
        }

        #endregion

    }
}
