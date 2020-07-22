![Build](https://github.com/raget/Passwords/workflows/Build/badge.svg)
# Secure random password generator for .Net Core
Generates passwords based on given entropy. So you don't have to worry if your password is strong enough or not.  

## How to use it
#### Generate alphanumeric password
```c#
// RandomNumberGenerator from the system
// see https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator?view=netcore-3.1)
using System.Security.Cryptography; 
using Raget.Security.Passwords

var entropy = 160;
string password;
using (var rng = RandomNumberGenerator.Create())
{
    var generator = new PasswordGenerator(rng);
    password = generator.GenerateAlphanumericPassword(entropy);
}
```
Password length is computed from password entropy and used character pool which is numeric, lower and upper english letters in this case.  
  
To learn more about password strength, see [https://en.wikipedia.org/wiki/Password_strength](https://en.wikipedia.org/wiki/Password_strength)  

#### Generate password from custom character pool
Example 1: PIN
```c#
var entropy = 128
// use predefined collection for digits
var pin = generator.GeneratePassword(entropy, PasswordGenerator.Digits);
```
Example 2: Alphanumerics with some special characters
```c#
var entropy = 128
var pool = PasswordGenerator.Alphanumerics.Union(new int[] {'*', '.', '/', '?'});
var password = generator.GeneratePassword(entropy, pool);
```

#### Generate password using concrete crypto service provider of random numbers
```c#
using System.Security.Cryptography; 
using Raget.Security.Passwords

var entropy = 160;
string password;
using (var rng = new RNGCryptoServiceProvider())
{
    var generator = new PasswordGenerator(rng);
    password = generator.GenerateAlphanumericPassword(entropy);
}
```