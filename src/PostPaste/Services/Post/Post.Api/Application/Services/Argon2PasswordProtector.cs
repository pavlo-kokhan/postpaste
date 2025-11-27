using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Options;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services;

public class Argon2PasswordProtector : IPasswordProtector
{
    private readonly PasswordHashingOptions _options;
    
    public Argon2PasswordProtector(IOptions<PasswordHashingOptions> options) 
        => _options = options.Value;

    public Result<(string Hash, string Salt)> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return Result<(string Hash, string Salt)>.ValidationFailure(PasswordErrors.PasswordEmpty);
        
        var saltBytes = RandomNumberGenerator.GetBytes(_options.SaltLength);
        var hashBytes = DeriveKey(password, saltBytes);

        var hash = Convert.ToBase64String(hashBytes);
        var salt = Convert.ToBase64String(saltBytes);

        return (hash, salt);
    }

    public bool Verify(string password, string hash, string salt)
    {
        if (string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(salt))
            return false;

        var saltBytes = Convert.FromBase64String(salt);
        var expectedHashBytes = Convert.FromBase64String(hash);
        var actualHashBytes = DeriveKey(password, saltBytes);

        return CryptographicOperations.FixedTimeEquals(expectedHashBytes, actualHashBytes);
    }
    
    private byte[] DeriveKey(string password, byte[] saltBytes)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var argon2 = new Argon2id(passwordBytes)
        {
            Salt = saltBytes,
            DegreeOfParallelism = _options.DegreeOfParallelism,
            Iterations = _options.Iterations,
            MemorySize = _options.MemorySizeKb
        };

        return argon2.GetBytes(_options.HashLength);
    }
}