using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Post.Api.Application.Options;

namespace Post.Api.Extensions;

public static class ServiceCollectionAuthenticationExtensions
{
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.IssuerSigningKey = 
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:Key"]!));
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidateIssuer = false;
                options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);
            })
            .Services
            .Configure<JwtTokenOptions>(options =>
            {
                options.Key = Encoding.UTF8.GetBytes(configuration["JwtToken:Key"]!);
                options.ExpiresIn = configuration.GetValue<int>("JwtToken:ExpiresIn");
            });
}