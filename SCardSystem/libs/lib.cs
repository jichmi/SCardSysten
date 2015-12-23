using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SCardSystem
{
    class lib
    {
        public static String getMD5(String pwd)
        {
            byte[] result = Encoding.Default.GetBytes(pwd); //MD5加密 
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }

    }
}
