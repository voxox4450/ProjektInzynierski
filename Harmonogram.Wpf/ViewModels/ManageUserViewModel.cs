using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Common.ValidationRules;
using MvvmDialogs;
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
        }

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
        [MinLength(2, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 2 znaki.")]
        [MaxLength(24, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 24 znaki.")]
        private string _name;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MinLength(2, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 2 znaki.")]
        [MaxLength(24, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 24 znaki.")]
        private string _lastname;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(24, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 24 znaki.")]
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
        [MaxLength(48, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 48 znaków.")]
        private string _password = string.Empty;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [MinLength(3, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 3 znaki.")]
        [MaxLength(48, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 48 znaków.")]
        [MatchProperty(nameof(Password), ErrorMessage = "Hasła nie są takie same")]
        private string _passwordRepeat = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Numer konta jest wymagany.")]
        [MinLength(1, ErrorMessage = "Pole tekstowe musi zawierać więcej niż 1 znaki.")]
        [MaxLength(26, ErrorMessage = "Pole tekstowe nie może zawierać więcej niż 26 znaki.")]
        private string _accountNumber = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        [Required(ErrorMessage = "Pole płatność (na godzinę) jest wymagane.")]
        //dodac ze tylko cyfry
        private string _paymentPerHour = string.Empty;

        [RelayCommand]
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

            _userService.Update(EditedUser);
            Close();
        }

        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }
    }
}
