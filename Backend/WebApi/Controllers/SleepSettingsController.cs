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
    public class SleepSettingsController : ControllerBase
    {
        private readonly postgresContext _context;

        public SleepSettingsController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/SleepSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SleepSetting>>> GetSleepSettings()
        {
          if (_context.SleepSettings == null)
          {
              return NotFound();
          }
            return await _context.SleepSettings.ToListAsync();
        }

        // GET: api/SleepSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SleepSetting>> GetSleepSetting(int id)
        {
          if (_context.SleepSettings == null)
          {
              return NotFound();
          }
            var sleepSetting = await _context.SleepSettings.FindAsync(id);

            if (sleepSetting == null)
            {
                return NotFound();
            }

            return sleepSetting;
        }

        // PUT: api/SleepSettings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSleepSetting(int id, SleepSetting sleepSetting)
        {
            if (id != sleepSetting.Id)
            {
                return BadRequest();
            }

            _context.Entry(sleepSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SleepSettingExists(id))
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

        // POST: api/SleepSettings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SleepSetting>> PostSleepSetting(SleepSetting sleepSetting)
        {
          if (_context.SleepSettings == null)
          {
              return Problem("Entity set 'postgresContext.SleepSettings'  is null.");
          }
            _context.SleepSettings.Add(sleepSetting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSleepSetting", new { id = sleepSetting.Id }, sleepSetting);
        }

        // DELETE: api/SleepSettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSleepSetting(int id)
        {
            if (_context.SleepSettings == null)
            {
                return NotFound();
            }
            var sleepSetting = await _context.SleepSettings.FindAsync(id);
            if (sleepSetting == null)
            {
                return NotFound();
            }

            _context.SleepSettings.Remove(sleepSetting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SleepSettingExists(int id)
        {
            return (_context.SleepSettings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
