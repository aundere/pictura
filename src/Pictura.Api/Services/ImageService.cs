using Microsoft.EntityFrameworkCore;
using Pictura.Api.Data;
using Pictura.Api.Entities;

namespace Pictura.Api.Services
{
    public class ImageService
    {
        private readonly AppDbContext _db;

        public ImageService(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<ImageEntity> CreateImageAsync(string url, IEnumerable<string> tags)
        {
            var tagEntities = await this._db.Tags
                .Where(t => tags.Contains(t.Name))
                .ToListAsync();

            var newTags = tags
                .Where(x => tagEntities.All(t => t.Name != x))
                .Select(x => new TagEntity { Id = 0, Name = x })
                .ToList();
            
            var allTags = tagEntities.Concat(newTags).ToList();
            
            var image = new ImageEntity
            {
                Id = 0,
                Url = url,
                Tags = allTags
            };
            
            this._db.Images.Add(image);
            await this._db.SaveChangesAsync();

            return image;
        }
        
        public async Task<IEnumerable<ImageEntity>> GetImagesByTagsAsync(int from, int limit, IEnumerable<string> tags)
        {
            return await this._db.Images
                .Include(x => x.Tags)
                .Where(x => !tags.Except(x.Tags.Select(t => t.Name)).Any())
                .Skip(from)
                .Take(limit)
                .ToListAsync();
        }
        
        public async Task<ImageEntity?> GetRandomImageAsync(IEnumerable<string> tags)
        {
            return await this._db.Images
                .Include(x => x.Tags)
                .Where(x => !tags.Except(x.Tags.Select(t => t.Name)).Any())
                .OrderBy(_ => EF.Functions.Random())
                .FirstOrDefaultAsync();
        }
    }
}
