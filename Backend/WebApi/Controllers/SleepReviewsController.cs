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
    public class SleepReviewsController : ControllerBase
    {
        private readonly postgresContext _context;

        public SleepReviewsController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/SleepReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SleepReview>>> GetSleepReviews()
        {
          if (_context.SleepReviews == null)
          {
              return NotFound();
          }
            return await _context.SleepReviews.ToListAsync();
        }

        // GET: api/SleepReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SleepReview>> GetSleepReview(int id)
        {
          if (_context.SleepReviews == null)
          {
              return NotFound();
          }
            var sleepReview = await _context.SleepReviews.FindAsync(id);

            if (sleepReview == null)
            {
                return NotFound();
            }

            return sleepReview;
        }

        // PUT: api/SleepReviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSleepReview(int id, SleepReview sleepReview)
        {
            if (id != sleepReview.Id)
            {
                return BadRequest();
            }

            _context.Entry(sleepReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SleepReviewExists(id))
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

        // POST: api/SleepReviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SleepReview>> PostSleepReview(SleepReview sleepReview)
        {
          if (_context.SleepReviews == null)
          {
              return Problem("Entity set 'postgresContext.SleepReviews'  is null.");
          }
            _context.SleepReviews.Add(sleepReview);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSleepReview", new { id = sleepReview.Id }, sleepReview);
        }

        // DELETE: api/SleepReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSleepReview(int id)
        {
            if (_context.SleepReviews == null)
            {
                return NotFound();
            }
            var sleepReview = await _context.SleepReviews.FindAsync(id);
            if (sleepReview == null)
            {
                return NotFound();
            }

            _context.SleepReviews.Remove(sleepReview);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SleepReviewExists(int id)
        {
            return (_context.SleepReviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
