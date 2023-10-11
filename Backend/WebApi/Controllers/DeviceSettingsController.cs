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
    public class DeviceSettingsController : ControllerBase
    {
        private readonly postgresContext _context;

        public DeviceSettingsController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/DeviceSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceSetting>>> GetDeviceSetting()
        {
          if (_context.DeviceSetting == null)
          {
              return NotFound();
          }
            return await _context.DeviceSetting.ToListAsync();
        }

        // GET: api/DeviceSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceSetting>> GetDeviceSetting(int id)
        {
          if (_context.DeviceSetting == null)
          {
              return NotFound();
          }
            var deviceSetting = await _context.DeviceSetting.FindAsync(id);

            if (deviceSetting == null)
            {
                return NotFound();
            }

            return deviceSetting;
        }

        // PUT: api/DeviceSettings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceSetting(int id, DeviceSetting deviceSetting)
        {
            if (id != deviceSetting.Id)
            {
                return BadRequest();
            }

            _context.Entry(deviceSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceSettingExists(id))
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

        // POST: api/DeviceSettings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeviceSetting>> PostDeviceSetting(DeviceSetting deviceSetting)
        {
          if (_context.DeviceSetting == null)
          {
              return Problem("Entity set 'postgresContext.DeviceSetting'  is null.");
          }
            _context.DeviceSetting.Add(deviceSetting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeviceSetting", new { id = deviceSetting.Id }, deviceSetting);
        }

        // DELETE: api/DeviceSettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceSetting(int id)
        {
            if (_context.DeviceSetting == null)
            {
                return NotFound();
            }
            var deviceSetting = await _context.DeviceSetting.FindAsync(id);
            if (deviceSetting == null)
            {
                return NotFound();
            }

            _context.DeviceSetting.Remove(deviceSetting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceSettingExists(int id)
        {
            return (_context.DeviceSetting?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
