using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class CustomSchedule
    {
        public Guid user_id { get; set; }
        public int? day_of_week { get; set; }
        public TimeOnly? wake_time { get; set; }
    }
}