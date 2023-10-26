using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Interfaces
{
	public interface IWearableDataService
	{
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
	}
}
