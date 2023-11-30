using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class SettingsService : ISettingsService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly postgresContext _databaseContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public SettingsService(postgresContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<SleepSetting>> GetAllSleepSettings()
        {
            if (_databaseContext.SleepSettings == null)
            {
                return new List<SleepSetting>();
            }
            return await _databaseContext.SleepSettings.Include(s => s.DeviceSettings).ToListAsync();
        }

        public async Task<SleepSetting?> GetSleepSetting(int id)
        {
            var sleepSetting = await _databaseContext.SleepSettings.Include(s => s.DeviceSettings).FirstOrDefaultAsync(s => s.Id == id);
            return sleepSetting;
        }

        public async Task<SleepSetting?> PostSleepSetting(SleepSetting sleepSetting)
        {
            _databaseContext.SleepSettings.Add(sleepSetting);
            await _databaseContext.SaveChangesAsync();
            return sleepSetting;
        }

        public async Task<int> PutSleepSetting(int id, SleepSetting sleepSetting)
        {
            if (id != sleepSetting.Id)
            {
                return 400;
            }

            _databaseContext.Entry(sleepSetting).State = EntityState.Modified;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SleepSettingExists(id))
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

        public async Task<int> DeleteSleepSetting(int id)
        {
            var sleepSetting = await _databaseContext.SleepSettings.FindAsync(id);
            if (sleepSetting == null)
            {
                return 404;
            }
            _databaseContext.SleepSettings.Remove(sleepSetting);
            await _databaseContext.SaveChangesAsync();
            return 204;
        }

        private bool SleepSettingExists(int id)
        {
            return (_databaseContext.SleepSettings?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IEnumerable<DeviceSetting>> GetAllDeviceSettings()
        {
            if (_databaseContext.DeviceSetting == null)
            {
                return new List<DeviceSetting>();
            }
            return await _databaseContext.DeviceSetting.Include(ds => ds.Device).ToListAsync();
        }

        public async Task<DeviceSetting?> GetDeviceSetting(int id)
        {
            var deviceSetting = await _databaseContext.DeviceSetting.Include(ds => ds.Device)
				.FirstOrDefaultAsync(ds => ds.Id == id);
            return deviceSetting;
        }

        public async Task<DeviceSetting> PostDeviceSetting(DeviceSetting deviceSetting)
        {
            _databaseContext.DeviceSetting.Add(deviceSetting);
            await _databaseContext.SaveChangesAsync();
            return deviceSetting;
        }

        public async Task<int> PutDeviceSetting(int id, DeviceSetting deviceSetting)
        {
            if (id != deviceSetting.Id)
            {
                return 400;
            }

            _databaseContext.Entry(deviceSetting).State = EntityState.Modified;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceSettingExists(id))
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

        public async Task<int> DeleteDeviceSetting(int id)
        {
            var deviceSetting = await _databaseContext.DeviceSetting.FindAsync(id);
            if (deviceSetting == null)
            {
                return 404;
            }
            _databaseContext.DeviceSetting.Remove(deviceSetting);
            await _databaseContext.SaveChangesAsync();
            return 204;
        }

        private bool DeviceSettingExists(int id)
        {
            return (_databaseContext.DeviceSetting?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}