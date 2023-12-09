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
    public class CustomSchedulesController : ControllerBase
    {
        private readonly IUserDataService _userDataService;

        public CustomSchedulesController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        // GET: api/CustomSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomSchedule>>> GetCustomSchedules()
        {
            var customSchedules = await _userDataService.GetAllCustomSchedules();
            return customSchedules.ToList();
        }

        // GET: api/CustomSchedules/.../1
        [HttpGet("{userId}/{dayOfWeek}")]
        public async Task<ActionResult<CustomSchedule>> GetCustomSchedule(Guid userId, int dayOfWeek)
        {
            var customSchedule = await _userDataService.GetCustomSchedule(userId, dayOfWeek);
            if (customSchedule == null) NotFound();
            return customSchedule!;
        }

        // PUT: api/CustomSchedules/5/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}/{dayOfWeek}")]
        public async Task<IActionResult> PutCustomSchedule(Guid userId, int dayOfWeek, CustomSchedule updatedSchedule)
        {
            var success = await _userDataService.PutCustomSchedule(userId, dayOfWeek, updatedSchedule);
            switch(success)
            {
                case 201:
                    return CreatedAtAction("GetCustomSchedule", new { userId, dayOfWeek }, updatedSchedule);
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

        // POST: api/CustomSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomSchedule>> PostCustomSchedule(CustomSchedule customSchedule)
        {
            var newCustomSchedule = await _userDataService.PostCustomSchedule(customSchedule);
            if(newCustomSchedule == null) return Problem("Entity set 'postgresContext.CustomSchedule' is null.");
            return CreatedAtAction("GetCustomSchedule", new { id = customSchedule.UserId }, customSchedule);
        }

        // DELETE: api/CustomSchedules/5
        [HttpDelete("{userId}/{dayOfWeek}")]
        public async Task<IActionResult> DeleteCustomSchedule(Guid userId, int dayOfWeek)
        {
            var statusCode = await _userDataService.DeleteCustomSchedule(userId, dayOfWeek);
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
