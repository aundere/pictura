using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Pictura.Api.Data;
using Pictura.Api.Entities;

namespace Pictura.Api.Tests.Infrastructure
{
    public class TestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        private void RemoveDescriptor<TDescriptor>(IServiceCollection services) where TDescriptor : class
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TDescriptor));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                this.RemoveDescriptor<IDbContextOptionsConfiguration<AppDbContext>>(services);
                this.RemoveDescriptor<DbConnection>(services);

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
                    }
                ]
            });

            context.SaveChanges();
        }
    }
}
