using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        string alphabets = "abcdefghijklmnopqrstuvwxyz";
        //to get index of choosen character
        public int index(char ch)
        {
            int i = 0;
            while (i < alphabets.Length)
            {
                if (alphabets[i] == ch)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public char Vigenere_intersection(char characterp, char caharcterk)
        {
            return alphabets[(index(characterp) + index(caharcterk)) % 26];
        }
        public char DeVigenere_intersection(char characterc, char characterk)
        {
            // +26 if result is negative
            return alphabets[(index(characterc) - index(characterk) + 26) % 26];
        }
        public string Analyse(string plainText, string cipherText)
        {
            // throw new NotImplementedException();

            string key_stream = "";
            string keystream_final = "";
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                key_stream += DeVigenere_intersection(cipherText[i], plainText[i]);
            }
            for (int i = 0; i < key_stream.Length; i++)
            {
                string pattern = plainText.Substring(0, key_stream.Length - i);
                if (key_stream.Contains(pattern))
                {
                    break;
                }
                keystream_final += key_stream[i];
            }
            return keystream_final;



        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            string key_stream = key;
            string plaintext = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                plaintext += DeVigenere_intersection(cipherText[i], key_stream[i]);
                if (key_stream.Length < cipherText.Length)
                {
                    key_stream += plaintext[i];
                }
            }
            return plaintext;
            
        }

        public string Encrypt(string plainText, string key)
        {



            // throw new NotImplementedException();
            plainText = plainText.ToLower();
            string key_stream = key;
           
            int cnt = 0;
            while (plainText.Length != key_stream.Length)
            {
                key_stream += plainText[cnt];
                cnt++;
            }

           /* for (int i = 0; i < plainText.Length; i++)
            {
                int cnt = i % plainText.Length;
                key_stream += plainText[cnt];
            }*/

            string ciphertext = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                ciphertext += Vigenere_intersection(plainText[i], key_stream[i]);
            }
            return ciphertext;
        }
    }
}
