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
    public class SleepSettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SleepSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // GET: api/SleepSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SleepSetting>>> GetSleepSettings()
        {
            var sleepSettings = await _settingsService.GetAllSleepSettings();
            return sleepSettings.ToList();
        }

        // GET: api/SleepSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SleepSetting>> GetSleepSetting(int id)
        {
            var sleepSetting = await _settingsService.GetSleepSetting(id);
            if (sleepSetting == null) NotFound();
            return sleepSetting!;
        }

        // PUT: api/SleepSettings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSleepSetting(int id, SleepSetting sleepSetting)
        {
            var success = await _settingsService.PutSleepSetting(id, sleepSetting);
            switch(success)
            {
                case 204:
                    return NoContent();
                case 400:
                    return BadRequest();
                case 404:
                    return NotFound();
                default:
                    return BadRequest();
            }
        }

        // POST: api/SleepSettings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SleepSetting>> PostSleepSetting(SleepSetting sleepSetting)
        {
            var newSleepSetting = await _settingsService.PostSleepSetting(sleepSetting);
            if(newSleepSetting == null) return Problem("Entity set 'postgresContext.SleepSettings' is null.");
            return CreatedAtAction("GetSleepSetting", new { id = newSleepSetting!.Id }, newSleepSetting!);
        }

        // DELETE: api/SleepSettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSleepSetting(int id)
        {
            var statusCode = await _settingsService.DeleteSleepSetting(id);
            switch (statusCode)
            {
                case 204:
                    return NoContent();
                case 400:
                    return BadRequest();
                case 404:
                    return NotFound();
                default:
                    return BadRequest();
            }
        }
    }
}
