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
        private readonly ImageService _imageService;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(ImageService imageService, ILogger<ImagesController> logger)
        {
            this._imageService = imageService;
            this._logger = logger;
        }
        
        [HttpGet]
        public async Task<ImagesResponseDto> Get([FromQuery] ImagesQueryDto queryDto)
        {
            var images = await this._imageService
                .GetImagesByTagsAsync(queryDto.Offset, queryDto.Limit, queryDto.Tags);

            var imagesDto = images.Select(ImageResponseDto.FromImageEntity);
            
            return new ImagesResponseDto { Images = imagesDto };
        }

        [HttpPost]
        public async Task<ImageResponseDto> Post([FromBody] CreateImageDto request)
        {
            var image = await this._imageService.CreateImageAsync(request.Url, request.Tags);
            
            this._logger.LogInformation("Image created: {Url}", request.Url);

            return ImageResponseDto.FromImageEntity(image);
        }
        
        [HttpGet("random")]
        public async Task<IResult> GetRandomImage([FromQuery] RandomImageQueryDto queryDto)
        {
            var image = await this._imageService.GetRandomImageAsync(queryDto.Tags);
            
            return image is null ? Results.NotFound() : Results.Ok(ImageResponseDto.FromImageEntity(image));
        }
    }
}
