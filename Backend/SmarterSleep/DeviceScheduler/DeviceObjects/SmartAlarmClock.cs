using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceScheduler.DeviceObjects
{
    public class SmartAlarmClock
    {
        int deviceId;
        DateTime scheduledTime;
        string? settings;

        private struct AlarmSettings
        {
            int deviceId;
            DateTime alarmSet;

            public AlarmSettings(int selectedId, DateTime selectedAlarm)
            {
                deviceId = selectedId;
                alarmSet = selectedAlarm;
            }
        }

        public SmartAlarmClock(int selectedId, DateTime selectedDate)
        {
            deviceId = selectedId;
            scheduledTime = selectedDate;
        }

        //sets settings to be serialized JSON representation of alarm to set for selected date
        //uses selector value of 0-3 to determine exact function
        public bool NewSettings(DateTime previousAlarm, DateTime upBy, int selector)
        {
            bool success = false;
            settings = null;
            TimeOnly upByTime = TimeOnly.FromDateTime(upBy);
            TimeOnly previousTime = TimeOnly.FromDateTime(previousAlarm);
            TimeOnly newTime;
            DateOnly scheduledDate = DateOnly.FromDateTime(scheduledTime);
            DateTime setAlarm;
            AlarmSettings newAlarm;
            switch(selector)
            {
                case 0: //alarm set to time that user needs to be up by
                    setAlarm = scheduledDate.ToDateTime(upByTime);
                    newAlarm = new AlarmSettings(deviceId, setAlarm);
                    settings = JsonSerializer.Serialize(newAlarm);
                    break;
                case 1: //alarm set to previous alarm time
                    setAlarm = scheduledDate.ToDateTime(previousTime);
                    newAlarm = new AlarmSettings(deviceId, setAlarm);
                    settings = JsonSerializer.Serialize(newAlarm);
                    break;
                case 2: //alarm set 15 minutes earlier
                    newTime = previousTime.AddMinutes(-15);
                    setAlarm = scheduledDate.ToDateTime(newTime);
                    newAlarm = new AlarmSettings(deviceId, setAlarm);
                    settings = JsonSerializer.Serialize(newAlarm);
                    break;
                case 3: //alarm set 15 minutes later, but no later than upBy time
                    newTime = previousTime.AddMinutes(15);
                    if(newTime < upByTime)
                    {
                        setAlarm = scheduledDate.ToDateTime(newTime);
                    }
                    else
                    {
                        setAlarm = scheduledDate.ToDateTime(upByTime);
                    }
                    newAlarm = new AlarmSettings(deviceId, setAlarm);
                    settings = JsonSerializer.Serialize(newAlarm);
                    break;
                default:
                    break;
            }
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

        public string? SerializeSmartAlarmClock()
        {
            string? serializedSmartAlarmClock = null;
            serializedSmartAlarmClock = JsonSerializer.Serialize(this);
            return serializedSmartAlarmClock;
        }
    }
}