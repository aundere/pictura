using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pictura.Api.Data;
using Pictura.Api.Entities;

#pragma warning disable ClassNeverInstantiated.Global // This class is instantiated by the test framework, not directly in code.

namespace Pictura.Api.Tests.Infrastructure
{
    public class TestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.RemoveAll<DbConnection>();

                services.AddSingleton<DbConnection>(_ =>
                {
                    var connection = new SqliteConnection("Data Source=:memory:");
                    connection.Open();

                    return connection;
                });
                
                services.AddDbContext<AppDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });
            });

            builder.UseEnvironment("Testing");
        }
        
        public void SeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            context.Images.Add(new ImageEntity
            {
                Url = "example.com",
                Tags =
                [
                    new TagEntity
                    {
                        Name = "test"
                    },
                    new TagEntity
                    {
                        Name = "tag"
                    }
                ]
            });

            context.SaveChanges();
        }
    }
}

#pragma warning restore ClassNeverInstantiated.Global
