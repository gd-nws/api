using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models.DBModels;
using GoodNews.Models.Requests;
using GoodNews.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.DBModels.Mongo;

namespace GoodNews.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AnnotationsController : ControllerBase
  {
    private readonly INewsHeadlineRepository _headlineRepository;
    private readonly ISessionRepository _sessionRepository;

    public AnnotationsController(
        INewsHeadlineRepository headlineRepository,
        ISessionRepository sessionRepository
    )
    {
      _headlineRepository = headlineRepository;
      _sessionRepository = sessionRepository;
    }

    /// <summary>
    ///   Annotate a headline
    /// </summary>
    /// <param name="headlineId"></param>
    /// <param name="annotation"></param>
    /// <returns></returns>
    [HttpPost("{headlineId}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> StoreSessionAnnotation(int headlineId, NewSessionAnnotationRequest annotation)
    {
      INewsHeadline headline = await _headlineRepository.GetHeadline(headlineId);
      var session = await _sessionRepository.GetById(annotation.SessionToken);

      if (headline == null) return NotFound("Headline not found");
      if (session == null) return NotFound("Session not found");

      var annotations = new List<HeadlineAnnotation>(headline.Annotations);
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
  }
}