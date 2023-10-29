using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class DeviceSchedulingService : IDeviceSchedulingService
    {
        private readonly IDeviceService _deviceService;
        private readonly ISettingsService _settingsService;
        private readonly ISleepDataService _sleepDataService;
        private readonly IUserDataService _userDataService;
        public DeviceSchedulingService(IDeviceService deviceService, ISettingsService settingsService, ISleepDataService sleepDataService, IUserDataService userDataService)
        {
            _deviceService = deviceService;
            _settingsService = settingsService;
            _sleepDataService = sleepDataService;
            _userDataService = userDataService;
        }
        public async Task<SleepSetting?> ScheduleTomorrow(Guid userId, DateTime? inDay = null)
        {

            SleepSetting? final = null;
            DateTime scheduleDay;
            if (inDay == null)
            {
                scheduleDay = DateTime.Today;
            }
            else
            {
                scheduleDay = (DateTime)inDay;
            }
            DateTime tomorrow = scheduleDay.AddDays(1);
            AppUser? user = await _userDataService.GetUser(userId);
            if (user == null)
            {
                final = null;
                return final;
            }
            var getUserDevices = await _deviceService.GetAllDevices();
            List<Device> userDevices = getUserDevices.ToList();
            if (userDevices.Count == 0)
            {
                final = null;
                return final;
            }
            else
            {
                for (int i = 0; i < userDevices.Count; i++)
                {
                    if (userDevices[i].UserId != userId)
                    {
                        userDevices.RemoveAt(i);
                        i--;
                    }
                }
            }
            var getSleepReviews = await _sleepDataService.GetSleepReviews();
            List<SleepReview> sleepReviews = getSleepReviews.ToList();
            SleepReview? lastWeekTomorrowReview;
            if (sleepReviews.Count != 0)
            {
                for (int i = 0; i < sleepReviews.Count; i++)
                {
                    if (sleepReviews[i].UserId != userId)
                    {
                        sleepReviews.RemoveAt(i);
                        i--;
                    }
                    else if (sleepReviews[i].CreatedAt.Date != tomorrow.AddDays(-7).Date)
                    {
                        sleepReviews.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (sleepReviews.Count != 0)
            {
                lastWeekTomorrowReview = sleepReviews[0];
            }
            else
            {
                lastWeekTomorrowReview = null;
            }
            WearableData? lastWeekTomorrowData = null;
            Survey? lastWeekTomorrowSurvey = null;
            if (lastWeekTomorrowReview != null)
            {
                if (lastWeekTomorrowReview.WearableLog != null)
                {
                    lastWeekTomorrowData = lastWeekTomorrowReview.WearableLog;
                }
                if (lastWeekTomorrowReview.Survey != null)
                {
                    lastWeekTomorrowSurvey = lastWeekTomorrowReview.Survey;
                }
            }
            var getCustomSchedules = await _userDataService.GetAllCustomSchedules();
            List<CustomSchedule> customSchedules = getCustomSchedules.ToList();
            CustomSchedule? tomorrowSchedule;
            int day = -1;
            switch (tomorrow.DayOfWeek)
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
            if (customSchedules.Count != 0)
            {
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
            }
            if (customSchedules.Count != 0)
            {
                tomorrowSchedule = customSchedules[0];
            }
            else
            {
                tomorrowSchedule = null;
            }
            var getSleepSettings = await _settingsService.GetAllSleepSettings();
            List<SleepSetting> sleepSettings = getSleepSettings.ToList();
            SleepSetting? lastWeekTomorrowSettings;
            if (sleepSettings.Count != 0)
            {
                for (int i = 0; i < sleepSettings.Count; i++)
                {
                    if (sleepSettings[i].UserId != userId)
                    {
                        sleepSettings.RemoveAt(i);
                        i--;
                    }
                    else if (sleepSettings[i].ScheduledWake.Date != tomorrow.AddDays(-7).Date)
                    {
                        sleepSettings.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (sleepSettings.Count != 0)
            {
                lastWeekTomorrowSettings = sleepSettings[0];
            }
            else
            {
                lastWeekTomorrowSettings = null;
            }
            int[] hypnoIntArray;
            int[] wearableWeight;
            if (lastWeekTomorrowData != null)
            {
                if (lastWeekTomorrowData.Hypnogram != null)
                {
                    hypnoIntArray = Array.ConvertAll(lastWeekTomorrowData.Hypnogram.ToCharArray(), c => (int)Char.GetNumericValue(c));
                    wearableWeight = WeightHypno(hypnoIntArray);
                }
                else
                {
                    wearableWeight = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        wearableWeight[i] = 0;
                    }
                }
            }
            else
            {
                wearableWeight = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    wearableWeight[i] = 0;
                }
            }
            int[] surveyWeight;
            if (lastWeekTomorrowSurvey != null)
            {
                surveyWeight = WeightSurvey(lastWeekTomorrowSurvey);
            }
            else
            {
                surveyWeight = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    surveyWeight[i] = 0;
                }
            }
            int[] combinedWeight = CombineWeight(wearableWeight, surveyWeight);
            SleepSetting newSettings = new SleepSetting();
            newSettings.UserId = userId;
            DateTime wakeTime = DateOnly.FromDateTime(scheduleDay).AddDays(1).ToDateTime(new TimeOnly(6, 0));
            if (tomorrowSchedule != null)
            {
                if (tomorrowSchedule.WakeTime != null)
                {
                    wakeTime = DateOnly.FromDateTime(scheduleDay).AddDays(1).ToDateTime((TimeOnly)tomorrowSchedule.WakeTime);
                }
            }
            DateTime sleepTime = wakeTime.AddHours(-8);
            newSettings.ScheduledSleep = sleepTime;
            newSettings.ScheduledWake = wakeTime;
            newSettings.ScheduledHypnogram = "scheduled hypnogram";
            SleepSetting? returnSettings = await _settingsService.PostSleepSetting(newSettings);
            if(returnSettings != null)
            {
                newSettings = returnSettings;
            }
            else
            {
                final = null;
                return final;
            }
            List<DeviceSetting> listDeviceSettings = new List<DeviceSetting>();
            if (lastWeekTomorrowSettings != null && lastWeekTomorrowSettings.DeviceSettings != null)
            {
                List<DeviceSetting> lastWeekTomorrowDeviceSettings = lastWeekTomorrowSettings.DeviceSettings.ToList();
                for (int i = 0; i < userDevices.Count; i++)
                {
                    List<DeviceSetting> lastWeekSettings = new List<DeviceSetting>();
                    for (int j = 0; j < lastWeekTomorrowDeviceSettings.Count; j++)
                    {
                        if (lastWeekTomorrowDeviceSettings[j].DeviceId == userDevices[i].Id)
                        {
                            lastWeekSettings.Add(lastWeekTomorrowDeviceSettings[j]);
                        }
                    }
                    lastWeekSettings.TrimExcess();
                    switch (userDevices[i].Type)
                    {
                        case "alarm":
                            if (lastWeekSettings.Count > 0)
                            {
                                listDeviceSettings.Add(SetAlarm(combinedWeight[2], lastWeekSettings[0], userDevices[i].Id, newSettings, lastWeekTomorrowSettings));
                            }
                            else
                            {
                                listDeviceSettings.Add(SetAlarm(userDevices[i].Id, newSettings));
                            }
                            break;
                        case "lights":
                            if (lastWeekSettings.Count > 0)
                            {
                                for (int l = 0; l < lastWeekSettings.Count; l++)
                                {
                                    listDeviceSettings.Add(SetLights(combinedWeight[0], lastWeekSettings[l], userDevices[i].Id, newSettings, lastWeekTomorrowSettings));
                                }
                            }
                            else
                            {
                                for (int l = 0; l < 4; l++)
                                {
                                    listDeviceSettings.Add(SetLights(l, userDevices[i].Id, newSettings));
                                }
                            }
                            break;
                        case "thermostat":
                            if (lastWeekSettings.Count > 0)
                            {
                                for (int l = 0; l < lastWeekSettings.Count; l++)
                                {
                                    listDeviceSettings.Add(SetThermostat(combinedWeight[1], lastWeekSettings[l], userDevices[i].Id, newSettings, lastWeekTomorrowSettings));
                                }
                            }
                            else
                            {
                                for (int l = 0; l < 4; l++)
                                {
                                    listDeviceSettings.Add(SetThermostat(l, userDevices[i].Id, newSettings));
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < userDevices.Count; i++)
                {
                    switch (userDevices[i].Type)
                    {
                        case "alarm":
                            listDeviceSettings.Add(SetAlarm(userDevices[i].Id, newSettings));
                            break;
                        case "lights":
                            for (int l = 0; l < 4; l++)
                            {
                                listDeviceSettings.Add(SetLights(l, userDevices[i].Id, newSettings));
                            }
                            break;
                        case "thermostat":
                            for (int l = 0; l < 4; l++)
                            {
                                listDeviceSettings.Add(SetThermostat(l, userDevices[i].Id, newSettings));
                            }
                            break;
                    }
                }
            }
            for (int i = 0; i < listDeviceSettings.Count; i++)
            {
                await _settingsService.PostDeviceSetting(listDeviceSettings[i]);
            }
            final = await _settingsService.GetSleepSetting(newSettings.Id);
            return final;
        }

        private int[] WeightHypno(int[] hypnoIntArray)
        {
            //Determine how to weight changes based on hypno
            //light, temp, alarm
            int[] wearableWeight = { 0, 0, 0 };
            return wearableWeight;
        }

        private int[] WeightSurvey(Survey inSurvey)
        {
            //Determine how to weight changes based on survey
            //light, temp, alarm
            int[] surveyWeight = { 0, 0, 0 };
            return surveyWeight;
        }

        private int[] CombineWeight(int[] hypnoIntArray, int[] surveyIntArray)
        {
            //Determine how to combine weights
            //light, temp, alarm
            int[] combinedWeight = { 0, 0, 0 };
            return combinedWeight;
        }

        private DeviceSetting SetAlarm(int alarmOption, DeviceSetting previousAlarmSettings, int deviceId, SleepSetting inSettings, SleepSetting oldSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            newDeviceSettings.ScheduledTime = inSettings.ScheduledWake;
            newDeviceSettings.Settings = previousAlarmSettings.Settings;
            return newDeviceSettings;
        }

        private DeviceSetting SetLights(int lightOption, DeviceSetting previousLightSettings, int deviceId, SleepSetting inSettings, SleepSetting oldSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            TimeSpan spanPreviousToPreviousWake = previousLightSettings.ScheduledTime.Subtract(oldSettings.ScheduledWake);
            switch (lightOption)
            {
                case 0: //same
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake);
                    break;
                case 1: //earlier
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake).AddMinutes(-5);
                    break;
                case 2: //later no later than wake time
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake).AddMinutes(5);
                    if (newDeviceSettings.ScheduledTime > inSettings.ScheduledWake)
                    {
                        newDeviceSettings.ScheduledTime = inSettings.ScheduledWake;
                    }
                    break;
            }
            newDeviceSettings.Settings = previousLightSettings.Settings;
            return newDeviceSettings;
        }

        private DeviceSetting SetThermostat(int tempOption, DeviceSetting previousTempSettings, int deviceId, SleepSetting inSettings, SleepSetting oldSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            TimeSpan spanPreviousToPreviousWake = previousTempSettings.ScheduledTime.Subtract(oldSettings.ScheduledWake);
            switch (tempOption)
            {
                case 0: //same
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake);
                    break;
                case 1: //earlier
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake).AddMinutes(-5);
                    break;
                case 2: //later
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake).AddMinutes(5);
                    break;
            }
            newDeviceSettings.Settings = previousTempSettings.Settings;
            return newDeviceSettings;
        }

        private DeviceSetting SetAlarm(int deviceId, SleepSetting inSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            newDeviceSettings.ScheduledTime = inSettings.ScheduledWake;
            newDeviceSettings.Settings = "{\"alarm\": \"set\"}";
            return newDeviceSettings;
        }

        private DeviceSetting SetLights(int option, int deviceId, SleepSetting inSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            switch (option)
            {
                case 0: //set dim for sleep
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledSleep.AddMinutes(-30);
                    newDeviceSettings.Settings = "{\"lights\": 40}";
                    break;
                case 1: //set off for sleep
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledSleep;
                    newDeviceSettings.Settings = "{\"lights\": 0}";
                    break;
                case 2: //set dim for wake
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.AddMinutes(-5);
                    newDeviceSettings.Settings = "{\"lights\": 20}";
                    break;
                case 3: //set on for wake
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake;
                    newDeviceSettings.Settings = "{\"lights\": 100}";
                    break;
            }
            return newDeviceSettings;
        }

        private DeviceSetting SetThermostat(int option, int deviceId, SleepSetting inSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            switch (option)
            {
                case 0: //set cool for sleep
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledSleep.AddMinutes(-30);
                    newDeviceSettings.Settings = "{\"temperature\": 68}";
                    break;
                case 1: //set sleep temp
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledSleep;
                    newDeviceSettings.Settings = "{\"temperature\": 65}";
                    break;
                case 2: //set warm for wake
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.AddMinutes(-30);
                    newDeviceSettings.Settings = "{\"temperature\": 68}";
                    break;
                case 3: //set wake temp
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake;
                    newDeviceSettings.Settings = "{\"temperature\": 72}";
                    break;
            }
            return newDeviceSettings;
        }
    }
}