using DissertationThemes.ViewerApp.Models;
using DissertationThemes.ViewerApp.Services;
using DissertationThemes.ViewerApp.ViewModels;
using System.Windows;

namespace DissertationThemes.ViewerApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly MenuBarViewModel _menuBarViewModel;
    private readonly FilterModel _filterModel;
    private readonly DataService _dataService;

    public App()
    {
        _filterModel = new FilterModel();
        _dataService = new DataService();
        _menuBarViewModel = new MenuBarViewModel(_filterModel, _dataService);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(_menuBarViewModel, _filterModel, _dataService)
        };
        _menuBarViewModel.CloseWindowRequest += MainWindow.Close;
        MainWindow.Show();

        base.OnStartup(e);
    }
}
