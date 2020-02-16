using GoodNews.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodNews
{
    public class GoodNewsDBContext : DbContext
    {
        public GoodNewsDBContext(DbContextOptions opt) : base(opt)
        {
        }

        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<NewsHeadline> NewsHeadlines { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionAnnotation> SessionAnnotations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionAnnotation>(eb => { eb.HasNoKey(); });
        }
    }
}