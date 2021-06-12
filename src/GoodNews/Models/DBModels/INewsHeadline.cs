using System;
using System.Collections.Generic;

namespace GoodNews.Models.DBModels
{
  public interface INewsHeadline : IDatabaseModel
  {
    new string Id { get; set; }
    new DateTime CreatedAt { get; set; }
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

    IHeadlineVotes Votes { get; set; }

    IEnumerable<IHeadlineAnnotation> Annotations { get; set; }
  }
}

