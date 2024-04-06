using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {

            // make sure that the cipher text and key don't start with "0x"
            if (cipherText.StartsWith("0x"))
            {
                cipherText = cipherText.Substring(2);
            }

            if (key.StartsWith("0x"))
            {
                key = key.Substring(2);
            }

            // make cipher text and key upper case
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();

            // generate state matrix with the cipher text and key
            string[,] cipherState = generateStateMatrix(cipherText);
            string[,] keyState = generateStateMatrix(key);

            // generate round keys in reverse order
            for (int i = 0; i <= 9; i++)
            {
                keyState = GenerateRoundKey(keyState, i);
            }

            // add initial round key
            cipherState = addRoundKey(cipherState, keyState);

            // perform rounds in reverse order
            for (int i = 9; i > 0; i--)
            {
                // reverse operations of encryption rounds
                cipherState = shiftRows(cipherState, true);
                cipherState = subBytes(cipherState, true);

                // regenerate round key for each round
                keyState = generateStateMatrix(key);
                for (int j = 0; j < i; j++)
                {
                    keyState = GenerateRoundKey(keyState, j);
                }

                // add round key
                cipherState = addRoundKey(keyState, cipherState);

                // reverse mix columns operation
                cipherState = mixColumns(cipherState, true);
            }

            // final round operations (without mix columns)
            cipherState = shiftRows(cipherState, true);
            cipherState = subBytes(cipherState, true);
            keyState = generateStateMatrix(key);
            cipherState = addRoundKey(keyState, cipherState);

            // convert the state matrix to result
            string result = "0x";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result += cipherState[j, i];
                }
            }

            return result;

            //throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            // make sure that the plain and key doesn't start with 0x
            if (plainText.StartsWith("0x"))
            {
                plainText = plainText.Substring(2);
            }

            if (key.StartsWith("0x"))
            {
                key = key.Substring(2);
            }

            // make plain and key upper case
            plainText = plainText.ToUpper();
            key = key.ToUpper();

            // generate state matrix with the plain and key
            string[,] plainState = generateStateMatrix(plainText);
            string[,] keyState = generateStateMatrix(key);

            // (1) add round key
            plainState = addRoundKey(plainState, keyState);

            for (int i = 0; i < 9; i++)
            {
                // (2) sub bytes
                plainState = subBytes(plainState, false);

                // (3) shift rows
                plainState = shiftRows(plainState, false);

                // (4) mix columns
                plainState = mixColumns(plainState, false);

                // (5) generate round key
                keyState = GenerateRoundKey(keyState, i);

                // (6) add round key
                plainState = addRoundKey(plainState, keyState);
            }

            // (7) sub bytes
            plainState = subBytes(plainState, false);

            // (8) shift rows
            plainState = shiftRows(plainState, false);

            // (9) generate round key
            keyState = GenerateRoundKey(keyState, 9);

            // (10) add round key
            plainState = addRoundKey(plainState, keyState);

            // convert the state matrix to result
            string result = "0x";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result += plainState[j, i];
                }
            }

            return result;
            //throw new NotImplementedException();
        }

        string[,] GenerateRoundKey(string[,] previousKey, int RconIndex)
        {

            // w[0] = old key w[0] XOR Sub bytes (old key w3 >> 8) XOR Rcon[RconIndex]
            // w[i] = old key w[i] XOR current key w[i-1]

            // Rcon Array with hexa values
            string[] Rcon = new string[] { "01000000", "02000000", "04000000", "08000000", "10000000", "20000000", "40000000", "80000000", "1B000000", "36000000" };
            Dictionary<string, string> sBox = new Dictionary<string, string>()
        {
            {"00", "63"}, {"01", "7C"}, {"02", "77"}, {"03", "7B"}, {"04", "F2"}, {"05", "6B"}, {"06", "6F"}, {"07", "C5"},
            {"08", "30"}, {"09", "01"}, {"0A", "67"}, {"0B", "2B"}, {"0C", "FE"}, {"0D", "D7"}, {"0E", "AB"}, {"0F", "76"},
            {"10", "CA"}, {"11", "82"}, {"12", "C9"}, {"13", "7D"}, {"14", "FA"}, {"15", "59"}, {"16", "47"}, {"17", "F0"},
            {"18", "AD"}, {"19", "D4"}, {"1A", "A2"}, {"1B", "AF"}, {"1C", "9C"}, {"1D", "A4"}, {"1E", "72"}, {"1F", "C0"},
            {"20", "B7"}, {"21", "FD"}, {"22", "93"}, {"23", "26"}, {"24", "36"}, {"25", "3F"}, {"26", "F7"}, {"27", "CC"},
            {"28", "34"}, {"29", "A5"}, {"2A", "E5"}, {"2B", "F1"}, {"2C", "71"}, {"2D", "D8"}, {"2E", "31"}, {"2F", "15"},
            {"30", "04"}, {"31", "C7"}, {"32", "23"}, {"33", "C3"}, {"34", "18"}, {"35", "96"}, {"36", "05"}, {"37", "9A"},
            {"38", "07"}, {"39", "12"}, {"3A", "80"}, {"3B", "E2"}, {"3C", "EB"}, {"3D", "27"}, {"3E", "B2"}, {"3F", "75"},
            {"40", "09"}, {"41", "83"}, {"42", "2C"}, {"43", "1A"}, {"44", "1B"}, {"45", "6E"}, {"46", "5A"}, {"47", "A0"},
            {"48", "52"}, {"49", "3B"}, {"4A", "D6"}, {"4B", "B3"}, {"4C", "29"}, {"4D", "E3"}, {"4E", "2F"}, {"4F", "84"},
            {"50", "53"}, {"51", "D1"}, {"52", "00"}, {"53", "ED"}, {"54", "20"}, {"55", "FC"}, {"56", "B1"}, {"57", "5B"},
            {"58", "6A"}, {"59", "CB"}, {"5A", "BE"}, {"5B", "39"}, {"5C", "4A"}, {"5D", "4C"}, {"5E", "58"}, {"5F", "CF"},
            {"60", "D0"}, {"61", "EF"}, {"62", "AA"}, {"63", "FB"}, {"64", "43"}, {"65", "4D"}, {"66", "33"}, {"67", "85"},
            {"68", "45"}, {"69", "F9"}, {"6A", "02"}, {"6B", "7F"}, {"6C", "50"}, {"6D", "3C"}, {"6E", "9F"}, {"6F", "A8"},
            {"70", "51"}, {"71", "A3"}, {"72", "40"}, {"73", "8F"}, {"74", "92"}, {"75", "9D"}, {"76", "38"}, {"77", "F5"},
            {"78", "BC"}, {"79", "B6"}, {"7A", "DA"}, {"7B", "21"}, {"7C", "10"}, {"7D", "FF"}, {"7E", "F3"}, {"7F", "D2"},
            {"80", "CD"}, {"81", "0C"}, {"82", "13"}, {"83", "EC"}, {"84", "5F"}, {"85", "97"}, {"86", "44"}, {"87", "17"},
            {"88", "C4"}, {"89", "A7"}, {"8A", "7E"}, {"8B", "3D"}, {"8C", "64"}, {"8D", "5D"}, {"8E", "19"}, {"8F", "73"},
            {"90", "60"}, {"91", "81"}, {"92", "4F"}, {"93", "DC"}, {"94", "22"}, {"95", "2A"}, {"96", "90"}, {"97", "88"},
            {"98", "46"}, {"99", "EE"}, {"9A", "B8"}, {"9B", "14"}, {"9C", "DE"}, {"9D", "5E"}, {"9E", "0B"}, {"9F", "DB"},
            {"A0", "E0"}, {"A1", "32"}, {"A2", "3A"}, {"A3", "0A"}, {"A4", "49"}, {"A5", "06"}, {"A6", "24"}, {"A7", "5C"},
            {"A8", "C2"}, {"A9", "D3"}, {"AA", "AC"}, {"AB", "62"}, {"AC", "91"}, {"AD", "95"}, {"AE", "E4"}, {"AF", "79"},
            {"B0", "E7"}, {"B1", "C8"}, {"B2", "37"}, {"B3", "6D"}, {"B4", "8D"}, {"B5", "D5"}, {"B6", "4E"}, {"B7", "A9"},
            {"B8", "6C"}, {"B9", "56"}, {"BA", "F4"}, {"BB", "EA"}, {"BC", "65"}, {"BD", "7A"}, {"BE", "AE"}, {"BF", "08"},
            {"C0", "BA"}, {"C1", "78"}, {"C2", "25"}, {"C3", "2E"}, {"C4", "1C"}, {"C5", "A6"}, {"C6", "B4"}, {"C7", "C6"},
            {"C8", "E8"}, {"C9", "DD"}, {"CA", "74"}, {"CB", "1F"}, {"CC", "4B"}, {"CD", "BD"}, {"CE", "8B"}, {"CF", "8A"},
            {"D0", "70"}, {"D1", "3E"}, {"D2", "B5"}, {"D3", "66"}, {"D4", "48"}, {"D5", "03"}, {"D6", "F6"}, {"D7", "0E"},
            {"D8", "61"}, {"D9", "35"}, {"DA", "57"}, {"DB", "B9"}, {"DC", "86"}, {"DD", "C1"}, {"DE", "1D"}, {"DF", "9E"},
            {"E0", "E1"}, {"E1", "F8"}, {"E2", "98"}, {"E3", "11"}, {"E4", "69"}, {"E5", "D9"}, {"E6", "8E"}, {"E7", "94"},
            {"E8", "9B"}, {"E9", "1E"}, {"EA", "87"}, {"EB", "E9"}, {"EC", "CE"}, {"ED", "55"}, {"EE", "28"}, {"EF", "DF"},
            {"F0", "8C"}, {"F1", "A1"}, {"F2", "89"}, {"F3", "0D"}, {"F4", "BF"}, {"F5", "E6"}, {"F6", "42"}, {"F7", "68"},
            {"F8", "41"}, {"F9", "99"}, {"FA", "2D"}, {"FB", "0F"}, {"FC", "B0"}, {"FD", "54"}, {"FE", "BB"}, {"FF", "16"} };


            string[,] generated_key = new string[4, 4];
            // first, deal with the first column
            // the last column of the previous key (w3) and shift it to the right
            string[] oldKeyword3 = new string[4];

            for (int i = 1; i < 4; i++)
            {
                oldKeyword3[i - 1] = previousKey[i, 3]; // Corrected index
            }

            oldKeyword3[3] = previousKey[0, 3]; // Corrected index

            string modified_w3 = "";
            for (int i = 0; i < 4; i++)
            {
                modified_w3 += sBox[oldKeyword3[i]];
            }

            // XOR the modified w3 with Rcon[RconIndex]
            string RconValue = Rcon[RconIndex];
            string result = XOR(convertToBinary(modified_w3), convertToBinary(RconValue)); // Corrected function name

            // getting the first column of the previous key
            string oldKeyword0 = "";
            for (int i = 0; i < 4; i++)
            {
                oldKeyword0 += previousKey[i, 0];
            }

            // XOR the result with the first column of the previous key
            result = convertToHex(XOR(result, convertToBinary(oldKeyword0))); // Corrected function name

            // fill the first column of the generated key
            for (int i = 0; i < 4; i++)
            {
                generated_key[i, 0] = result.Substring(i * 2, 2);
            }


            // deal with the rest of the columns
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    generated_key[j, i] = convertToHex(XOR(convertToBinary(previousKey[j, i]), convertToBinary(generated_key[j, i - 1]))); // Corrected function name
                }
            }

            return generated_key;
        }

        string multiplyBinaryByHex(string s1, string by)
        {
            if (by == "01")
            {
                return s1;
            }

            if (by == "02")
            {
                if (s1[0] == '0')
                {
                    return s1.Substring(1, s1.Length - 1) + "0";
                }
                else
                {
                    return XOR(s1.Substring(1, s1.Length - 1) + "0", "00011011");
                }
            }

            if (by == "03")
            {
                return XOR(multiplyBinaryByHex(s1, "02"), s1);
            }

            if (by == "09")
            {
                return XOR(multiplyBinaryByHex(multiplyBinaryByHex(multiplyBinaryByHex(s1, "02"), "02"), "02"), s1); //x*9=(((x*2)*2)*2)+x
            }

            if (by == "0B")
            {
                return XOR(multiplyBinaryByHex(XOR(multiplyBinaryByHex(multiplyBinaryByHex(s1, "02"), "02"), s1), "02"), s1); //x*11=((((x*2)*2)+x)*2)+x
            }

            if (by == "0D")
            {
                return XOR(multiplyBinaryByHex(multiplyBinaryByHex(multiplyBinaryByHex(s1, "03"), "02"), "02"), s1); //x*13=(((x*3)×2)×2)+x
            }

            if (by == "0E")
            {
                return multiplyBinaryByHex(XOR(multiplyBinaryByHex(multiplyBinaryByHex(s1, "03"), "02"), s1), "02"); //x*14=(((x*3)×2)+x)×2
            }

            return string.Empty;
        }

        string[,] mixColumns(string[,] text, bool inverse)
        {
            string[,] result = new string[4, 4];
            string[,] mixColumnMatrix = new string[4, 4]
            {
        {"02", "03", "01", "01"},
        {"01", "02", "03", "01"},
        {"01", "01", "02", "03"},
        {"03", "01", "01", "02"}
            };

            if (inverse == true)
            {
                // Use inverse mix column matrix
                mixColumnMatrix = new string[4, 4]
                {
            {"0E", "0B", "0D", "09"},
            {"09", "0E", "0B", "0D"},
            {"0D", "09", "0E", "0B"},
            {"0B", "0D", "09", "0E"}
                };
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        string binary = convertToBinary(text[k, j]);
                        string hex = mixColumnMatrix[i, k];
                        if (result[i, j] == null)
                        {
                            result[i, j] = multiplyBinaryByHex(binary, hex);
                        }
                        else
                        {
                            result[i, j] = XOR(multiplyBinaryByHex(binary, hex), result[i, j]);
                        }
                    }

                    result[i, j] = convertToHex(result[i, j]);
                }
            }

            return result;
        }

        string[,] shiftRows(string[,] text, bool inverse)
        {
            string[,] result = new string[4, 4];

            if (inverse == true)
            {
                // Perform inverse shift rows
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        result[i, j] = text[i, (j - i + 4) % 4];
                    }
                }
            }
            else
            {
                // Perform normal shift rows
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        result[i, j] = text[i, (j + i) % 4];
                    }
                }
            }

            return result;
        }


        string[,] subBytes(string[,] text, bool inverse)
        {
            Dictionary<string, string> sBox = new Dictionary<string, string>()
    {
        {"00", "63"}, {"01", "7C"}, {"02", "77"}, {"03", "7B"}, {"04", "F2"}, {"05", "6B"}, {"06", "6F"}, {"07", "C5"},
        {"08", "30"}, {"09", "01"}, {"0A", "67"}, {"0B", "2B"}, {"0C", "FE"}, {"0D", "D7"}, {"0E", "AB"}, {"0F", "76"},
        {"10", "CA"}, {"11", "82"}, {"12", "C9"}, {"13", "7D"}, {"14", "FA"}, {"15", "59"}, {"16", "47"}, {"17", "F0"},
        {"18", "AD"}, {"19", "D4"}, {"1A", "A2"}, {"1B", "AF"}, {"1C", "9C"}, {"1D", "A4"}, {"1E", "72"}, {"1F", "C0"},
        {"20", "B7"}, {"21", "FD"}, {"22", "93"}, {"23", "26"}, {"24", "36"}, {"25", "3F"}, {"26", "F7"}, {"27", "CC"},
        {"28", "34"}, {"29", "A5"}, {"2A", "E5"}, {"2B", "F1"}, {"2C", "71"}, {"2D", "D8"}, {"2E", "31"}, {"2F", "15"},
        {"30", "04"}, {"31", "C7"}, {"32", "23"}, {"33", "C3"}, {"34", "18"}, {"35", "96"}, {"36", "05"}, {"37", "9A"},
        {"38", "07"}, {"39", "12"}, {"3A", "80"}, {"3B", "E2"}, {"3C", "EB"}, {"3D", "27"}, {"3E", "B2"}, {"3F", "75"},
        {"40", "09"}, {"41", "83"}, {"42", "2C"}, {"43", "1A"}, {"44", "1B"}, {"45", "6E"}, {"46", "5A"}, {"47", "A0"},
        {"48", "52"}, {"49", "3B"}, {"4A", "D6"}, {"4B", "B3"}, {"4C", "29"}, {"4D", "E3"}, {"4E", "2F"}, {"4F", "84"},
        {"50", "53"}, {"51", "D1"}, {"52", "00"}, {"53", "ED"}, {"54", "20"}, {"55", "FC"}, {"56", "B1"}, {"57", "5B"},
        {"58", "6A"}, {"59", "CB"}, {"5A", "BE"}, {"5B", "39"}, {"5C", "4A"}, {"5D", "4C"}, {"5E", "58"}, {"5F", "CF"},
        {"60", "D0"}, {"61", "EF"}, {"62", "AA"}, {"63", "FB"}, {"64", "43"}, {"65", "4D"}, {"66", "33"}, {"67", "85"},
        {"68", "45"}, {"69", "F9"}, {"6A", "02"}, {"6B", "7F"}, {"6C", "50"}, {"6D", "3C"}, {"6E", "9F"}, {"6F", "A8"},
        {"70", "51"}, {"71", "A3"}, {"72", "40"}, {"73", "8F"}, {"74", "92"}, {"75", "9D"}, {"76", "38"}, {"77", "F5"},
        {"78", "BC"}, {"79", "B6"}, {"7A", "DA"}, {"7B", "21"}, {"7C", "10"}, {"7D", "FF"}, {"7E", "F3"}, {"7F", "D2"},
        {"80", "CD"}, {"81", "0C"}, {"82", "13"}, {"83", "EC"}, {"84", "5F"}, {"85", "97"}, {"86", "44"}, {"87", "17"},
        {"88", "C4"}, {"89", "A7"}, {"8A", "7E"}, {"8B", "3D"}, {"8C", "64"}, {"8D", "5D"}, {"8E", "19"}, {"8F", "73"},
        {"90", "60"}, {"91", "81"}, {"92", "4F"}, {"93", "DC"}, {"94", "22"}, {"95", "2A"}, {"96", "90"}, {"97", "88"},
        {"98", "46"}, {"99", "EE"}, {"9A", "B8"}, {"9B", "14"}, {"9C", "DE"}, {"9D", "5E"}, {"9E", "0B"}, {"9F", "DB"},
        {"A0", "E0"}, {"A1", "32"}, {"A2", "3A"}, {"A3", "0A"}, {"A4", "49"}, {"A5", "06"}, {"A6", "24"}, {"A7", "5C"},
        {"A8", "C2"}, {"A9", "D3"}, {"AA", "AC"}, {"AB", "62"}, {"AC", "91"}, {"AD", "95"}, {"AE", "E4"}, {"AF", "79"},
        {"B0", "E7"}, {"B1", "C8"}, {"B2", "37"}, {"B3", "6D"}, {"B4", "8D"}, {"B5", "D5"}, {"B6", "4E"}, {"B7", "A9"},
        {"B8", "6C"}, {"B9", "56"}, {"BA", "F4"}, {"BB", "EA"}, {"BC", "65"}, {"BD", "7A"}, {"BE", "AE"}, {"BF", "08"},
        {"C0", "BA"}, {"C1", "78"}, {"C2", "25"}, {"C3", "2E"}, {"C4", "1C"}, {"C5", "A6"}, {"C6", "B4"}, {"C7", "C6"},
        {"C8", "E8"}, {"C9", "DD"}, {"CA", "74"}, {"CB", "1F"}, {"CC", "4B"}, {"CD", "BD"}, {"CE", "8B"}, {"CF", "8A"},
        {"D0", "70"}, {"D1", "3E"}, {"D2", "B5"}, {"D3", "66"}, {"D4", "48"}, {"D5", "03"}, {"D6", "F6"}, {"D7", "0E"},
        {"D8", "61"}, {"D9", "35"}, {"DA", "57"}, {"DB", "B9"}, {"DC", "86"}, {"DD", "C1"}, {"DE", "1D"}, {"DF", "9E"},
        {"E0", "E1"}, {"E1", "F8"}, {"E2", "98"}, {"E3", "11"}, {"E4", "69"}, {"E5", "D9"}, {"E6", "8E"}, {"E7", "94"},
        {"E8", "9B"}, {"E9", "1E"}, {"EA", "87"}, {"EB", "E9"}, {"EC", "CE"}, {"ED", "55"}, {"EE", "28"}, {"EF", "DF"},
        {"F0", "8C"}, {"F1", "A1"}, {"F2", "89"}, {"F3", "0D"}, {"F4", "BF"}, {"F5", "E6"}, {"F6", "42"}, {"F7", "68"},
        {"F8", "41"}, {"F9", "99"}, {"FA", "2D"}, {"FB", "0F"}, {"FC", "B0"}, {"FD", "54"}, {"FE", "BB"}, {"FF", "16"}
    };

            if (inverse == true)
            {
                // Perform inverse operation
                Dictionary<string, string> inverseSBox = sBox.ToDictionary(kv => kv.Value, kv => kv.Key);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        text[j, i] = inverseSBox[text[j, i]];
                    }
                }
            }
            else
            {
                // Perform normal operation
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        text[j, i] = sBox[text[j, i]];
                    }
                }
            }

            return text;
        }

        string[,] generateStateMatrix(string text)
        {

            string[,] state = new string[4, 4];
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[j, i] = text.Substring(index, 2);
                    index += 2;
                }
            }
            return state;

        }

        string convertToHex(string binary)
        {

            string hex = "";
            int size = binary.Length;
            for (int i = 0; i < size; i += 4)
            {
                string temp = binary.Substring(i, 4);
                switch (temp)
                {
                    case "0000":
                        hex += "0";
                        break;
                    case "0001":
                        hex += "1";
                        break;
                    case "0010":
                        hex += "2";
                        break;
                    case "0011":
                        hex += "3";
                        break;
                    case "0100":
                        hex += "4";
                        break;
                    case "0101":
                        hex += "5";
                        break;
                    case "0110":
                        hex += "6";
                        break;
                    case "0111":
                        hex += "7";
                        break;
                    case "1000":
                        hex += "8";
                        break;
                    case "1001":
                        hex += "9";
                        break;
                    case "1010":
                        hex += "A";
                        break;
                    case "1011":
                        hex += "B";
                        break;
                    case "1100":
                        hex += "C";
                        break;
                    case "1101":
                        hex += "D";
                        break;
                    case "1110":
                        hex += "E";
                        break;
                    case "1111":
                        hex += "F";
                        break;
                }
            }
            return hex;

        }

        string convertToBinary(string hex)
        {
            string binary = "";
            int size = hex.Length;
            for (int i = 0; i < size; i++)
            {
                switch (hex[i])
                {
                    case '0':
                        binary += "0000";
                        break;
                    case '1':
                        binary += "0001";
                        break;
                    case '2':
                        binary += "0010";
                        break;
                    case '3':
                        binary += "0011";
                        break;
                    case '4':
                        binary += "0100";
                        break;
                    case '5':
                        binary += "0101";
                        break;
                    case '6':
                        binary += "0110";
                        break;
                    case '7':
                        binary += "0111";
                        break;
                    case '8':
                        binary += "1000";
                        break;
                    case '9':
                        binary += "1001";
                        break;
                    case 'A':
                        binary += "1010";
                        break;
                    case 'B':
                        binary += "1011";
                        break;
                    case 'C':
                        binary += "1100";
                        break;
                    case 'D':
                        binary += "1101";
                        break;
                    case 'E':
                        binary += "1110";
                        break;
                    case 'F':
                        binary += "1111";
                        break;
                }
            }
            return binary;
        }

        string XOR(string bin1, string bin2)
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

        string[,] addRoundKey(string[,] text, string[,] key)
        {
            string[,] result = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[j, i] = XOR(convertToBinary(text[j, i]), convertToBinary(key[j, i]));
                }
            }

            // convert the result to hex
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[j, i] = convertToHex(result[j, i]);
                }
            }

            return result;

        }

    }
}
