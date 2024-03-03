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
			int j;
			while (i < plainText.Length)
			{
				j = 0;
				while (j < alphabet.Length)
				{
					if (alphabet[j] == plainText[i])
					{
						encryptedText += alphabet[(j + key) % 26];
						break;
					}
					j++;
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
			int j;
			while (i < cipherText.Length)
			{
				j = 0;
				while (j < alphabet.Length)
				{
					int res;
					if (alphabet[j] == cipherText[i])
					{
						res = j - key;
						if (res < 0)
						{
							res = res + 26;
						}
						decryptedText += alphabet[res];
						break;
					}
					j++;
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
