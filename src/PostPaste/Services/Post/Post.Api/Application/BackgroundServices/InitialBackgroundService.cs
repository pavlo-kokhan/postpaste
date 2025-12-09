using Post.Api.Application.Services.Abstract;

namespace Post.Api.Application.BackgroundServices;

public class InitialBackgroundService : IHostedService
{
    private readonly ILogger<InitialBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public InitialBackgroundService(
        ILogger<InitialBackgroundService> logger, 
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initial background service started...");

        using var scope = _serviceProvider.CreateScope();
        var blobContainerManager = scope.ServiceProvider.GetRequiredService<IBlobContainerManager>();
        
        await blobContainerManager.CheckConnectionAsync();
        await blobContainerManager.EnsureContainersExistsAsync();
        
        _logger.LogInformation("Initial background service finished...");
    }

    public Task StopAsync(CancellationToken cancellationToken) 
        => Task.CompletedTask;
}