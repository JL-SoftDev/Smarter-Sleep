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
    public class WearableDataInjectionController : ControllerBase
    {
        private readonly IWearableDataInjectionService _wearableDataInjectionService;
        public WearableDataInjectionController(IWearableDataInjectionService wearableDataInjectionService)
        {
            _wearableDataInjectionService = wearableDataInjectionService;
        }

        [Route("better")]
        [HttpGet]
        public async Task<IActionResult> GetGoodWearableData(Guid UserId, DateTime? dateTime)
        {
            var wearableData = await _wearableDataInjectionService.GetGoodWearableData(UserId, dateTime ?? DateTime.Today);
            if (wearableData == null) return BadRequest();
            return Ok(wearableData);
        }

        [Route("worse")]
        [HttpGet]
        public async Task<IActionResult> GetBadWearableData(Guid UserId, DateTime? dateTime)
        {
            var wearableData = await _wearableDataInjectionService.GetBadWearableData(UserId, dateTime ?? DateTime.Today);
            if (wearableData == null) return BadRequest();
            return Ok(wearableData);
        }
    }
}