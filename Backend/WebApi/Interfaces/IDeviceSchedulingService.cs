using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IDeviceSchedulingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="inDay"></param>
        /// <returns></returns>
        Task<SleepSetting?> ScheduleTomorrow(Guid userId, DateTime? inDay = null);
    }
}