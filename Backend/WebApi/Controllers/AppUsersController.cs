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
    public class AppUsersController : ControllerBase
    {
		private readonly IUserDataService _userDataService;

        public AppUsersController(IUserDataService appUserService)
        {
			_userDataService = appUserService;
        }

        // GET: api/AppUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAppUsers()
        {
            var appUsers = await _userDataService.GetAllUsers();
            return appUsers.ToList();

		}

        // GET: api/AppUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetAppUser(Guid id)
        {
            var appUser = await _userDataService.GetUser(id);
            if (appUser == null) NotFound();
            return appUser!;
		}

        // PUT: api/AppUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppUser(Guid id, AppUser appUser)
        {
            var success = await _userDataService.PutUser(id, appUser);
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

        // POST: api/AppUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostAppUser(AppUser appUser)
        {
            var userData = await _userDataService.PostUser(appUser);
            if(userData == null) return Problem("Entity set 'postgresContext.WearableData'  is null.");
			return CreatedAtAction("GetUser", new { id = userData!.UserId }, userData!);
		}

        // DELETE: api/AppUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppUser(Guid id)
        {
            var statusCode = await _userDataService.DeleteUser(id);
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
