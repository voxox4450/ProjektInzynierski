using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using MvvmDialogs;
using System.Collections.ObjectModel;

namespace Harmonogram.Wpf.ViewModels.ListViewModels
{
    public partial class PropertiesListViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IWorkBlockService _workBlockService;

        public PropertiesListViewModel(WorkBlockListViewModel? selectedDay = null)
        {
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();

            SelectedWorkBlock = selectedDay;
            if (selectedDay is null) { return; }
            LoadData();

        }

        [ObservableProperty]
        private DateTime _selectedDay;
        //TODO: naprawic przeysałanie dnayhcg
        [ObservableProperty]
        private WorkBlockListViewModel _selectedWorkBlock;

        [ObservableProperty]
        private bool? _dialogResult;

        [ObservableProperty]
        private string _day;

        [ObservableProperty]
        private string _fullName;

        [ObservableProperty]
        private string _hourRange;

        [ObservableProperty]
        private string _hourAmount;

        [ObservableProperty]
        private ObservableCollection<WorkBlockListViewModel> _teamWork = [];

        private void LoadData()
        {

            // Pobierz wszystkie bloki pracy dla użytkownika
            var workBlocks = _workBlockService.GetAll();
            var workBlocksVms = new List<WorkBlockListViewModel>();



            // Filtruj bloki pracy na podstawie wybranego dnia
            foreach (var workBlock in workBlocks)
            {
                var workBlockVm = CreateWorkBlockVm(workBlock);



                if (workBlock.Date == SelectedWorkBlock.Date)
                {
                    workBlocksVms.Add(workBlockVm);
                }
            }

            workBlocksVms.Sort((x, y) => x.Date.CompareTo(y.Date));

            TeamWork = new ObservableCollection<WorkBlockListViewModel>(workBlocksVms);
        }
        private static WorkBlockListViewModel CreateWorkBlockVm(WorkBlock workBlock)
        {
            return new WorkBlockListViewModel(workBlock);
        }
        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }

    }
}
