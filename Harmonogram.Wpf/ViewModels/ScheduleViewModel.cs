using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ScheduleViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;

        public ScheduleViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }

        [ObservableProperty]
        private bool? _dialogResult;
    }
}