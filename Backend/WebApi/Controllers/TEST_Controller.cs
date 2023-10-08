using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	public class TEST_Controller
	{
		[HttpGet("routetester")]
		public async Task<IActionResult> TestingRoute()
		{
			return new OkObjectResult("HELLO WORLD - CS411W");
		}
		
	}
}
