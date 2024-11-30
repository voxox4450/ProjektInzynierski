using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmDialogs;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using System.Collections.ObjectModel;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class DisplayUserInformationViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IUserService _userService;
        public DisplayUserInformationViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _userService = Ioc.Default.GetRequiredService<IUserService>();
        }
        [ObservableProperty]
        public bool? _dialogResult;

        [ObservableProperty]
        private User _currentUser;

    }
}
