using Pictura.Api.Entities;

namespace Pictura.Api.Dtos
{
    public record ImagesResponseDto
    {
        public IEnumerable<ImageEntity> Images { get; init; } = new List<ImageEntity>();
    }
}
