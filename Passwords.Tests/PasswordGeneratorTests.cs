using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using Raget.Security.Passwords.Tests.Helpers;

namespace Raget.Security.Passwords.Tests
{
    [TestFixture]
    public class PasswordTests
    {
        private static double EntropyOf(int passwordLength, int charactersInPool) => 
            Math.Log(charactersInPool, 2) * passwordLength;

        [Test]
        public void EntropyMustBeGreaterOrEqualToOne()
        {
            var generator = new PasswordGenerator(new RNGCryptoServiceProvider());

            Assert.That(()=>generator.GeneratePassword(0, PasswordGenerator.Lowers), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        
        [Test]
        public void CharacterPoolCannotBeNull()
        {
            var generator = new PasswordGenerator(new RNGCryptoServiceProvider());

            Assert.That(()=>generator.GeneratePassword(10, null), Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void PasswordLengtMustBeLessThan256()
        {
            var generator = new PasswordGenerator(new RNGCryptoServiceProvider());

            Assert.That(()=>generator.GeneratePassword(851, PasswordGenerator.Digits), Throws.ArgumentException);
        }

        [Test]
        [TestCaseSource(nameof(_entropyTestCases))]
        public void PasswordLengthMeetsMinimalEntropy(int minimalEntropy, IEnumerable<int> characterPool)
        {
            var characterList = characterPool.ToList();
            var generator = new PasswordGenerator(new RNGCryptoServiceProvider());

            var alphaNumPassword = generator.GeneratePassword(minimalEntropy, characterList);

            var passwordEntropy = EntropyOf(alphaNumPassword.Length, characterList.Count);
            Assert.That(passwordEntropy, Is.GreaterThanOrEqualTo(minimalEntropy));
        }
        
        static object[] _entropyTestCases =
        {
            new object[] { 128, PasswordGenerator.Alphanumerics },
            new object[] { 256, PasswordGenerator.Lowers },
            new object[] { 63, PasswordGenerator.Uppers },
            new object[] { 300, PasswordGenerator.Alphanumerics }
        };

        [Test]
        [TestCaseSource(nameof(_fakeGeneratorTestCases))]
        public void FakeNumberGeneratorReturnsZero_PasswordContains_OnlyFirstCharacterFromPool(
            int entropy, IEnumerable<int> characterPool)
        {
            var characterList = characterPool.ToList();
            var zeroGenerator = new SingleNumberGenerator(0);
            var passwordGenerator = new PasswordGenerator(zeroGenerator);

            var password = passwordGenerator.GeneratePassword(entropy, characterList);

            var repeatedFirstCharacter = Enumerable.Repeat((char) characterList.First(), password.Length).ToArray();
            var expectedPassword = new string(repeatedFirstCharacter);
            Assert.That(password, Is.EqualTo(expectedPassword));
        }
        
        static object[] _fakeGeneratorTestCases =
        {
            new object[] { 1, PasswordGenerator.Alphanumerics },
            new object[] { 2, PasswordGenerator.Lowers },
            new object[] { 26, PasswordGenerator.Uppers },
            new object[] { 65, PasswordGenerator.Alphanumerics },
            new object[] { 850, PasswordGenerator.Digits }
        };
        
        [Test]
        [TestCaseSource(nameof(_fakeGeneratorTestCases))]
        public void FakeNumberGeneratorReturnsLastIndexInCharacterPool_PasswordContains_OnlyLastCharacterFromPool(
            int entropy, IEnumerable<int> characterPool)
        {
            var characterList = characterPool.ToList();
            var lastIndexGenerator = new SingleNumberGenerator(characterList.Count-1);
            var passwordGenerator = new PasswordGenerator(lastIndexGenerator);

            var password = passwordGenerator.GeneratePassword(entropy, characterList);

            var repeatedLastCharacter = Enumerable.Repeat((char) characterList.Last(), password.Length).ToArray();
            var expectedPassword = new string(repeatedLastCharacter);
            Assert.That(password, Is.EqualTo(expectedPassword));
        }
        
        [Test]
        [TestCaseSource(nameof(_fakeGeneratorTestCases))]
        public void FakeNumberGeneratorReturnsSequenceFromZero_PasswordContains_FirstNLettersFromCharacterFromPool(
            int entropy, IEnumerable<int> characterPool)
        {
            var characterList = characterPool.ToList();
            var indexGenerator = new IncreasingSequenceNumberGenerator(0);
            var passwordGenerator = new PasswordGenerator(indexGenerator);

            var password = passwordGenerator.GeneratePassword(entropy, characterList);
            
            var repeatedCharecterPool= Enumerable.Repeat(characterList, password.Length / characterList.Count + 1);
            var firstCharacters = repeatedCharecterPool.SelectMany(x=>x)
                .Take(password.Length)
                .Select(i=>(char)i)
                .ToArray();
            var expectedPassword = new string(firstCharacters);
            Assert.That(password, Is.EqualTo(expectedPassword));
        }
        
        [Test]
        [TestCaseSource(nameof(_fakeGeneratorTestCases))]
        public void FakeNumberGeneratorReturnsSequenceFrom255_PasswordContains_LastNLettersFromCharacterFromPool(
            int entropy, IEnumerable<int> characterPool)
        {
            var characterList = characterPool.ToList();
            var indexGenerator = new DecreasingSequenceNumberGenerator(characterList.Count-1);
            var passwordGenerator = new PasswordGenerator(indexGenerator);

            var password = passwordGenerator.GeneratePassword(entropy, characterList);

            characterList.Reverse();
            var reversedRepeatedCharecterPool= Enumerable.Repeat(characterList, password.Length / characterList.Count + 1);
            var lastCharacters = reversedRepeatedCharecterPool.SelectMany(x=>x)
                .Take(password.Length)
                .Select(i=>(char)i)
                .ToArray();
            
            var expectedPassword = new string(lastCharacters);
            Assert.That(password, Is.EqualTo(expectedPassword));
        }
    }
}