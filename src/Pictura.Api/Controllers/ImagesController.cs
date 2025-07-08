using Microsoft.AspNetCore.Mvc;
using Pictura.Api.Dtos;
using Pictura.Api.Entities;
using Pictura.Api.Services;

// ReSharper disable UnusedMember.Global

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
            var images = this._imagesStorage
                .FindByTags(queryDto.Tags) // Filter images by tags
                .Skip(queryDto.Offset) // Apply offset for pagination
                .Take(queryDto.Limit); // Limit the number of images returned
            
            return new ImagesResponseDto { Images = images };
        }

        [HttpPost]
        public void Post([FromBody] CreateImageDto request)
        {
            this._imagesStorage.AddImage(new ImageEntity
            {
                Url = request.Url,
                Tags = request.Tags.ToHashSet()
            });
            
            this._logger.LogInformation("Image created: {Url}", request.Url);
        }
        
        [HttpGet("random")]
        public IResult GetRandomImage([FromQuery] RandomImageQueryDto queryDto)
        {
            var images = this._imagesStorage
                .FindByTags(queryDto.Tags) // Filter images by tags
                .ToList();

            if (images.Count == 0)
            {
                return Results.NotFound();
            }
            
            var randomIndex = this._random.Next(0, images.Count);
            return Results.Ok(images.ElementAt(randomIndex));
        }
    }
}
