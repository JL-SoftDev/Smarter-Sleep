using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class SleepSettings
    {
        public int id { get; set; }
        public Guid user_id { get; set; }
        public DateTime scheduled_sleep { get; set; }
        public DateTime scheduled_wake { get; set; }
        public string? scheduled_hypnogram { get; set; }
        public List<DeviceSettings>? deviceSettings { get; set; }
    }
}