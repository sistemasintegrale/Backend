using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Utilities.Codec
{
    public static class Codec
    {
        public static string Encriptar(this string _cadenaAencriptar)
        {
            string result = null;
            if (!string.IsNullOrEmpty(_cadenaAencriptar))
            {
                byte[] encryted = Encoding.Unicode.GetBytes(_cadenaAencriptar);
                result = Convert.ToBase64String(encryted);
            }
            return result;
        }

       
        public static string DesEncriptar(this string _cadenaAdesencriptar)
        {
            string result = null;
            if (!string.IsNullOrEmpty(_cadenaAdesencriptar))
            {
                byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);               
                result = Encoding.Unicode.GetString(decryted);
            }
            return result;
        }
    }
}
