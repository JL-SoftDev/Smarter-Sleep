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
    public class DeviceService : IDeviceService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly postgresContext _databaseContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public DeviceService(postgresContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            if (_databaseContext.Devices == null)
            {
                return new List<Device>();
            }
            return await _databaseContext.Devices.ToListAsync();
        }

        public async Task<Device?> GetDevice(int id)
        {
            var device = await _databaseContext.Devices.FindAsync(id);
            return device;
        }

        public async Task<Device> PostDevice(Device device)
        {
            _databaseContext.Devices.Add(device);
            await _databaseContext.SaveChangesAsync();
            return device;
        }

        public async Task<int> PutDevice(int id, Device device)
        {
            if (id != device.Id)
            {
                return 400;
            }

            _databaseContext.Entry(device).State = EntityState.Modified;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        public async Task<int> DeleteDevice(int id)
        {
            var device = await _databaseContext.Devices.FindAsync(id);
            if (device == null)
            {
                return 404;
            }
            _databaseContext.Devices.Remove(device);
            await _databaseContext.SaveChangesAsync();
            return 204;
        }

        private bool DeviceExists(int id)
        {
            return (_databaseContext.Devices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}