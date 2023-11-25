using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IWearableDataInjectionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="inDay"></param>
        /// <returns></returns>
        Task<WearableData?> GetGoodWearableData(Guid userId, DateTime? inDay = null);
        Task<WearableData?> GetBadWearableData(Guid userId, DateTime? inDay = null);
    }
}