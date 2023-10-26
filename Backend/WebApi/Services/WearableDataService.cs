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

		public async Task<WearableData?> GetWearableData(int id)
		{
			var wearableData = await _databaseContext.WearableData.FindAsync(id);
			return wearableData;
		}

		public async Task<int> PutWearableData(int id, WearableData wearableData)
		{
			if (id != wearableData.Id)
			{
				return 400;
			}

			_databaseContext.Entry(wearableData).State = EntityState.Modified;

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!WearableDataExists(id))
				{
					return 404;
				}
				else
				{
					throw;
				}
			}
			return 204;
		}
		public async Task<WearableData?> PostWearableData(WearableData wearableData)
		{
			_databaseContext.WearableData.Add(wearableData);
			await _databaseContext.SaveChangesAsync();
			return wearableData;
		}
		public async Task<int> DeleteWearableData(int id)
		{
			var wearableData = await _databaseContext.WearableData.FindAsync(id);
			if (wearableData == null)
			{
				return 404;
			}
			_databaseContext.WearableData.Remove(wearableData);
			await _databaseContext.SaveChangesAsync();
			return 204;
		}

		private bool WearableDataExists(int id)
		{
			return (_databaseContext.WearableData?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
