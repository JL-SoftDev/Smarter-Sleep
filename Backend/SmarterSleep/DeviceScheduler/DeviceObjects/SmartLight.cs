using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceScheduler.DeviceObjects
{
    public class SmartLight
    {
        int deviceId;
        DateTime scheduledTime;
        string? settings;

        private struct LightSettings
        {
            int deviceId;
            DateTime[] lightSet;
            int[] lightPercent = {40, 0 , 40, 100};

            public LightSettings(int selectedId, DateTime[] selectedLightTimes)
            {
                deviceId = selectedId;
                lightSet = selectedLightTimes;
            }
        }

        public SmartLight(int selectedId, DateTime selectedDate)
        {
            deviceId = selectedId;
            scheduledTime = selectedDate;
        }

        //sets settings to be serialized JSON representation of light settings for selected date
        //uses sleepSelector value of 0-3 to determine exact function in relation to sleep light dimming to off
        //uses wakeSelector value of 0-3 to determine exact function in relation to wake light brightening to on
        public bool NewSettings(DateTime[] previousLightTimes, DateTime upBy, int sleepSelector, int wakeSelector)
        {
            bool success = false;
            settings = null;
            TimeOnly upByTime = TimeOnly.FromDateTime(upBy);
            TimeOnly sleepTime = upByTime.AddHours(-8);
            TimeOnly[] previousTimes = new TimeOnly[4];
            for(int i = 0; i < 4; i++)
            {
                previousTimes[i] = TimeOnly.FromDateTime(previousLightTimes[i]);
            }
            TimeOnly newTime;
            TimeOnly[] newTimes = new TimeOnly[4];
            newTimes[3] = upByTime;
            newTimes[1] = sleepTime;
            DateOnly scheduledWakeDate = DateOnly.FromDateTime(scheduledTime);
            TimeOnly midnight = new TimeOnly(0, 0);
            TimeOnly eightAM = new TimeOnly(8, 0);
            TimeSpan midToWake = midnight - upByTime;
            TimeSpan eightHours = midnight - eightAM;
            DateOnly scheduledSleepDate = new DateOnly();
            if(midToWake < eightHours)
            {
                scheduledSleepDate = scheduledWakeDate.AddDays(-1);
            }
            else if(midToWake >= eightHours)
            {
                scheduledSleepDate = scheduledWakeDate;
            }
            DateTime[] setLights = new DateTime[4];
            setLights[3] = scheduledWakeDate.ToDateTime(newTimes[3]);
            setLights[1] = scheduledSleepDate.ToDateTime(newTimes[1]);
            LightSettings newSettings;
            TimeSpan wakeGap = previousTimes[2] - previousTimes[3];
            int wrap = 0;
            int addWrap = 0;
            switch(wakeSelector)
            {
                case 0: //Set dim light on 15 minutes before upBy
                    newTime = upByTime.AddMinutes(-15, out wrap);
                    newTimes[2] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[2] = scheduledWakeDate.AddDays(-wrap).ToDateTime(newTimes[2]);
                    }
                    else
                    {
                        setLights[2] = scheduledWakeDate.ToDateTime(newTimes[2]);
                    }
                    break;
                case 1: //Set dim light to same gap from upBy as previous
                    newTime = upByTime.Add(-wakeGap, out wrap);
                    newTimes[2] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[2] = scheduledWakeDate.AddDays(-wrap).ToDateTime(newTimes[2]);
                    }
                    else
                    {
                        setLights[2] = scheduledWakeDate.ToDateTime(newTimes[2]);
                    }
                    break;
                case 2: //Set dim light 5 minutes earlier than previous
                    newTime = upByTime.Add(-wakeGap, out wrap);
                    newTime = newTime.AddMinutes(-5, out addWrap);
                    wrap = Math.Abs(wrap);
                    addWrap = Math.Abs(addWrap);
                    wrap = wrap + addWrap;
                    newTimes[2] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[2] = scheduledWakeDate.AddDays(-wrap).ToDateTime(newTimes[2]);
                    }
                    else
                    {
                        setLights[2] = scheduledWakeDate.ToDateTime(newTimes[2]);
                    }
                    break;
                case 3: //Set dim light 5 minutes later than previus, but no later than 5 minutes before upBy
                    newTime = upByTime.Add(-wakeGap, out wrap);
                    newTime = newTime.AddMinutes(5, out addWrap);
                    wrap = Math.Abs(wrap);
                    addWrap = Math.Abs(addWrap);
                    wrap = wrap + addWrap;
                    if(newTime.IsBetween(upByTime.AddMinutes(-5), upByTime))
                    {
                        newTime = upByTime.AddMinutes(-5);
                        wrap = 0;
                    }
                    newTimes[2] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[2] = scheduledWakeDate.AddDays(-wrap).ToDateTime(newTimes[2]);
                    }
                    else
                    {
                        setLights[2] = scheduledWakeDate.ToDateTime(newTimes[2]);
                    }
                    break;
                default:
                    break;
            }
            TimeSpan sleepGap = previousLightTimes[0] - previousLightTimes[1];
            wrap = 0;
            addWrap = 0;
            switch(sleepSelector)
            {
                case 0: //Set dim light  15 minutes before sleepTime
                    newTime = sleepTime.AddMinutes(-15, out wrap);
                    newTimes[0] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[0] = scheduledSleepDate.AddDays(-wrap).ToDateTime(newTimes[0]);
                    }
                    else
                    {
                        setLights[0] = scheduledSleepDate.ToDateTime(newTimes[0]);
                    }
                    break;
                case 1: //Set dim light to same gap from sleepTime as previous
                    newTime = sleepTime.Add(-sleepGap, out wrap);
                    newTimes[0] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[0] = scheduledSleepDate.AddDays(-wrap).ToDateTime(newTimes[0]);
                    }
                    else
                    {
                        setLights[0] = scheduledSleepDate.ToDateTime(newTimes[0]);
                    }
                    break;
                case 2: //Set dim light 5 minutes earlier than previous
                    newTime = sleepTime.Add(-sleepGap, out wrap);
                    newTime = newTime.AddMinutes(-5, out addWrap);
                    wrap = Math.Abs(wrap);
                    addWrap = Math.Abs(addWrap);
                    wrap = wrap + addWrap;
                    newTimes[0] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[0] = scheduledSleepDate.AddDays(-wrap).ToDateTime(newTimes[0]);
                    }
                    else
                    {
                        setLights[0] = scheduledSleepDate.ToDateTime(newTimes[0]);
                    }
                    break;
                case 3: //Set dim light 5 minutes later than previus, but no later than 5 minutes before sleepTime
                    newTime = sleepTime.Add(-sleepGap, out wrap);
                    newTime = newTime.AddMinutes(5, out addWrap);
                    wrap = Math.Abs(wrap);
                    addWrap = Math.Abs(addWrap);
                    wrap = wrap + addWrap;
                    if(newTime.IsBetween(sleepTime.AddMinutes(-5), sleepTime))
                    {
                        wrap = 0;
                        newTime = sleepTime.AddMinutes(-5, out wrap);
                    }
                    newTimes[0] = newTime;
                    if(wrap != 0)
                    {
                        wrap = Math.Abs(wrap);
                        setLights[0] = scheduledSleepDate.AddDays(-wrap).ToDateTime(newTimes[0]);
                    }
                    else
                    {
                        setLights[0] = scheduledSleepDate.ToDateTime(newTimes[0]);
                    }
                    break;
                default:
                    break;
            }
            newSettings = new LightSettings(deviceId, setLights);
            settings = JsonSerializer.Serialize(newSettings);
            if(String.IsNullOrEmpty(settings))
            {
                success = false;
            }
            else
            {
                success = true;
            }
            return success;
        }

        public string? SerializeSmartLight()
        {
            string? serializedSmartLight = null;
            serializedSmartLight = JsonSerializer.Serialize(this);
            return serializedSmartLight;
        }
    }
}