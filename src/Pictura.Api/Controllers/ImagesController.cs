using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Pictura.Api.Dtos;
using Pictura.Api.Infrastructure.Filters;
using Pictura.Api.Services;

// ReSharper disable UnusedMember.Global

namespace Pictura.Api.Controllers
{
    [ApiController]
    [ApiKeyAuthorize]
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
        
        [HttpDelete("{id:int}")]
        public async Task<IResult> Delete([FromRoute] [Range(0, int.MaxValue, ErrorMessage = "Id must be a non-negative integer.")] int id)
        {
            var deleted = await this._imageService.DeleteImageAsync(id);

            if (!deleted)
            {
                return Results.Problem(new ProblemDetails
                {
                    Title = "Image not found",
                    Status = 404,
                    Detail = "No image found matching the specified id."
                });
            }
            
            this._logger.LogInformation("Image deleted: {Id}", id);
            
            return Results.NoContent();

        }
        
        [HttpGet("random")]
        public async Task<IResult> GetRandomImage([FromQuery] RandomImageQueryDto queryDto)
        {
            var image = await this._imageService.GetRandomImageAsync(queryDto.Tags);

            if (image is not null)
            {
                return Results.Ok(ImageResponseDto.FromImageEntity(image));
            }

            return Results.Problem(new ProblemDetails
            {
                Title = "Image not found",
                Status = 404,
                Detail = "No image found matching the specified tags."
            });
        }
    }
}
