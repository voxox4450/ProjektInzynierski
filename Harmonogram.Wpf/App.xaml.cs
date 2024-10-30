using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Common.Repositories;
using Harmonogram.Common.Services;
using Harmonogram.Wpf.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Harmonogram.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddDbContext<Context>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<MainViewModel>()
                .AddTransient<ScheduleCreatorViewModel>()
                .AddTransient<ScheduleViewModel>()
                .AddTransient<WorkTimeViewModel>()
                .AddScoped<IDialogService, DialogService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IConstants, Constants>()
                .AddScoped<ISeederContext, SeederContext>()
                .BuildServiceProvider()
                );

            using var scope = Ioc.Default.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<ISeederContext>();
            seeder.Seed();
        }
    }
}