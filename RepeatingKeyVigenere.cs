using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
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
            //throw new NotImplementedException();
            string key_stream = "";
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                key_stream += DeVigenere_intersection(cipherText[i], plainText[i]);
            }
            for (int i = 1; i < key_stream.Length; i++)
            {
                string pattern = key_stream.Substring(0, i);
                string remaining = key_stream.Substring(i,i);

                if (remaining.Contains(pattern))
                {
                    return pattern;
                }
            }
            return null;
        }

        public string Decrypt(string cipherText, string key)
        {
            // throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            string key_stream = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                int cnt = i % key.Length;
                key_stream += key[cnt];
            }
            string plaintext = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                plaintext += DeVigenere_intersection(cipherText[i], key_stream[i]);
            }
            return plaintext;
        }

        public string Encrypt(string plainText, string key)
        {
            // throw new NotImplementedException();
            plainText = plainText.ToLower();
            string key_stream = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                int cnt = i % key.Length;
                key_stream += key[cnt];
            }
            string ciphertext = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                ciphertext += Vigenere_intersection(plainText[i], key_stream[i]);
            }
            return ciphertext;

        }
    }
}