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
    public class DailyStreaksController : ControllerBase
    {
        private readonly postgresContext _context;

        public DailyStreaksController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/DailyStreaks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyStreak>>> GetDailyStreak()
        {
          if (_context.DailyStreak == null)
          {
              return NotFound();
          }
            return await _context.DailyStreak.ToListAsync();
        }

        // GET: api/DailyStreaks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DailyStreak>> GetDailyStreak(int id)
        {
          if (_context.DailyStreak == null)
          {
              return NotFound();
          }
            var dailyStreak = await _context.DailyStreak.FindAsync(id);

            if (dailyStreak == null)
            {
                return NotFound();
            }

            return dailyStreak;
        }

        // PUT: api/DailyStreaks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDailyStreak(int id, DailyStreak dailyStreak)
        {
            if (id != dailyStreak.Id)
            {
                return BadRequest();
            }

            _context.Entry(dailyStreak).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyStreakExists(id))
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

        // POST: api/DailyStreaks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DailyStreak>> PostDailyStreak(DailyStreak dailyStreak)
        {
          if (_context.DailyStreak == null)
          {
              return Problem("Entity set 'postgresContext.DailyStreak'  is null.");
          }
            _context.DailyStreak.Add(dailyStreak);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDailyStreak", new { id = dailyStreak.Id }, dailyStreak);
        }

        // DELETE: api/DailyStreaks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDailyStreak(int id)
        {
            if (_context.DailyStreak == null)
            {
                return NotFound();
            }
            var dailyStreak = await _context.DailyStreak.FindAsync(id);
            if (dailyStreak == null)
            {
                return NotFound();
            }

            _context.DailyStreak.Remove(dailyStreak);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DailyStreakExists(int id)
        {
            return (_context.DailyStreak?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
