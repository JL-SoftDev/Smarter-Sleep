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

        public async Task<AppUser> PostUser(AppUser appUser)
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
    }
}