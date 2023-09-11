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
    public class ChallengesController : ControllerBase
    {
        private readonly postgresContext _context;

        public ChallengesController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/Challenges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Challenge>>> GetChallenges()
        {
          if (_context.Challenges == null)
          {
              return NotFound();
          }
            return await _context.Challenges.ToListAsync();
        }

        // GET: api/Challenges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Challenge>> GetChallenge(int id)
        {
          if (_context.Challenges == null)
          {
              return NotFound();
          }
            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null)
            {
                return NotFound();
            }

            return challenge;
        }

        // PUT: api/Challenges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallenge(int id, Challenge challenge)
        {
            if (id != challenge.Id)
            {
                return BadRequest();
            }

            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeExists(id))
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

        // POST: api/Challenges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Challenge>> PostChallenge(Challenge challenge)
        {
          if (_context.Challenges == null)
          {
              return Problem("Entity set 'postgresContext.Challenges'  is null.");
          }
            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChallenge", new { id = challenge.Id }, challenge);
        }

        // DELETE: api/Challenges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            if (_context.Challenges == null)
            {
                return NotFound();
            }
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            _context.Challenges.Remove(challenge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChallengeExists(int id)
        {
            return (_context.Challenges?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
