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

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> PostGoodWearableData(Guid UserId, [FromBody] DateTime? dateTime)
        {
            var wearableData = await _wearableDataInjectionService.AddGoodWearableData(UserId, dateTime ?? DateTime.Today);
            if (wearableData == null) return BadRequest();
            return Ok(wearableData);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> PostBadWearableData(Guid UserId, [FromBody] DateTime? dateTime)
        {
            var wearableData = await _wearableDataInjectionService.AddBadWearableData(UserId, dateTime ?? DateTime.Today);
            if (wearableData == null) return BadRequest();
            return Ok(wearableData);
        }
    }
}