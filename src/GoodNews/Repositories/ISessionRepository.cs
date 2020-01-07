using System.Threading.Tasks;
using GoodNews.Models;

namespace GoodNews.Repositories
{
    public interface ISessionRepository
    {
        Task<Session> GetSession(string id);
        string CreateSession();
    }
}