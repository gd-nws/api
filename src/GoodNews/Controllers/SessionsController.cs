using System;
using System.Threading.Tasks;
using GoodNews.Models.DBModels.Mongo;
using GoodNews.Models.Responses;
using GoodNews.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GoodNews.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class SessionsController : ControllerBase
  {
    private readonly ISessionRepository _sessionRepository;

    public SessionsController(ISessionRepository sessionRepository)
    {
      _sessionRepository = sessionRepository;
    }

    /**
     * Get a new session.
     */
    [HttpGet]
    public async Task<ActionResult<NewSessionResponse>> CreateSession()
    {
      var session = new Session() { CreatedAt = DateTime.Now };
      var uuid = await _sessionRepository.Create(session);
      return new NewSessionResponse
      {
        SessionToken = session.Id
      };
    }
  }
}