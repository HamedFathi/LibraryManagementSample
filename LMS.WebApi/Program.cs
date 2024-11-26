
using HamedStack.CQRS.ServiceCollection;
using HamedStack.TheRepository.EntityFrameworkCore.Outbox;
using HamedStack.TheRepository.ServiceCollection;
using LMS.Domain.BookContext.Repositories;
using LMS.Infrastructure;
using LMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using System.Threading.RateLimiting;
using Asp.Versioning;
using HamedStack.AspNetCore.Endpoint;
using HamedStack.TheResult.AspNetCore;
using Serilog;

namespace LMS.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddMinimalApiEndpoints();

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddRateLimiter(options => {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext => RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(), factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 1000,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
        });

        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        builder.Services.AddHealthChecks();

        builder.Services.AddHttpClient("MyAPI", client =>
        {
            client.BaseAddress = new Uri("https://api");
        })
        .AddResilienceHandler("MyResilienceStrategy", resilienceBuilder =>
        {
            resilienceBuilder.AddRetry(new HttpRetryStrategyOptions 
            {
                MaxRetryAttempts = 5,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<HttpRequestException>()
                .HandleResult(response => !response.IsSuccessStatusCode)
            });

            resilienceBuilder.AddTimeout(TimeSpan.FromMinutes(1));

            resilienceBuilder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.2,
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(1),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                .Handle<HttpRequestException>()
                .HandleResult(response => !response.IsSuccessStatusCode)
            });
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddDbContext<LibSysDbContext>(options =>
            options.UseSqlite("Data Source=casestudydb.db"));

        builder.Services.AddScoped<IBookRepository, BookRepository>();

        builder.Services.AddInfrastructureServices<LibSysDbContext>();
        builder.Services.AddApplicationServices();
        builder.Services.AddOutboxBackgroundService(options =>
        {
            options.PollingIntervalSeconds = 10;
            options.BatchSize = 10;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseSecurityHeaders();

        app.UseResponseCompression();

        app.UseStaticFiles();

        app.UseSerilogRequestLogging();

        app.UseAuthorization();

        app.MapControllers();
        app.MapMinimalApiEndpoints();

        app.UseResultException();

        app.MapHealthChecks("/healthz");

        app.Run();
    }
}