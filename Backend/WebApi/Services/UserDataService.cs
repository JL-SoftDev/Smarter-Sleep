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
    public class UserDataService : IUserDataService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly postgresContext _databaseContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public UserDataService(postgresContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            if (_databaseContext.AppUsers == null)
            {
                return new List<AppUser>();
            }
            return await _databaseContext.AppUsers.ToListAsync();
        }

        public async Task<AppUser?> GetUser(Guid id)
        {
            var appUser = await _databaseContext.AppUsers.FindAsync(id);
            return appUser;
        }

        public async Task<AppUser?> PostUser(AppUser appUser)
        {
            _databaseContext.AppUsers.Add(appUser);
            await _databaseContext.SaveChangesAsync();
            return appUser;
        }

        public async Task<int> PutUser(Guid id, AppUser appUser)
        {
            if (id != appUser.UserId)
            {
                return 400;
            }

            _databaseContext.Entry(appUser).State = EntityState.Modified;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        public async Task<int> DeleteUser(Guid id)
        {
            var appUser = await _databaseContext.AppUsers.FindAsync(id);
            if (appUser == null)
            {
                return 404;
            }
            _databaseContext.AppUsers.Remove(appUser);
            await _databaseContext.SaveChangesAsync();
            return 204;
        }

        private bool UserExists(Guid id)
        {
            return (_databaseContext.AppUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        public async Task<IEnumerable<CustomSchedule>> GetAllCustomSchedules()
        {
            if (_databaseContext.CustomSchedules == null)
            {
                return new List<CustomSchedule>();
            }
            return await _databaseContext.CustomSchedules.ToListAsync();
        }

        public async Task<CustomSchedule?> GetCustomSchedule(Guid userId, int dayOfWeek)
        {
            var customSchedule = await _databaseContext.CustomSchedules
                .FirstOrDefaultAsync(cs => cs.UserId == userId && cs.DayOfWeek == dayOfWeek);
            return customSchedule;
        }

        public async Task<int> PutCustomSchedule(Guid userId, int dayOfWeek, CustomSchedule updatedSchedule)
        {
            if (userId != updatedSchedule.UserId || dayOfWeek != updatedSchedule.DayOfWeek)
            {
                return 400; 
            }

            if (!CustomScheduleExists(userId, dayOfWeek))
            {
                _databaseContext.CustomSchedules.Add(updatedSchedule);
                await _databaseContext.SaveChangesAsync();
                return 201;
            } else {

                _databaseContext.Entry(updatedSchedule).State = EntityState.Modified;

                try
                {
                    await _databaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return 400;
                }

                return 204;
            }
        }

        public async Task<CustomSchedule?> PostCustomSchedule(CustomSchedule customSchedule)
        {
            _databaseContext.CustomSchedules.Add(customSchedule);
            await _databaseContext.SaveChangesAsync();
            return customSchedule;
        }

        public async Task<int> DeleteCustomSchedule(Guid userId, int dayOfWeek)
        {
            var customSchedule = await _databaseContext.CustomSchedules
                .FirstOrDefaultAsync(cs => cs.UserId == userId && cs.DayOfWeek == dayOfWeek);

            if (customSchedule == null)
            {
                return 404;
            }
            _databaseContext.CustomSchedules.Remove(customSchedule);
            await _databaseContext.SaveChangesAsync();
            return 204;
        }

        private bool CustomScheduleExists(Guid userId, int dayOfWeek)
        {
            return (_databaseContext.CustomSchedules?.Any(e => e.UserId == userId && e.DayOfWeek == dayOfWeek)).GetValueOrDefault();
        }
    }
}