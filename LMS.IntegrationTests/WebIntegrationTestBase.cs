using HamedStack.CQRS;
using LMS.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.IntegrationTests;

public abstract class WebIntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>
{
    public HttpClient HttpClient { get; }
    protected ICommandQueryDispatcher Dispatcher { get; }
    protected LibSysDbContext DbContext { get; }

    protected WebIntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        var scope = factory.Services.CreateScope();
        Dispatcher = scope.ServiceProvider.GetRequiredService<ICommandQueryDispatcher>();
        DbContext = scope.ServiceProvider.GetRequiredService<LibSysDbContext>();
        HttpClient = factory.CreateClient();
    }
}