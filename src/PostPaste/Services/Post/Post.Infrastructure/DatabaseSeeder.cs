using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Post.Domain.Constants;

namespace Post.Infrastructure;

public class DatabaseSeeder
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public DatabaseSeeder(RoleManager<IdentityRole<int>> roleManager) 
        => _roleManager = roleManager;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!await _roleManager.Roles.AnyAsync(cancellationToken: cancellationToken))
        {
            foreach (var role in Enum.GetValues<Roles>())
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(role.ToString()));
            }
        }
    }
}