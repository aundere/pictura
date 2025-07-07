namespace Pictura.Api.Entities
{
    public record ImageEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        
        public required string Url { get; init; }
    }
}
