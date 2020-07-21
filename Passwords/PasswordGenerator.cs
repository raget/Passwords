using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Raget.Security.Passwords
{
    public class PasswordGenerator
    {
        public static readonly IEnumerable<int> Lowers = Enumerable.Range('a', 'z' - 'a' + 1);
        public static readonly IEnumerable<int> Uppers = Enumerable.Range('A', 'Z' - 'A' + 1);
        public static readonly IEnumerable<int> Numbers = Enumerable.Range('0', '9' - '0' + 1);

        public static readonly IEnumerable<int> Alphanumerics = Lowers.Union(Uppers).Union(Numbers).ToList();

        private readonly RandomNumberGenerator _generator;

        public PasswordGenerator(RandomNumberGenerator generator)
        {
            _generator = generator;
        }
        
        /// <summary>
        /// Generate password from numbers and alpha characters (lower and upper case).
        /// </summary>
        /// <param name="minimumEntropy">Minimum entropy that password generation need to have.</param>
        /// <exception cref="ArgumentOutOfRangeException">When minimumEntropy is less than one.</exception>
        /// <exception cref="ArgumentNullException">When characterPool is null</exception>
        /// <exception cref="ArgumentException">When password length computed from minimumEntropy and characterPool is longer han 256 characters.</exception>
        /// <returns>Secure random password</returns>
        public string GenerateAlphanumericPassword(int minimumEntropy)
        {
            return GeneratePassword(minimumEntropy, Alphanumerics);
        }

        /// <summary>
        /// Generate password from given characterPool.
        /// </summary>
        /// <param name="minimumEntropy">Minimum entropy that password generation need to have.</param>
        /// <param name="characterPool">Set of characters from which password should be generated.</param>
        /// <exception cref="ArgumentOutOfRangeException">When minimumEntropy is less than one.</exception>
        /// <exception cref="ArgumentNullException">When characterPool is null</exception>
        /// <exception cref="ArgumentException">When password length computed from minimumEntropy and characterPool is longer han 256 characters.</exception>
        /// <returns>Secure random password</returns>
        public string GeneratePassword(int minimumEntropy, IEnumerable<int> characterPool)
        {
            if (minimumEntropy < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumEntropy));
            }
            
            if (characterPool == null)
            {
                throw new ArgumentNullException(nameof(characterPool));
            }

            var characterList = characterPool.ToList();
            var poolCount = characterList.Count;
            var passwordLength = ComputePasswordLength(minimumEntropy, poolCount);
            if (passwordLength > 256)
            {
                throw new ArgumentException("Computed password length is longer than 256 characters. Use large size pool or decrease the entropy.");
            }
            
            var password = new char[passwordLength]; 
            for (var i = 0; i < passwordLength; i++)
            {
                var randomIndex = GenerateRandomIndex(poolCount);
                password[i] = (char) characterList[randomIndex];
            }

            return new string(password);
        }

        private static int ComputePasswordLength(int minimumEntropy, int poolCount) =>
            Convert.ToInt32(Math.Ceiling(minimumEntropy / Math.Log(poolCount, 2)));

        private int GenerateRandomIndex(int poolSize)
        {
            var randomNumber = new byte[1];
            do
            {
                _generator.GetBytes(randomNumber);
            } while (!IsFairRoll(randomNumber[0], (byte)poolSize));

            return randomNumber[0] % poolSize;
        }
        
        private static bool IsFairRoll(byte randomNumber, byte poolSize)
        {
            // Example: poolSize is 100 that means two full sets from 255 values.
            // When randomNumber is for example 240, it means that it falls into
            // not full set of values (third set has only 55 values) and that is
            // not fair and will have impact on a scatter.
            var fullSetsOfValues = byte.MaxValue / poolSize;
            return randomNumber < poolSize * fullSetsOfValues;
        }
    }
}