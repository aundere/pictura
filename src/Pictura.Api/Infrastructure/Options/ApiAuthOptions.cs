namespace Pictura.Api.Infrastructure.Options
{
    public class ApiAuthOptions
    {
        public string SecretKey { get; init; } = string.Empty;

        public RequiredAuthEndpoints AuthEndpoints { get; init; } = RequiredAuthEndpoints.None;
        
        public enum RequiredAuthEndpoints
        {
            All,
            Modifying,
            None
        }
    }
}
