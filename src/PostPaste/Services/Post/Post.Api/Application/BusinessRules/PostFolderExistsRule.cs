using Microsoft.EntityFrameworkCore;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Infrastructure.Persistence;
using Shared.Result.Results;

namespace Post.Api.Application.BusinessRules;

public record PostFolderExistsRule(int? FolderId) : IBusinessRule
{
    public class Validator : IBusinessRuleValidator<PostFolderExistsRule>
    {
        private readonly ApplicationDbContext _dbContext;

        public Validator(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public Error Error => PostFolderErrors.NotFound;
        
        public async Task<bool> IsBrokenAsync(PostFolderExistsRule rule, CancellationToken cancellationToken = default) 
            => await _dbContext.PostFolders.AnyAsync(f => f.Id == rule.FolderId, cancellationToken);
    }
}