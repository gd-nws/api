using System.Threading.Tasks;
using GoodNews.Models.DBModels;

namespace GoodNews.Repositories
{
    public interface ISessionRepository
    {
        Task<ISession> Create(ISession session);
        Task<ISession> GetById(string id);
    }
}