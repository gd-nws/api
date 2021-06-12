using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoodNews.Models.DBModels.Mongo
{
  public class BaseMongoEntity
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
  }
}