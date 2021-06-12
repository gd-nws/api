using System;

namespace Models.DBModels
{
  public interface IHeadlineAnnotation
  {
    int Vote { get; set; }
    string SessionId { get; set; }
    DateTime createdAt { get; set; }
  }
}