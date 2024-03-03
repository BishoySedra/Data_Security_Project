using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            // Variables
            int keySize = cipherText.Count;
            int matrixSize = (int)Math.Sqrt(key.Count);
            List<List<int>> matrixAugmented = new List<List<int>>();
            List<List<int>> keyMatrix = new List<List<int>>();
            List<List<int>> inverseMatrix = new List<List<int>>();

            // Create array holding letters
            Dictionary<char, int> letters = new Dictionary<char, int>();
            for (int i = 0; i < 26; i++)
            {
                letters[(char)(i + 79)] = i; // maps A -> Z to 0 -> 25
            }

            Dictionary<int, char> numbers = new Dictionary<int, char>();
            foreach (var pair in letters)
            {
                numbers[pair.Value] = pair.Key;
            }

            // Create key matrix 
            int map = 0;
            for (int i = 0; i < matrixSize; i++)
            {
                List<int> row = new List<int>(); // list containing elements in row
                for (int j = 0; j < matrixSize; j++)
                {
                    if (map < key.Count)
                    {
                        row.Add(key[map]);
                        map++;
                    }
                    else
                    {
                        break;
                    }
                }
                keyMatrix.Add(row);
            }

            // Cipher text matrix

            List<List<int>> cipherTextMatrix = new List<List<int>>();
            int index = 0;
            int columns = (int)Math.Ceiling((double)cipherText.Count / matrixSize);

            for (int i = 0; i < columns; i++)
            {
                List<int> column = new List<int>();
                for (int j = 0; j < matrixSize; j++)
                {
                    if (index < cipherText.Count)
                    {
                        column.Add(cipherText[index]);
                        index++;
                    }
                }
                cipherTextMatrix.Add(column);
            }

            int keyMatrixCount = keyMatrix.Count;
            int keyMatrixColumns = keyMatrix[0].Count;

            int cipherMatrixCount = cipherTextMatrix.Count;
            int cipherMatrixColumns = cipherTextMatrix[0].Count;

            //// Inverse matrix

            // Get determinant
            switch (matrixSize)
            {
                case 2: // Case: 2x2 matrix
                    int determinant2 = 1 / ((keyMatrix[0][0] * keyMatrix[1][1]) - (keyMatrix[0][1] * keyMatrix[1][0]));
                    int index11 = keyMatrix[1][1];
                    int index10 = -1 * keyMatrix[1][0];
                    int index01 = -1 * keyMatrix[0][1];
                    int index00 = keyMatrix[0][0];
                    List<int> row1 = new List<int>() { index11, index10 };
                    List<int> row2 = new List<int>() { index01, index00 };
                    List<List<int>> tempMatrix = new List<List<int>>();
                    tempMatrix.Add(row1);
                    tempMatrix.Add(row2);
                    
                    for (int i = 0; i < matrixSize; i++)
                    {
                        for (int j = 0;j < matrixSize; j++)
                        {
                            inverseMatrix[i][j] = determinant2 * tempMatrix[i][j];
                        }
                    }

                    break;
                case 3: // Case: 3x3 matrix
                    int determinant3 = 0;
                    int determinantRemainder;
                    int bValue = -1;
                    int bRemainder;
                    int mod = 1;

                    for (int i = 0;i < matrixSize; i++) // get determinant and modulo it
                        determinant3 = determinant3 + (keyMatrix[0][i] * (keyMatrix[1][(i + 1) % 3] * keyMatrix[2][(i + 2) % 3] - keyMatrix[1][(i + 2) % 3] * keyMatrix[2][(i + 1) % 3]));
                    determinant3 %= 26;
                    
                    while(bValue < 0) // loop until we get a viable `b` value (+ve int)
                    {determinantRemainder = 26 - determinant3;
                    bRemainder = mod / determinantRemainder;

                        if (bRemainder > 0)
                            bValue = 26 - bRemainder;
                        else mod += 26;
                    }

                    for (int i = 0; i < matrixSize; i++)
                    {
                        for (int j = 0; j < matrixSize; j++)
                        {
                            List<int> subDeterminantRow1 = new List<int>() { keyMatrix[(i + 1) % matrixSize][(j + 1) % matrixSize], keyMatrix[(i + 1) % matrixSize][(j + 2) % matrixSize] };
                            List<int> subDeterminantRow2 = new List<int>() { keyMatrix[(i + 2) % matrixSize][(j + 1) % matrixSize], keyMatrix[(i + 2) % matrixSize][(j + 2) % matrixSize] };

                            int subDeterminant = (subDeterminantRow1[0] * subDeterminantRow2[1]) - (subDeterminantRow1[1] * subDeterminantRow2[0]);
                            subDeterminant %= 26;
                            int one = (int)Math.Pow(-1, i + j);
                            inverseMatrix[i][j] = (bValue * one * (subDeterminant)) % 26; // inverse of key matrix but not fr fr
                        }
                    }

                    List<List<int>> inverseMatrixFrFr = new List<List<int>>();
                    for (int i = 0; i < matrixSize; i++)
                    {
                        for(int j = 0; j < matrixSize; j++)
                        {
                            inverseMatrixFrFr[i][j] = inverseMatrix[j][i]; // inverse of 3x3 matrix 
                        }
                    }

                    break;
                    /*List<List<int>> cofactorMatrix = new List<List<int>>(); // get inverse matrix fr fr
                    for (int i = 0; i < matrixSize; i++)
                        for (int j = 0; j < matrixSize; j++)
                        {
                            int cofactoredValues = (keyMatrix[(j + 1) % matrixSize][(i + 1) % matrixSize] * keyMatrix[(j + 2) % matrixSize][(i + 2) % matrixSize]) - (keyMatrix[(j + 1) % matrixSize][(i + 2) % matrixSize] * keyMatrix[(j + 2) % matrixSize][(i + 1) % matrixSize]);
                            cofactorMatrix[i][j] = cofactoredValues / determinant3;
                        }*/
            }


            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            // Variables
            int keySize = key.Count;
            int matrixSize = (int)Math.Sqrt(keySize); // key length is always a perfect square (4, 9, 16, etc...)
            List<List<int>> keyMatrix = new List<List<int>>();
            List<List<int>> cipher = new List<List<int>>();
            List<int> cipherArr = new List<int>();
            string cipherText = "";

            // Store char -> int and int -> char
            Dictionary<char, int> letters = new Dictionary<char, int>();
            for (int i = 0; i < 26; i++)
            {
                letters[(char)(i + 65)] = i; // maps A -> Z to 0 -> 25
            }

            Dictionary<int, char> numbers = new Dictionary<int, char>();
            foreach (var pair in letters)
            {
                numbers[pair.Value] = pair.Key;
            }

            // Create key matrix 
            int index = 0;
            for (int i = 0; i < matrixSize; i++)
            {
                List<int> row = new List<int>(); // list containing elements in row
                for (int j = 0; j < matrixSize; j++)
                {
                    if (index < key.Count)
                    {
                        row.Add(key[index]);
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }
                keyMatrix.Add(row);
            }

            // key matrix dimensions
            int keyMatrixCnt = keyMatrix.Count;
            int keyMatrixClmn = keyMatrix[0].Count;


            // build plain text matrix with size (matrixSize rows X `columns` columns
            List<List<int>> plainTextMatrix = new List<List<int>>();
            int map = 0;
            int columns = (int)Math.Ceiling((double)plainText.Count / matrixSize);

            for (int i = 0; i < columns; i++)
            {
                List<int> column = new List<int>();
                for (int j = 0; j < matrixSize; j++)
                {
                    if (map < plainText.Count)
                    {
                        column.Add(plainText[map]);
                        map++;
                    }
                }
                plainTextMatrix.Add(column);
            }

            // plain text matrix dimensions
            int plainTextMatrixCnt = plainTextMatrix.Count;
            int plainTextMatrixClmn = plainTextMatrix[0].Count;

            // matrix multiplication
            for (int i = 0; i < keyMatrixCnt; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < plainTextMatrixCnt; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < keyMatrixClmn; k++)
                    {
                        sum += keyMatrix[i][k] * plainTextMatrix[j][k];
                        sum %= 26;
                    }
                    row.Add(sum);
                }
                cipher.Add(row);
            }

            int cipherCnt = cipher.Count;
            int cipherClmn = cipher[0].Count;

            // convert cipher numbers to letters
            for (int i = 0; i < cipherClmn; i++)
            {
                for (int j = 0; j < cipherCnt; j++)
                {
                    cipherArr.Add(cipher[j][i]);
                }
            }

            return cipherArr;

            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            // Variables
          
            string capitalPlainText = plainText.ToUpper(); // Convert plaintext to capital letters
            string capitalKey = key.ToUpper();
            int keySize = key.Length;
            int matrixSize = (int)Math.Sqrt(keySize); // key length is always a perfect square (4, 9, 16, etc...)
            List<int> mappedPlainText = new List<int>();
            List<int> keyNumbers = new List<int>();
            List<List<int>> keyMatrix = new List<List<int>>();
            List<List<int>> plainTextMatrix = new List<List<int>>();
            string cipherText = "";
            List<List<int>> cipher = new List<List<int>>();
            List<int> plainTextNumbers = new List<int>();

            const int ASCII_MOD = 65;

            // Create array holding letters
            Dictionary<char, int> letters = new Dictionary<char, int>();
            for (int i = 0; i < 26; i++)
            {
                letters[(char)(i + 79)] = i; // maps A -> Z to 0 -> 25
            }

            Dictionary<int, char> numbers = new Dictionary<int, char>();
            foreach (var pair in letters)
            {
                numbers[pair.Value] = pair.Key;
            }

           

            //// Convert key to numbers
            foreach (char c in capitalKey)
            {
                keyNumbers.Add(letters[c]);
            }

            //// Remove spaces
            capitalPlainText = Regex.Replace(capitalPlainText, @"\s+", ""); // Removes whitespaces

            foreach (var letter in capitalPlainText)
                plainTextNumbers.Add(letters[letter]);

            //// Split string into equal substrings
            List<string> matrixColumns = new List<string>();

            for (int i = 0; i < capitalPlainText.Length; i += matrixSize)
            {
                int length = Math.Min(matrixSize, capitalPlainText.Length - i);
                string newString = capitalPlainText.Substring(i, length);

                if (length < matrixSize)
                {
                    for (int j = 0; j < matrixSize - length; j++)
                    {
                        newString += 'X'; // if substring is less than column size, add X
                    }
                }

                matrixColumns.Add(newString);
            }

            //// Map substrings
            foreach (string str in matrixColumns)
                foreach (char c in str)
                    mappedPlainText.Add(letters[c]);

            List<int> cipherArr = Encrypt(plainTextNumbers, keyNumbers);

            //// Convert cipher numbers to letters
            

            foreach (var c in cipherArr)
            {
                cipherText += numbers[c];
            }

            return cipherText;


            //throw new NotImplementedException();
        }

        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {
            throw new NotImplementedException();
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }
    }
}
