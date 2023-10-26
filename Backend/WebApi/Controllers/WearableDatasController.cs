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
		/*
        private readonly postgresContext _context;

        public WearableDatasController(postgresContext context)
        {
            _context = context;
        }
        */
		//private readonly postgresContext _context;

		private readonly IWearableDataService _wearableDataService;

        public WearableDatasController(IWearableDataService wearableDataService)
        {
			_wearableDataService = wearableDataService;
        }

        // GET: api/WearableDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WearableData>>> GetWearableData()
        {
            var wearableData = await _wearableDataService.GetAllWearableData();
            return wearableData.ToList();

		}

        // GET: api/WearableDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WearableData>> GetWearableData(int id)
        {
            var wearableData = await _wearableDataService.GetWearableData(id);
            if (wearableData == null) NotFound();
            return wearableData!;
		}

        // PUT: api/WearableDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWearableData(int id, WearableData wearableData)
        {
            var success = await _wearableDataService.PutWearableData(id, wearableData);
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
            var newWearableData = await _wearableDataService.PostWearableData(wearableData);
            if(newWearableData == null) return Problem("Entity set 'postgresContext.WearableData'  is null.");
			return CreatedAtAction("GetWearableData", new { id = newWearableData!.Id }, newWearableData!);
		}

        // DELETE: api/WearableDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWearableData(int id)
        {
            var statusCode = await _wearableDataService.DeleteWearableData(id);
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
