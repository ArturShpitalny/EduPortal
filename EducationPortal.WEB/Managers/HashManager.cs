using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EducationPortal.WEB.Managers
{
    internal class HashManager
    {
        private const string hashSalt = "eduSalt";

        public static string HashData(string hashString)
        {
            byte[] data = Encoding.ASCII.GetBytes($"{hashString}{hashSalt}");
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] sha1data = sha1.ComputeHash(data);

            return Encoding.ASCII.GetString(sha1data);
        }
    }
}