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

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ManageUserViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IUserService _userService;


        public ManageUserViewModel(UserViewModel? user = null)
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            User = user;
            LoadData();
        }
        private bool _firstRun = true;

        [ObservableProperty]
        private User _editedUser;

        [ObservableProperty]
        private UserViewModel _user;

        [ObservableProperty]
        private bool? _dialogResult;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 2 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaki.")]
        [OnlyLetters(ErrorMessage = "Pole tekstowe może zawierać tylko litery.")]
        private string _name;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 2 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaki.")]
        [OnlyLetters(ErrorMessage = "Pole tekstowe może zawierać tylko litery.")]
        private string _lastname;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaki.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail.")]
        private string _mail;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [MinLength(9, ErrorMessage = "Pole tekstowe musi zawierać 9 znaków.")]
        [MaxLength(9, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 9 znaków.")]
        [OnlyNumbers(ErrorMessage = "Pole tekstowe musi zawierać tylko cyfry.")]
        private string _phone;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaków.")]
        [StrongPassword(ErrorMessage = "Hasło musi być silne.")]
        private string _password = string.Empty;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(32, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 32 znaków.")]
        [StrongPassword(ErrorMessage = "Hasło musi być silne.")]
        [MatchProperty(nameof(Password), ErrorMessage = "Hasła nie są takie same")]
        private string _passwordRepeat = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Numer konta jest wymagany.")]
        [MinLength(26, ErrorMessage = "Pole tekstowe musi zawierać dokłądnie 26 znaków.")]
        [MaxLength(26, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 26 znaki.")]
        [OnlyNumbers(ErrorMessage = "Pole tekstowe musi zawierać tylko cyfry.")]
        private string _accountNumber;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Pole płatność (na godzinę) jest wymagane.")]
        [DecimalNumber(ErrorMessage = "Pole tekstowe musi zawierać poprawny format liczbowy (cyfra, przecinek/kropka).")]
        private string _paymentPerHour;

        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }

        private void LoadData()
        {
            if (User is null)
                return;
            Name = User.Name;
            Lastname = User.LastName;
            Mail = User.Mail;
            Phone = User.PhoneNumber;
            AccountNumber = User.AccountNumber;
            PaymentPerHour = User.PaymentPerHour.ToString();
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
        private void Edit()
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
            user.Id = EditedUser.Id;
            _userService.Update(user);



            Close();
        }


    }
}
