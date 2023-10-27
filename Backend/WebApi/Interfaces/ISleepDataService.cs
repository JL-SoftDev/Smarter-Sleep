using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Interfaces
{
	public interface ISleepDataService
	{
		#region Sleep Review Data
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Task<IEnumerable<SleepReview>> GetSleepReviews();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<SleepReview> GetSleepReview(int id);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="sleepReview"></param>
		/// <returns></returns>
		public Task<int> PutSleepReview(int id, SleepReview sleepReview);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sleepReview"></param>
		/// <returns></returns>
		public Task<SleepReview> PostSleepReview(SleepReview sleepReview);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<int> DeleteSleepReview(int id);

		#endregion

		#region Wearable Data
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<WearableData>> GetAllWearableData();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<WearableData?> GetWearableData(int id);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="wearableData"></param>
		/// <returns></returns>
		Task<int> PutWearableData(int id, WearableData wearableData);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="wearableData"></param>
		/// <returns></returns>
		Task<WearableData?> PostWearableData(WearableData wearableData);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<int> DeleteWearableData(int id);
		#endregion

		#region Survey Data
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Task<IEnumerable<Survey>> GetSurveys();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<Survey> GetSurvey(int id);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="survey"></param>
		/// <returns></returns>
		public Task<int> PutSurvey(int id, Survey survey);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="survey"></param>
		/// <returns></returns>
		public Task<Survey> PostSurvey(Survey survey);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Task<int> DeleteSurvey(int id);
		#endregion
	}
}
