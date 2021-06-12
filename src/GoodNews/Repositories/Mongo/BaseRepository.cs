using System.Threading.Tasks;
using GoodNews.Models.DBModels;
using GoodNews.Models.Settings;
using MongoDB.Driver;

namespace GoodNews.Repositories.Mongo
{
    public abstract class BaseRepository<T> where T : IDatabaseModel
    {
        protected readonly IMongoDatabase _db;

        public BaseRepository(IMongoSettings settings)
        {
            _db = new MongoClient(settings.ConnectionString)
              .GetDatabase(settings.DatabaseName);
        }

        public abstract Task<T> Create(T t);
        public abstract Task<T> GetById(string id);
    }
}