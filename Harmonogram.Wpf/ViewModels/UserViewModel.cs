using CommunityToolkit.Mvvm.ComponentModel;
using Harmonogram.Common.Entities;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class UserViewModel(User user) : ObservableObject
    {
        [ObservableProperty]
        private User _user = user;

        [ObservableProperty]
        private int _id = user.Id;

        [ObservableProperty]
        private string _name = user.Name;

        [ObservableProperty]
        private string _lastName = user.LastName;

        [ObservableProperty]
        private string _mail = user.Mail;

        [ObservableProperty]
        private string _phoneNumber = user.PhoneNumber;

        [ObservableProperty]
        private string _fullName = string.Concat(user.Name, " ", user.LastName);

        [ObservableProperty]
        private bool _isChecked = user.IsChecked;

        public void Update(User user)
        {
            FullName = string.Concat(user.Name, " ", user.LastName);
            Mail = user.Mail;
            PhoneNumber = user.PhoneNumber;
            IsChecked = user.IsChecked;
        }
    }
}