using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plugin_WorkAssignment.ModelAPI
{
    public class UtilizationChartData
    {
        public Nullable<decimal> SCHEDULED_HRS { get; set; }
        public Nullable<decimal> AVAILABLE_HRS { get; set; }
        public string ScheduledColor { get; set; }
        public DateTime WORK_DATE { get; set; }
        public string SHOP { get; set; }
        public string TECH_NAME { get; set; }
    }
}