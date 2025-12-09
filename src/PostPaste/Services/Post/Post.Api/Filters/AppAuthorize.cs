using Microsoft.AspNetCore.Authorization;
using Post.Domain.Constants;

namespace Post.Api.Filters;

public class AppAuthorize : AuthorizeAttribute
{
    public AppAuthorize(Roles role)
    {
        var roles = Enum
            .GetValues<Roles>()
            .Where(r => role.HasFlag(r))
            .Select(r => r.ToString());
        
        Roles = string.Join(",", roles);
    }
}