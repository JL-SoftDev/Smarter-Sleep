using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class Survey
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public int? sleep_quality { get; set; }
        public int? wake_preference { get; set; }
        public int? temp_preference { get; set; }
        public bool? lights_disturbance { get; set; }
        public bool? sleep_earlier { get; set; }
        public int? sleep_duration { get; set; }
        public DateOnly survey_date { get; set; }
    }
}