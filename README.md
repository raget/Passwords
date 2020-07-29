![Build](https://github.com/raget/Passwords/workflows/Build/badge.svg)
# Secure random password generator for .Net Core
Generates passwords based on given entropy.  
Use entropy 128 or more for secure alphanumerics passwords.

## Samples
```c#
const int entropy = 128;

// Example 1: Generate password using default random number generator
using var defaultPasswordGenerator = new DefaultPasswordGenerator();
var password = defaultPasswordGenerator.GenerateAlphanumericPassword(entropy);

Console.WriteLine(password);

// Example 2: Generate password from custom character pool
var pool = PasswordGenerator.Alphanumerics.Union(new int[] {'*', '.', '/', '?', '-', '.', '_'});
password = PasswordGenerator.Default.GeneratePassword(entropy, pool);

Console.WriteLine(password);

// Example 3: Generate password using custom random number generator
using (var randomNumberGenerator = new RNGCryptoServiceProvider())
{
    var passwordGenerator = new PasswordGenerator(randomNumberGenerator);
    password = passwordGenerator.GenerateAlphanumericPassword(entropy);
}

Console.WriteLine(password);
```