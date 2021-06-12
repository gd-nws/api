using System.Threading.Tasks;
using GoodNews.Models.DBModels.Mongo;

namespace GoodNews.Repositories
{
  public interface ISessionRepository
  {
    Task<Session> Create(Session session);
    Task<Session> GetById(string id);
  }
}