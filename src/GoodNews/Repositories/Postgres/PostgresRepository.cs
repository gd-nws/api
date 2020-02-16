namespace GoodNews.Repositories.Postgres
{
    public class PostgresRepository
    {
        protected readonly GoodNewsDBContext Db;

        protected PostgresRepository(GoodNewsDBContext db)
        {
            Db = db;
        }
    }
}