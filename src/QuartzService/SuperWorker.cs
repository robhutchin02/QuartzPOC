namespace QuartzService;

using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


public class SuperWorker :
    BackgroundService
{
    readonly IServiceScopeFactory _scopeFactory;

    public SuperWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var messageScheduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();

        await messageScheduler.SchedulePublish(TimeSpan.FromSeconds(60), new DemoMessage { Value = "Hello, World 60" }, stoppingToken);
        await messageScheduler.SchedulePublish(TimeSpan.FromSeconds(70), new DemoMessage { Value = "Hello, World 70" }, stoppingToken);
        await messageScheduler.SchedulePublish(TimeSpan.FromSeconds(80), new DemoMessage { Value = "Hello, World 80" }, stoppingToken);
        await messageScheduler.SchedulePublish(TimeSpan.FromSeconds(90), new DemoMessage { Value = "Hello, World 95" }, stoppingToken);
        await messageScheduler.SchedulePublish(TimeSpan.FromSeconds(300), new DemoMessage { Value = "Hello, World 300" }, stoppingToken);
    }
}