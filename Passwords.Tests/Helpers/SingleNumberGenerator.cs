using System.Security.Cryptography;

namespace Raget.Security.Passwords.Tests.Helpers
{
    internal class SingleNumberGenerator : RandomNumberGenerator
    {
        private readonly int _number;

        public SingleNumberGenerator(int number)
        {
            _number = number;
        }
        
        public override void GetBytes(byte[] data)
        {
            data[0] = (byte) _number;
        }
    }
}