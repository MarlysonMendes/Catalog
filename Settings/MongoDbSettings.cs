namespace Catalog.Settings
{
    public class MongoDbSettings
    {
        public string host { get; set; }
        public int port { get; set; }

        public string ConnectionString => $"mongodb://{host}:{port}";	
    }
}