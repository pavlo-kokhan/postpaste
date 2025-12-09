using Shared.Result.Results;

namespace Post.Api.Application.BusinessRules.Abstract;

public interface IBusinessRuleValidator<in TRule> where TRule : IBusinessRule
{
    Error Error { get; }
    
    Task<bool> IsBrokenAsync(TRule rule, CancellationToken cancellationToken = default);
}