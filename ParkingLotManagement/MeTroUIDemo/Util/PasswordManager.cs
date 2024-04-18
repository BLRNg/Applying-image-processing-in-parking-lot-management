using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Util
{
    class PasswordManager
    {
        // Constants for hashing
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        // Method to generate a salt
        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }

        // Method to hash a password using PBKDF2
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        // Method to verify a password
        public static bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            byte[] enteredHash = HashPassword(enteredPassword, storedSalt);
            return CompareByteArrays(enteredHash, storedHash);
        }

        // Method to compare byte arrays
        private static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null || array1.Length != array2.Length)
                return false;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
        // Method to convert byte array to hexadecimal string
        public static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
