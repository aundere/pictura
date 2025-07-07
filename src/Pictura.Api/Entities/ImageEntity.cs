namespace Pictura.Api.Entities
{
    public record ImageEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        
        public HashSet<string> Tags { get; init; } = new HashSet<string>();
        
        public required string Url { get; init; }
        
    }
}
