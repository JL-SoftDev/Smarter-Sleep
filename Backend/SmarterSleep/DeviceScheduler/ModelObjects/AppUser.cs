using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceScheduler.ModelObjects
{
    public class AppUser
    {
        public Guid user_id { get; set; }
        public string username { get; set; }
        public DateTime? created_at { get; set; }
        public int? points { get; set; }
    }
}