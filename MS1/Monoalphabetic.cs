using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            //check if the plain text and the cipher text have the same length
            if (plainText.Length != cipherText.Length)
            {
                throw new IndexOutOfRangeException();
            }
            //make a dictionary to map the plain text to the cipher text but only letters and ignore the reapeated key and its value
            Dictionary<char, char> keyDictionary = new Dictionary<char, char>();
            for (int i = 0; i < plainText.Length; i++)
            {
                if (char.IsLetter(plainText[i]) && char.IsLetter(cipherText[i]))
                {
                    if (!keyDictionary.ContainsKey(plainText[i]) && !keyDictionary.ContainsValue(cipherText[i]))
                    {
                        keyDictionary.Add(plainText[i], cipherText[i]);
                    }
                }
            }
            //make a string of chipher text with the missing letters in aphabet
            string missigletterscipher = "";
            for (int i = 0; i < 26; i++)
            {
                if (!keyDictionary.ContainsValue((char)('a' + i)))
                {
                    missigletterscipher += (char)('a' + i);
                }
            }
            //make a string of the missing letters in the key of keydictionary
            string missingLetters = "";
            for (int i = 0; i < 26; i++)
            {
                if (!keyDictionary.ContainsKey((char)('a' + i)))
                {
                    missingLetters += (char)('a' + i);
                }
            }
            //add the missing letters to the dictionary to the missing letters in the key of keydictionary
            for (int i = 0; i < missingLetters.Length; i++)
            {
                keyDictionary.Add(missingLetters[i], ' ');
            }
            //add the missing value in the dictionary to the missing letters
            for (int i = 0; i < missingLetters.Length; i++)
            {
                keyDictionary[missingLetters[i]] = missigletterscipher[i];
            }
            //sort the dictionary by key in ascending order
            var sortedKeyDictionary = keyDictionary.OrderBy(x => x.Key);
            string key = "";
            foreach (var item in sortedKeyDictionary)
            {
                key += item.Value;
            }
            return key;
            //throw new NotImplementedException();
           
        }

        public string Decrypt(string cipherText, string key)
        {
            //use the key to decrypt the cipher text
            //convert the key to a dictionary
            Dictionary<char, char> keyDictionary = new Dictionary<char, char>();
            for (int i = 0; i < 26; i++)
            {
                keyDictionary.Add(key[i], (char)('a' + i));
            }
            //use the dictionary to decrypt the cipher text
            string plainText = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                if (char.IsLetter(cipherText[i]))
                {
                    if (char.IsUpper(cipherText[i]))
                    {
                        plainText += char.ToUpper(keyDictionary[char.ToLower(cipherText[i])]);
                    }
                    else
                    {
                        plainText += keyDictionary[cipherText[i]];
                    }
                }
                else
                {
                    plainText += cipherText[i];
                }
            }
            return plainText;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            //convert the key to a dictionary
            Dictionary<char, char> keyDictionary = new Dictionary<char, char>();
            for (int i = 0; i < 26; i++)
            {
                keyDictionary.Add((char)('a' + i), key[i]);
            }
            //use the dictionary to encrypt the plain text
            string cipherText = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                if (char.IsLetter(plainText[i]))
                {
                    
                    if (char.IsUpper(plainText[i]))
                    {
                        cipherText += char.ToUpper(keyDictionary[char.ToLower(plainText[i])]);
                    }
                    else
                    {
                        cipherText += keyDictionary[plainText[i]];
                    }
                }
                else
                {
                    cipherText += plainText[i];
                }
            }
            return cipherText;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            //make a string of the frequency letters of the alphabets
            string AlphaInfo = "etaoinsrhldcumfpgwybvkxjqz";
            string cipherInfo = "";
            cipherInfo = cipher.ToLower();

            //get the frequency of the letters in the cipherInfo and store it in a dictionary just the letters
            Dictionary<char, int> frequency = new Dictionary<char, int>();
            for (int i = 0; i < cipherInfo.Length; i++)
            {
                if (char.IsLetter(cipherInfo[i]))
                {
                    if (frequency.ContainsKey(cipherInfo[i]))
                    {
                        frequency[cipherInfo[i]]++;
                    }
                    else
                    {
                        frequency.Add(cipherInfo[i], 1);
                    }
                }
            }

            //sort the dictionary by value in descending order and if the value is the same sort by key in ascending order
            var sortedFrequency = frequency.OrderByDescending(x => x.Value).ThenBy(x => x.Key);

            //map the sorted frequency to the alphainfo
            Dictionary<char, char> keyDictionary = new Dictionary<char, char>();
            int j = 0;
            foreach (var item in sortedFrequency)
            {
                if (!keyDictionary.ContainsKey(item.Key))
                {
                    keyDictionary.Add(item.Key, AlphaInfo[j]);
                    j++;
                }
                
            }
            //make a two string store the missing letters in the key of keydictionary and the value of keydictionary
            string missingLetters = "";
            string missingValues = "";
            for (int i = 0; i < 26; i++)
            {
                if (!keyDictionary.ContainsKey((char)('a' + i)))
                {
                    missingLetters += (char)('a' + i);
                }
                if (!keyDictionary.ContainsValue((char)('a' + i)))
                {
                    missingValues += (char)('a' + i);
                }
            }
            //add the missing letters to the dictionary 
            for (int i = 0; i < missingLetters.Length; i++)
            {
                keyDictionary.Add(missingLetters[i], ' ');
            }
            //add the missing value in the dictionary 
            for (int i = 0; i < missingLetters.Length; i++)
            {
                keyDictionary[missingLetters[i]] = missingValues[i];
            }
            
            //use the keyDictionary to decrypt the cipher text
            string plainText = "";
            for (int i = 0; i < cipherInfo.Length; i++)
            {
                if (char.IsLetter(cipherInfo[i]))
                {
                    if (char.IsUpper(cipherInfo[i]))
                    {
                        plainText += char.ToUpper(keyDictionary[char.ToLower(cipherInfo[i])]);
                    }
                    else
                    {
                        plainText += keyDictionary[cipherInfo[i]];
                    }
                }
                else
                {
                    plainText += cipherInfo[i];
                }
            }
            return plainText;
            //throw new NotImplementedException();
        }
    }
}
