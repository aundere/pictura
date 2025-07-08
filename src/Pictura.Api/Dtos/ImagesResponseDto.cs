namespace Pictura.Api.Dtos
{
    public record ImagesResponseDto
    {
        public IEnumerable<ImageResponseDto> Images { get; init; } = [];
    }
}
