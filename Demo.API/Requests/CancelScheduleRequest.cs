using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Requests
{
    public class CancelScheduleRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public DateTime ScheduleDate { get; set; }
        public string Procedure { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}