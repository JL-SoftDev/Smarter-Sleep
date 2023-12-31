﻿using System;
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
        public async Task<ActionResult<IEnumerable<DailyStreak>>> GetDailyStreaks()
        {
          if (_context.DailyStreaks == null)
          {
              return NotFound();
          }
            return await _context.DailyStreaks.ToListAsync();
        }

        // GET: api/DailyStreaks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DailyStreak>> GetDailyStreak(Guid id)
        {
          if (_context.DailyStreaks == null)
          {
              return NotFound();
          }
            var dailyStreak = await _context.DailyStreaks.FindAsync(id);

            if (dailyStreak == null)
            {
                return NotFound();
            }

            return dailyStreak;
        }

        // PUT: api/DailyStreaks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDailyStreak(Guid id, DailyStreak dailyStreak)
        {
            if (id != dailyStreak.UserId)
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
          if (_context.DailyStreaks == null)
          {
              return Problem("Entity set 'postgresContext.DailyStreaks'  is null.");
          }
            _context.DailyStreaks.Add(dailyStreak);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DailyStreakExists(dailyStreak.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDailyStreak", new { id = dailyStreak.UserId }, dailyStreak);
        }

        // DELETE: api/DailyStreaks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDailyStreak(Guid id)
        {
            if (_context.DailyStreaks == null)
            {
                return NotFound();
            }
            var dailyStreak = await _context.DailyStreaks.FindAsync(id);
            if (dailyStreak == null)
            {
                return NotFound();
            }

            _context.DailyStreaks.Remove(dailyStreak);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DailyStreakExists(Guid id)
        {
            return (_context.DailyStreaks?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
