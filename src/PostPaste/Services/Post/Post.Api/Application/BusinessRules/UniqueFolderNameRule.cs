using Microsoft.EntityFrameworkCore;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Infrastructure.Persistence;
using Shared.Result.Results;

namespace Post.Api.Application.BusinessRules;

public record UniqueFolderNameRule(string Name, int OwnerId) : IBusinessRule
{
    public class Validator : IBusinessRuleValidator<UniqueFolderNameRule>
    {
        private readonly ApplicationDbContext _dbContext;

        public Validator(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public Error Error => PostFolderErrors.AlreadyExists;

        public async Task<bool> IsBrokenAsync(UniqueFolderNameRule rule, CancellationToken cancellationToken = default)
            => await _dbContext.PostFolders
                .AnyAsync(p => p.OwnerId == rule.OwnerId && p.Name == rule.Name, cancellationToken);
    }
}