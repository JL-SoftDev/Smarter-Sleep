using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
	public class WearableDataService : IWearableDataService
	{
		/// <summary>
		/// Reference to database context
		/// </summary>
		private readonly postgresContext _databaseContext;

		public WearableDataService(postgresContext databaseContext)
		{
			_databaseContext = databaseContext;
		}
		public async Task<IEnumerable<WearableData>> GetAllWearableData()
		{
			if (_databaseContext.WearableData == null)
			{
				return new List<WearableData>();
			}
			return await _databaseContext.WearableData.ToListAsync();
		}

		public async Task<WearableData> GetWearableData(int id)
		{
			throw new NotImplementedException();
		}
		public async Task<IActionResult> PutWearableData(int id, WearableData wearableData)
		{
			throw new NotImplementedException();
		}
		public async Task<ActionResult<WearableData>> PostWearableData(WearableData wearableData)
		{
			throw new NotImplementedException();
		}
		public async Task<IActionResult> DeleteWearableData(int id)
		{
			throw new NotImplementedException();
		}

	}
}
