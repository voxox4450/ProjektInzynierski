namespace Harmonogram.Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
        public double PaymentPerHour { get; set; } = 0.0;
        public string AccountNumber { get; set; } = string.Empty;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<WorkBlock> WorkBlocks { get; set; } = new List<WorkBlock>();

    }
}
