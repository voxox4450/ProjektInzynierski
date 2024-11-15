using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System.Collections.ObjectModel;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class UsersListViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IUserService _userService;
        public UsersListViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            _userService.UserUpdated += OnUserUpdate;

            LoadUsers();
        }
        [ObservableProperty]
        private bool? _dialogResult;

        public ObservableCollection<UserViewModel> Users { get; set; } = [];

        [ObservableProperty]
        private UserViewModel _selectedUser;

        private void LoadUsers()
        {
            var users = _userService.GetAll();
            var userVms = new List<UserViewModel>();


            foreach (var user in users)
            {
                userVms.Add(CreateUserVM(user));
            }

            Users = new ObservableCollection<UserViewModel>(userVms);
        }
        private static UserViewModel CreateUserVM(User user)
        {
            return new UserViewModel(user);
        }

        private void OnUserUpdate(object sender, User UpdatedUser)
        {
            var userToUpdate = Users.FirstOrDefault(u => u.Id == UpdatedUser.Id);
            if (userToUpdate != null)
            {
                return;
            }
            userToUpdate.Update(UpdatedUser);
        }


        [RelayCommand]
        private void OpenManageUser()
        {
            if (SelectedUser is null)
            {
                return;

            }
            var dialogViewModel = new ManageUserViewModel(SelectedUser);
            _dialogService.ShowDialog<ManageUserWindow>(this, dialogViewModel);
        }


        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }
    }
}
