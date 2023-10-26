using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class DeviceSettings
    {
        public int id { get; set; }
        public int device_id { get; set; }
        public int sleep_settings_id { get; set; }
        public DateTime scheduled_time { get; set; }
        public string? settings { get; set; }
    }
}