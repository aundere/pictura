using Pictura.Api.Entities;

namespace Pictura.Api.Dtos
{
    public class ImagesResponseDto
    {
        public IEnumerable<ImageEntity> Images { get; init; } = new List<ImageEntity>();
    }
}
