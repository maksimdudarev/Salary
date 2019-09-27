using System;

namespace SimonGilbert.Blog
{
    class Program
    {
        static void Main(string[] args)
        {
            var password = "simongilbert123";

            var md5Hash = CryptographyService.HashPasswordUsingMD5(password);

            Console.WriteLine("MD5 Hash:");
            Console.WriteLine(md5Hash);

            var pbkdf2Hash = CryptographyService.HashPasswordUsingPBKDF2(password);

            Console.WriteLine("PBKDF2 Hash:");
            Console.WriteLine(pbkdf2Hash);

            Console.ReadKey();
        }
    }
}
