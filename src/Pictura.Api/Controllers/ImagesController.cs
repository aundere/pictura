using Microsoft.AspNetCore.Mvc;
using Pictura.Api.Dtos;
using Pictura.Api.Entities;
using Pictura.Api.Services;

namespace Pictura.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController
    {
        private readonly Random _random = new Random();
        
        private readonly ImagesStorage _imagesStorage;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(ImagesStorage imagesStorage, ILogger<ImagesController> logger)
        {
            this._imagesStorage = imagesStorage;
            this._logger = logger;
        }
        
        [HttpGet]
        public ImagesResponseDto Get([FromQuery] ImagesQueryDto queryDto)
        {
            var queryTags = queryDto.Tags.ToHashSet();
            
            var images = this._imagesStorage.Images.Values
                .Where(x => !queryTags.Except(x.Tags).Any()) // Filter images by tags
                .OrderBy(_ => this._random.Next()) // Randomize order
                .Take(queryDto.Limit); // Limit the number of images returned
            
            return new ImagesResponseDto { Images = images };
        }

        [HttpPost]
        public void Post([FromBody] CreateImageDto request)
        {
            this._imagesStorage.AddImage(new ImageEntity
            {
                Url = request.Url,
                Tags = request.Tags
            });
            
            this._logger.LogInformation("Image created: {Url}", request.Url);
        }
    }
}
