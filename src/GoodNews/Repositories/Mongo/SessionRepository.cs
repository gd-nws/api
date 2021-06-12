using System;
using System.Threading.Tasks;
using GoodNews.Models.DBModels;
using GoodNews.Models.DBModels.Mongo;
using GoodNews.Models.Settings;
using MongoDB.Driver;

namespace GoodNews.Repositories.Mongo
{
  public class SessionRepository : BaseRepository<ISession>, ISessionRepository
  {
    private readonly IMongoCollection<Session> _sessions;
    public SessionRepository(IMongoSettings settings) : base(settings)
    {
      var db = base._db;
      Console.WriteLine(db);
      _sessions = db.GetCollection<Session>("Sessions");
    }

    public async override Task<ISession> Create(ISession session)
    {
      await _sessions.InsertOneAsync((Session)session);
      return session;
    }

    public async override Task<ISession> GetById(string id)
    {
      var session = await _sessions.Find(s => s.Id.Equals(id)).SingleOrDefaultAsync();
      return session;
    }
  }
}