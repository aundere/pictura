using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Pictura.Api.Dtos;
using Pictura.Api.Infrastructure.Filters;
using Pictura.Api.Services;

namespace Pictura.Api.Controllers
{
    [ApiController]
    [ApiKeyAuthorize]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImagesController(ImageService imageService)
        {
            this._imageService = imageService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ImagesResponseDto>> Get([FromQuery] ImagesQueryDto queryDto)
        {
            var images = await this._imageService
                .GetImagesByTagsAsync(queryDto.Offset, queryDto.Limit, queryDto.Tags);

            var imagesDto = images.Select(ImageResponseDto.FromImageEntity);
            
            return this.Ok(new ImagesResponseDto { Images = imagesDto });
        }
        
        [HttpPost]
        public async Task<ImageResponseDto> Post([FromBody] CreateImageDto request)
        {
            var image = await this._imageService.CreateImageAsync(request.Url, request.Tags);
            return ImageResponseDto.FromImageEntity(image);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] [Range(0, int.MaxValue, ErrorMessage = "Id must be a non-negative integer.")] int id)
        {
            var deleted = await this._imageService.DeleteImageAsync(id);
            
            return deleted
                ? this.NoContent()
                : this.NotFound();
        }
        
        [HttpGet("random")]
        public async Task<ActionResult<ImageResponseDto>> GetRandomImage([FromQuery] RandomImageQueryDto queryDto)
        {
            var image = await this._imageService.GetRandomImageAsync(queryDto.Tags);
            
            return image is not null
                ? this.Ok(ImageResponseDto.FromImageEntity(image))
                : this.NotFound();
        }
    }
}
