using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models;
using GoodNews.Models.DBModels;

namespace GoodNews.Repositories
{
  public interface IAnnotationRepository
  {
    Task<List<SessionAnnotation>> GetSessionAnnotations(string session);
    Task CreateSessionAnnotation(INewsHeadline headline, Session session, HeadlineSentiment sentiment);
  }
}