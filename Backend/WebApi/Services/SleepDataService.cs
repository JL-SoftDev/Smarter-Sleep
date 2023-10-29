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

		public async Task<IEnumerable<SleepReview>> GetSleepReviews()
		{
			return await _databaseContext.SleepReviews
				.Include(sr => sr.Survey)
				.Include(sr => sr.WearableLog)
				.ToListAsync();
		}

		public async Task<SleepReview?> GetSleepReview(int id)
		{
			var sleepReview = await _databaseContext.SleepReviews
				.Include(sr => sr.Survey)
				.Include(sr => sr.WearableLog)
				.FirstOrDefaultAsync(sr => sr.Id == id);
			return sleepReview;
		}

		public async Task<int> PutSleepReview(int id, SleepReview sleepReview)
		{
			if (id != sleepReview.Id)
			{
				return 400;
			}

			_databaseContext.Entry(sleepReview).State = EntityState.Modified;

			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SleepReviewExists(id))
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

		public async Task<SleepReview?> PostSleepReview(SleepReview sleepReview)
		{
			_databaseContext.SleepReviews.Add(sleepReview);
			await _databaseContext.SaveChangesAsync();
			return sleepReview;
		}

		public async Task<int> DeleteSleepReview(int id)
		{
			var sleepReview = await _databaseContext.SleepReviews.FindAsync(id);
			if (sleepReview == null)
			{
				return 404;
			}
			_databaseContext.SleepReviews.Remove(sleepReview);
			await _databaseContext.SaveChangesAsync();
			return 204;
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
		public async Task<IEnumerable<Survey>> GetSurveys()
		{
			return await _databaseContext.Surveys.ToListAsync();
		}

		public async Task<Survey?> GetSurvey(int id)
		{
			var survey = await _databaseContext.Surveys.FindAsync(id);
			return survey;
		}

		public async Task<int> PutSurvey(int id, Survey survey)
		{
			_databaseContext.Entry(survey).State = EntityState.Modified;
			try
			{
				await _databaseContext.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SurveyExists(id))
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

		public async Task<Survey?> PostSurvey(Survey survey)
		{
			_databaseContext.Surveys.Add(survey);
			await _databaseContext.SaveChangesAsync();
			return survey;
		}

		public async Task<int> DeleteSurvey(int id)
		{
			var survey = await _databaseContext.Surveys.FindAsync(id);
			if (survey == null)
			{
				return 404;
			}
			_databaseContext.Surveys.Remove(survey);
			await _databaseContext.SaveChangesAsync();
			return 204;
		}
		private bool SurveyExists(int id)
		{
			return (_databaseContext.Surveys?.Any(e => e.Id == id)).GetValueOrDefault();
		}


		#endregion
	}
}
