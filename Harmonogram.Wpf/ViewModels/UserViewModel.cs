using CommunityToolkit.Mvvm.ComponentModel;
using Harmonogram.Common.Entities;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class UserViewModel(User user) : ObservableObject
    {
        public int Id { get; set; } = user.Id;

        [ObservableProperty]
        private string _name = user.Name;

        [ObservableProperty]
        private string _lastName = user.LastName;

        [ObservableProperty]
        private string _fullName = string.Concat(user.Name, " ", user.LastName);

        [ObservableProperty]
        private string _mail = user.Mail;

        [ObservableProperty]
        private string _phoneNumber = user.PhoneNumber;

        [ObservableProperty]
        private string _accountNumber = user.AccountNumber;

        [ObservableProperty]
        private string _paymentPerHour = user.PaymentPerHour.ToString();

        [ObservableProperty]
        private bool _isChecked;

        public User User { get; private set; } = user;

        public void Update(User user)
        {
            Name = user.Name;
            LastName = user.LastName;
            FullName = string.Concat(user.Name, " ", user.LastName);
            Mail = user.Mail;
            PhoneNumber = user.PhoneNumber;
            AccountNumber = user.AccountNumber;
            PaymentPerHour = user.PaymentPerHour.ToString();

            User = user;
        }
    }
}
