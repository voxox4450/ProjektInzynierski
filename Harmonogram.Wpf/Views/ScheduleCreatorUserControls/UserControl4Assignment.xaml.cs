﻿using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Harmonogram.Wpf.Views.ScheduleCreatorUserControls
{
    /// <summary>
    /// Logika interakcji dla klasy UserControl4Assignment.xaml
    /// </summary>
    public partial class UserControl4Assignment : UserControl
    {
        public UserControl4Assignment()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}