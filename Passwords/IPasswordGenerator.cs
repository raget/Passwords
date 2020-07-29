using System;
using System.Collections.Generic;

namespace Raget.Security.Passwords
{
    public interface IPasswordGenerator
    {
        /// <summary>
        /// Generate password from numbers and alpha characters (lower and upper case).
        /// </summary>
        /// <param name="minimumEntropy">Minimum entropy that password generation need to have.</param>
        /// <exception cref="ArgumentOutOfRangeException">When minimumEntropy is less than one.</exception>
        /// <exception cref="ArgumentNullException">When characterPool is null</exception>
        /// <exception cref="ArgumentException">When password length computed from minimumEntropy and characterPool is longer han 256 characters.</exception>
        /// <returns>Secure random password</returns>
        string GenerateAlphanumericPassword(int minimumEntropy);

        /// <summary>
        /// Generate password from given characterPool.
        /// </summary>
        /// <param name="minimumEntropy">Minimum entropy that password generation need to have.</param>
        /// <param name="characterPool">Set of characters from which password should be generated.</param>
        /// <exception cref="ArgumentOutOfRangeException">When minimumEntropy is less than one.</exception>
        /// <exception cref="ArgumentNullException">When characterPool is null</exception>
        /// <exception cref="ArgumentException">When password length computed from minimumEntropy and characterPool is longer han 256 characters.</exception>
        /// <returns>Secure random password</returns>
        string GeneratePassword(int minimumEntropy, IEnumerable<int> characterPool);
    }
}