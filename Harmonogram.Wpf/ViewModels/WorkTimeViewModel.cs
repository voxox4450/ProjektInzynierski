﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class WorkTimeViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        public WorkTimeViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }
        [ObservableProperty]
        private bool? _dialogResult;

        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }
    }
}
