using Microsoft.EntityFrameworkCore;
using Pictura.Api.Data;
using Pictura.Api.Services;

namespace Pictura.Api
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddSqlite<AppDbContext>("Data Source=pictura.db");;
            builder.Services.AddTransient<ImageService>();
            
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
