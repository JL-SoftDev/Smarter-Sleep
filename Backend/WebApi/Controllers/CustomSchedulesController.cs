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
    public class CustomSchedulesController : ControllerBase
    {
        private readonly postgresContext _context;

        public CustomSchedulesController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/CustomSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomSchedule>>> GetCustomSchedules()
        {
          if (_context.CustomSchedules == null)
          {
              return NotFound();
          }
            return await _context.CustomSchedules.ToListAsync();
        }

        // GET: api/CustomSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomSchedule>> GetCustomSchedule(Guid id)
        {
          if (_context.CustomSchedules == null)
          {
              return NotFound();
          }
            var customSchedule = await _context.CustomSchedules.FindAsync(id);

            if (customSchedule == null)
            {
                return NotFound();
            }

            return customSchedule;
        }

        // PUT: api/CustomSchedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomSchedule(Guid id, CustomSchedule customSchedule)
        {
            if (id != customSchedule.UserId)
            {
                return BadRequest();
            }

            _context.Entry(customSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomScheduleExists(id))
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

        // POST: api/CustomSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomSchedule>> PostCustomSchedule(CustomSchedule customSchedule)
        {
          if (_context.CustomSchedules == null)
          {
              return Problem("Entity set 'postgresContext.CustomSchedules'  is null.");
          }
            _context.CustomSchedules.Add(customSchedule);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomScheduleExists(customSchedule.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomSchedule", new { id = customSchedule.UserId }, customSchedule);
        }

        // DELETE: api/CustomSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomSchedule(Guid id)
        {
            if (_context.CustomSchedules == null)
            {
                return NotFound();
            }
            var customSchedule = await _context.CustomSchedules.FindAsync(id);
            if (customSchedule == null)
            {
                return NotFound();
            }

            _context.CustomSchedules.Remove(customSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomScheduleExists(Guid id)
        {
            return (_context.CustomSchedules?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
