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
    //Controller disabled for prototype
    //[ApiController]
    public class PurchaseLogsController : ControllerBase
    {
        private readonly postgresContext _context;

        public PurchaseLogsController(postgresContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseLog>>> GetPurchaseLogs()
        {
          if (_context.PurchaseLogs == null)
          {
              return NotFound();
          }
            return await _context.PurchaseLogs
                .Include(pl => pl.Transaction)
                .ToListAsync();
        }

        // GET: api/PurchaseLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseLog>> GetPurchaseLog(int id)
        {
          if (_context.PurchaseLogs == null)
          {
              return NotFound();
          }
            var purchaseLog = await _context.PurchaseLogs
                .Include(pl => pl.Transaction)
                .FirstOrDefaultAsync(pl => pl.Id == id);

            if (purchaseLog == null)
            {
                return NotFound();
            }

            return purchaseLog;
        }

        // PUT: api/PurchaseLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseLog(int id, PurchaseLog purchaseLog)
        {
            if (id != purchaseLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseLogExists(id))
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

        // POST: api/PurchaseLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PurchaseLog>> PostPurchaseLog(PurchaseLog purchaseLog)
        {
          if (_context.PurchaseLogs == null)
          {
              return Problem("Entity set 'postgresContext.PurchaseLogs'  is null.");
          }
            _context.PurchaseLogs.Add(purchaseLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseLog", new { id = purchaseLog.Id }, purchaseLog);
        }

        // DELETE: api/PurchaseLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseLog(int id)
        {
            if (_context.PurchaseLogs == null)
            {
                return NotFound();
            }
            var purchaseLog = await _context.PurchaseLogs.FindAsync(id);
            if (purchaseLog == null)
            {
                return NotFound();
            }

            _context.PurchaseLogs.Remove(purchaseLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseLogExists(int id)
        {
            return (_context.PurchaseLogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
