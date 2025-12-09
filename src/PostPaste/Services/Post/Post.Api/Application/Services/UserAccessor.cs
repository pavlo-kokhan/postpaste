using System.Security.Claims;
using Post.Api.Application.Services.Abstract;

namespace Post.Api.Application.Services;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor) 
        => _httpContextAccessor = httpContextAccessor;

    public int? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdClaim?.Value, out var id))
                return id;
            
            return null;
        }
    }
}