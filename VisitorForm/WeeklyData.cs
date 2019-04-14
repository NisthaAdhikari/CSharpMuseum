using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorForm
{
    public class WeeklyData
    {
        public string DayOfWeek { get; set; }
        public string totalVisitors { get; set; }
        public string totalDuration { get; set; }

        public WeeklyData(string DayOfWeek, string totalVisitors, string totalDuration)
        {
            this.DayOfWeek = DayOfWeek;
            this.totalVisitors = totalVisitors;
            this.totalDuration = totalDuration;

        }
    }
}
