using CommunityToolkit.Mvvm.Input;
using DissertationThemes.ViewerApp.Models;
using DissertationThemes.ViewerApp.Services;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace DissertationThemes.ViewerApp.ViewModels;

public class MenuBarViewModel : ViewModelBase
{
    public event Action CloseWindowRequest;
    public ICommand ExportToCsvCommand { get; }
    public ICommand ExitCommand { get; }
    public ICommand AboutCommand { get; }

    public MenuBarViewModel(FilterModel filterModel, DataService dataService) 
    {
        AboutCommand = new RelayCommand(OpenAbout);
        ExitCommand = new RelayCommand(OnCloseWindow);
        ExportToCsvCommand = new RelayCommand(() => OnExportToCsv(filterModel, dataService));
    }

    private async void OnExportToCsv(FilterModel filterModel, DataService dataService)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "CSV file(*.csv) | *.csv| All files (*.*) | *.*";
        sfd.CheckFileExists = false;
        sfd.CheckPathExists = true;
        sfd.ShowDialog();
        //https://localhost:7066/theme/themes2csv?year=23&stProgramId=56

        string apiURL = $"https://localhost:7066/theme/themes2csv?year={filterModel.SelectedYear}&stProgramId={filterModel.SelectedStudyProgramId}";

        byte[] fileBytes = await dataService.LoadDataAsync<byte[]>(apiURL);
        await File.WriteAllBytesAsync(sfd.FileName, fileBytes);

        Process.Start(new ProcessStartInfo
        {
            FileName = sfd.FileName,
            UseShellExecute = true
        });
    }

    private void OnCloseWindow()
    {
        CloseWindowRequest?.Invoke();
    }

    private void OpenAbout()
    {
        AboutViewModel viewModel = new AboutViewModel();
        AboutWindow aboutWindow = new AboutWindow()
        {
            DataContext = viewModel
        };
        viewModel.CloseWindowRequest += aboutWindow.CloseWindowHandler;
        aboutWindow.ShowDialog();
    }
}
