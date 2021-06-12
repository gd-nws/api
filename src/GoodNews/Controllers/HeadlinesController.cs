using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNews.Models;
using GoodNews.Models.DBModels;
using GoodNews.Models.DBModels.Mongo;
using GoodNews.Models.Requests;
using GoodNews.Models.Responses;
using GoodNews.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GoodNews.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class HeadlinesController : ControllerBase
  {
    private readonly INewsHeadlineRepository _headlineRepository;
    private readonly ISessionRepository _sessionRepository;

    public HeadlinesController(
      INewsHeadlineRepository headlineRepository,
      ISessionRepository sessionRepository)
    {
      _headlineRepository = headlineRepository;
      _sessionRepository = sessionRepository;
    }

    /// <summary>
    ///     Fetch daily headlines ordered by their sentiment.
    /// </summary>
    /// <param name="sentiment"></param>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <param name="dateTime"></param>
    /// <param name="sessionToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<HeadlinesResponse>> GetHeadlinesByDay(
        [FromQuery(Name = "sentiment")] HeadlineSentiment sentiment,
        [FromQuery(Name = "limit")] int limit, [FromQuery(Name = "page")] int page,
        [FromQuery(Name = "date")] DateTime dateTime,
        [FromHeader(Name = "annotation-session")] string sessionToken)
    {
      if (page <= 0) return BadRequest("Page must be greater than 0");

      var today = DateTime.Now;
      if (dateTime == new DateTime()) dateTime = today;

      if (dateTime > today) return BadRequest("Date cannot be in the future");

      var dateOffset = (today - dateTime).TotalDays;
      var offset = (page - 1) * limit;

      var headlines =
          await _headlineRepository.FetchHeadlinesBySentiment(sentiment, (int)dateOffset, limit, offset);
      var count = await _headlineRepository.FetchHeadlinesBySentimentCount(sentiment, (int)dateOffset);

      foreach (var headline in headlines)
      {
        headline.Annotations = FilterHeadlineAnnotations(headline, sessionToken);
      }

      return new HeadlinesResponse
      {
        Headlines = headlines,
        Count = count
      };
    }

    /// <summary>
    ///     Get a news headline
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sessionToken"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetHeadline")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<HeadlineResponse>> GetHeadline(
      string id, [FromHeader(Name = "annotation-session")] string sessionToken)
    {
      var headline = await _headlineRepository.GetById(id);
      if (headline == null) return NotFound();

      headline.Annotations = FilterHeadlineAnnotations(headline, sessionToken);

      return new HeadlineResponse
      {
        Headline = headline
      };
    }

    /// <summary>
    ///     Search headlines for a term.
    /// </summary>
    /// <param name="sentiment"></param>
    /// <param name="limit"></param>
    /// <param name="page"></param>
    /// <param name="term"></param>
    /// <param name="sessionToken"></param>
    /// <returns></returns>
    [HttpGet("search", Name = "SearchHeadlines")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<HeadlinesResponse>> SearchHeadlines(
        [FromQuery(Name = "term")] string term,
        [FromHeader(Name = "annotation-session")] string sessionToken,
        [FromQuery(Name = "sentiment")] HeadlineSentiment sentiment = HeadlineSentiment.POSITIVE,
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "limit")] int limit = 10)
    {
      if (page <= 0) return BadRequest("Page must be greater than 0");
      var offset = (page - 1) * limit;

      var headlines =
          await _headlineRepository.SearchHeadlines(sentiment, term, limit, offset);
      var count = await _headlineRepository.SearchHeadlinesCount(sentiment, term);

      foreach (var headline in headlines)
      {
        headline.Annotations = FilterHeadlineAnnotations(headline, sessionToken);
      }

      return new HeadlinesResponse
      {
        Headlines = (System.Collections.Generic.IList<Models.DBModels.INewsHeadline>)headlines,
        Count = count
      };
    }

    /// <summary>
    ///   Annotate a headline
    /// </summary>
    /// <param name="headlineId"></param>
    /// <param name="annotation"></param>
    /// <returns></returns>
    [HttpPost("{headlineId}/annotations")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> AnnotateHeadline(string headlineId, NewSessionAnnotationRequest annotation)
    {
      INewsHeadline headline = await _headlineRepository.GetById(headlineId);
      var session = await _sessionRepository.GetById(annotation.SessionToken);

      if (headline == null) return NotFound("Headline not found");
      if (session == null) return NotFound("Session not found");

      var annotations = headline.Annotations.ToList();
      annotations.Add(new HeadlineAnnotation()
      {
        SessionId = annotation.SessionToken,
        Vote = annotation.Annotation == Models.HeadlineSentiment.POSITIVE ? 1 : -1,
        CreatedAt = DateTime.Now
      });
      headline.Annotations = annotations;

      await _headlineRepository.UpdateHeadline(headline);

      return NoContent();
    }

    private List<IHeadlineAnnotation> FilterHeadlineAnnotations(INewsHeadline headline, string session)
    {
      return String.IsNullOrEmpty(session) ?
        new List<IHeadlineAnnotation>() :
        headline.Annotations
          .Where(a => a.SessionId.Equals(session))
          .ToList();
    }
  }
}