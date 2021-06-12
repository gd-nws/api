namespace GoodNews.Models.Settings
{
  public interface IMongoSettings
  {
    public string HeadlinesCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public class MongoSettings : IMongoSettings
  {
    public string HeadlinesCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }
}