using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SampleMWALLETOTCHPCMVC.Models
{
    public static class Helper
    {
        public static string CalculateHMACSHA256(string SecureSecret, SortedList<string, string> ParamList)
        {
            // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
            // hex decoding eliminates this source of error as it is independent of the character encoding

            StringBuilder sb = new StringBuilder();
            SortedList<string, string> list = ParamList;
            foreach (KeyValuePair<string, string> kvp in list)
            {
                if (!String.IsNullOrEmpty(kvp.Value))
                {
                    sb.Append(kvp.Value + "&");
                }
            }
            sb.Remove(sb.Length - 1, 1);

            // Create secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new HMACSHA256(Encoding.UTF8.GetBytes(SecureSecret)))
            {

                byte[] utf8bytes = Encoding.UTF8.GetBytes(sb.ToString());
                byte[] iso8859bytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("iso-8859-1"), utf8bytes);
                byte[] hashValue = hasher.ComputeHash(iso8859bytes);

                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }

        public static string CalculateHash(SortedList<string, string> Fields)
        {
            string Hash = string.Empty;

            StringBuilder builder = new StringBuilder();
            StringBuilder Keys = new StringBuilder();
            int i;

            for (i = 0; i < Fields.Count; i++)
            {
                builder.Append(Fields[Fields.Keys[i]]);
                Keys.Append(Fields.Keys[i]);
            }

            byte[] TextBytes = ASCIIEncoding.ASCII.GetBytes(builder.ToString());
            SHA256Managed objSHA256 = new SHA256Managed();
            byte[] hashedByte = objSHA256.ComputeHash(TextBytes);


            StringBuilder sOutput = new StringBuilder(hashedByte.Length);
            for (i = 0; i < hashedByte.Length; i++)
            {
                sOutput.Append(hashedByte[i].ToString("X2"));
            }
            return sOutput.ToString();


        }

    }
}