using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Npgsql;
using Post.Infrastructure;
using Post.Infrastructure.Persistence;

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

    // todo: add repositories
    public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services;
}