using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        byte[] S = new byte[256];
        byte[] T = new byte[256];
        byte[] keystream;

        public RC4()
        {
            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
                T[i] = (byte)i;
            }
        }

        // Convert hexa to array of bytes
        void Swap(int i, int j)
        {
            byte temp = S[i];
            S[i] = S[j];
            S[j] = temp;
        }

        // Convert hexa to array of bytes
        byte[] HexToBytes(string hexa)
        {
            byte[] convertedHexa = new byte[hexa.Length / 2];
            for (int i = 0; i < hexa.Length; i += 2)
            {
                string hexByte = hexa.Substring(i, 2);
                convertedHexa[i / 2] = Convert.ToByte(hexByte, 16);
            }
            return convertedHexa;
        }

        string ByteArrayToHexString(byte[] bytes)
        {
            char[] hexChars = new char[bytes.Length * 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                int high = (bytes[i] >> 4) & 0xF;
                int low = bytes[i] & 0xF;

                hexChars[2 * i] = (char)(high < 10 ? '0' + high : 'A' + (high - 10));
                hexChars[2 * i + 1] = (char)(low < 10 ? '0' + low : 'A' + (low - 10));
            }

            return new string(hexChars);
        }


        void KeySchedulingAlgorithm(string key)
        {
            keystream = new byte[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                keystream[i] = (byte)key[i];
            }
            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + keystream[i % keystream.Length]) % 256;
                Swap(i, j);
            }
        }

        byte[] PseudoRandomGenerationAlgorithm(int length)
        {
            byte[] keyStream = new byte[length];
            int i = 0, j = 0;
            for (int k = 0; k < length; k++)
            {
                i = (i + 1) % 256;
                j = (j + S[i]) % 256;
                Swap(i, j);
                keyStream[k] = S[(S[i] + S[j]) % 256];
            }
            return keyStream;
        }

        string RC4Logic(string text, string key)
        {
            byte[] textByte = new byte[text.Length / 2];
            byte[] keyByte = new byte[key.Length];
            byte[] keyStream;
            byte[] converted;
            string convertedText = "";
            bool isHex = false;
            int textLength = text.Length;

            if (text.StartsWith("0x") && key.StartsWith("0x"))
            {
                isHex = true;
                text = text.Substring(2);
                textByte = HexToBytes(text);
                key = key.Substring(2);
                keyByte = HexToBytes(key);
                textLength = textByte.Length;

                string keyByteString = "";
                for (int i = 0; i < keyByte.Length; i++)
                {
                    keyByteString += (char)keyByte[i];
                }
                KeySchedulingAlgorithm(keyByteString);
            }
            Console.WriteLine(text.Length);
            Console.WriteLine(textLength);


            if (!isHex)
                KeySchedulingAlgorithm(key);

            keyStream = PseudoRandomGenerationAlgorithm(text.Length);
            converted = new byte[textLength];
            for (int i = 0; i < textLength; i++)
            {
                if (!isHex)
                    converted[i] = (byte)text[i];
                else converted[i] = (byte)textByte[i];
            }
            for (int i = 0; i < textLength; i++)
            {
                converted[i] = (byte)(converted[i] ^ keyStream[i]);
            }

            convertedText = "";

            for (int i = 0; i < textLength; i++)
                convertedText += (char)converted[i];

            if (!isHex)
                return convertedText;
            else
            {
                convertedText = "";
                convertedText = ByteArrayToHexString(converted);
                return "0x" + convertedText;
            }
        }

        public override string Decrypt(string cipherText, string key)
        {
            return RC4Logic(cipherText, key);
            //throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            return RC4Logic(plainText, key);
            //throw new NotImplementedException();
        }
    }
}
