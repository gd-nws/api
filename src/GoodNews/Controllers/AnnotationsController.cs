using System.Threading.Tasks;
using GoodNews.Models.Requests;
using GoodNews.Models.Responses;
using GoodNews.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GoodNews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnnotationsController : ControllerBase
    {
        private readonly IAnnotationRepository _annotationRepository;
        private readonly INewsHeadlineRepository _headlineRepository;
        private readonly ISessionRepository _sessionRepository;

        public AnnotationsController(IAnnotationRepository annotationRepository,
            INewsHeadlineRepository headlineRepository, ISessionRepository sessionRepository)
        {
            _annotationRepository = annotationRepository;
            _headlineRepository = headlineRepository;
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        ///     Get all annotations for a session.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("sessions/{sessionId}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<SessionAnnotationsResponse>> GetSessionAnnotations(string sessionId)
        {
            var annotations = await _annotationRepository.GetSessionAnnotations(sessionId);
            return new SessionAnnotationsResponse
            {
                Annotations = annotations
            };
        }

        [HttpPost("{headlineId}")]
        public async Task<IActionResult> StoreSessionAnnotation(int headlineId, NewSessionAnnotationRequest annotation)
        {
            var headline = await _headlineRepository.GetHeadline(headlineId);
            var session = await _sessionRepository.GetSession(annotation.SessionToken);

            if (headline == null) return NotFound("Headline not found");
            if (session == null) return NotFound("Session not found");

            await _annotationRepository.CreateSessionAnnotation(headline, session, annotation.Annotation);

            return NoContent();
        }
    }
}