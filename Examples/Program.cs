using System;
using System.Linq;
using System.Security.Cryptography;
using Raget.Security.Passwords;

namespace Raget.Security.Passwords.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            
            const int entropy = 128;
            
            // Example 1: Generate password using default random number generator
            using var defaultPasswordGenerator = new DefaultPasswordGenerator();
            var password = defaultPasswordGenerator.GenerateAlphanumericPassword(entropy);
            
            Console.WriteLine(password);
            
            // Example 2: Generate password from custom character pool
            var pool = PasswordGenerator.Alphanumerics.Union(new int[] {'*', '.', '/', '?', '-', '.', '_'});
            password = defaultPasswordGenerator.GeneratePassword(entropy, pool);
            
            Console.WriteLine(password);
            
            // Example 3: Generate password using custom random number generator
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var passwordGenerator = new PasswordGenerator(randomNumberGenerator);
                password = passwordGenerator.GenerateAlphanumericPassword(entropy);
            }
            
            Console.WriteLine(password);
        }
        
        
    }
}