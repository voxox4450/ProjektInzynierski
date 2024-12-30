using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using System.Windows;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class WorkBlockViewModel(WorkBlock workBlock, User user) : ObservableObject
    {
        private readonly IColorService _colorService = Ioc.Default.GetRequiredService<IColorService>();

        [ObservableProperty]
        private User _user = user;

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
        private int _colorId = workBlock.ColorId;

        [ObservableProperty]
        private Color _color = workBlock.Color;

        [ObservableProperty]
        private double _width = double.NaN;

        [ObservableProperty]
        private double _height = 0;

        [ObservableProperty]
        private double _top = 0;

        [ObservableProperty]
        private Thickness _margin;

        [ObservableProperty]
        private double _blockHeight = 40;

        public void LoadBlock(double width)
        {
            Width = width;
            Height = (EndHour - StartHour) * BlockHeight;
            Top = StartHour * BlockHeight + 20;
            Color = _colorService.GetById(ColorId);
            Name = $"Zmiana:\n {workBlock.User.Name}\n {workBlock.User.LastName}\n{StartHour}:00 - {EndHour}:00";
        }

        public void Update(WorkBlock workBlock)
        {
            User = workBlock.User;
            WorkBlock = workBlock;
            DayId = workBlock.DayId;
            StartHour = workBlock.StartHour;
            EndHour = workBlock.EndHour;
            ColorId = workBlock.ColorId;
            Color = workBlock.Color;
        }
    }
}