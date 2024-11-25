
using HamedStack.AspNetCore.Endpoint;
using HamedStack.CQRS.ServiceCollection;
using HamedStack.TheRepository.EntityFrameworkCore.Outbox;
using HamedStack.TheRepository.ServiceCollection;
using LMS.Domain.BookContext.Repositories;
using LMS.Infrastructure;
using LMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddMinimalApiEndpoints();

        builder.Services.AddDbContext<LibSysDbContext>(options =>
            options.UseSqlite("Data Source=casestudydb.db"));

        builder.Services.AddScoped<IBookRepository, BookRepository>();

        builder.Services.AddInfrastructureServices<LibSysDbContext>();
        builder.Services.AddApplicationServices();
        builder.Services.AddOutboxBackgroundService(options => options.PollingIntervalSeconds = 1);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        app.MapMinimalApiEndpoints();

        app.Run();
    }
}