using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Harmonogram.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddTransient<MainViewModel>()
                .AddTransient<ScheduleCreatorViewModel>()
                .AddTransient<ScheduleViewModel>()
                .AddTransient<WorkTimeViewModel>()
                .AddScoped<IDialogService, DialogService>()
                .BuildServiceProvider()
                );
        }
    }

}
