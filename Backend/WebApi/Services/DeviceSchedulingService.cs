using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            SleepReview? previousReview1;
            SleepReview? previousReview2;
            if (sleepReviews.Count != 0)
            {
                for (int i = 0; i < sleepReviews.Count; i++)
                {
                    if (sleepReviews[i].UserId != userId)
                    {
                        sleepReviews.RemoveAt(i);
                        i--;
                    }
                    else if (sleepReviews[i].CreatedAt.Date != tomorrow.AddDays(-7).Date && sleepReviews[i].CreatedAt.Date != tomorrow.AddDays(-14).Date)
                    {
                        sleepReviews.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (sleepReviews.Count == 2)
            {
                previousReview1 = sleepReviews[0];
                previousReview2 = sleepReviews[1];
            }
            else if (sleepReviews.Count == 1)
            {
                previousReview1 = sleepReviews[0];
                previousReview2 = null;
            }
            else
            {
                previousReview1 = null;
                previousReview2 = null;
            }
            WearableData? previousWearableData1 = null;
            WearableData? previousWearableData2 = null;
            Survey? previousSurvey1 = null;
            Survey? previousSurvey2 = null;
            if (previousReview1 != null)
            {
                if (previousReview1.WearableLog != null)
                {
                    previousWearableData1 = previousReview1.WearableLog;
                }
                if (previousReview1.Survey != null)
                {
                    previousSurvey1 = previousReview1.Survey;
                }
            }
            if (previousReview2 != null)
            {
                if (previousReview2.WearableLog != null)
                {
                    previousWearableData2 = previousReview2.WearableLog;
                }
                if (previousReview2.Survey != null)
                {
                    previousSurvey2 = previousReview2.Survey;
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
            SleepSetting? previousSettings1;
            SleepSetting? previousSettings2;
            if (sleepSettings.Count != 0)
            {
                for (int i = 0; i < sleepSettings.Count; i++)
                {
                    if (sleepSettings[i].UserId != userId)
                    {
                        sleepSettings.RemoveAt(i);
                        i--;
                    }
                    else if (sleepSettings[i].ScheduledWake.Date != tomorrow.AddDays(-7).Date && sleepSettings[i].ScheduledWake.Date != tomorrow.AddDays(-14).Date)
                    {
                        sleepSettings.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (sleepSettings.Count == 2)
            {
                previousSettings1 = sleepSettings[0];
                previousSettings2 = sleepSettings[1];
            }
            else if (sleepSettings.Count == 1)
            {
                previousSettings1 = sleepSettings [0];
                previousSettings2 = null;
            }
            else
            {
                previousSettings1 = null;
                previousSettings2 = null;
            }
            SleepSetting? useSettings = WeightHypno(previousWearableData1, previousWearableData2, previousSettings1, previousSettings2);
            int?[] surveyWeight = WeightSurvey(previousSurvey1, previousSurvey2, tomorrow.AddDays(-7));
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
            if (surveyWeight[0] == 0)
            {
                wakeTime.AddMinutes(-5);
            }
            else if (surveyWeight[0] == 2)
            {
                wakeTime.AddMinutes(5);
            }
            DateTime sleepTime = wakeTime.AddHours(-8);
            if (surveyWeight[3] == 1)
            {
                sleepTime.AddMinutes(-5);
            }
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
            if (useSettings != null && useSettings.DeviceSettings != null)
            {
                List<DeviceSetting> lastWeekTomorrowDeviceSettings = useSettings.DeviceSettings.ToList();
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
                            listDeviceSettings.Add(SetAlarm(userDevices[i].Id, newSettings));
                            break;
                        case "light":
                            if (lastWeekSettings.Count > 0)
                            {
                                for (int l = 0; l < lastWeekSettings.Count; l++)
                                {
                                    listDeviceSettings.Add(SetLights(surveyWeight[3], lastWeekSettings[l], userDevices[i].Id, newSettings, useSettings));
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
                                    listDeviceSettings.Add(SetThermostat(surveyWeight[1], lastWeekSettings[l], userDevices[i].Id, newSettings, useSettings));
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
                        case "light":
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

        private SleepSetting? WeightHypno(WearableData? data1, WearableData? data2, SleepSetting? setting1, SleepSetting? setting2)
        {
            if (data1 != null)
            {
                if (setting1 != null)
                {
                    if (data1.SleepDate != DateOnly.FromDateTime(setting1.ScheduledSleep))
                    {
                        if (setting2 != null)
                        {
                            SleepSetting temp = setting1;
                            setting1 = setting2;
                            setting2 = temp;
                        }
                    }
                }
            }
            if (data2 != null)
            {
                if (setting2 != null)
                {
                    if (data2.SleepDate != DateOnly.FromDateTime(setting2.ScheduledSleep))
                    {
                        if (setting1 != null)
                        {
                            SleepSetting temp = setting1;
                            setting1 = setting2;
                            setting2 = temp;
                        }
                    }
                }
            }
            if (data1 != null && data2 != null)
            {
                if (data1.SleepScore >= data2.SleepScore)
                {
                    if (setting1 != null)
                    {
                        return setting1;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (setting2 != null)
                    {
                        return setting2;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else if (data1 != null)
            {
                if (setting1 != null)
                {
                    return setting1;
                }
                else
                {
                    return null;
                }
            }
            else if (data2 != null)
            {
                if (setting2 != null)
                {
                    return setting2;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private int?[] WeightSurvey(Survey? survey1, Survey? survey2, DateTime useDate)
        {
            int?[] surveyWeight = { 0, 0, 0, 0};
            if (survey1 != null && survey1.SurveyDate == DateOnly.FromDateTime(useDate))
            {
                surveyWeight[0] = survey1.WakePreference;
                surveyWeight[1] = survey1.TemperaturePreference;
                if (survey1.LightsDisturbance != null)
                {
                    if ((bool)survey1.LightsDisturbance)
                    {
                        surveyWeight[2] = 1;
                    }
                    else
                    {
                        surveyWeight[2] = 0;
                    }
                }
                if (survey1.SleepEarlier != null)
                {
                    if ((bool)survey1.SleepEarlier)
                    {
                        surveyWeight[3] = 1;
                    }
                    else
                    {
                        surveyWeight[3] = 0;
                    }
                }
            }
            else if (survey2 != null && survey2.SurveyDate == DateOnly.FromDateTime(useDate))
            {
                surveyWeight[0] = survey2.WakePreference;
                surveyWeight[1] = survey2.TemperaturePreference;
                if (survey2.LightsDisturbance != null)
                {
                    if ((bool)survey2.LightsDisturbance)
                    {
                        surveyWeight[2] = 1;
                    }
                    else
                    {
                        surveyWeight[2] = 0;
                    }
                }
                if (survey2.SleepEarlier != null)
                {
                    if ((bool)survey2.SleepEarlier)
                    {
                        surveyWeight[3] = 1;
                    }
                    else
                    {
                        surveyWeight[3] = 0;
                    }
                }
            }
            return surveyWeight;
        }

        private DeviceSetting SetLights(int? lightOption, DeviceSetting previousLightSettings, int deviceId, SleepSetting inSettings, SleepSetting oldSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            TimeSpan spanSleepToWake = inSettings.ScheduledWake.Subtract(inSettings.ScheduledSleep);
            TimeSpan spanPreviousToPreviousWake = previousLightSettings.ScheduledTime.Subtract(oldSettings.ScheduledWake);
            if (TimeSpan.Compare(spanSleepToWake, spanPreviousToPreviousWake)  == 1 && TimeSpan.Compare(TimeSpan.Zero, spanPreviousToPreviousWake)  != 0)
            {
                if (lightOption == 1)
                {
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake).AddMinutes(5);
                    if (newDeviceSettings.ScheduledTime > inSettings.ScheduledWake)
                    {
                        newDeviceSettings.ScheduledTime = inSettings.ScheduledWake;
                    }
                }
                else
                {
                    newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake);
                }
            }
            else
            {
                newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake);
            }
            newDeviceSettings.Settings = previousLightSettings.Settings;
            return newDeviceSettings;
        }

        private DeviceSetting SetThermostat(int? tempOption, DeviceSetting previousTempSettings, int deviceId, SleepSetting inSettings, SleepSetting oldSettings)
        {
            DeviceSetting newDeviceSettings = new DeviceSetting();
            newDeviceSettings.DeviceId = deviceId;
            newDeviceSettings.SleepSettingId = inSettings.Id;
            TimeSpan spanPreviousToPreviousWake = previousTempSettings.ScheduledTime.Subtract(oldSettings.ScheduledWake);
            newDeviceSettings.ScheduledTime = inSettings.ScheduledWake.Add(spanPreviousToPreviousWake);
            String previousSet = Regex.Match(previousTempSettings.Settings!, @"\d+").Value;
            int previousTemp = Int32.Parse(previousSet);
            if (tempOption != null)
            {
                if (tempOption == 0)
                {
                    previousTemp += 2;
                }
                else if (tempOption == 2)
                {
                    previousTemp -= 2;
                }
            }
            newDeviceSettings.Settings = "{\"temperature\": " + previousTemp + "}";
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