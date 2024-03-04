using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
		char[] alphabet = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
		string encryptedText = "";
		string decryptedText = "";


		public string Encrypt(string plainText, int key)
        {
			// throw new NotImplementedException();
			int i = 0;
			while (i < plainText.Length)
			{
				char currentChar = plainText[i];
				int index = Array.IndexOf(alphabet, currentChar);
				if (index != -1)
				{
					int newIndex = (index + key) % 26;
					encryptedText += alphabet[newIndex];
				}
				else
				{
					encryptedText += currentChar; // If character not found in alphabet, keep it unchanged
				}
				i++;
			}
			return encryptedText;
		}

        public string Decrypt(string cipherText, int key)
        {
			//throw new NotImplementedException();

			cipherText = cipherText.ToLower();
			int i = 0;
			while (i < cipherText.Length)
			{
				char currentChar = cipherText[i];
				int index = Array.IndexOf(alphabet, currentChar);
				if (index != -1)
				{
					int newIndex = (index - key) % 26;
					if (newIndex < 0)
					{
						newIndex += 26;
					}
					decryptedText += alphabet[newIndex];
				}
				else
				{
					decryptedText += currentChar; // If character not found in alphabet, keep it unchanged
				}
				i++;
			}
			return decryptedText;
		}

        public int Analyse(string plainText, string cipherText)
        {
			//throw new NotImplementedException();
			int ret = 0;
			int rets = 0;
			cipherText = cipherText.ToLower();
			plainText = plainText.ToLower();

			int i = 0;
			while (i < alphabet.Length)
			{
				if (plainText[0] == alphabet[i])
					ret = i;
				if (cipherText[0] == alphabet[i])
					rets = i;
				i++;
			}
			return rets - ret < 0 ? (rets - ret) + 26 : (rets - ret);
		}
    }
}
