using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly postgresContext _context;

        public SurveysController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/Surveys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Survey>>> GetSurveys()
        {
          if (_context.Surveys == null)
          {
              return NotFound();
          }
            return await _context.Surveys.ToListAsync();
        }

        // GET: api/Surveys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Survey>> GetSurvey(int id)
        {
          if (_context.Surveys == null)
          {
              return NotFound();
          }
            var survey = await _context.Surveys.FindAsync(id);

            if (survey == null)
            {
                return NotFound();
            }

            return survey;
        }

        // PUT: api/Surveys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurvey(int id, Survey survey)
        {
            if (id != survey.Id)
            {
                return BadRequest();
            }

            _context.Entry(survey).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Surveys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Survey>> PostSurvey(Survey survey)
        {
          if (_context.Surveys == null)
          {
              return Problem("Entity set 'postgresContext.Surveys'  is null.");
          }
            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSurvey", new { id = survey.Id }, survey);
        }

        // DELETE: api/Surveys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
            if (_context.Surveys == null)
            {
                return NotFound();
            }
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SurveyExists(int id)
        {
            return (_context.Surveys?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
