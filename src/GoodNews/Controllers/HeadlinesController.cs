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
        [HttpGet("sentiment/{sentiment}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<HeadlinesResponse>> GetHeadlinesByDay(HeadlineSentiment sentiment,
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
        [HttpGet( "{id}", Name = "GetHeadline")]
        [Produces("application/json")]
        
        public async Task<ActionResult<NewsHeadline>> GetHeadline(int id)
        {
            var headline = await _headlineRepository.GetHeadline(id);
            if (headline == null) return NotFound();

            return headline;
        }
    }
}