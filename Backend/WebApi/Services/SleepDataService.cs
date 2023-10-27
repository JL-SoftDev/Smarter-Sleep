using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
	public class SleepDataService : ISleepDataService
	{
		/// <summary>
		/// Reference to database context
		/// </summary>
		private readonly postgresContext _databaseContext;

		public SleepDataService(postgresContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		//Sleep Review Data Functions
		#region Sleep Review Data

		public Task<IEnumerable<SleepReview>> GetSleepReviews()
		{
			throw new NotImplementedException();
		}

		public Task<SleepReview> GetSleepReview(int id)
		{
			throw new NotImplementedException();
		}

		public Task<int> PutSleepReview(int id, SleepReview sleepReview)
		{
			throw new NotImplementedException();
		}

		public Task<SleepReview> PostSleepReview(SleepReview sleepReview)
		{
			throw new NotImplementedException();
		}

		public Task<int> DeleteSleepReview(int id)
		{
			throw new NotImplementedException();
		}

		private bool SleepReviewExists(int id)
		{
			return (_databaseContext.SleepReviews?.Any(e => e.Id == id)).GetValueOrDefault();
		}
		#endregion

		//Wearable Data Functions
		#region Wearable Data

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

		#endregion

		//Survey Data Functions
		#region Survey Data
		public Task<IEnumerable<Survey>> GetSurveys()
		{
			throw new NotImplementedException();
		}

		public Task<Survey> GetSurvey(int id)
		{
			throw new NotImplementedException();
		}

		public Task<int> PutSurvey(int id, Survey survey)
		{
			throw new NotImplementedException();
		}

		public Task<Survey> PostSurvey(Survey survey)
		{
			throw new NotImplementedException();
		}

		public Task<int> DeleteSurvey(int id)
		{
			throw new NotImplementedException();
		}
		private bool SurveyExists(int id)
		{
			return (_databaseContext.Surveys?.Any(e => e.Id == id)).GetValueOrDefault();
		}


		#endregion
	}
}
