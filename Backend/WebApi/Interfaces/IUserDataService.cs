using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IUserDataService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppUser>> GetAllUsers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppUser?> GetUser(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appUser"></param>
        /// <returns></returns>
        Task<int> PutUser(Guid id, AppUser appUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        Task<AppUser?> PostUser(AppUser appUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteUser(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CustomSchedule>> GetAllCustomSchedules();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomSchedule?> GetCustomSchedule(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customSchedule"></param>
        /// <returns></returns>
        Task<int> PutCustomSchedule(Guid id, CustomSchedule customSchedule);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customSchedule"></param>
        /// <returns></returns>
        Task<CustomSchedule?> PostCustomSchedule(CustomSchedule customSchedule);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteCustomSchedule(Guid id);
    }
}