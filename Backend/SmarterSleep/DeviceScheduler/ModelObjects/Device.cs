using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class Device
    {
        public int id { get; set; }
        public Guid user_id { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public string? ip { get; set; }
        public int? port { get; set; }
        public string? status { get; set; }
    }
}