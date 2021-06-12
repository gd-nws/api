using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GoodNews.Models.DBModels.Mongo
{
  public class HeadlineAnnotation : IHeadlineAnnotation
  {
    [BsonElement("vote")]
    public int Vote { get; set; }

    [BsonElement("sessionId")]
    public string SessionId { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
  }
}