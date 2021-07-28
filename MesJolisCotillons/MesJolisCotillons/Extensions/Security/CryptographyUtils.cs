using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MesJolisCotillons.Extensions.Security
{

    //Code coming from http://www.codeshare.co.uk/blog/sha-256-and-sha-512-hash-examples/


    public static class CryptographyUtils
    {
        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        static readonly char[] AvailableCharacters = {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };

        //based on code coming from http://stackoverflow.com/questions/19298801/generating-random-string-using-rngcryptoserviceprovider
        public static string GenerateSalt(int length = 32)
        {

            char[] identifier = new char[length];
            byte[] randomData = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomData);
            }

            for (int idx = 0; idx < identifier.Length; idx++)
            {
                int pos = randomData[idx] % AvailableCharacters.Length;
                identifier[idx] = AvailableCharacters[pos];
            }

            return new string(identifier);
        }

        public static SaltHashedText GenerateSHA256SaltedText(string text)
        {
            string salt = CryptographyUtils.GenerateSalt(32);
            string saltAndText = salt + text;
            string shA256String = CryptographyUtils.GenerateSHA256String(saltAndText);

            return new SaltHashedText
            {
                Salt = salt,
                Text = text,
                HashedText = shA256String
            };
        }

        #region Util class
        public class SaltHashedText
        {
            public string Salt { get; set; }
            public string Text { get; set; }
            public string HashedText { get; set; }
        }
        #endregion
    }
}