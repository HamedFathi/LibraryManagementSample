using LMS.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LMS.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<LMS.WebApi.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<LibSysDbContext>));

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddDbContext<LibSysDbContext>((_, options) =>
            {
                options.UseSqlite(connection);
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibSysDbContext>();

            db.Database.EnsureCreated();
            //db.Database.Migrate();
            builder.UseEnvironment("Development");
        });
    }
}