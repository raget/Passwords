using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Raget.Security.Passwords
{
    /// <summary>
    /// Password generator with system default random number generator
    /// </summary>
    public class DefaultPasswordGenerator : IPasswordGenerator, IDisposable
    {
        private readonly PasswordGenerator _passwordGenerator;
        private readonly RandomNumberGenerator _numberGenerator;
        public DefaultPasswordGenerator()
        {
            _numberGenerator = RandomNumberGenerator.Create();
            _passwordGenerator = new PasswordGenerator(_numberGenerator);
        }
        
        public string GenerateAlphanumericPassword(int minimumEntropy) =>
            _passwordGenerator.GenerateAlphanumericPassword(minimumEntropy);


        public string GeneratePassword(int minimumEntropy, IEnumerable<int> characterPool) =>
            _passwordGenerator.GeneratePassword(minimumEntropy, characterPool);

        public void Dispose() =>
            _numberGenerator.Dispose();
    }
}