using Microsoft.EntityFrameworkCore;
using Pictura.Api.Data;
using Pictura.Api.Infrastructure.Options;
using Pictura.Api.Services;

namespace Pictura.Api
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add configuration
            builder.Services.Configure<ApiAuthOptions>(builder.Configuration.GetSection("ApiAuth"));
            
            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services.AddOpenApi();
            
            builder.Services.AddTransient<ImageService>();
            
            // Configure Entity Framework Core with the database options
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var config = builder.Configuration.GetSection("Database").Get<DatabaseOptions>() 
                    ?? new DatabaseOptions();

                var connectionString = config.ConnectionString;

                if (config.Type == DatabaseOptions.DatabaseType.Sqlite)
                {
                    options.UseSqlite(connectionString);
                }
                
                // Other database types can be added here in the future
            });
            
            var app = builder.Build();
            
            // Migrate the database
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            
            // Configure Swagger/OpenAPI
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Configure the HTTP request pipeline
            app.UseHttpsRedirection();
            app.UseAuthorization();
            
            app.MapControllers();
            
            app.Run();
        }
    }
}
