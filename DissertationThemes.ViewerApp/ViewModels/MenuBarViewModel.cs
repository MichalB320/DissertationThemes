using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows.Input;

namespace DissertationThemes.ViewerApp.ViewModels;

public class MenuBarViewModel : ViewModelBase
{
    public event Action CloseWindowRequest;
    public ICommand ExportToCsvCommand { get; }
    public ICommand ExitCommand { get; }
    public ICommand AboutCommand { get; }

    public MenuBarViewModel() 
    {
        AboutCommand = new RelayCommand(OpenAbout);
        ExitCommand = new RelayCommand(OnCloseWindow);
        ExportToCsvCommand = new RelayCommand(OnExportToCsv);
    }

    private async void OnExportToCsv()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "CSV file(*.csv) | *.csv| All files (*.*) | *.*";
        sfd.CheckFileExists = false;
        sfd.CheckPathExists = true;
        sfd.ShowDialog();

        string apiURL = "https://localhost:7066/theme/themes2csv";

        using (HttpClient httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(apiURL);
            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(sfd.FileName, fileBytes);

            Process.Start(new ProcessStartInfo
            {
                FileName = sfd.FileName,
                UseShellExecute = true 
            });
        }
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
