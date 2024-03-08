using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public List<List<int>> getInverseMatrix(List<List<int>> matrix, int matrixSize)
        {
            List<List<int>> inverseMatrix = new List<List<int>>();
            int bValue = -1;
            double bRemainder;
            int mod = 1;

            switch (matrixSize)
            {
                case 2: // Case: 2x2 matrix
                    int determinant2 = ((matrix[0][0] * matrix[1][1]) - (matrix[0][1] * matrix[1][0])) % 26;

                    while (determinant2 < 0)
                    {
                        determinant2 += 26;
                        determinant2 %= 26;
                    }

                    // Check GCD between 26 & determinant

                    int tempDeterminant2 = determinant2;
                    int GCDValue = 26;
                    while (tempDeterminant2 != 0)
                    {
                        int rem = GCDValue % tempDeterminant2;
                        GCDValue = tempDeterminant2;
                        tempDeterminant2 = rem;
                    }

                    if (GCDValue != 1)
                        throw new SystemException();

                    while (bValue < 0) // loop until we get a viable `b` value (+ve int)
                    {
                        int determinantRemainder = 26 - determinant2;
                        //Console.WriteLine("Det rem: " + determinantRemainder);
                        //Console.WriteLine("mod b4: " + mod);
                        bRemainder = (double)mod / (double)determinantRemainder;

                        //Console.WriteLine("B rem: " + bRemainder);

                        if ((bRemainder > 0 && bRemainder < 26) && (bRemainder == Math.Floor(bRemainder)))
                            bValue = 26 - (int)bRemainder;
                        else mod += 26;
                        //Console.WriteLine("mod after: " + mod);
                        //Console.WriteLine("B after: " + bValue);
                    }

                    Console.WriteLine("B: " + bValue);

                    Console.WriteLine("Det2: " + determinant2);
                    int index11 = matrix[1][1];
                    int index10 = -1 * matrix[1][0] % 26;
                    int index01 = -1 * matrix[0][1] % 26;
                    int index00 = matrix[0][0];

                    // Make sure no -ve values are present
                    while (index10 < 0)
                    {
                        index10 += 26;
                        index10 %= 26;
                    }
                    while (index01 < 0)
                    {
                        index01 += 26;
                        index01 %= 26;
                    }


                    List<int> row1 = new List<int>() { index11, index10 };
                    List<int> row2 = new List<int>() { index01, index00 };
                    List<List<int>> tempMatrix = new List<List<int>>();
                    inverseMatrix.Add(new List<int> { 0, 0 });
                    inverseMatrix.Add(new List<int> { 0, 0 });
                    tempMatrix.Add(row1);
                    tempMatrix.Add(row2);



                    for (int i = 0; i < matrixSize; i++)
                    {
                        for (int j = 0; j < matrixSize; j++)
                        {
                            tempMatrix[i][j] *= bValue;
                        }
                    }

                    for (int i = 0; i < matrixSize; i++)
                    {
                        for (int j = 0; j < matrixSize; j++)
                        {
                            inverseMatrix[i][j] = tempMatrix[j][i] % 26;
                        }
                        Console.WriteLine();
                    }

                    foreach (var row in inverseMatrix)
                    {
                        foreach (var c in row)
                        {
                            Console.Write(c + " ");
                        }
                        Console.WriteLine();
                    }


                    break;
                case 3: // Case: 3x3 matrix
                    int determinant3 = 0;
                    /*Console.WriteLine("----");
                    foreach (var row in keyMatrix)
                    {
                        foreach (var num in row)
                        {
                            Console.Write(num + " ");
                        }
                        Console.WriteLine();
                    }*/

                    Console.WriteLine("----");
                    for (int i = 0; i < matrixSize; i++) // get determinant and modulo it
                    {
                        int subDeterminant = (matrix[0][i] * (matrix[1][(i + 1) % matrixSize] * matrix[2][(i + 2) % matrixSize] -
                                                                 matrix[1][(i + 2) % matrixSize] * matrix[2][(i + 1) % matrixSize]));
                        determinant3 += subDeterminant;

                        /*Console.WriteLine("0, " + (i + keyMatrix[0][i]) + "\t" + keyMatrix[0][i]);
                        Console.WriteLine("1, " + (i + 1) % matrixSize + "\t" + keyMatrix[1][(i + 1) % matrixSize]);
                        Console.WriteLine("2, " + (i + 2) % matrixSize + "\t" + keyMatrix[2][(i + 2) % matrixSize]);
                        Console.WriteLine("1, " + (i + 2) % matrixSize + "\t" + keyMatrix[1][(i + 2) % matrixSize]);
                        Console.WriteLine("2, " + (i + 1) % matrixSize + "\t" + keyMatrix[2][(i + 1) % matrixSize]);
                        Console.WriteLine(determinant3);
                        Console.WriteLine();*/


                    }
                    determinant3 %= 26;

                    while (determinant3 < 0)
                    {
                        determinant3 += 26;
                        determinant3 %= 26;
                    }

                    // Check GCD between 26 & determinant

                    int tempDeterminant3 = determinant3;
                    int otherGCDValue = 26;
                    while (tempDeterminant3 != 0)
                    {
                        int rem = otherGCDValue % tempDeterminant3;
                        otherGCDValue = tempDeterminant3;
                        tempDeterminant3 = rem;
                    }

                    if (otherGCDValue != 1)
                        throw new SystemException();

                    while (bValue < 0) // loop until we get a viable `b` value (+ve int)
                    {
                        int determinantRemainder = 26 - determinant3;

                        //Console.WriteLine("mod b4: " + mod);
                        bRemainder = (double)mod / (double)determinantRemainder;

                        //Console.WriteLine("B rem: " + bRemainder);

                        if ((bRemainder > 0 && bRemainder < 26) && (bRemainder == Math.Floor(bRemainder)))
                            bValue = 26 - (int)bRemainder;
                        else mod += 26;
                        //Console.WriteLine("mod after: " + mod);
                    }
                    /*Console.WriteLine("B: " + bValue); // nova*/

                    List<List<int>> adjointMatrix = new List<List<int>>();
                    for (int i = 0; i < matrixSize; i++)
                    {
                        List<int> cofactorRow = new List<int>();
                        for (int j = 0; j < matrixSize; j++)
                        {
                            List<int> subDeterminantRow1 = new List<int>() { matrix[(i + 1) % matrixSize][(j + 1) % matrixSize], matrix[(i + 1) % matrixSize][(j + 2) % matrixSize] };
                            List<int> subDeterminantRow2 = new List<int>() { matrix[(i + 2) % matrixSize][(j + 1) % matrixSize], matrix[(i + 2) % matrixSize][(j + 2) % matrixSize] };

                            int subDeterminant = (subDeterminantRow1[0] * subDeterminantRow2[1]) - (subDeterminantRow1[1] * subDeterminantRow2[0]);

                            while (subDeterminant < 0)
                            {
                                subDeterminant += 26;
                                subDeterminant %= 26;
                            }
                            subDeterminant %= 26;
                            cofactorRow.Add(subDeterminant);
                        }
                        adjointMatrix.Add(cofactorRow);
                    }

                    /*foreach (var row in adjointMatrix)
                    {
                        foreach (var num in row)
                        {
                            Console.Write(num + " ");
                        }
                        Console.WriteLine();
                    }*/


                    for (int i = 0; i < matrixSize; i++)
                    {
                        List<int> inverseRow = new List<int>();
                        for (int j = 0; j < matrixSize; j++)
                        {
                            int one = (int)Math.Pow(-1, i + j);

                            inverseRow.Add((bValue * (adjointMatrix[j][i])) % 26); // inverse of key matrix fr fr
                        }
                        inverseMatrix.Add(inverseRow);
                    }


                    /*foreach (var row in inverseMatrix)
                    {
                        foreach (var num in row)
                        {
                            Console.Write(num + " ");
                        }
                        Console.WriteLine();
                    }*/

                    break;
            }
            return inverseMatrix;
        }

        public List<List<int>> createMatrix(List<int> textList, int matrixSize, bool isKeyMatrix, bool byRow)
        {
            switch (isKeyMatrix)
            {
                case true:
                    List<List<int>> keyMatrix = new List<List<int>>();

                    for (int i = 0; i < matrixSize; i++)
                    {
                        List<int> row = new List<int>();
                        for (int j = 0; j < matrixSize; j++)
                        {
                            row.Add(0);
                        }

                        keyMatrix.Add(row);
                    }

                    int keyIndex = 0;
                    for (int i = 0; i < matrixSize; i++)
                    {
                        List<int> row = new List<int>(); // list containing elements in row
                        for (int j = 0; j < matrixSize; j++)
                        {
                            if (keyIndex < textList.Count)
                            {
                                //row.Add(textList[keyIndex]);
                                keyMatrix[i][j] = textList[keyIndex];
                                keyIndex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //keyMatrix.Add(row);
                    }
                    return keyMatrix;

                default: // not key matrix

                    int map = 0;
                    int columns = (int)Math.Ceiling((double)textList.Count / matrixSize);

                    List<List<int>> textMatrix = new List<List<int>>();

                    for (int i = 0; i < matrixSize; i++)
                    {
                        List<int> row = new List<int>();
                        for (int j = 0; j < columns; j++)
                        {
                            row.Add(0);
                        }

                        textMatrix.Add(row);
                    }

                    if (byRow)
                    {
                        for (int i = 0; i < matrixSize; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                if (map < textList.Count)
                                {
                                    textMatrix[i][j] = textList[map];
                                    map++;
                                }
                                else
                                {
                                    textMatrix[i][j] = 0;
                                }

                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < matrixSize; j++)
                            {
                                if (map < textList.Count)
                                {
                                    textMatrix[j][i] = textList[map];
                                    map++;
                                }
                                else
                                {
                                    textMatrix[i][j] = 0;
                                }
                            }
                        }
                    }
                    return textMatrix;
            }

        }

        public List<List<int>> multiplyMatrices(List<List<int>> matrix1, List<List<int>> matrix2, int matrixSize)
        {
            List<List<int>> result = new List<List<int>>();
            int matrix1Count = matrix1.Count;
            int matrix1Columns = matrix1[0].Count;
            int matrix2Columns = matrix2[0].Count;

            Console.WriteLine(matrix1Count + " " + matrix1Columns + " " + matrix2Columns);

            for (int i = 0; i < matrix1Count; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < matrix2Columns; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < matrix1Columns; k++)
                    {
                        //Console.WriteLine("inverse: " + inverseMatrix[i][k] + "\tcipher: " + cipherTextMatrix[j][k]);
                        sum += matrix1[i][k] * matrix2[k][j];

                        sum %= 26;
                    }
                    row.Add(sum);
                }
                result.Add(row);
            }
            return result;
        }


        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            // Variables
            int plainSize = plainText.Count;
            int cipherSize = cipherText.Count;
            int plainMatrixSize = (int)Math.Sqrt(plainSize);
            int cipherMatrixSize = (int)Math.Sqrt(cipherSize);
            bool correctKey = true;

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


            // Cipher text matrix

            List<List<int>> cipherTextMatrix = createMatrix(cipherText, 2, false, false);

            // build plain text matrix with size (matrixSize rows X `columns` columns
            List<List<int>> plainTextMatrix = createMatrix(plainText, 2, false, false);

            List<int> key = new List<int>() { 0, 0, 0, 0 };
            List<int> ret = new List<int>();

            int x = 0;

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int m = 0; m < 26; m++)
                        {
                            key[0] = i; key[1] = j; key[2] = k; key[3] = m;
                            //correctKey = true;
                            List<int> result = Encrypt(plainText, key);
                            ret = result;
                            /*Console.Write("Key: " + correctKey + " ");
                            Console.WriteLine(key[0] + " " + key[1] + " " + key[2] + " " + key[3]);*/
                            for (int l = 0; l < cipherSize; l++)
                            {
                                correctKey = true;
                                /*Console.WriteLine(result[l] + "\t" + cipherText[l]);*/
                                if (result[l] != cipherText[l])
                                {
                                    //Console.WriteLine("NOT");
                                    x = 10;
                                    correctKey = false;
                                    break;
                                }
                            }
                            if (correctKey)
                            {
                                /*Console.WriteLine("YES");*/
                                return key;
                            }
                            /*Console.WriteLine("------");*/
                        }
                    }
                }
            }

            /*foreach (var num in ret)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();

            Console.WriteLine(x);

            Console.WriteLine(correctKey);

            if (correctKey)
                return key;
                */


            /*foreach (var row in test)
            {
                foreach (var num in row)
                {
                    Console.Write(num + " ");
                }
                Console.WriteLine();
            }*/

            /*List<List<int>> keyMatrix = multiplyMatrices(inversedPlainMatrix, cipherTextMatrix);*/

            throw new InvalidAnlysisException();

            //throw new NotImplementedException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            // Variables
            string capitalPlain = plainText.ToUpper();
            string capitalCipher = cipherText.ToUpper();
            List<int> keyNumbers = new List<int>();
            List<int> cipherTextNumber = new List<int>();
            List<int> plainTextNumbers = new List<int>();
            string resultString = "";

            // Create array holding letters
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

           

            capitalCipher = Regex.Replace(capitalCipher, @"\s+", ""); // Removes whitespaces, just in case

            foreach (var letter in capitalCipher)
                cipherTextNumber.Add(letters[letter]);

            
            capitalPlain = Regex.Replace(capitalPlain, @"\s+", ""); // Removes whitespaces

            foreach (var letter in capitalPlain)
                plainTextNumbers.Add(letters[letter]);

            List<int> result = Analyse(plainTextNumbers, cipherTextNumber);

            foreach (var c in result)
            {
                resultString += numbers[c];
            }

            return resultString;

            //throw new NotImplementedException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            // Variables
            int matrixSize = (int)Math.Sqrt(key.Count);
            List<int> plainArr = new List<int>();
            bool isKeyMatrix = true;

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

            List<List<int>> keyMatrix = createMatrix(key, matrixSize, isKeyMatrix, true);

            // Cipher text matrix

            List<List<int>> cipherTextMatrix = createMatrix(cipherText, matrixSize, !isKeyMatrix, false);


            //// Inverse matrix

            List<List<int>> inverseMatrix = getInverseMatrix(keyMatrix, matrixSize);

            // matrix multiplication
            List<List<int>> plain = multiplyMatrices(inverseMatrix, cipherTextMatrix, matrixSize);

            int plainCount = plain.Count;
            int plainColumns = plain[0].Count;

            // convert cipher numbers to letters
            for (int i = 0; i < plainColumns; i++)
            {
                for (int j = 0; j < plainCount; j++)
                {
                    plainArr.Add(plain[j][i]);
                }
            }

            return plainArr;

            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            // Variables
            string capitalCipher = cipherText.ToUpper();
            string capitalKey = key.ToUpper();
            int keySize = key.Length;
            int matrixSize = (int)Math.Sqrt(keySize);
            List<int> mappedCipherText = new List<int>();
            List<int> keyNumbers = new List<int>();
            List<List<int>> keyMatrix = new List<List<int>>();
            List<List<int>> cipherTextMatrix = new List<List<int>>();
            string plainText = "";
            List<List<int>> plain = new List<List<int>>();
            List<int> cipherTextNumber = new List<int>();

            // Create array holding letters
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

            //// Convert key to numbers
            foreach (char c in capitalKey)
            {
                keyNumbers.Add(letters[c]);
            }

            capitalCipher = Regex.Replace(capitalCipher, @"\s+", ""); // Removes whitespaces, just in case

            foreach (var letter in capitalCipher)
                cipherTextNumber.Add(letters[letter]);

            //// Split string into equal substrings
            List<string> matrixColumns = new List<string>();

            for (int i = 0; i < capitalCipher.Length; i += matrixSize)
            {
                int length = Math.Min(matrixSize, capitalCipher.Length - i);
                string newString = capitalCipher.Substring(i, length);

                /*if (length < matrixSize)
                {
                    for (int j = 0; j < matrixSize - length; j++)
                    {
                        newString += 'X'; // if substring is less than column size, add X
                    }
                }*/ // IDK really know if adding 'X' is neccessary or not

                matrixColumns.Add(newString);
            }

            //// Map substrings
            
            // same as in encryption
            foreach (string str in matrixColumns)
                foreach (char c in str)
                    mappedCipherText.Add(letters[c]);

            List<int> plainArr = Decrypt(cipherTextNumber, keyNumbers);

            // Convert numbers to strings
            foreach (var c in plainArr)
            {
                plainText += numbers[c];
            }

            return plainText;

            //throw new NotImplementedException();
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)

        {
            // Variables
            double rootKey = Math.Sqrt(key.Count);
            //Console.WriteLine(rootKey);
            int keySize = key.Count;
            int matrixSize = (int)Math.Sqrt(key.Count); // key length is always a perfect square (4, 9, 16, etc...)
            List<int> cipherArr = new List<int>();
            bool isKeyMatrix = true;

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

            List<List<int>> keyMatrix = createMatrix(key, matrixSize, true, true);



            // build plain text matrix with size (matrixSize rows X `columns` columns
            List<List<int>> plainTextMatrix = createMatrix(plainText, matrixSize, !isKeyMatrix, false);

            /*foreach (var row in plainTextMatrix)
            {
                foreach (var num in row)
                {
                    Console.Write(num + " ");
                }
                Console.WriteLine();
            }*/

            // matrix multiplication

            List<List<int>> cipher = multiplyMatrices(keyMatrix, plainTextMatrix, matrixSize);


            int cipherRow = cipher.Count;
            int cipherColumn = cipher[0].Count;

            // convert cipher numbers to letters
            for (int i = 0; i < cipherColumn; i++)
            {
                for (int j = 0; j < cipherRow; j++)
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
                letters[(char)(i + 65)] = i; // maps A -> Z to 0 -> 25
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
            // this is probably uselss, I don't remeber why I wrote it
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
            int plainSize = plain3.Count;
            int inverseSize = (int)Math.Sqrt(plainSize);
            int cipherSize = cipher3.Count;

            // Cipher text matrix
            List<List<int>> cipherTextMatrix = createMatrix(cipher3, 3, false, false);

            foreach (var row in cipherTextMatrix)
            {
                foreach (var num in row)
                {
                    Console.Write(num + " ");
                }
                Console.WriteLine();
            }
            // build plain text matrix with size
            List<List<int>> plainTextMatrix = createMatrix(plain3, 3, false, false);
            /*List<List<int>> plainTextMatrix = new List<List<int>>();
            plainTextMatrix.Add(new List<int> { 5, 5, 19 });
            plainTextMatrix.Add(new List<int> { 21, 2, 14 });
            plainTextMatrix.Add(new List<int> { 2, 16, 1 });
            Console.WriteLine("----");*/

            List<List<int>> inversePlainMatrix = getInverseMatrix(plainTextMatrix, 3);

            List<List<int>> keyMatrix = multiplyMatrices(cipherTextMatrix, inversePlainMatrix, 3);
            foreach (var row in keyMatrix)
            {
                foreach (var num in row)
                {
                    Console.Write(num + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("_____");

            List<int> key = new List<int>();

            foreach (var row in keyMatrix)
            {
                foreach (var num in row)
                {
                    key.Add(num);
                }
            }

            List<int> test = Encrypt(plain3, key);

            foreach (var num in test)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine("\n------");

            return key;
            //throw new NotImplementedException();

            //throw new NotImplementedException();
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }
    }
}
