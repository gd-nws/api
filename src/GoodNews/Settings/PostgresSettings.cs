namespace GoodNews.Models.Settings
{
    public interface IPostgresSettings
    {
        string ConnectionString { get; set; }
    }

    public class PostgresSettings : IPostgresSettings
    {
        public string ConnectionString { get; set; }
    }
}