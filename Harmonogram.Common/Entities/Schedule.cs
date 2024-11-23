namespace Harmonogram.Common.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddHours(1);
        public DateTime? ModifiedOn { get; set; } = null;
        public bool IsArchived { get; set; } = false;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<WorkBlock> WorkBlocks { get; set; } = new List<WorkBlock>();
    }
}