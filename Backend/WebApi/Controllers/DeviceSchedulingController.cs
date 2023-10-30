using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceSchedulingController : ControllerBase
    {
        private readonly IDeviceSchedulingService _deviceSchedulingService;

        public DeviceSchedulingController(IDeviceSchedulingService deviceSchedulingService)
        {
            _deviceSchedulingService = deviceSchedulingService;
        }

        [HttpPost]
        public async Task<IActionResult> PostDeviceSchedule(Guid UserId, [FromBody] DateTime? dateTime)
        {
            var schedule = await _deviceSchedulingService.ScheduleTomorrow(UserId, dateTime ?? DateTime.Today);
            if(schedule == null) return BadRequest();
            return Ok(schedule);
        }
    }
}