using Microsoft.EntityFrameworkCore;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Infrastructure.Persistence;
using Shared.Result.Results;

namespace Post.Api.Application.BusinessRules;

public record PostOwnedByUserRule(int PostId, int OwnerId) : IBusinessRule
{
    public class Validator : IBusinessRuleValidator<PostOwnedByUserRule>
    {
        private readonly ApplicationDbContext _dbContext;

        public Validator(ApplicationDbContext dbContext)
            => _dbContext = dbContext;

        public Error Error => PostErrors.Forbidden;

        public async Task<bool> IsBrokenAsync(PostOwnedByUserRule rule, CancellationToken cancellationToken = default)
            => !await _dbContext.Posts
                .AsNoTracking()
                .AnyAsync(p => p.Id == rule.PostId && p.OwnerId == rule.OwnerId, cancellationToken);
    }
}