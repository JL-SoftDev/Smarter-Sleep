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
    public class DeviceController : ControllerBase
    {
		private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
			_deviceService = deviceService;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            var devices = await _deviceService.GetAllDevices();
            return devices.ToList();

		}

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _deviceService.GetDevice(id);
            if (device == null) NotFound();
            return device!;
		}

        // PUT: api/Devices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, Device device)
        {
            var success = await _deviceService.PutDevice(id, device);
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

        // POST: api/Devices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            var deviceData = await _deviceService.PostDevice(device);
            if(deviceData == null) return Problem("Entity set 'postgresContext.Devices'  is null.");
			return CreatedAtAction("GetDevice", new { id = deviceData!.Id }, deviceData!);
		}

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var statusCode = await _deviceService.DeleteDevice(id);
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
