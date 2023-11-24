using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class WearableDataInjectionService : IWearableDataInjectionService
    {
        private readonly ISleepDataService _sleepDataService;
        private readonly ISettingsService _settingsService;
        private readonly IUserDataService _userDataService;
        private WearableDataList dataSet = new WearableDataList();
        public WearableDataInjectionService(ISleepDataService sleepDataService, ISettingsService settingsService, IUserDataService userDataService)
        {
            _sleepDataService = sleepDataService;
            _settingsService = settingsService;
            _userDataService = userDataService;
        }
        public async Task<WearableData?> GetGoodWearableData(Guid userId, DateTime? inDay = null)
        {
            DateTime today = DateTime.Today;
            if (inDay != null)
            {
                today = (DateTime)inDay;
            }
            int day = -1;
            switch (today.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day = 0;
                    break;
                case DayOfWeek.Monday:
                    day = 1;
                    break;
                case DayOfWeek.Tuesday:
                    day = 2;
                    break;
                case DayOfWeek.Wednesday:
                    day = 3;
                    break;
                case DayOfWeek.Thursday:
                    day = 4;
                    break;
                case DayOfWeek.Friday:
                    day = 5;
                    break;
                case DayOfWeek.Saturday:
                    day = 6;
                    break;
            }
            WearableData? newWearableData = new WearableData();
            newWearableData.SleepDate = DateOnly.FromDateTime(today.AddDays(-1));
            var getSleepSettings = await _settingsService.GetAllSleepSettings();
            List<SleepSetting> sleepSettings = getSleepSettings.ToList();
            var getSleepReviews = await _sleepDataService.GetSleepReviews();
            List<SleepReview> sleepReviews = getSleepReviews.ToList();
            var getCustomSchedules = await _userDataService.GetAllCustomSchedules();
            List<CustomSchedule> customSchedules = getCustomSchedules.ToList();
            for (int i = 0; i < customSchedules.Count; i++)
            {
                if (customSchedules[i].UserId != userId)
                {
                    customSchedules.RemoveAt(i);
                    i--;
                }
                else if (customSchedules[i].DayOfWeek != day)
                {
                    customSchedules.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < sleepSettings.Count; i++)
            {
                if (sleepSettings[i].UserId != userId)
                {
                    sleepSettings.RemoveAt(i);
                    i--;
                }
                else if (DateOnly.FromDateTime(sleepSettings[i].ScheduledWake) != DateOnly.FromDateTime(today))
                {
                    sleepSettings.RemoveAt(i);
                    i--;
                }
            }
            if (sleepSettings.Count == 1)
            {
                newWearableData.SleepDate = DateOnly.FromDateTime(sleepSettings[0].ScheduledSleep);
                newWearableData.SleepStart = sleepSettings[0].ScheduledSleep;
                newWearableData.SleepEnd = sleepSettings[0].ScheduledWake;
            }
            else if (customSchedules.Count == 1)
            {
                if (customSchedules[0].WakeTime != null)
                {
                    newWearableData.SleepEnd = DateOnly.FromDateTime(today).ToDateTime((TimeOnly)customSchedules[0].WakeTime!);
                    newWearableData.SleepStart = DateOnly.FromDateTime(today).ToDateTime((TimeOnly)customSchedules[0].WakeTime!).AddHours(-8);
                }
            }
            else
            {
                newWearableData.SleepEnd = DateOnly.FromDateTime(today).ToDateTime(new TimeOnly(6, 0));
                newWearableData.SleepStart = DateOnly.FromDateTime(today).ToDateTime(new TimeOnly(6, 0)).AddHours(-8);
            }
            for (int i = 0; i < sleepReviews.Count; i++)
            {
                if (sleepReviews[i].UserId != userId)
                {
                    sleepReviews.RemoveAt(i);
                    i--;
                }
                else if (DateOnly.FromDateTime(sleepReviews[i].CreatedAt) != DateOnly.FromDateTime(today.AddDays(-7)))
                {
                    sleepReviews.RemoveAt(i);
                    i--;
                }
            }
            if(sleepReviews.Count == 1)
            {
                if(sleepReviews[0].WearableLog != null)
                {
                    int index = dataSet.hypnograms.IndexOf(sleepReviews[0].WearableLog!.Hypnogram!);
                    if(index == -1)
                    {
                        index = 1;
                    }
                    if(index < 4)
                    {
                        index ++;
                    } 
                    newWearableData.Hypnogram = dataSet.hypnograms[index];
                    newWearableData.SleepScore = dataSet.scores[index];
                }
            }
            else
            {
                newWearableData.Hypnogram = dataSet.hypnograms[2];
                newWearableData.SleepScore = dataSet.scores[2];
            }
            return newWearableData;
        }

        
        public async Task<WearableData?> GetBadWearableData(Guid userId, DateTime? inDay = null)
        {
            DateTime today = DateTime.Today;
            if (inDay != null)
            {
                today = (DateTime)inDay;
            }
            int day = -1;
            switch (today.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day = 0;
                    break;
                case DayOfWeek.Monday:
                    day = 1;
                    break;
                case DayOfWeek.Tuesday:
                    day = 2;
                    break;
                case DayOfWeek.Wednesday:
                    day = 3;
                    break;
                case DayOfWeek.Thursday:
                    day = 4;
                    break;
                case DayOfWeek.Friday:
                    day = 5;
                    break;
                case DayOfWeek.Saturday:
                    day = 6;
                    break;
            }
            WearableData? newWearableData = new WearableData();
            newWearableData.SleepDate = DateOnly.FromDateTime(today.AddDays(-1));
            var getSleepSettings = await _settingsService.GetAllSleepSettings();
            List<SleepSetting> sleepSettings = getSleepSettings.ToList();
            var getSleepReviews = await _sleepDataService.GetSleepReviews();
            List<SleepReview> sleepReviews = getSleepReviews.ToList();
            var getCustomSchedules = await _userDataService.GetAllCustomSchedules();
            List<CustomSchedule> customSchedules = getCustomSchedules.ToList();
            for (int i = 0; i < customSchedules.Count; i++)
            {
                if (customSchedules[i].UserId != userId)
                {
                    customSchedules.RemoveAt(i);
                    i--;
                }
                else if (customSchedules[i].DayOfWeek != day)
                {
                    customSchedules.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < sleepSettings.Count; i++)
            {
                if (sleepSettings[i].UserId != userId)
                {
                    sleepSettings.RemoveAt(i);
                    i--;
                }
                else if (DateOnly.FromDateTime(sleepSettings[i].ScheduledWake) != DateOnly.FromDateTime(today))
                {
                    sleepSettings.RemoveAt(i);
                    i--;
                }
            }
            if (sleepSettings.Count == 1)
            {
                newWearableData.SleepDate = DateOnly.FromDateTime(sleepSettings[0].ScheduledSleep);
                newWearableData.SleepStart = sleepSettings[0].ScheduledSleep;
                newWearableData.SleepEnd = sleepSettings[0].ScheduledWake;
            }
            else if (customSchedules.Count == 1)
            {
                if (customSchedules[0].WakeTime != null)
                {
                    newWearableData.SleepEnd = DateOnly.FromDateTime(today).ToDateTime((TimeOnly)customSchedules[0].WakeTime!);
                    newWearableData.SleepStart = DateOnly.FromDateTime(today).ToDateTime((TimeOnly)customSchedules[0].WakeTime!).AddHours(-8);
                }
            }
            else
            {
                newWearableData.SleepEnd = DateOnly.FromDateTime(today).ToDateTime(new TimeOnly(6, 0));
                newWearableData.SleepStart = DateOnly.FromDateTime(today).ToDateTime(new TimeOnly(6, 0)).AddHours(-8);
            }
            for (int i = 0; i < sleepReviews.Count; i++)
            {
                if (sleepReviews[i].UserId != userId)
                {
                    sleepReviews.RemoveAt(i);
                    i--;
                }
                else if (DateOnly.FromDateTime(sleepReviews[i].CreatedAt) != DateOnly.FromDateTime(today.AddDays(-7)))
                {
                    sleepReviews.RemoveAt(i);
                    i--;
                }
            }
            if(sleepReviews.Count == 1)
            {
                if(sleepReviews[0].WearableLog != null)
                {
                    int index = dataSet.hypnograms.IndexOf(sleepReviews[0].WearableLog!.Hypnogram!);
                    if(index == -1)
                    {
                        index = 3;
                    }
                    if(index > 0)
                    {
                        index --;
                    } 
                    newWearableData.Hypnogram = dataSet.hypnograms[index];
                    newWearableData.SleepScore = dataSet.scores[index];
                }
            }
            else
            {
                newWearableData.Hypnogram = dataSet.hypnograms[2];
                newWearableData.SleepScore = dataSet.scores[2];
            }
            return newWearableData;
        }
    }
}