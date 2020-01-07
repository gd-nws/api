namespace GoodNews.Repositories.MySQL
{
    public class MySqlRepository
    {
        protected readonly GoodNewsDBContext Db;

        protected MySqlRepository(GoodNewsDBContext db)
        {
            Db = db;
        }
    }
}