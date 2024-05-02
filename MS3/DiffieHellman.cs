using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
        public static int power(int num, int pow, int mod)
        {
            int result = 1;
            for (int i = 0; i < pow; i++)
            {
                result = (result * num) % mod;
            }
            return result;
        }
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            //q -> mod prime 
            //alpha - > generator
            // xa , xb -> myprivate 
            // each one generate a private key 
            // public = alpha^private mod (q)

            List<int> result = new List<int>();

            //public keys
            int ya = power(alpha, xa, q);
            int yb = power(alpha, xb, q);
            // private keys
            // switch public keys 
            // private = yourPublic^myprivate mod (q) 
            int secretKey1 = power(ya, xb, q);
            int secretKey2 = power(yb, xa, q);

            result.Add(secretKey1);
            result.Add(secretKey2);
            return result;
            // throw new NotImplementedException();
        }
    }
}
