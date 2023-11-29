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
				.Include(sr => sr.SleepSetting)
				.ToListAsync();
		}

		public async Task<SleepReview?> GetSleepReview(int id)
		{
			var sleepReview = await _databaseContext.SleepReviews
				.Include(sr => sr.Survey)
				.Include(sr => sr.WearableLog)
				.Include(sr => sr.SleepSetting)
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

		
		public async Task<SleepReview?> GenerateReview(Guid userId, Survey survey, WearableData wearableData, DateTime? appTime = null)
		{
			
			DateTime currentTime = appTime ?? DateTime.Now;

			List<UserChallenge> assignedChallenges = _databaseContext.UserChallenges.Where(e => e.UserId == userId).ToList();
			//Adjust sleep date to actual sleepdate following Oura documentation
			DateOnly adjustedSleepDate = wearableData.SleepDate.AddDays(1);
			
			//Start sleep score at 100
			double sleepScore = 100.0;

			//Multiply by the user reported sleep quality
			if(survey.SleepQuality.HasValue){
				sleepScore *= (double)survey.SleepQuality/10 + .2;
			}

			//If user wants to wake earlier, assign sleep early challenge
			if(survey.WakePreference==0){
				if(assignedChallenges.Count < 3 && !assignedChallenges.Any(uc => uc.ChallengeId == 1)){
					UserChallenge sleepEarly = new UserChallenge{
						UserId = userId,
						ChallengeId = 1,
						StartDate = currentTime,
						ExpireDate = currentTime.AddDays(5),
						UserSelected = true
					};
					assignedChallenges.Add(sleepEarly);
					_databaseContext.UserChallenges.Add(sleepEarly);
				}
			}

			//If user was too hot or cold, -5
			if(survey.TemperaturePreference.HasValue){
				sleepScore += Math.Abs((int)survey.TemperaturePreference-1)*-5;
			}

			//Calculate distance from 8 hours of sleep
			double distanceFromEightHours = Math.Abs(survey.SleepDuration.Value - 480.0);
			//Apply gaussian factor onto sleep score
			sleepScore = sleepScore * (Math.Exp(-Math.Pow(distanceFromEightHours / 240, 2)) + 1/10);
			
			//If over a hour away, assign 8 hour challenge
			if(distanceFromEightHours >= 60){
				if(assignedChallenges.Count < 3 && !assignedChallenges.Any(uc => uc.ChallengeId == 2)){
					UserChallenge eightHours = new UserChallenge{
						UserId = userId,
						ChallengeId = 2,
						StartDate = currentTime,
						ExpireDate = currentTime.AddDays(7),
						UserSelected = false
					};
					assignedChallenges.Add(eightHours);
					_databaseContext.UserChallenges.Add(eightHours);
				}
			}

			//Assign no eating challenge if ate late
			if(survey.AteLate ?? false){
				sleepScore -= 5;
				if(assignedChallenges.Count < 3 && !assignedChallenges.Any(uc => uc.ChallengeId == 3)){
					UserChallenge noEat = new UserChallenge{
						UserId = userId,
						ChallengeId = 3,
						StartDate = currentTime,
						ExpireDate = currentTime.AddDays(5),
						UserSelected = true
					};
					assignedChallenges.Add(noEat);
					_databaseContext.UserChallenges.Add(noEat);
				}
			}
			Console.WriteLine(adjustedSleepDate);
			//Substract one point for every modified schedule, add .1 for every setting applied
			SleepSetting sleepSetting = _databaseContext.SleepSettings.FirstOrDefault(e => e.UserId == userId && DateOnly.FromDateTime(e.ScheduledSleep) == adjustedSleepDate);
			if(sleepSetting != null){
				int modCounter = 0;
				foreach(DeviceSetting deviceSetting in sleepSetting.DeviceSettings){
					if(deviceSetting.UserModified == true){
						sleepScore -= 1.0;
						modCounter++;
					} else {
						sleepScore += 0.1;
					}
				}
				//Assign no modifications if 2 settings were changed
				if(modCounter >= 2 && assignedChallenges.Count < 3 && !assignedChallenges.Any(uc => uc.ChallengeId == 5)){
					UserChallenge noMod = new UserChallenge{
						UserId = userId,
						ChallengeId = 5,
						StartDate = currentTime,
						ExpireDate = currentTime.AddDays(7),
						UserSelected = false
					};
					assignedChallenges.Add(noMod);
					_databaseContext.UserChallenges.Add(noMod);
				}
			}

			//If no challenges were assigned, assign 14 day streak
			if(assignedChallenges.Count == 0){
				UserChallenge streak = new UserChallenge{
					UserId = userId,
					ChallengeId = 4,
					StartDate = currentTime,
					ExpireDate = currentTime.AddDays(14),
					UserSelected = false
				};
				assignedChallenges.Add(streak);
				_databaseContext.UserChallenges.Add(streak);
			}
			
			//Average SmarterSleepScore with wearable sleep score.
			if(wearableData.SleepScore.HasValue){
				sleepScore = (sleepScore*0.7 + (double)wearableData.SleepScore*0.3);
			}

			//Ensure sleep score is within range 0-100
			sleepScore = Math.Min(100,Math.Max(0, sleepScore));
			
			SleepReview review = new SleepReview {
				UserId = userId,
				SleepSettingsId = sleepSetting != null ? sleepSetting.Id : null,
				CreatedAt = currentTime,
				SmarterSleepScore = Convert.ToInt32(sleepScore),
				Survey = survey,
				WearableLog = wearableData
			};
			_databaseContext.SleepReviews.Add(review);
			await _databaseContext.SaveChangesAsync();
			
			return review;
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
