namespace Pictura.Api.Entities
{
    public record ImageEntity
    {
        public int Id { get; init; }

        public string Url { get; init; } = string.Empty;
        
        public List<TagEntity> Tags { get; init; } = [];
        
    }
}
