using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models;
using GoodNews.Models.DBModels;
using GoodNews.Models.DBModels.Postgres;

namespace GoodNews.Repositories
{
  public interface IAnnotationRepository
  {
    Task<List<SessionAnnotation>> GetSessionAnnotations(string session);
    Task CreateSessionAnnotation(INewsHeadline headline, Session session, HeadlineSentiment sentiment);
  }
}