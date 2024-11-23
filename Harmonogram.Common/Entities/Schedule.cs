namespace Harmonogram.Common.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsArchived { get; set; } = false;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<WorkBlock> WorkBlocks { get; set; } = new List<WorkBlock>();
    }
}