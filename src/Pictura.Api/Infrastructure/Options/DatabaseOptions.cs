namespace Pictura.Api.Infrastructure.Options
{
    public class DatabaseOptions
    {
        public DatabaseType Type { get; init; } = DatabaseType.Sqlite;
        
        public string ConnectionString { get; init; } = "Data Source=pictura.db";
        
        public enum DatabaseType
        {
            Sqlite
            // Other database types can be added here in the future
        }
    }
}
