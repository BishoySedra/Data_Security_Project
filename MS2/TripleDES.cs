using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        public string Decrypt(string cipherText, List<string> key)
        {
            DES des = new DES();
            string plain = des.Decrypt(cipherText, key[0]);
            plain = des.Encrypt(plain, key[1]);
            plain = des.Decrypt(plain, key[0]);
            return plain;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            DES des = new DES();
            string cipher = des.Encrypt(plainText, key[0]);
            cipher = des.Decrypt(cipher, key[1]);
            cipher = des.Encrypt(cipher, key[0]);
            return cipher;
        }

        public List<string> Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}