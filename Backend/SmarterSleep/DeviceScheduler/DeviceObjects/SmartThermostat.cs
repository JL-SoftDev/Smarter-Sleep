using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceScheduler.DeviceObjects
{
    public class SmartThermostat
    {
        public int deviceId { get; set; }
        public DateTime scheduledTime { get; set; }
        public string? settings { get; set; }

        private struct TemperatureSettings
        {
            int deviceId;
            DateTime[] tempSet;
            int[] temps = { 70, 68, 70, 72 };

            public TemperatureSettings(int selectedId, DateTime[] selectedTemptimes)
            {
                deviceId = selectedId;
                tempSet = selectedTemptimes;
            }

            public TemperatureSettings(int selectedId, DateTime[] selectedTemptimes, int[] selectedTemps)
            {
                deviceId = selectedId;
                tempSet = selectedTemptimes;
                temps = selectedTemps;
            }
        }

        public SmartThermostat(int selectedId, DateTime selectedDate)
        {
            deviceId = selectedId;
            scheduledTime = selectedDate;
        }

        public bool NewSettings(DateTime[] previousTempTimes, DateTime upBy, int nightCoolSelector, int sleepTempSelector, int wakeWarmSelector, int wakeTempSelctor)
        {
            bool success = false;
            settings = null;

            TimeOnly upByTime = TimeOnly.FromDateTime(upBy);
            DateOnly scheduledWakeDate = DateOnly.FromDateTime(scheduledTime);
            TimeOnly[] previousTimes = new TimeOnly[4];
            for(int i = 0; i < 4; i++)
            {
                previousTimes[i] = TimeOnly.FromDateTime(previousTempTimes[i]);
            }
            TimeOnly[] newTimes = new TimeOnly[4];
            DateTime[] setTemps = new DateTime[4];
            TemperatureSettings newSettings;
            int wrap = 0;

            switch(wakeTempSelctor)
            {
                case 0: //set full wake temp to upByTime
                    newTimes[3] = upByTime;
                    setTemps[3] = scheduledWakeDate.ToDateTime(newTimes[3]);
                    break;
                case 1: //set full wake temp to same time as previous
                    newTimes[3] = previousTimes[3];
                    setTemps[3] = scheduledWakeDate.ToDateTime(newTimes[3]);
                    break;
                case 2: //set full wake temp to 5 minutes earlier than previous
                    newTimes[3] = previousTimes[3].AddMinutes(-5, out wrap);
                    setTemps[3] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[3]);
                    break;
                case 3: //set full wake temp to 5 minutes later than previous
                    newTimes[3] = previousTimes[3].AddMinutes(5, out wrap);
                    setTemps[3] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[3]);
                    break;
                default:
                    break;
            }

            switch(wakeWarmSelector)
            {
                case 0: //set wake warm temp to 15 minutes before upByTime
                    newTimes[2] = upByTime.AddMinutes(-15, out wrap);
                    setTemps[2] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[2]);
                    break;
                case 1: //set wake warm temp to same time as previous
                    newTimes[2] = previousTimes[2];
                    setTemps[2] = scheduledWakeDate.ToDateTime(newTimes[2]);
                    break;
                case 2: //set wake warm temp to 5 minutes earlier than previous
                    newTimes[2] = previousTimes[2].AddMinutes(-5, out wrap);
                    setTemps[2] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[2]);
                    break;
                case 3: //set wake warm temp to 5 minutes later than previous
                    newTimes[2] = previousTimes[2].AddMinutes(5, out wrap);
                    setTemps[2] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[2]);
                    break;
                default:
                    break;
            }

            switch(sleepTempSelector)
            {
                case 0: //set full sleep temp to 8 hours before upbytime
                    newTimes[1] = upByTime.AddHours(-8, out wrap);
                    setTemps[1] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[1]);
                    break;
                case 1: //set full sleep temp to same time as previous
                    newTimes[1] = previousTimes[1];
                    setTemps[1] = scheduledWakeDate.ToDateTime(newTimes[2]);
                    break;
                case 2: //set full sleep temp to 5 minutes earlier than previous
                    newTimes[1] = previousTimes[1].AddMinutes(-5, out wrap);
                    setTemps[1] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[1]);
                    break;
                case 3: //set full sleep temp to 5 minutes later than previous
                    newTimes[1] = previousTimes[1].AddMinutes(5, out wrap);
                    setTemps[1] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[1]);
                    break;
                default:
                    break;
            }

            switch(nightCoolSelector)
            {
                case 0: //set sleep cool temp to 8 hours before upbytime
                    newTimes[0] = upByTime.AddHours(-8, out wrap);
                    setTemps[0] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[0]);
                    break;
                case 1: //set sleep cool temp to same time as previous
                    newTimes[0] = previousTimes[0];
                    setTemps[0] = scheduledWakeDate.ToDateTime(newTimes[2]);
                    break;
                case 2: //set sleep cool temp to 5 minutes earlier than previous
                    newTimes[0] = previousTimes[0].AddMinutes(-5, out wrap);
                    setTemps[0] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[0]);
                    break;
                case 3: //set sleep cool temp to 5 minutes later than previous
                    newTimes[0] = previousTimes[0].AddMinutes(5, out wrap);
                    setTemps[0] = scheduledWakeDate.AddDays(-Math.Abs(wrap)).ToDateTime(newTimes[0]);
                    break;
                default:
                    break;
            }

            newSettings = new TemperatureSettings(deviceId, setTemps);
            settings = JsonSerializer.Serialize(newSettings);
            if (String.IsNullOrEmpty(settings))
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
