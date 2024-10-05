
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmDialogs;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class SchedulePanelViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        public SchedulePanelViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }
        [ObservableProperty]
        private bool? _dialogResult;
    }
}
