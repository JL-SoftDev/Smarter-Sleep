using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceScheduler.ModelObjects;

namespace DeviceScheduler
{
    public class DeviceScheduler
    {
        public static async Task<SleepSettings> ScheduleTomorrow(Guid userId, DateTime? inDay = null)
        {
            SleepSettings final = null;
            DateTime scheduleDay;
            if (inDay == null)
            {
                scheduleDay = DateTime.Today;
            }
            else
            {
                scheduleDay = (DateTime)inDay;
            }
            await DeviceSchedulerAPIClient.RunAsync();
            AppUser user = await DeviceSchedulerAPIClient.GetAppUserAsync(userId);
            List<Device> userDevices = await DeviceSchedulerAPIClient.GetDevicesAsync(userId);
            SleepReview lastWeekTomorrowReview = await DeviceSchedulerAPIClient.GetLastWeekTomorrowSleepReviewAsync(userId);
            WearableData? lastWeekTomorrowData = null;
            Survey lastWeekTomorrowSurvey = null;
            if (lastWeekTomorrowReview != null)
            {
                if (lastWeekTomorrowReview.wearable_log_id != null)
                {
                    lastWeekTomorrowData = await DeviceSchedulerAPIClient.GetLastWeekTomorrowWearableDataAsync((int)lastWeekTomorrowReview.wearable_log_id);
                }
                if (lastWeekTomorrowReview.survey_id != null)
                {
                    lastWeekTomorrowSurvey = await DeviceSchedulerAPIClient.GetLastWeekTomorrowSurveyAsync((int)lastWeekTomorrowReview.survey_id);
                }
            }
            CustomSchedule tomorrowSchedule = await DeviceSchedulerAPIClient.GetTomorrowScheduleAsync(userId);
            SleepSettings lastWeekTomorrowSettings = await DeviceSchedulerAPIClient.GetLastWeekTomorrowSettingsAsync(userId);
            int[] hypnoIntArray = null;
            int[] wearableWeight = null;
            if (lastWeekTomorrowData != null)
            {
                if (lastWeekTomorrowData.hypnogram != null)
                {
                    hypnoIntArray = Array.ConvertAll(lastWeekTomorrowData.hypnogram.ToCharArray(), c => (int)Char.GetNumericValue(c));
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
            int[] surveyWeight = null;
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
            SleepSettingsDefaultID newSettings = new SleepSettingsDefaultID();
            newSettings.user_id = userId;
            DateTime wakeTime;
            if (tomorrowSchedule.wake_time != null)
            {
                wakeTime = DateOnly.FromDateTime(scheduleDay).AddDays(1).ToDateTime((TimeOnly)tomorrowSchedule.wake_time);
            }
            else
            {
                wakeTime = DateOnly.FromDateTime(scheduleDay).AddDays(1).ToDateTime(new TimeOnly(6, 0));
            }
            DateTime sleepTime = wakeTime.AddHours(-8);
            newSettings.scheduled_sleep = sleepTime;
            newSettings.scheduled_wake = wakeTime;
            newSettings.scheduled_hypnogram = "scheduled hypnogram";
            SleepSettings returnSettings = await DeviceSchedulerAPIClient.CreateSleepSettingsAsync(newSettings);
            List<DeviceSettingsDefaultID> listDeviceSettings = new List<DeviceSettingsDefaultID>();
            if (lastWeekTomorrowSettings != null && lastWeekTomorrowSettings.deviceSettings != null)
            {
                for (int i = 0; i < userDevices.Count; i++)
                {
                    List<DeviceSettings> lastWeekSettings = new List<DeviceSettings>();
                    for (int j = 0; j < lastWeekTomorrowSettings.deviceSettings.Count; j++)
                    {
                        if (lastWeekTomorrowSettings.deviceSettings[j].device_id == userDevices[i].id)
                        {
                            lastWeekSettings.Add(lastWeekTomorrowSettings.deviceSettings[j]);
                        }
                    }
                    lastWeekSettings.TrimExcess();
                    switch (userDevices[i].type)
                    {
                        case "alarm":
                            if (lastWeekSettings.Count > 0)
                            {
                                listDeviceSettings.Add(SetAlarm(combinedWeight[2], lastWeekSettings[0], i, returnSettings));
                            }
                            else
                            {
                                listDeviceSettings.Add(SetAlarm(i, returnSettings));
                            }
                            break;
                        case "lights":
                            if (lastWeekSettings.Count > 0)
                            {
                                for (int l = 0; l < lastWeekSettings.Count; l++)
                                {
                                    listDeviceSettings.Add(SetLights(combinedWeight[0], lastWeekSettings[l], i, returnSettings));
                                }
                            }
                            else
                            {
                                for (int l = 0; l < 4; l++)
                                {
                                    listDeviceSettings.Add(SetLights(l, i, returnSettings));
                                }
                            }
                            break;
                        case "thermostat":
                            if (lastWeekSettings.Count > 0)
                            {
                                for (int l = 0; l < lastWeekSettings.Count; l++)
                                {
                                    listDeviceSettings.Add(SetThermostat(combinedWeight[1], lastWeekSettings[l], i, returnSettings));
                                }
                            }
                            else
                            {
                                for (int l = 0; l < 4; l++)
                                {
                                    listDeviceSettings.Add(SetThermostat(l, i, returnSettings));
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
                    switch (userDevices[i].type)
                    {
                        case "alarm":
                            listDeviceSettings.Add(SetAlarm(i, returnSettings));
                            break;
                        case "lights":
                            for (int l = 0; l < 4; l++)
                            {
                                listDeviceSettings.Add(SetLights(l, i, returnSettings));
                            }
                            break;
                        case "thermostat":
                            for (int l = 0; l < 4; l++)
                            {
                                listDeviceSettings.Add(SetThermostat(l, i, returnSettings));
                            }
                            break;
                    }
                }
            }
            for(int i = 0; i<listDeviceSettings.Count; i++)
            {
                await DeviceSchedulerAPIClient.CreateDeviceSettingsAsync(listDeviceSettings[i]);
            }
            final = await DeviceSchedulerAPIClient.GetSleepSettingsAsync(returnSettings.id);
            return final;
        }

        private static int[] WeightHypno(int[] hypnoIntArray)
        {
            //Determine how to weight changes based on hypno
            //light, temp, alarm
            int[] wearableWeight = { 0, 0, 0 };
            return wearableWeight;
        }

        private static int[] WeightSurvey(Survey inSurvey)
        {
            //Determine how to weight changes based on survey
            //light, temp, alarm
            int[] surveyWeight = { 0, 0, 0 };
            return surveyWeight;
        }

        private static int[] CombineWeight(int[] hypnoIntArray, int[] surveyIntArray)
        {
            //Determine how to combine weights
            //light, temp, alarm
            int[] combinedWeight = { 0, 0, 0 };
            return combinedWeight;
        }

        private static DeviceSettingsDefaultID SetAlarm(int alarmOption, DeviceSettings previousAlarmSettings, int deviceId, SleepSettings inSettings)
        {
            DeviceSettingsDefaultID newDeviceSettings = new DeviceSettingsDefaultID();
            newDeviceSettings.device_id = deviceId;
            newDeviceSettings.sleep_settings_id = inSettings.id;
            newDeviceSettings.scheduled_time = inSettings.scheduled_wake;
            newDeviceSettings.settings = previousAlarmSettings.settings;
            return newDeviceSettings;
        }

        private static DeviceSettingsDefaultID SetLights(int lightOption, DeviceSettings previousLightSettings, int deviceId, SleepSettings inSettings)
        {
            DeviceSettingsDefaultID newDeviceSettings = new DeviceSettingsDefaultID();
            newDeviceSettings.device_id = deviceId;
            newDeviceSettings.sleep_settings_id = inSettings.id;
            switch (lightOption)
            {
                case 0: //same
                    newDeviceSettings.scheduled_time = previousLightSettings.scheduled_time.AddDays(7);
                    break;
                case 1: //earlier
                    newDeviceSettings.scheduled_time = previousLightSettings.scheduled_time.AddDays(7).AddMinutes(-5);
                    break;
                case 2: //later no later than wake time
                    newDeviceSettings.scheduled_time = previousLightSettings.scheduled_time.AddDays(7).AddMinutes(5);
                    if(newDeviceSettings.scheduled_time > inSettings.scheduled_wake)
                    {
                        newDeviceSettings.scheduled_time = inSettings.scheduled_wake;
                    }
                    break;
            }
            newDeviceSettings.settings = previousLightSettings.settings;
            return newDeviceSettings;
        }

        private static DeviceSettingsDefaultID SetThermostat(int tempOption, DeviceSettings previousTempSettings, int deviceId, SleepSettings inSettings)
        {
            DeviceSettingsDefaultID newDeviceSettings = new DeviceSettingsDefaultID();
            newDeviceSettings.device_id = deviceId;
            newDeviceSettings.sleep_settings_id = inSettings.id;
            switch (tempOption)
            {
                case 0: //same
                    newDeviceSettings.scheduled_time = previousTempSettings.scheduled_time.AddDays(7);
                    break;
                case 1: //earlier
                    newDeviceSettings.scheduled_time = previousTempSettings.scheduled_time.AddDays(7).AddMinutes(-5);
                    break;
                case 2: //later
                    newDeviceSettings.scheduled_time = previousTempSettings.scheduled_time.AddDays(7).AddMinutes(5);
                    break;
            }
            newDeviceSettings.settings = previousTempSettings.settings;
            return newDeviceSettings;
        }

        private static DeviceSettingsDefaultID SetAlarm(int deviceId, SleepSettings inSettings)
        {
            DeviceSettingsDefaultID newDeviceSettings = new DeviceSettingsDefaultID();
            newDeviceSettings.device_id = deviceId;
            newDeviceSettings.sleep_settings_id = inSettings.id;
            newDeviceSettings.scheduled_time = inSettings.scheduled_wake;
            newDeviceSettings.settings = "{\\\"alarm\\\": set}";
            return newDeviceSettings;
        }

        private static DeviceSettingsDefaultID SetLights(int option, int deviceId, SleepSettings inSettings)
        {
            DeviceSettingsDefaultID newDeviceSettings = new DeviceSettingsDefaultID();
            newDeviceSettings.device_id = deviceId;
            newDeviceSettings.sleep_settings_id = inSettings.id;
            switch (option)
            {
                case 0: //set dim for sleep
                    newDeviceSettings.scheduled_time = inSettings.scheduled_sleep.AddMinutes(-30);
                    newDeviceSettings.settings = "{\\\"lights\\\": 40}";
                    break;
                case 1: //set off for sleep
                    newDeviceSettings.scheduled_time = inSettings.scheduled_sleep;
                    newDeviceSettings.settings = "{\\\"lights\\\": 0}";
                    break;
                case 2: //set dim for wake
                    newDeviceSettings.scheduled_time = inSettings.scheduled_wake.AddMinutes(-5);
                    newDeviceSettings.settings = "{\\\"lights\\\": 20}";
                    break;
                case 3: //set on for wake
                    newDeviceSettings.scheduled_time = inSettings.scheduled_wake;
                    newDeviceSettings.settings = "{\\\"lights\\\": 100}";
                    break;
            }
            return newDeviceSettings;
        }

        private static DeviceSettingsDefaultID SetThermostat(int option, int deviceId, SleepSettings inSettings)
        {
            DeviceSettingsDefaultID newDeviceSettings = new DeviceSettingsDefaultID();
            newDeviceSettings.device_id = deviceId;
            newDeviceSettings.sleep_settings_id = inSettings.id;
            switch (option)
            {
                case 0: //set cool for sleep
                    newDeviceSettings.scheduled_time = inSettings.scheduled_sleep.AddMinutes(-30);
                    newDeviceSettings.settings = "{\\\"temperature\\\": 68}";
                    break;
                case 1: //set sleep temp
                    newDeviceSettings.scheduled_time = inSettings.scheduled_sleep;
                    newDeviceSettings.settings = "{\\\"temperature\\\": 65}";
                    break;
                case 2: //set warm for wake
                    newDeviceSettings.scheduled_time = inSettings.scheduled_wake.AddMinutes(-30);
                    newDeviceSettings.settings = "{\\\"temperature\\\": 68}";
                    break;
                case 3: //set wake temp
                    newDeviceSettings.scheduled_time = inSettings.scheduled_wake;
                    newDeviceSettings.settings = "{\\\"temperature\\\": 72}";
                    break;
            }
            return newDeviceSettings;
        }
    }
}