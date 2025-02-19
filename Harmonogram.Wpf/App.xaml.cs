﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Common.Repositories;
using Harmonogram.Common.Services;
using Harmonogram.Wpf.ViewModels;
using Harmonogram.Wpf.ViewModels.ListViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
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

            Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .AddDbContext<Context>(options => options
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))
            .AddTransient<LoginViewModel>()
            .AddTransient<MainViewModel>()
            .AddTransient<WorkTimeViewModel>()
            .AddTransient<SchedulePanelViewModel>()
            .AddTransient<WorkBlockCreatorViewModel>()
            .AddTransient<WorkBlockEditorViewModel>()
            .AddTransient<ScheduleCreatorViewModel>()
            .AddTransient<ScheduleEditorViewModel>()
            .AddTransient<CreateUserViewModel>()
            .AddTransient<UsersListViewModel>()
            .AddTransient<UserViewModel>()
            .AddTransient<ManageUserViewModel>()
            .AddTransient<WorkBlockListViewModel>()
            .AddTransient<DisplayUserInformationViewModel>()
            .AddTransient<PropertiesListViewModel>()
            .AddScoped<IDialogService, DialogService>()
            .AddScoped<IUserService, UserServices>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IDayService, DayService>()
            .AddScoped<IDayRepository, DayRepository>()
            .AddScoped<IScheduleService, ScheduleService>()
            .AddScoped<IScheduleRepository, ScheduleRepository>()
            .AddScoped<IWorkBlockService, WorkBlockService>()
            .AddScoped<IWorkBlockRepository, WorkBlockRepository>()
            .AddScoped<IColorService, ColorService>()
            .AddScoped<IColorRepository, ColorRepository>()
            .AddScoped<SeederContext>()
            .AddScoped<Constants>()
            .BuildServiceProvider());

            using var scope = Ioc.Default.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<SeederContext>();
            seeder.SeedUser();
            seeder.SeedDays();
            seeder.SeedColors();
        }
    }

}
