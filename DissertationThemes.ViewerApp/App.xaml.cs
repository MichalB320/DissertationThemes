using DissertationThemes.ViewerApp.ViewModels;
using System.Windows;

namespace DissertationThemes.ViewerApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly MenuBarViewModel _menuBarViewModel;

    public App()
    {
        _menuBarViewModel = new MenuBarViewModel();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(_menuBarViewModel)
        };
        _menuBarViewModel.CloseWindowRequest += MainWindow.Close;
        MainWindow.Show();

        base.OnStartup(e);
    }
}
