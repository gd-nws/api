using System;
using System.Collections.Generic;
using GoodNews.Models.DBModels.Mongo;

namespace GoodNews.Models.DBModels
{
  public interface INewsHeadline
  {
    string Id { get; set; }
    long LegacyId { get; set; }
    string Headline { get; set; }
    long? PredictedClass { get; set; }

    string Link { get; set; }

    string Origin { get; set; }

    double SemanticValue { get; set; }

    string Hashcode { get; set; }

    double Pos { get; set; }

    double Neg { get; set; }

    double Nue { get; set; }

    DateTime PublishedAt { get; set; }

    string DisplayImage { get; set; }

    DateTime CreatedAt { get; set; }

    HeadlineVotes Votes { get; set; }

    List<HeadlineAnnotation> Annotations { get; set; }
  }
}

