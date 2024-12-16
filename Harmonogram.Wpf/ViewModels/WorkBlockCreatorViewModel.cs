using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class WorkBlockCreatorViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDayService _dayService;
        private readonly IWorkBlockService _workBlockService;
        private readonly IColorService _colorService;

        public WorkBlockCreatorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _dayService = Ioc.Default.GetRequiredService<IDayService>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();
            _colorService = Ioc.Default.GetRequiredService<IColorService>();

            Colors = new ObservableCollection<Color>(_colorService.GetAll());
            SelectedColor = _colors[0];
        }

        #region Fields & Properties

        [Required(ErrorMessage = "Godzina rozpoczęcia jest wymagana.")]
        [ObservableProperty]
        private int _workStart = 0;

        [Required(ErrorMessage = "Godzina zakończenia jest wymagana.")]
        [ObservableProperty]
        private int _workEnd = 23;

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private string _day;

        [ObservableProperty]
        private ObservableCollection<Color> _colors;

        [ObservableProperty]
        private Color _selectedColor;

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _checkedUsers = [];

        [ObservableProperty]
        private UserViewModel _selectedUser;

        [ObservableProperty]
        private bool? _dialogResult;

        #endregion Fields & Properties

        private bool IsValid()
        {
            ValidateAllProperties();
            return !HasErrors;
        }

        [RelayCommand(CanExecute = nameof(IsValid))]
        private void Save()
        {
            if (WorkStart >= 0 &&
                WorkStart <= 23 &&
                WorkEnd >= 0 &&
                WorkEnd <= 24 &&
                WorkEnd > WorkStart)
            {
                var workBlock = new WorkBlock
                {
                    StartHour = WorkStart,
                    EndHour = WorkEnd,
                    DayId = _dayService.GetDayId(Day),
                    UserId = SelectedUser.Id,
                    Date = Date,
                    Color = SelectedColor
                };

                _workBlockService.Add(workBlock);

                DialogResult = true;
            }
            else
            {
                HC.MessageBox.Show("Wszystkie pola muszą być uzupełnione!\n" +
                    "Godziny rozpoczęcia i zakończneia muszą mieścić się w zakresie <0,23> (int).\n" +
                    "Godzina rozpoczęcia nie może być większa od godziny zakończenia.",
                    "Ostrzeżenie",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}