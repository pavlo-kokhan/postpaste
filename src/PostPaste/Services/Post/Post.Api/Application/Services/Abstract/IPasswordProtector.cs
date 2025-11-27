using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services.Abstract;

public interface IPasswordProtector
{
    Result<(string Hash, string Salt)> Create(string password);
    
    bool Verify(string password, string hash, string salt);
}