using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ISettingsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SleepSetting>> GetAllSleepSettings();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SleepSetting?> GetSleepSetting(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sleepSetting"></param>
        /// <returns></returns>
        Task<int> PutSleepSetting(int id, SleepSetting sleepSetting);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sleepSetting"></param>
        /// <returns></returns>
        Task<SleepSetting?> PostSleepSetting(SleepSetting sleepSetting);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSleepSetting(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DeviceSetting>> GetAllDeviceSettings();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceSetting?> GetDeviceSetting(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceSetting"></param>
        /// <returns></returns>
        Task<int> PutDeviceSetting(int id, DeviceSetting deviceSetting);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceSetting"></param>
        /// <returns></returns>
        Task<DeviceSetting> PostDeviceSetting(DeviceSetting deviceSetting);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteDeviceSetting(int id);
    }
}