using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceScheduler.DeviceObjects
{
    public class SmartThermostat
    {
        int deviceId;
        DateTime scheduledTime;
        string? settings;

        private struct TemperatureSettings
        {
            int deviceId;
            DateTime[] tempSet;
            int[] temps = {70, 68 , 70, 72};

            public TemperatureSettings(int selectedId, DateTime[] selectedTemptimes)
            {
                deviceId = selectedId;
                tempSet = selectedTemptimes;
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

            newSettings = new TemperatureSettings(deviceId, setTemps);
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
}