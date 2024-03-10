using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {

            int n = plainText.Length;
            string encyprtedTest = null;
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            for (int i = 2; i < n; i++)
            {
                encyprtedTest = Encrypt(plainText, i);
                if (encyprtedTest == cipherText)
                    return i;
            }
            return 0;
        }

        public string Decrypt(string cipherText, int key)
        {
            int col_size, word_length = cipherText.Length, cnt = 0;

            if (word_length % key == 0)
                col_size = word_length / key;
            else
                col_size = (word_length / key) + 1;

            char[,] arr = new char[key, col_size];



            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < col_size; j++)
                {
                    arr[i, j] += cipherText[cnt++];
                    if (cnt == word_length)
                        break;
                }
                if (cnt == word_length)
                    break;
            }

            string decryptedData = null;
            cnt = 0;

            while (cnt < word_length)
            {
                decryptedData += arr[cnt % key, cnt / key];
                cnt++;
            }

            return decryptedData;
        }

        public string Encrypt(string plainText, int key)
        {
            int word_length = plainText.Length;
            int col_size;
            int depth = key;

            if (word_length % depth == 0)
                col_size = word_length / depth;
            else
                col_size = (word_length / depth) + 1;

            char[,] arr = new char[depth, col_size];

            int cnt = 0;

            while (cnt < word_length)
            {
                arr[cnt % depth, cnt / depth] = plainText[cnt];
                cnt++;
            }
            /*            
                        while (cnt < (depth * col_size))
                        {
                            arr[cnt % depth, cnt / depth] = 'x';
                            cnt++;
                        }
            */
            string encryptedData = null;
            for (int i = 0; i < depth; i++)
                for (int j = 0; j < col_size; j++)
                {
                    char x = arr[i, j];
                    if ((x > 64 && x < 91) || (x > 96 && x < 123))
                        encryptedData += arr[i, j];
                }

            return encryptedData;
        }
    }
}
