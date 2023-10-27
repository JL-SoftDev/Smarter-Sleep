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
    public class WearableDatasController : ControllerBase
    {
		private readonly ISleepDataService _sleepDataService;

        public WearableDatasController(ISleepDataService wearableDataService)
        {
			_sleepDataService = wearableDataService;
        }

        // GET: api/WearableDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WearableData>>> GetWearableData()
        {
            var wearableData = await _sleepDataService.GetAllWearableData();
            return wearableData.ToList();

		}

        // GET: api/WearableDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WearableData>> GetWearableData(int id)
        {
            var wearableData = await _sleepDataService.GetWearableData(id);
            if (wearableData == null) NotFound();
            return wearableData!;
		}

        // PUT: api/WearableDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWearableData(int id, WearableData wearableData)
        {
            var success = await _sleepDataService.PutWearableData(id, wearableData);
            switch(success)
            {
				//Successful:
				case 204:
                    return NoContent();
                //Bad Request
                case 400:
                    return BadRequest();
                //Not Found
                case 404:
                    return NotFound();
                default:
                    return BadRequest();
            }
        }

        // POST: api/WearableDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WearableData>> PostWearableData(WearableData wearableData)
        {
            var newWearableData = await _sleepDataService.PostWearableData(wearableData);
            if(newWearableData == null) return Problem("Entity set 'postgresContext.WearableData'  is null.");
			return CreatedAtAction("GetWearableData", new { id = newWearableData!.Id }, newWearableData!);
		}

        // DELETE: api/WearableDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWearableData(int id)
        {
            var statusCode = await _sleepDataService.DeleteWearableData(id);
            switch (statusCode)
            {
                //Successful:
                case 204:
                    return NoContent();
                //Bad Request
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
