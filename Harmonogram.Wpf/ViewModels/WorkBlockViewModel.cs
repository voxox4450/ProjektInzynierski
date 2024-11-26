using CommunityToolkit.Mvvm.ComponentModel;
using Harmonogram.Common.Entities;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class WorkBlockViewModel(WorkBlock workBlock, User user) : ObservableObject
    {
        [ObservableProperty]
        private WorkBlock _workBlock = workBlock;

        [ObservableProperty]
        private int _id = workBlock.Id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _dayId = workBlock.DayId;

        [ObservableProperty]
        private int _startHour = workBlock.StartHour;

        [ObservableProperty]
        private int _endHour = workBlock.EndHour;

        [ObservableProperty]
        private double _width = double.NaN;

        [ObservableProperty]
        private double _height = 0;

        [ObservableProperty]
        private double _top = 0;

        [ObservableProperty]
        private double _left = 0;

        [ObservableProperty]
        private double _blockHeight = 40;

        public void LoadBlock()
        {
            Width = 100;
            Height = (EndHour - StartHour) * BlockHeight;
            Top = StartHour * BlockHeight + 20;
        }

        public void SetName()
        {
            Name = $"Zmiana {user.Name} + {user.LastName}\n{StartHour} - {EndHour}";
        }
    }
}