using System;

namespace GoodNews.Models.DBModels
{
  public interface IDatabaseModel
  {
    string Id { get; set; }
    DateTime CreatedAt { get; set; }
  }
}