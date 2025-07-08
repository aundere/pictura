using System.ComponentModel.DataAnnotations;

namespace Pictura.Api.Dtos
{
    public record CreateImageDto
    {
        [Required(ErrorMessage = "Image URL is required.")]
        [RegularExpression("https?://.+", ErrorMessage = "Invalid URL format. Must start with http:// or https://.")]
        public required string Url { get; init; }
        
        public required HashSet<string> Tags { get; init; } = new HashSet<string>();
    }
}
