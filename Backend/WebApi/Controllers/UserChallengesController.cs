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
    public class UserChallengesController : ControllerBase
    {
        private readonly postgresContext _context;
        private readonly IChallengeProgressService _challengeProgressService;

        public UserChallengesController(postgresContext context, IChallengeProgressService challengeProgressService)
        {
            _context = context;
            _challengeProgressService = challengeProgressService;
        }

        // GET: api/UserChallenges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserChallenge>>> GetUserChallenges()
        {
          if (_context.UserChallenges == null)
          {
              return NotFound();
          }
            return await _context.UserChallenges.ToListAsync();
        }

        // GET: api/UserChallenges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserChallenge>> GetUserChallenge(int id)
        {
          if (_context.UserChallenges == null)
          {
              return NotFound();
          }
            var userChallenge = await _context.UserChallenges.FindAsync(id);

            if (userChallenge == null)
            {
                return NotFound();
            }

            return userChallenge;
        }

        // PUT: api/UserChallenges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserChallenge(int id, UserChallenge userChallenge)
        {
            if (id != userChallenge.Id)
            {
                return BadRequest();
            }

            _context.Entry(userChallenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserChallengeExists(id))
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

        // POST: api/UserChallenges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserChallenge>> PostUserChallenge(UserChallenge userChallenge)
        {
          if (_context.UserChallenges == null)
          {
              return Problem("Entity set 'postgresContext.UserChallenges'  is null.");
          }
            _context.UserChallenges.Add(userChallenge);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserChallenge", new { id = userChallenge.Id }, userChallenge);
        }

        // DELETE: api/UserChallenges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserChallenge(int id)
        {
            if (_context.UserChallenges == null)
            {
                return NotFound();
            }
            var userChallenge = await _context.UserChallenges.FindAsync(id);
            if (userChallenge == null)
            {
                return NotFound();
            }

            _context.UserChallenges.Remove(userChallenge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Route("progress")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<IChallengeProgressService.ChallengeReturn>>> GetChallengeProgress(Guid userId)
        {
            var getChallengeProgressList = await _challengeProgressService.GetChallengeProgress(userId);
            List<IChallengeProgressService.ChallengeReturn> challengeProgressList = getChallengeProgressList.ToList();
            return challengeProgressList;
        }

        private bool UserChallengeExists(int id)
        {
            return (_context.UserChallenges?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
