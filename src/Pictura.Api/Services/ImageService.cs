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
        
        public async Task<ImageEntity> CreateImageAsync(string url, IEnumerable<string> tags)
        {
            var tagList = tags.ToList();
            
            var existingTags = await this._db.Tags
                .Where(t => tagList.Contains(t.Name))
                .ToListAsync();

            var newTags = tagList
                .Except(existingTags.Select(t => t.Name), StringComparer.OrdinalIgnoreCase)
                .Select(name => new TagEntity { Name = name})
                .ToList();

            if (newTags.Count != 0)
            {
                this._db.Tags.AddRange(newTags);
                await this._db.SaveChangesAsync();
                
                this._logger.LogInformation("Added {NewTagCount} new tags", newTags.Count);
            }
            else
            {
                this._logger.LogInformation("No new tags to add, using existing tags");
            }

            var allTags = existingTags.Concat(newTags).ToList();
            
            var image = new ImageEntity { Url = url, Tags = allTags };
            
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
        
        public async Task<IEnumerable<ImageEntity>> GetImagesByTagsAsync(int from, int limit, IEnumerable<string> tags)
        {
            var tagList = tags.ToList();
            
            var query = this._db.Images
                .Include(x => x.Tags)
                .AsQueryable();
            
            if (tagList.Count > 0)
            {
                query = query.Where(x => tagList.All(r => x.Tags.Any(t => r == t.Name)));
            }
            
            return await query
                .OrderBy(x => x.Id)
                .Skip(from)
                .Take(limit)
                .ToListAsync();
        }
        
        public async Task<ImageEntity?> GetRandomImageAsync(IEnumerable<string> tags)
        {
            var tagList = tags.ToList();

            var query = this._db.Images
                .Include(x => x.Tags)
                .AsQueryable();
            
            if (tagList.Count > 0)
            {
                query = query.Where(x => tagList.All(r => x.Tags.Any(t => r == t.Name)));
            }
            
            return await query
                .OrderBy(_ => EF.Functions.Random())
                .FirstOrDefaultAsync();
        }
    }
}
