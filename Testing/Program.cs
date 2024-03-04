using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

class Program
{
    static string Encrypt(string plainText, string key)
    {
        return "";
        //throw new NotImplementedException();
    }

    static string Decrypt(string cipherText, string key)
    {
        return "";
        //throw new NotImplementedException();
    }

    static void Main(string[] args)
    {
        do
        {
            Console.WriteLine("Choose the operation: \n encryption -> type 'e' \n decryption -> type 'd' ");
            string op = Console.ReadLine().ToLower();

            if (op == "e")
            {
                Console.WriteLine("PlainText: ");
                string plainText = Console.ReadLine();

                Console.WriteLine("Key: ");
                string k = Console.ReadLine();

                Console.WriteLine("Encrypted Text: ");
                Console.WriteLine(Encrypt(plainText, k));

                Console.WriteLine("=========================");

                continue;
            }

            Console.WriteLine("cipherText: ");
            string cipherText = Console.ReadLine();

            Console.WriteLine("Key: ");
            string key = Console.ReadLine();

            //Console.WriteLine("Decrypted Text: ");
            Console.WriteLine(Decrypt(cipherText, key));

            Console.WriteLine("=========================");

        } while (true);
    }
}
