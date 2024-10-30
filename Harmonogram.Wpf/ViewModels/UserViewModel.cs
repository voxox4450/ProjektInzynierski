using CommunityToolkit.Mvvm.ComponentModel;
using Harmonogram.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class UserViewModel(User user) : ObservableObject
    {
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

        public void Update(User user)
        {
            FullName = string.Concat(user.Name, " ", user.LastName);
            Mail = user.Mail;
            PhoneNumber = user.PhoneNumber;
        }
    }
}