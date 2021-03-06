using System.Threading.Tasks;
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
            var uuid = await _sessionRepository.CreateSession();
            return new NewSessionResponse
            {
                SessionToken = uuid
            };
        }
    }
}