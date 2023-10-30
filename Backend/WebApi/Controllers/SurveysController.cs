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
    public class SurveysController : ControllerBase
    {
        private readonly postgresContext _context;
        private readonly ISleepDataService _sleepDataService;

        public SurveysController(postgresContext context, ISleepDataService sleepDataService)
        {
            _context = context;
            _sleepDataService = sleepDataService;
        }

        // GET: api/Surveys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Survey>>> GetSurveys()
        {
            var surveys = await _sleepDataService.GetSurveys();
            if (surveys == null) return NotFound();
            return surveys.ToList();
        }

        // GET: api/Surveys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Survey>> GetSurvey(int id)
        {
			var survey = await _sleepDataService.GetSurvey(id);
			if (survey == null) return NotFound();
			return survey;
		}

        // PUT: api/Surveys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurvey(int id, Survey survey)
        {
			var success = await _sleepDataService.PutSurvey(id, survey);
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

        // POST: api/Surveys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Survey>> PostSurvey(Survey survey)
        {
			var newSurvey = await _sleepDataService.PostSurvey(survey);
			if (newSurvey == null) return Problem("Entity set 'postgresContext.Surveys'  is null.");
			return CreatedAtAction("GetSurvey", new { id = survey.Id }, survey);
        }

        // DELETE: api/Surveys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
			var statusCode = await _sleepDataService.DeleteSurvey(id);
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
    }
}
