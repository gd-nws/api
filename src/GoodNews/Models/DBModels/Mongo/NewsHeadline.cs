using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace GoodNews.Models.DBModels.Mongo
{
  public class MongoNewsHeadline : BaseMongoEntity, INewsHeadline
  {
    public MongoNewsHeadline()
    {
      Annotations = new List<HeadlineAnnotation>();
    }

    // Old numeric Id
    [BsonElement("legacyId")]
    public long LegacyId { get; set; }

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

    [BsonElement("votes")]
    [BsonSerializer(typeof(ImpliedImplementationInterfaceSerializer<IHeadlineVotes, HeadlineVotes>))]
    public IHeadlineVotes Votes { get; set; }

    [BsonElement("annotations")]
    [BsonSerializer(typeof(ImpliedImplementationInterfaceSerializer<IEnumerable<IHeadlineAnnotation>, List<HeadlineAnnotation>>))]
    public IEnumerable<IHeadlineAnnotation> Annotations { get; set; }
  }
}