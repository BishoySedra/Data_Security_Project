using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        /// <summary>
        /// The most common diagrams in english (sorted): TH, HE, AN, IN, ER, ON, RE, ED, ND, HA, AT, EN, ES, OF, NT, EA, TI, TO, IO, LE, IS, OU, AR, AS, DE, RT, VE
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Analyse(string plainText)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            // remove spaces
            cipherText = cipherText.Replace(" ", "");

            // remove numbers
            cipherText = new string(cipherText.Where(c => !char.IsDigit(c)).ToArray());

            // convert to upperCase
            cipherText = cipherText.ToUpper();

            // replace J with I
            cipherText = cipherText.Replace('J', 'I');

            // remove spaces
            key = key.Replace(" ", "");

            // remove numbers
            key = new string(key.Where(c => !char.IsDigit(c)).ToArray());

            // convert to upperCase
            key = key.ToUpper();

            // replace J with I
            key = key.Replace('J', 'I');

            // generate the matrix 5*5
            string alphabetic = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            List<char> newKeyList = new List<char>();

            // Adding the letters of the key without duplicates
            foreach (char c in key)
            {
                if (!newKeyList.Contains(c) && char.IsLetter(c))
                {
                    newKeyList.Add(c);
                }
            }

            // Adding the remaining alphabetic letters
            foreach (char c in alphabetic)
            {
                if (!newKeyList.Contains(c))
                {
                    newKeyList.Add(c);
                }
            }

            string updated_key = new string(newKeyList.ToArray()).Substring(0, 25);

            char[,] squareMatrix = new char[5, 5];
            int k = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    squareMatrix[i, j] = updated_key[k++];
                }
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        Console.Write(squareMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}

            List<KeyValuePair<char, char>> pairs = new List<KeyValuePair<char, char>>();

            int idx = 0;
            int n = cipherText.Length;
            while (idx < n)
            {
                if (idx == n - 1 || cipherText[idx] == cipherText[idx + 1])
                {
                    pairs.Add(new KeyValuePair<char, char>(cipherText[idx], 'X'));
                    idx++;
                }
                else
                {
                    pairs.Add(new KeyValuePair<char, char>(cipherText[idx], cipherText[idx + 1]));
                    idx += 2;
                }
            }

            //Console.WriteLine("The Pairs:");
            //foreach (KeyValuePair<char, char> pair in pairs)
            //{
            //    Console.Write(pair.Key + " " + pair.Value + " - ");
            //}

            //Console.WriteLine();

            string result = "";
            foreach (KeyValuePair<char, char> pair in pairs)
            {
                //Console.Write(pair.Key + " " + pair.Value + " - ");

                int[] positions_1 = new int[2];
                bool is_found = false;

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (squareMatrix[i, j] == pair.Key)
                        {
                            positions_1[0] = i;
                            positions_1[1] = j;
                            is_found = true;
                            break;
                        }
                    }
                    if (is_found)
                    {
                        break;
                    }
                }

                is_found = false;
                int[] positions_2 = new int[2];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (squareMatrix[i, j] == pair.Value)
                        {
                            positions_2[0] = i;
                            positions_2[1] = j;
                            is_found = true;
                            break;
                        }
                    }
                    if (is_found)
                    {
                        break;
                    }
                }

                int row1 = positions_1[0];
                int col1 = positions_1[1];

                int row2 = positions_2[0];
                int col2 = positions_2[1];

                // Case(1): The same row
                if (row1 == row2)
                {
                    if (col1 > 0)
                    {
                        col1--;
                    }
                    else
                    {
                        col1 = 4;
                    }

                    if (col2 > 0)
                    {
                        col2--;
                    }
                    else
                    {
                        col2 = 4;
                    }

                    result += squareMatrix[row1, col1];
                    result += squareMatrix[row2, col2];

                    continue;
                }

                // Case(2): The same Column
                if (col1 == col2)
                {
                    if (row1 > 0)
                    {
                        row1--;
                    }
                    else
                    {
                        row1 = 4;
                    }

                    if (row2 > 0)
                    {
                        row2--;
                    }
                    else
                    {
                        row2 = 4;
                    }

                    result += squareMatrix[row1, col1];
                    result += squareMatrix[row2, col2];

                    continue;
                }

                // Case(3): Intersection
                result += squareMatrix[row1, col2];
                result += squareMatrix[row2, col1];
            }

            int size = result.Length;

            if (result.EndsWith("X"))
            {
                result = result.Remove(size - 1);
            }

            StringBuilder newResultList = new StringBuilder();
            char[] resultArr = result.ToCharArray();

            newResultList.Append(resultArr[0]);

            for (int i = 1; i < resultArr.Length; i++)
            {
                if (resultArr[i] == 'X' && resultArr[i - 1] == resultArr[i + 1])
                {
                    continue;
                }

                newResultList.Append(resultArr[i]);
            }

            string updated_result = newResultList.ToString();

            return updated_result;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            // remove spaces
            plainText = plainText.Replace(" ", "");

            // remove numbers
            plainText = new string(plainText.Where(c => !char.IsDigit(c)).ToArray());

            // convert to upperCase
            plainText = plainText.ToUpper();

            // replace J with I
            plainText = plainText.Replace('J', 'I');

            // remove spaces
            key = key.Replace(" ", "");

            // remove numbers
            key = new string(key.Where(c => !char.IsDigit(c)).ToArray());

            // convert to upperCase
            key = key.ToUpper();

            // replace J with I
            key = key.Replace('J', 'I');

            // generate the matrix 5*5
            string alphabetic = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            List<char> newKeyList = new List<char>();

            // Adding the letters of the key without duplicates
            foreach (char c in key)
            {
                if (!newKeyList.Contains(c) && char.IsLetter(c))
                {
                    newKeyList.Add(c);
                }
            }

            // Adding the remaining alphabetic letters
            foreach (char c in alphabetic)
            {
                if (!newKeyList.Contains(c))
                {
                    newKeyList.Add(c);
                }
            }

            string updated_key = new string(newKeyList.ToArray()).Substring(0, 25);

            char[,] squareMatrix = new char[5, 5];
            int k = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    squareMatrix[i, j] = updated_key[k++];
                }
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        Console.Write(squareMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();
            //}

            List<KeyValuePair<char, char>> pairs = new List<KeyValuePair<char, char>>();

            int idx = 0;
            int n = plainText.Length;
            while (idx < n)
            {
                if (idx == n - 1 || plainText[idx] == plainText[idx + 1])
                {
                    pairs.Add(new KeyValuePair<char, char>(plainText[idx], 'X'));
                    idx++;
                }
                else
                {
                    pairs.Add(new KeyValuePair<char, char>(plainText[idx], plainText[idx + 1]));
                    idx += 2;
                }
            }


            //Console.WriteLine("The Pairs:");
            //foreach (KeyValuePair<char, char> pair in pairs) {
            //    Console.Write(pair.Key + " " + pair.Value + " - ");
            //}

            //Console.WriteLine();

            string result = "";
            foreach (KeyValuePair<char, char> pair in pairs)
            {
                //Console.Write(pair.Key + " " + pair.Value + " - ");

                int[] positions_1 = new int[2];
                bool is_found = false;

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (squareMatrix[i, j] == pair.Key)
                        {
                            positions_1[0] = i;
                            positions_1[1] = j;
                            is_found = true;
                            break;
                        }
                    }
                    if (is_found)
                    {
                        break;
                    }
                }

                is_found = false;
                int[] positions_2 = new int[2];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (squareMatrix[i, j] == pair.Value)
                        {
                            positions_2[0] = i;
                            positions_2[1] = j;
                            is_found = true;
                            break;
                        }
                    }
                    if (is_found)
                    {
                        break;
                    }
                }

                int row1 = positions_1[0];
                int col1 = positions_1[1];

                int row2 = positions_2[0];
                int col2 = positions_2[1];

                // Case(1): The same row
                if (row1 == row2)
                {
                    if (col1 < 4)
                    {
                        col1++;
                    }
                    else
                    {
                        col1 = 0;
                    }

                    if (col2 < 4)
                    {
                        col2++;
                    }
                    else
                    {
                        col2 = 0;
                    }

                    result += squareMatrix[row1, col1];
                    result += squareMatrix[row2, col2];

                    continue;
                }

                // Case(2): The same Column
                if (col1 == col2)
                {
                    if (row1 < 4)
                    {
                        row1++;
                    }
                    else
                    {
                        row1 = 0;
                    }

                    if (row2 < 4)
                    {
                        row2++;
                    }
                    else
                    {
                        row2 = 0;
                    }

                    result += squareMatrix[row1, col1];
                    result += squareMatrix[row2, col2];

                    continue;
                }

                // Case(3): Intersection
                result += squareMatrix[row1, col2];
                result += squareMatrix[row2, col1];
            }

            return result;
            //throw new NotImplementedException();
        }
    }
}
