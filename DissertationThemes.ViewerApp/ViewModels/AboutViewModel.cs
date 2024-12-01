using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace DissertationThemes.ViewerApp.ViewModels;

internal class AboutViewModel : ViewModelBase
{
    public event Action CloseWindowRequest;
    public ICommand OkCommand { get; }

    public AboutViewModel()
    {
        OkCommand = new RelayCommand(OnCloseWindows);
    }

    private void OnCloseWindows()
    {
        CloseWindowRequest?.Invoke();
    }
}
