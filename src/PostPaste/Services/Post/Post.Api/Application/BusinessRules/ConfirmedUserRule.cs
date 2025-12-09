using Microsoft.EntityFrameworkCore;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Constants.Errors;
using Post.Infrastructure.Persistence;
using Shared.Result.Results;

namespace Post.Api.Application.BusinessRules;

public record ConfirmedUserRule(int UserId) : IBusinessRule
{
    public class Validator : IBusinessRuleValidator<ConfirmedUserRule>
    {
        private readonly ApplicationDbContext _dbContext;

        public Validator(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public Error Error => IdentityErrors.EmailNotConfirmed;
        
        public async Task<bool> IsBrokenAsync(ConfirmedUserRule rule, CancellationToken cancellationToken = default) 
            => await _dbContext.Users.AnyAsync(x => x.Id == rule.UserId && !x.EmailConfirmed, cancellationToken);
    }
}