namespace Harmonogram.Common.Entities
{
    public class WorkBlock
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DayId { get; set; }
        public int ColorId { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; } = default!;
        public Day Day { get; set; } = default!;
        public Color Color { get; set; } = default!;
        public int? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
    }
}