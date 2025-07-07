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
            // TODO: Implement search functionality based on query parameters
            
            return new ImagesResponseDto { Images = this._imagesStorage.Images.Values };
        }

        [HttpPost]
        public void Post([FromBody] CreateImageDto request)
        {
            this._imagesStorage.AddImage(new ImageEntity
            {
                Url = request.Url
            });
        }
    }
}
