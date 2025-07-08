using Pictura.Api.Entities;

namespace Pictura.Api.Dtos
{
    public record ImageResponseDto
    {
        public required int Id { get; init; }
        
        public required string Url { get; init; } = string.Empty;
        
        public required IEnumerable<string> Tags { get; init; } = new List<string>();
        
        public static ImageResponseDto FromImageEntity(ImageEntity image)
        {
            return new ImageResponseDto
            {
                Id = image.Id,
                Url = image.Url,
                Tags = image.Tags.Select(tag => tag.Name).ToList()
            };
        }
    }
}
