using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Common.ValidationRules;
using MvvmDialogs;
using ReportsSender4.Common.ValidationRules;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class CreateUserViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IUserService _userService;
        public CreateUserViewModel()
        {
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }

        private bool _firstRun = true;
        public User userRegister { get; set; }


        [ObservableProperty]
        private bool? _dialogResult;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 2 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaki.")]
        [OnlyLetters(ErrorMessage = "Pole tekstowe może zawierać tylko litery.")]
        private string _name = string.Empty;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 2 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaki.")]
        [OnlyLetters(ErrorMessage = "Pole tekstowe może zawierać tylko litery.")]
        private string _lastname = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaki.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail.")]
        private string _mail = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [MinLength(9, ErrorMessage = "Pole tekstowe musi zawierać 9 znaków.")]
        [MaxLength(9, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 9 znaków.")]
        [OnlyNumbers(ErrorMessage = "Pole tekstowe musi zawierać tylko cyfry.")]
        private string _phone = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaków.")]
        [StrongPassword(ErrorMessage = "Hasło musi być silne.")]
        private string _password = string.Empty;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaków.")]
        [StrongPassword(ErrorMessage = "Hasło musi być silne.")]
        [MatchProperty(nameof(Password), ErrorMessage = "Hasła nie są takie same")]
        private string _passwordRepeat = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Numer konta jest wymagany.")]
        [MinLength(26, ErrorMessage = "Pole tekstowe musi zawierać dokłądnie 26 znaków.")]
        [MaxLength(26, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 26 znaków.")]
        [OnlyNumbers(ErrorMessage = "Pole tekstowe musi zawierać tylko cyfry.")]
        private string _accountNumber = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        [Required(ErrorMessage = "Pole płatność (na godzinę) jest wymagane.")]
        [DecimalNumber(ErrorMessage = "Pole tekstowe musi zawierać poprawny format liczbowy (cyfra, przecinek/kropka).")]
        private string _paymentPerHour = string.Empty;

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
        private void Register()
        {

            var user = new User()
            {
                Name = Name,
                LastName = Lastname,
                Mail = Mail,
                PhoneNumber = Phone,
                Password = PasswordHelper.Encrypt(Password),
                AccountNumber = AccountNumber,
                PaymentPerHour = double.Parse(PaymentPerHour),
            };
            userRegister = _userService.CheckFirst(user);

            if (userRegister is null)
            {
                _userService.Register(user);
                HC.MessageBox.Show("Rejestracja przebiegła pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                HC.MessageBox.Show("Błąd rejestracji. Użytkownik istnieje", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }
    }
}