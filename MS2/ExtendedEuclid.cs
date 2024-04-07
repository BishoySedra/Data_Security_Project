using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>

        public int fixMod(int n, int m) {
            return (n % m + m) % m;
        }

        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int q,
                a1 = 1,
                a2 = 0,
                a3 = baseN,
                b1 = 0,
                b2 = 1,
                b3 = number;

            while (b3 != 0 && b3 != 1)
            {
                q = a3 / b3;

                int temp = a1 - b1 * q;
                a1 = b1;
                b1 = temp;

                temp = a2 - b2 * q;
                a2 = b2;
                b2 = temp;

                temp = a3 - b3 * q;
                a3 = b3;
                b3 = temp;
            }

            if (b3 == 1)
            {
                if (b2 < 0)
                {
                    b2 = fixMod(b2, baseN);
                }

                return b2;
            }

            return -1; 

            //throw new NotImplementedException();
        }
    }
}
