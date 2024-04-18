using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTroUIDemo.Helper
{
    class ImageNameGenerator
    {
        public static string GenerateUniqueImageName()
        {
            // Generate a unique identifier (UUID)
            string uniqueIdentifier = Guid.NewGuid().ToString();

            // Generate a random string to add uniqueness
            string randomString = GenerateRandomString();

            // Combine the unique identifier and the random string
            string imageName = $"{uniqueIdentifier}_{randomString}.jpg"; // or any other image extension

            return imageName;
        }

        private static string GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
