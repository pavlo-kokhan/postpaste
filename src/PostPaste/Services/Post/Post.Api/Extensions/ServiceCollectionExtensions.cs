using System.Reflection;
using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Azure;
using Post.Api.Application.BackgroundServices;
using Post.Api.Application.BusinessRules;
using Post.Api.Application.BusinessRules.Abstract;
using Post.Api.Application.Options;
using Post.Api.Application.Services;
using Post.Api.Application.Services.Abstract;
using Post.Domain.Entities.Post;
using Post.Domain.Entities.User;
using Post.Infrastructure.Persistence;

namespace Post.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
        => services
            .AddIdentityCore<UserEntity>(builder =>
            {
                builder.User.RequireUniqueEmail = true;

                builder.SignIn.RequireConfirmedEmail = true;
                
                builder.Password.RequireDigit = true;
                builder.Password.RequireLowercase = true;
                builder.Password.RequireUppercase = true;
                builder.Password.RequireNonAlphanumeric = false;
                builder.Password.RequiredLength = 8;
                builder.Password.RequiredUniqueChars = 4;
            })
            .AddUserManager<UserManager<UserEntity>>()
            .AddRoles<IdentityRole<int>>()
            .AddRoleManager<RoleManager<IdentityRole<int>>>()
            .AddSignInManager<SignInManager<UserEntity>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .Services;

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        => services
            .AddFluentValidationAutoValidation(configuration =>
            {
                configuration.DisableDataAnnotationsValidation = true;
            })
            .AddValidatorsFromAssemblies(
                [Assembly.GetExecutingAssembly(), typeof(PostEntityValidator).Assembly], 
                ServiceLifetime.Singleton);

    public static IServiceCollection AddMediator(this IServiceCollection services)
        => services
            .AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                configuration.Lifetime = ServiceLifetime.Scoped;
            });

    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(clientBuilder => 
        {
            clientBuilder.AddBlobServiceClient(configuration["AzureStorage:ConnectionString"]); 
        });
        
        services.Configure<AzureStorageOptions>(cfg =>
        {
            cfg.PostsContainerName = configuration["AzureStorage:ContainerNames:Posts"]!;
        });
        
        return services;
    }
    
    public static IServiceCollection AddBlobContainerManager(this IServiceCollection services)
        => services
            .AddScoped<IBlobContainerManager, BlobContainerManager>();

    public static IServiceCollection AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
            .AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"))
            .Services;
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddScoped<IUserAccessor, UserAccessor>()
            .AddSingleton<IPasswordProtector, Argon2PasswordProtector>()
            .Configure<PasswordHashingOptions>(options =>
            {
                options.MemorySizeKb = configuration.GetValue<int>("PasswordHashing:MemorySizeKb");
                options.Iterations = configuration.GetValue<int>("PasswordHashing:Iterations");
                options.DegreeOfParallelism = configuration.GetValue<int>("PasswordHashing:DegreeOfParallelism");
                options.HashLength = configuration.GetValue<int>("PasswordHashing:HashLength");
                options.SaltLength = configuration.GetValue<int>("PasswordHashing:SaltLength");
            })
            .AddSingleton<IPostsStorageService, PostsStorageService>()
            .AddScoped<IIdentityService, IdentityService>()
            .Configure<EmailUrlsOptions>(options =>
            {
                options.EmailConfirmationBaseUrl = configuration["EmailUrls:EmailConfirmationBaseUrl"]!;
                options.LoginBaseUrl = configuration["EmailUrls:LoginBaseUrl"]!;
            })
            .AddScoped<IEmailService, SmtpEmailService>()
            .AddScoped<IJwtTokenService, JwtTokenService>()
            .AddSingleton<IBlobKeyService, BlobKeyService>();
    
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
        => services
            .AddHostedService<InitialBackgroundService>();
    
    public static IServiceCollection AddBusinessRules(this IServiceCollection services)
        => services
            .AddScoped<IBusinessRuleValidator<UniqueFolderNameRule>, UniqueFolderNameRule.Validator>()
            .AddScoped<IBusinessRuleValidator<PostFolderExistsRule>, PostFolderExistsRule.Validator>()
            .AddScoped<IBusinessRuleValidator<PostOwnedByUserRule>, PostOwnedByUserRule.Validator>();
}