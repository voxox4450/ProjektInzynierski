namespace Harmonogram.Common.Entities
{
    public class UserPayment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double PaymentPerHour { get; set; } = 0.0;
        public string AccountNumber { get; set; } = string.Empty;

        public User User { get; set; } = default!;
    }
}
