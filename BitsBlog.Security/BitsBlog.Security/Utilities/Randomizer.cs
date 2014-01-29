using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BitsBlog.Security.Utilities
{
    public static class Randomizer
    {
        /// <summary>
        /// Creates a 128 bit (16 bytes) cryptographicly randomly GUID.
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateCryptoUUID()
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            //Guid's are 128 bit (16 byte) values
            byte[] randomBytes = new byte[16];

            //get random bytes
            crypto.GetBytes(randomBytes);

            //use these bytes to generate the guid
            return new Guid(randomBytes);
        }

        /// <summary>
        /// Generate random salt (base 64). Minimum should be 32 bits (4 bytes).
        /// </summary>
        /// <param name="sizeInBits"></param>
        /// <returns></returns>
        public static string GenerateRandomSalt(int sizeInBits = 32)
        {
            //TODO make 32 a private class level constant field
            if (sizeInBits < 32) throw new ArgumentOutOfRangeException("sizeInBits", "The minimum salt size should 32 bits (4 bytes).");

            int sizeInBytes = sizeInBits / 8;
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] saltInBytes = new byte[sizeInBytes];
            crypto.GetBytes(saltInBytes);
   
            return Convert.ToBase64String(saltInBytes);
        }
    }
}
