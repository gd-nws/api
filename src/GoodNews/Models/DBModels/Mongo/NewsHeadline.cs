using System;
using GoodNews.Models.DBModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.DBModels.Mongo
{

  public class MongoNewsHeadline : INewsHeadline
  {
    public MongoNewsHeadline()
    {
      Annotations = new HeadlineAnnotation[] { };
    }

    // Old numeric Id
    [BsonElement("legacyId")]
    public long Id { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string HeadlineId { get; set; }

    [BsonElement("headline")]
    public string Headline { get; set; }

    [BsonElement("predictedClass")]
    public long? PredictedClass { get; set; }

    [BsonElement("link")]
    public string Link { get; set; }

    [BsonElement("origin")]
    public string Origin { get; set; }

    [BsonElement("semanticValue")]
    public double SemanticValue { get; set; }

    [BsonElement("hashcode")]
    public string Hashcode { get; set; }

    [BsonElement("pos")]
    public double Pos { get; set; }

    [BsonElement("neg")]
    public double Neg { get; set; }

    [BsonElement("neu")]
    public double Nue { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    [BsonElement("publishedAt")]
    public DateTime PublishedAt { get; set; }

    [BsonElement("displayImage")]
    public string DisplayImage { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("votes")]
    public HeadlineVotes Votes { get; set; }

    [BsonElement("annotations")]
    public HeadlineAnnotation[] Annotations { get; set; }
  }
}