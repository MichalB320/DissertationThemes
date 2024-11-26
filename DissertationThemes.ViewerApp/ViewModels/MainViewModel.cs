using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DissertationThemes.ViewerApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase CurrentViewModel { get; set; }

    public MainViewModel()
    {
        CurrentViewModel = new FilterViewModel();

    }
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

}
