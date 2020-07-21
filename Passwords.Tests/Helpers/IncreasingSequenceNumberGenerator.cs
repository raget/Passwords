using System.Security.Cryptography;

namespace Raget.Security.Passwords.Tests.Helpers
{
    internal class IncreasingSequenceNumberGenerator : RandomNumberGenerator
    {
        private readonly int _start;
        private int _callNumber;
        
        public IncreasingSequenceNumberGenerator(int start)
        {
            _start = start;
        }

        public override void GetBytes(byte[] data)
        {
            data[0] = (byte) (_start + _callNumber);
            _callNumber++;
        }
    }
}