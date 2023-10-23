using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class SleepReview
    {
        public int id { get; set; }
        public Guid user_id { get; set; }
        public int? wearable_log_id { get; set; }
        public int? survey_id { get; set; }
        public DateTime created_at { get; set; }
        public int? smarter_sleep_score { get; set; }
    }
}