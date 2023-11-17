using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SleepReviewsController : ControllerBase
    {
        private readonly ISleepDataService _sleepDataService;

        public SleepReviewsController(ISleepDataService sleepDataService)
        {
            _sleepDataService = sleepDataService;
		}

        // GET: api/SleepReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SleepReview>>> GetSleepReviews()
        {
            var sleepReviews = await _sleepDataService.GetSleepReviews();
            if (sleepReviews == null) return NotFound();
            return sleepReviews.ToList();
        }

        // GET: api/SleepReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SleepReview>> GetSleepReview(int id)
        {
            var sleepReview = await _sleepDataService.GetSleepReview(id);
            if (sleepReview == null) return NotFound();
            return sleepReview;
        }

        // PUT: api/SleepReviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSleepReview(int id, SleepReview sleepReview)
        {
			var success = await _sleepDataService.PutSleepReview(id, sleepReview);
			switch (success)
			{
				//Successful:
				case 204:
					return NoContent();
				//Bad Request
				case 400:
					return BadRequest();
				//Not Found
				case 404:
					return NotFound();
				default:
					return BadRequest();
			}
		}

        // POST: api/SleepReviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SleepReview>> PostSleepReview(SleepReview sleepReview)
        {
            var newSleepReview = await _sleepDataService.PostSleepReview(sleepReview);
			if (newSleepReview == null) return Problem("Entity set 'postgresContext.SleepReviews'  is null.");
			return CreatedAtAction("GetSleepReview", new { id = newSleepReview.Id }, newSleepReview);
		}

        // DELETE: api/SleepReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSleepReview(int id)
        {
			var statusCode = await _sleepDataService.DeleteSleepReview(id);
			switch (statusCode)
			{
				//Successful:
				case 204:
					return NoContent();
				//Bad Request
				case 400:
					return BadRequest();
				case 404:
					return NotFound();
				default:
					return BadRequest();
			}
		}

        public class ReviewPayload
        {
            public Survey Survey { get; set; }
            public WearableData WearableData { get; set; }
        }

        [HttpPost("GenerateReview/{userId}")]
        public async Task<ActionResult<SleepReview>> GenerateReview(Guid userId, [FromBody] ReviewPayload payload)
        {
            if (userId == Guid.Empty || payload == null || payload.Survey == null || payload.WearableData == null)
            {
                return BadRequest("Invalid payload or missing data.");
            }
            try {
                SleepReview generatedReview = await _sleepDataService.GenerateReview(userId, payload.Survey, payload.WearableData);
			    
                return CreatedAtAction("GetSleepReview", new { id = generatedReview.Id }, generatedReview);
            } catch(Exception e) {
                return StatusCode(500, "Internal server error");
            }
		}
    }
}
