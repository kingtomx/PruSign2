using System;
using System.Security.Cryptography;

namespace PruSign.Helpers
{
    public static class FileHelper
    {
        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string SHA512StringHash(byte[] input)
        {
            SHA512 shaM = new SHA512Managed();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaM.ComputeHash(input);
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
