using System.Collections.Generic;
using GoodNews.Models.DBModels;

namespace GoodNews.Models.Responses
{
  public class HeadlinesResponse
  {
    public IList<INewsHeadline> Headlines { get; set; }
    public int Count { get; set; }
  }
}