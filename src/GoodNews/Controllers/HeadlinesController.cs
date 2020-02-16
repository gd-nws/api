using System;
using System.Threading.Tasks;
using GoodNews.Models;
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

        public HeadlinesController(INewsHeadlineRepository headlineRepository)
        {
            _headlineRepository = headlineRepository;
        }

        /// <summary>
        /// Fetch daily headlines ordered by their sentiment.
        /// </summary>
        /// <param name="sentiment"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<HeadlinesResponse>> GetHeadlinesByDay(
            [FromQuery(Name = "sentiment")] HeadlineSentiment sentiment,
            [FromQuery(Name = "limit")] int limit, [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "date")] DateTime dateTime)
        {
            if (page <= 0) return BadRequest("Page must be greater than 0");

            var today = DateTime.Now;
            if (dateTime == new DateTime()) dateTime = today;

            if (dateTime > today) return BadRequest("Date cannot be in the future");

            var dateOffset = (today - dateTime).TotalDays;
            var offset = (page - 1) * limit;

            var headlines =
                await _headlineRepository.FetchHeadlinesBySentiment(sentiment, (int) dateOffset, limit, offset);
            var count = await _headlineRepository.FetchHeadlinesBySentimentCount(sentiment, (int) dateOffset);

            return new HeadlinesResponse
            {
                Headlines = headlines,
                Count = count
            };
        }

        /// <summary>
        /// Get a news headline
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetHeadline")]
        [Produces("application/json")]
        public async Task<ActionResult<HeadlineResponse>> GetHeadline(int id)
        {
            var headline = await _headlineRepository.GetHeadline(id);
            if (headline == null) return NotFound();

            return new HeadlineResponse
            {
                Headline = headline
            };
        }

        /// <summary>
        /// Search headlines for a term.
        /// </summary>
        /// <param name="sentiment"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        [HttpGet("search", Name = "SearchHeadlines")]
        [Produces("application/json")]
        public async Task<ActionResult<HeadlinesResponse>> SearchHeadlines([FromQuery(Name = "term")] string term,
            [FromQuery(Name = "sentiment")] HeadlineSentiment sentiment = HeadlineSentiment.POSITIVE,
            [FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "limit")] int limit = 10)
        {
            if (page <= 0) return BadRequest("Page must be greater than 0");
            var offset = (page - 1) * limit;

            var headlines =
                await _headlineRepository.SearchHeadlines(sentiment, term, limit, offset);
            var count = await _headlineRepository.SearchHeadlinesCount(term);

            return new HeadlinesResponse
            {
                Headlines = headlines,
                Count = count
            };
        }
    }
}