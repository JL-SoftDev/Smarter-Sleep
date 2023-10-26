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
		private readonly postgresContext _context;
		private readonly IWearableDataService _wearableDataService;

        public WearableDatasController(postgresContext context, IWearableDataService wearableDataService)
        {
			_context = context;
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
          if (_context.WearableData == null)
          {
              return NotFound();
          }
            var wearableData = await _context.WearableData.FindAsync(id);

            if (wearableData == null)
            {
                return NotFound();
            }

            return wearableData;
        }

        // PUT: api/WearableDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWearableData(int id, WearableData wearableData)
        {
            if (id != wearableData.Id)
            {
                return BadRequest();
            }

            _context.Entry(wearableData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WearableDataExists(id))
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

        // POST: api/WearableDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WearableData>> PostWearableData(WearableData wearableData)
        {
          if (_context.WearableData == null)
          {
              return Problem("Entity set 'postgresContext.WearableData'  is null.");
          }
            _context.WearableData.Add(wearableData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWearableData", new { id = wearableData.Id }, wearableData);
        }

        // DELETE: api/WearableDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWearableData(int id)
        {
            if (_context.WearableData == null)
            {
                return NotFound();
            }
            var wearableData = await _context.WearableData.FindAsync(id);
            if (wearableData == null)
            {
                return NotFound();
            }

            _context.WearableData.Remove(wearableData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WearableDataExists(int id)
        {
            return (_context.WearableData?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
