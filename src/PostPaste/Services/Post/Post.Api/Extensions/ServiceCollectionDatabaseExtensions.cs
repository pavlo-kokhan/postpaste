using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using Post.Domain;
using Post.Domain.Entities.Post;
using Post.Domain.Entities.PostFolder;
using Post.Domain.Entities.User;
using Post.Infrastructure;
using Post.Infrastructure.Persistence;
using Post.Infrastructure.Persistence.Repositories;

namespace Post.Api.Extensions;

public static class ServiceCollectionDatabaseExtensions
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dataSource = new NpgsqlDataSourceBuilder(configuration["ConnectionStrings:Postgres"])
                    .EnableDynamicJson()
                    .Build();

                options.UseNpgsql(dataSource);
                options.ConfigureWarnings(builder => builder.Ignore(RelationalEventId.PendingModelChangesWarning));

                if (serviceProvider.GetRequiredService<IHostEnvironment>().IsEnvironment("Debug"))
                {
                    options
                        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
                        .EnableSensitiveDataLogging();
                }
            })
            .AddScoped<DatabaseSeeder>();

    public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IPostRepository, PostRepository>()
            .AddScoped<IPostFolderRepository, PostFolderRepository>()
            .AddScoped<IUserRepository, UserRepository>();
}