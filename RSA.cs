using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
       
        public int Encrypt(int p, int q, int M, int e)
        {
            //  throw new NotImplementedException();
            // Cipher message= M^e mod (n)
            int n = p * q;
            int cipherMessage = 1;
            // 3^7 mod 21 
            // M   C 
            // 3 x 1 mod 21 = 3
            // 3 x 3 mod 21 = 9
            // 3 x 9 mod 21 = 6
            // 3 x 6 mod 21 = 18
            // 3 x 18 mod 21 = 12 
            // 3 x 12 mod 21 = 15 
            // 3 x 15 mod 21 = 3 this is the cipher message
            for (int i = 0; i < e; i++)
            {
                cipherMessage = (cipherMessage * M) % n;
            }
            return cipherMessage;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            // throw new NotImplementedException();
            // d = e^-1 mod (euler)
            // Plain message = C^d mod (p*q)

            int n = p * q;
            int euler = (p - 1) * (q - 1);
            int d = new ExtendedEuclid().GetMultiplicativeInverse(e, euler);

            int plainMessage = 1;
            for (int i = 0; i < d; i++)
            {
                plainMessage = (plainMessage * C) % n;
            }
            return plainMessage;
           
        }
    }
}
