using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Common.Entities
{
    public class WorkBlock
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DayId { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
        public Day Day { get; set; }
        public int? ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}