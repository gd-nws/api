using System;
using System.Linq;
using System.Threading.Tasks;
using GoodNews.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodNews.Repositories.Postgres
{
    public class SessionRepository : PostgresRepository, ISessionRepository
    {
        public SessionRepository(GoodNewsDBContext db) : base(db)
        {
        }

        public async Task<Session> GetSession(string id)
        {
            var result = await Db.Sessions.FromSqlRaw($@"
                SELECT *
                FROM good_news.uuids u
                WHERE u.uuid = '{id}'
            ").ToListAsync();

            return result.Count > 0 ? result.First() : null;
        }

        public async Task<string> CreateSession()
        {
            var guid = Guid.NewGuid().ToString();

            await Db.Database.ExecuteSqlRawAsync($@"
                INSERT INTO uuids (uuid)
                VALUES ('{guid}')
            ");
            return guid;
        }
    }
}