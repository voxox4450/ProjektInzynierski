using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using MvvmDialogs;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class LoginViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        private bool _firstRun = true;
        public event EventHandler<User>? IsLoggedIn;
        public User userToLogged { get; set; }


        public LoginViewModel()
        {
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }

        [ObservableProperty]
        private bool? _dialogResult;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(24, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 24 znaki.")]
        private string _mail = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(48, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 48 znaków.")]
        private string _password = string.Empty;

        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }

        private bool IsValid()
        {
            if (_firstRun)
            {
                ValidateAllProperties();

                _firstRun = false;
            }

            return !HasErrors;
        }



        [RelayCommand(CanExecute = nameof(IsValid))]
        private void Login()
        {
            var user = new User()
            {
                Mail = Mail,
                Password = PasswordHelper.Encrypt(Password),

            };

            userToLogged = _userService.Login(user);
            if (userToLogged is not null)
            {
                user.IsAdmin = userToLogged.IsAdmin;
                IsLoggedIn?.Invoke(this, user);
                Close();
            }
            else
            {
                HC.MessageBox.Show("Błąd logowania. Użytkownik nie istnieje", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
