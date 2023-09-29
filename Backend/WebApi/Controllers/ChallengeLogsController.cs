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
    public class ChallengeLogsController : ControllerBase
    {
        private readonly postgresContext _context;

        public ChallengeLogsController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/ChallengeLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallengeLog>>> GetChallengeLogs()
        {
          if (_context.ChallengeLogs == null)
          {
              return NotFound();
          }
            return await _context.ChallengeLogs
                .Include(cl => cl.Transaction)
                .ToListAsync();
        }

        // GET: api/ChallengeLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChallengeLog>> GetChallengeLog(int id)
        {
          if (_context.ChallengeLogs == null)
          {
              return NotFound();
          }
            var challengeLog = await _context.ChallengeLogs
                .Include(cl => cl.Transaction)
                .FirstOrDefaultAsync(cl => cl.Id == id);

            if (challengeLog == null)
            {
                return NotFound();
            }

            return challengeLog;
        }

        // PUT: api/ChallengeLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallengeLog(int id, ChallengeLog challengeLog)
        {
            if (id != challengeLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(challengeLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeLogExists(id))
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

        // POST: api/ChallengeLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChallengeLog>> PostChallengeLog(ChallengeLog challengeLog)
        {
          if (_context.ChallengeLogs == null)
          {
              return Problem("Entity set 'postgresContext.ChallengeLogs'  is null.");
          }
            _context.ChallengeLogs.Add(challengeLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChallengeLog", new { id = challengeLog.Id }, challengeLog);
        }

        // DELETE: api/ChallengeLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallengeLog(int id)
        {
            if (_context.ChallengeLogs == null)
            {
                return NotFound();
            }
            var challengeLog = await _context.ChallengeLogs.FindAsync(id);
            if (challengeLog == null)
            {
                return NotFound();
            }

            _context.ChallengeLogs.Remove(challengeLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChallengeLogExists(int id)
        {
            return (_context.ChallengeLogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
