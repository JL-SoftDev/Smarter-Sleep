using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class WearableData
    {
        public int id { get; set; }
        public DateTime? sleep_start { get; set; }
        public DateTime? sleep_end { get; set; }
        public string? hypnogram { get; set; }
        public int? sleep_score { get; set; }
        public DateOnly sleep_date { get; set; }
    }
}