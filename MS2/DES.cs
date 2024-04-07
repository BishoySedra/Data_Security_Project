using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {
        public static int[] PC_1 = {57, 49, 41, 33, 25, 17, 9,
                             1, 58, 50, 42, 34, 26, 18,
                             10, 2, 59, 51, 43, 35, 27,
                             19, 11, 3 ,60 ,52, 44, 36,
                             63, 55, 47, 39, 31, 23, 15,
                             7, 62, 54, 46, 38, 30 ,22,
                             14, 6, 61, 53, 45, 37, 29,
                             21, 13, 5, 28, 20, 12, 4}; 


        public static int[] PC_2 = {14, 17, 11, 24, 1, 5,
                                     3, 28, 15, 6, 21, 10,
                                    23, 19, 12, 4, 26, 8,
                                    16, 7, 27, 20, 13, 2,
                                    41, 52, 31, 37, 47, 55,
                                    30, 40, 51, 45, 33, 48,
                                    44, 49, 39, 56, 34, 53,
                                    46, 42, 50, 36, 29, 32};

        public static int[] text_ip = new int[]{
                                    58,50,42,34,26,18,10,2,
                                    60,52,44,36,28,20,12,4,
                                    62,54,46,38,30,22,14,6,
                                    64,56,48,40,32,24,16,8,
                                    57,49,41,33,25,17,9,1,
                                    59,51,43,35,27,19,11,3,
                                    61,53,45,37,29,21,13,5,
                                    63,55,47,39,31,23,15,7};

        public static int[] expansion_table = new int[]{
                                    32,1,2,3,4,5,4,5,
                                    6,7,8,9,8,9,10,11,
                                    12,13,12,13,14,15,16,17,
                                    16,17,18,19,20,21,20,21,
                                    22,23,24,25,24,25,26,27,
                                    28,29,28,29,30,31,32,1};


        public static int[] invereIP= {40, 8 ,48, 16, 56, 24, 64 ,32,
                                    39, 7, 47 ,15 ,55 ,23, 63, 31,
                                    38, 6 ,46 ,14 ,54 ,22 ,62, 30,
                                    37, 5 ,45 ,13 ,53 ,21 ,61, 29,
                                    36, 4 ,44 ,12 ,52 ,20, 60, 28,
                                    35, 3 ,43 ,11 ,51 ,19, 59, 27,
                                    34, 2 ,42 ,10 ,50 ,18, 58, 26,
                                    33, 1 ,41 ,9 ,49 ,17 ,57, 25};

        public static int[,,] sbox = new int[,,]
                {
                    {
                        { 14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7 },
                        { 0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8 },
                        { 4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0 },
                        { 15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13 }
                    },
                    {
                        { 15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10 },
                        { 3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5 },
                        { 0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15 },
                        { 13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9}
                    },
                    {
                        { 10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8 },
                        { 13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1 },
                        { 13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7 },
                        { 1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12 }
                    },
                    {
                        { 7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15 },
                        { 13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9 },
                        { 10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4 },
                        { 3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14 }
                    },
                    {
                        { 2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9 },
                        { 14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6 },
                        { 4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14 },
                        { 11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3 }
                    },
                    {
                        { 12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11 },
                        { 10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8 },
                        { 9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6 },
                        { 4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13 }
                    },
                    {
                        { 4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1},
                        { 13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6},
                        { 1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2},
                        { 6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12}
                    },
                    {
                        { 13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7 },
                        { 1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2 },
                        { 7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8 },
                        { 2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11 }
                    }
            };


        public static int[] sbox_permutation = new int[]{
                                16,7,20,21,29,12,28,17,
                                1,15,23,26,5,18,31,10,
                                2,8,24,14,32,27,3,9,
                                19,13,30,6,22,11,4,25};

        public static string PC1(string key)
        {
            string permutedKey = "";

            for (int i = 0; i < PC_1.Length; i++)
            {
                int keyIndex = PC_1[i] - 1;
                permutedKey += key[keyIndex];
            }
            return permutedKey; // C0D0
        }

        public static string GenerateSubKey(string permutedKey, int iterationNumber)
        {

            // Convert key to 2 subkeys of length 28

            string subKeyLeft = "";
            string subKeyRight = "";
            string subKey;
            string key = "";

            subKeyLeft = permutedKey.Substring(0, 28); // Ci
            subKeyRight = permutedKey.Substring(28, 28); // Di

            /*for (int i = 0; i < PC_1.Length / 2; i++)
                {
                    int originalKeyIndex = PC_1[i] - 1;
                    subKeyLeft += originalKey[originalKeyIndex];
                }

                for (int i = 28; i < PC_1.Length; i++)
                {
                    int originalKeyIndex = PC_1[i] - 1;
                    subKeyRight += originalKey[originalKeyIndex];
                }*/


            // Shift left rotate 

            if (iterationNumber == 1 || iterationNumber == 2 || iterationNumber == 9 || iterationNumber == 16)
            {
                char firstBitLeft = subKeyLeft[0];
                subKeyLeft = subKeyLeft.Substring(1, 27) + firstBitLeft;

                char firstBitRight = subKeyRight[0];
                subKeyRight = subKeyRight.Substring(1, 27) + firstBitRight;

            }
            else
            {
                string firstTwoBitsLeft = subKeyLeft.Substring(0, 2);
                subKeyLeft = subKeyLeft.Substring(2, 26) + firstTwoBitsLeft;

                string firstTwoBitsRight = subKeyRight.Substring(0, 2);
                subKeyRight = subKeyRight.Substring(2, 26) + firstTwoBitsRight;

            }

            subKey = subKeyLeft + subKeyRight;

            return subKey;
        }


        public static string PC2(string key)
        {
            // Form key
            string permutedKey = "";

            for (int i = 0; i < PC_2.Length; i++)
            {
                int subKeyIndex = PC_2[i] - 1;
                permutedKey += key[subKeyIndex];
            }

            return permutedKey;
        }


        public static string HexToBinary(string hexCode)
        {
            // Remove "0x" prefix if present
            if (hexCode.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                hexCode = hexCode.Substring(2);
            }
            string binaryCode = Convert.ToString(Convert.ToInt64(hexCode, 16), 2).PadLeft(hexCode.Length * 4, '0');
            return binaryCode;
        }

        public static string BinaryToHex(string binary)
        {
            // Pad the binary string with leading zeros to make its length divisible by 4
            int remainder = binary.Length % 4;
            if (remainder != 0)
            {
                binary = binary.PadLeft(binary.Length + (4 - remainder), '0');
            }

            // Initialize a StringBuilder to store the hexadecimal result
            System.Text.StringBuilder hex = new System.Text.StringBuilder();

            // Iterate over the binary string in groups of 4 from left to right
            for (int i = 0; i < binary.Length; i += 4)
            {
                // Extract a group of 4 binary digits
                string binaryChunk = binary.Substring(i, 4);

                // Convert the binary chunk to its decimal equivalent
                int decimalValue = Convert.ToInt32(binaryChunk, 2);

                // Append the hexadecimal representation to the result
                hex.Append(decimalValue.ToString("X"));
            }

            // Add '0x' prefix to the hexadecimal string
            return "0x" + hex.ToString();
        }

        public static string DecimalToBinary(int decimalNumber)
        {
            // Handle the case of zero separately
            if (decimalNumber == 0)
            {
                return "0";
            }

            // Create a StringBuilder to store the binary digits
            StringBuilder binaryBuilder = new StringBuilder();

            // Convert the decimal number to binary by repeated division by 2
            while (decimalNumber > 0)
            {
                // Get the remainder when dividing by 2
                int remainder = decimalNumber % 2;

                // Prepend the remainder to the binary representation
                binaryBuilder.Insert(0, remainder);

                // Divide the decimal number by 2
                decimalNumber /= 2;
            }

            // Return the binary representation as a string
            return binaryBuilder.ToString();
        }

        public static int BinaryToDecimal(string binary)
        {
            int decimalNumber = 0;
            int exponent = 0;

            // Iterate through the binary string from right to left
            for (int i = binary.Length - 1; i >= 0; i--)
            {
                // Check if the character is '1'
                if (binary[i] == '1')
                {
                    // Add 2^exponent to the decimal number
                    decimalNumber += (int)Math.Pow(2, exponent);
                }

                // Increment the exponent for the next position
                exponent++;
            }

            return decimalNumber;
        }


        public static string Permute(string originalText)
        {
            string permutedText = "";

            for (int i = 0; i < text_ip.Length; i++)
            {
                int textIndex = text_ip[i] - 1;
                permutedText += originalText[textIndex];
            }
            return permutedText;
        }

        public static string InversePermute(string cipherText)
        {
            string permutedCipher = "";

            for (int i = 0; i < invereIP.Length; i++)
            {
                int cipherIndex = invereIP[i] - 1;
                permutedCipher += cipherText[cipherIndex];
            }
            return permutedCipher;
        }

        public static string XOR(string bin1, string bin2)
        {
            string result = "";
            for (int i = 0; i < bin1.Length; i++)
            {
                if (bin1[i] == bin2[i])
                {
                    result += "0";
                }
                else
                {
                    result += "1";
                }
            }
            return result;
        }

        public static string ExpandRightPart(string subKey, string rightPart)
        {
            string expandedRight = "";

            for (int i = 0; i < expansion_table.Length; i++)
            {
                int textIndex = expansion_table[i] - 1;
                expandedRight += rightPart[textIndex];
            }
            expandedRight = XOR(expandedRight, subKey);
            return expandedRight;
        }

        public static string SBox(string rightPart)
        {
            string[] subStrings = new string[8];
            string sBoxed = "";

            int value = 0;
            for (int i = 0; i < 8; i++)
            {
                subStrings[i] = rightPart.Substring(i * 6, 6);
                string row;
                string column;
                row = subStrings[i].First().ToString() + subStrings[i].Last().ToString();
                column = subStrings[i].Substring(1, 4);

                int rowNum = BinaryToDecimal(row);
                int columnNum = BinaryToDecimal(column);

                /*Console.Write(i + "\t" + rowNum + "\t" + columnNum + "\t");

                Console.WriteLine(sbox[i, rowNum, columnNum]);
                */

                value = sbox[i, rowNum, columnNum];

                string SBoxOutput = DecimalToBinary(value);

                sBoxed += SBoxOutput.PadLeft(4, '0');
            }
            return sBoxed;
        }

        public static string PermuteAfterSBox(string sBoxed)
        {
            string permutedSBoxed = "";

            for (int i = 0; i < sbox_permutation.Length; i++)
            {
                int textIndex = sbox_permutation[i] - 1;
                permutedSBoxed += sBoxed[textIndex];
            }
            return permutedSBoxed;
        }

        public override string Decrypt(string cipherText, string key)
        {

            string binaryKey = HexToBinary(key);
            string binaryCipher = HexToBinary(cipherText);

            
            string permutedCipher = Permute(binaryCipher); // IP

            string leftCipher = permutedCipher.Substring(0, 32);
            string rightCipher = permutedCipher.Substring(32, 32);

            string[] keyArr = new string[17];
            binaryKey = PC1(binaryKey);
            string subKey = binaryKey;

            for (int i = 1; i <= 16; i++)
            {
                subKey = GenerateSubKey(subKey, i);
                keyArr[i] = subKey;
            }


            for (int i = 16; i >= 1; i--)
            {
                // Generate subkey
                //subKey = GenerateSubKey(subKey, i);
                // Permute subkey
                string permutedTwo = PC2(keyArr[i]);
                // Expand right part
                string expandedRight = ExpandRightPart(permutedTwo, rightCipher);
                // SBox
                string sBoxed = SBox(expandedRight);
                // Permute after SBox
                string permutedSBoxed = PermuteAfterSBox(sBoxed);
                // XOR
                string newRight = XOR(leftCipher, permutedSBoxed);
                // Update left and right
                leftCipher = rightCipher; //L16
                rightCipher = newRight; //R16



            }

            string x = rightCipher + leftCipher;

            string plain = InversePermute(x);

            plain = BinaryToHex(plain);
            //plain = "0x" + plain;

            return plain;

            //throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            string binaryKey = HexToBinary(key);
            string binaryText = HexToBinary(plainText);

            string permutedText = Permute(binaryText);

            string leftText = permutedText.Substring(0, 32);
            string rightText = permutedText.Substring(32, 32);


            binaryKey = PC1(binaryKey); //C0D0

            string subKey = binaryKey;
            for (int i = 1; i <= 16; i++)
            {
                subKey = GenerateSubKey(subKey, i);
                string permutedTwo = PC2(subKey);
                string expandedRight = ExpandRightPart(permutedTwo, rightText);
                string sBoxed = SBox(expandedRight);
                string permutedSBoxed = PermuteAfterSBox(sBoxed);
                string newRight = XOR(leftText, permutedSBoxed);
                leftText = rightText; //L16
                rightText = newRight; //R16
            }

            string x = rightText + leftText;

            string cipher = InversePermute(x);

            cipher = BinaryToHex(cipher);
            //cipher = "0x" + cipher;

            return cipher;
            //throw new NotImplementedException();
        }
    }
}
