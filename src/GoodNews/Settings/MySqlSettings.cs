namespace GoodNews.Models.Settings
{
    public interface IMySqlSettings
    {
        string ConnectionString { get; set; }
    }
    
    public class MySqlSettings : IMySqlSettings
    {
        public string ConnectionString { get; set; }
    }
}