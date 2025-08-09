using Microsoft.EntityFrameworkCore;
using Pictura.Api.Data;
using Pictura.Api.Entities;

namespace Pictura.Api.Services
{
    public class ImageService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ImageService> _logger;

        public ImageService(AppDbContext db, ILogger<ImageService> logger)
        {
            this._db = db;
            this._logger = logger;
        }
        
        private IQueryable<ImageEntity> ApplyTagFilter(IQueryable<ImageEntity> query, IList<string> tags)
        {
            return tags.Count != 0
                ? query.Where(x => tags.All(tag => x.Tags.Any(t => t.Name == tag)))
                : query;
        }
        
        private async Task<List<TagEntity>> GetOrCreateTagsAsync(IList<string> tags)
        {
            var existingTags = await this._db.Tags
                .Where(t => tags.Contains(t.Name))
                .ToListAsync();

            var newTags = tags
                .Except(existingTags.Select(t => t.Name), StringComparer.OrdinalIgnoreCase)
                .Select(name => new TagEntity { Name = name })
                .ToList();

            if (newTags.Count <= 0)
            {
                // No new tags to add, return existing ones
                return existingTags.Concat(newTags).ToList();
            }
            
            this._db.Tags.AddRange(newTags);
            await this._db.SaveChangesAsync();
                
            this._logger.LogInformation("Added {NewTagCount} new tags", newTags.Count);

            return existingTags.Concat(newTags).ToList();
        }
        
        public async Task<ImageEntity?> GetImageByIdAsync(int id)
        {
            return await this._db.Images
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<ImageEntity> CreateImageAsync(string url, IList<string> tags)
        {
            var tagEntities = await this.GetOrCreateTagsAsync(tags);

            var image = new ImageEntity { Url = url, Tags = tagEntities };
            
            this._db.Images.Add(image);
            await this._db.SaveChangesAsync();
            
            this._logger.LogInformation("Image created with ID {ImageId} and URL {ImageUrl}", image.Id, image.Url);

            return image;
        }
        
        public async Task<bool> DeleteImageAsync(int id)
        {
            var image = await this._db.Images.FindAsync(id);

            if (image is null)
            {
                return false;
            }

            this._db.Images.Remove(image);
            await this._db.SaveChangesAsync();
            
            this._logger.LogInformation("Image with ID {ImageId} deleted", id);

            return true;
        }
        
        public async Task<IEnumerable<ImageEntity>> GetImagesByTagsAsync(int from, int limit, IList<string> tags)
        {
            var query = this._db.Images
                .Include(x => x.Tags)
                .AsQueryable();

            query = this.ApplyTagFilter(query, tags);
            
            return await query
                .OrderBy(x => x.Id)
                .Skip(from)
                .Take(limit)
                .ToListAsync();
        }
        
        public async Task<ImageEntity?> GetRandomImageAsync(IList<string> tags)
        {
            var query = this._db.Images
                .Include(x => x.Tags)
                .AsQueryable();
            
            query = this.ApplyTagFilter(query, tags);
            
#pragma warning disable EntityFramework.ClientSideDbFunctionCall // false positive warning (trust me)
            
            return await query
                .OrderBy(_ => EF.Functions.Random())
                .FirstOrDefaultAsync();

#pragma warning restore EntityFramework.ClientSideDbFunctionCall
        }
    }
}
