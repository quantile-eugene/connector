using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BitAPI.Bitmex
{
    public class UtilsCrypto
    {
        public static byte[] GetHmacsha256(byte[] keyByte, byte[] messageBytes)
        {
            using (var hash = new HMACSHA256(keyByte))
            {
                return hash.ComputeHash(messageBytes);
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string GetSignature(string aApiSecret, string aMessage)
        {
            byte[] signatureBytes = GetHmacsha256(Encoding.UTF8.GetBytes(aApiSecret), Encoding.UTF8.GetBytes(aMessage));
            return ByteArrayToString(signatureBytes);
        }

        public static long GetNonce()
        {
            DateTime yearBegin = new DateTime(1990, 1, 1);
            return DateTime.UtcNow.Ticks - yearBegin.Ticks;
        }

    }
}
