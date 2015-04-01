using System;
using Drs.Infrastructure.Crypto;

namespace PopulateDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var pass = "99630110";
            var salt = String.Empty;

            Console.Write(pass.IsEqualHash("61a6a1a3-0f63-4ece-a3ab-af2d48662e13", "C6jdax+WudfykFSmwBCD5A=="));

            //var hashed = pass.CreateHash(out salt);
            //Console.Write(salt);
            //Console.Write(hashed);
        }
    }
}
