﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
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
using System.Windows.Shapes;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy WorkBlockCreatorWindow.xaml
    /// </summary>
    public partial class WorkBlockCreatorWindow : Window
    {
        public WorkBlockCreatorWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<WorkBlockCreatorViewModel>();
        }
    }
}
