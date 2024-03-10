using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
       
        public List<List<int>> GetPerms(List<int> list)
        {
            if (list.Count == 1)
            {
                return new List<List<int>> { list };
            }

            var perms = new List<List<int>>();

            foreach (var e in list)
            {
                var remaining = new List<int>(list);
                remaining.Remove(e);

                var subPerms = GetPerms(remaining);

                foreach (var subPerm in subPerms)
                {
                    subPerm.Insert(0, e);
                    perms.Add(subPerm);
                }
            }

            return perms;
        }
        public List<int> Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            for (int i = 2; i <= plainText.Length; i++)
            {
                List<List<int>> permutations = GetPerms(Enumerable.Range(1, i).ToList());
                foreach (var permutation in permutations)
                {
                    if (Encrypt(plainText, permutation) == cipherText)
                        return permutation;
                }
            }
            /*
                        int loopmx = plainText.Length;
                        for (int i = 2; i < loopmx; i++)
                        {
                            List<List<int>> listOfLists = new List<List<int>>();
                            List<int> current = new List<int>();
                            combinations(i, 1, current, listOfLists);
                            foreach (var theKey in listOfLists)
                            {
                                string s = Encrypt(plainText, theKey);
                                if (s == cipherText)
                                    return theKey;
                            }
                        }
            */
            return new List<int>();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int word_length = cipherText.Length;
            int col_size = key.Count, row_size, cnt = 0;
            int[] columns_order = new int[col_size];

            foreach (int i in key)
            {
                columns_order[i - 1] = cnt;
                cnt++;
            }

            if (word_length % col_size == 0)
                row_size = word_length / col_size;
            else
                row_size = (word_length / col_size) + 1;

            char[,] arr = new char[row_size, col_size];

            cnt = 0;

            for (int j = 0; j < col_size; j++)
                for (int i = 0; i < row_size; i++)
                    if (cnt < word_length)
                        arr[i, columns_order[j]] = cipherText[cnt++];

            string decryptedData = null;

            for (int i = 0; i < row_size; i++)
                for (int j = 0; j < col_size; j++)
                    decryptedData += arr[i, j];

            return decryptedData;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int word_length = plainText.Length;
            int col_size = key.Count, row_size, cnt = 0;
            int[] columns_order = new int[col_size];

            foreach (int i in key)
            {
                columns_order[i - 1] = cnt;
                cnt++;
            }

            if (word_length % col_size == 0)
                row_size = word_length / col_size;
            else
                row_size = (word_length / col_size) + 1;

            char[,] arr = new char[row_size, col_size];

            cnt = 0;
            for (int i = 0; i < row_size; i++)
            {
                for (int j = 0; j < col_size; j++)
                {
                    if (cnt < word_length)
                        arr[i, j] = plainText[cnt++];
                    //else
                    //  arr[i, j] = 'x';
                }
            }

            string encryptedData = null;

            for (int j = 0; j < col_size; j++)
                for (int i = 0; i < row_size; i++)
                {
                    char x = arr[i, columns_order[j]];
                    if ((x > 64 && x < 91) || (x > 96 && x < 123))
                        encryptedData += arr[i, columns_order[j]];
                }

            return encryptedData;
        }
    }
}

