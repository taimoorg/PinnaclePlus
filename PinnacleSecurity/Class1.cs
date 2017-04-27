using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PinnacleFunction
{
    public class URLEncrypt
    {
        public static string EncryptDesToHex(string stringToEncrypt, string encryptionKey)
        {
            byte[] key = { };
            byte[] iv = { 0x08, 0x07, 0x06, 0x05, 0x01, 0x02, 0x03, 0x04 };
            byte[] inputByteArray;
            byte[] outputByteArray;

            try
            {
                key = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                outputByteArray = ms.ToArray();
                string retstr = ToHex(outputByteArray);
                return retstr;
            }
            catch
            {
                return (string.Empty);
            }
        }
        public static string ToHex(Byte[] inputByte)
        {
            if ((inputByte == null) || (inputByte.Length == 0))
            {
                return "";
            }

            String HexFormat = "{0:X2}";
            StringBuilder sb = new StringBuilder();

            foreach (Byte b in inputByte)
            {
                sb.Append(String.Format(HexFormat, b));
            }
            return sb.ToString();
        }
    }

}
