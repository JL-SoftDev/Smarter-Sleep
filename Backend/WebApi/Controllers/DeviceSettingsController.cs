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
    public class DeviceSettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public DeviceSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // GET: api/DeviceSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceSetting>>> GetDeviceSetting()
        {
            var deviceSettings = await _settingsService.GetAllDeviceSettings();
            return deviceSettings.ToList();
        }

        // GET: api/DeviceSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceSetting>> GetDeviceSetting(int id)
        {
            var deviceSetting = await _settingsService.GetDeviceSetting(id);
            if(deviceSetting == null) NotFound();
            return deviceSetting!;
        }

        // PUT: api/DeviceSettings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceSetting(int id, DeviceSetting deviceSetting)
        {
            var success = await _settingsService.PutDeviceSetting(id, deviceSetting);
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

        // POST: api/DeviceSettings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeviceSetting>> PostDeviceSetting(DeviceSetting deviceSetting)
        {
            var newDeviceSetting = await _settingsService.PostDeviceSetting(deviceSetting);
            if(newDeviceSetting == null) return Problem("Entity set 'postgresContext.DeviceSettings' is null.");
            return CreatedAtAction("GetDeviceSetting", new { id = newDeviceSetting!.Id }, newDeviceSetting!);
        }

        // DELETE: api/DeviceSettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceSetting(int id)
        {
            var statusCode = await _settingsService.DeleteDeviceSetting(id);
            switch(statusCode)
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
