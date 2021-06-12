using System;

namespace GoodNews.Models.DBModels
{
  public interface ISession : IDatabaseModel
  {
    new string Id { get; set; }
    new DateTime CreatedAt { get; set; }
  }
}

