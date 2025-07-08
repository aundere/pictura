using Microsoft.EntityFrameworkCore;
using Pictura.Api.Entities;

namespace Pictura.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TagEntity> Tags => this.Set<TagEntity>();
        public DbSet<ImageEntity> Images => this.Set<ImageEntity>();
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageEntity>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Images);

            modelBuilder.Entity<TagEntity>()
                .HasIndex(e => e.Name).IsUnique();
        }
    }
}
