using CommunityToolkit.Mvvm.ComponentModel;
using MvvmDialogs;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class CreateUserViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        public CreateUserViewModel()
        {
            //_dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }

        [ObservableProperty]
        private bool? _dialogResult;
    }
}
