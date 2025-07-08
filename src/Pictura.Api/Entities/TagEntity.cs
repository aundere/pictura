namespace Pictura.Api.Entities
{
    public record TagEntity
    {
        public int Id { get; set; }

        public required string Name { get; set; } = string.Empty;
        
        public List<ImageEntity> Images { get; set; } = [];
    }
}
