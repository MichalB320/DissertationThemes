using DissertationThemes.ViewerApp.Models;
using DissertationThemes.ViewerApp.Services;

namespace DissertationThemes.ViewerApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase CurrentViewModel { get; set; }

    public MainViewModel(MenuBarViewModel menuBarViewModel, FilterModel filterModel, DataService dataService)
    {
        CurrentViewModel = new FilterViewModel(menuBarViewModel, filterModel, dataService);

    }

    //private void OnCurrentViewModelChanged()
    //{
    //    OnPropertyChanged(nameof(CurrentViewModel));
    //}
}
