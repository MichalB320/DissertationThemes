using CommunityToolkit.Mvvm.Input;
using DissertationThemes.SharedLibrary;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;

namespace DissertationThemes.ViewerApp.ViewModels;

public class FilterViewModel : ViewModelBase
{
    private List<StProgram> _programList;
    private List<string> _studyPrograms;

    public MenuBarViewModel MenuBarViewModel { get; set; }

    public List<string> StudyPrograms
    {
        get => _studyPrograms;
        set
        {
            _studyPrograms = value;
            OnPropertyChanged(nameof(StudyPrograms));
        }
    }

    private int? _selectedYear;
    public int? SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value; OnPropertyChanged(nameof(SelectedYear)); FilterThemes(SelectedYear);
        }
    }
    private string _selectedStudyProgram;
    public string SelectedStudyProgram
    {
        get => _selectedStudyProgram;
        set
        {
            _selectedStudyProgram = value;
            OnPropertyChanged(nameof(SelectedStudyProgram));
            int stProgramId = _programList.Where(c => c.Name == SelectedStudyProgram).Select(s => s.Id).FirstOrDefault();
            FilterThemes(stProgramId: stProgramId);
        }
    }

    private Theme _selectedTheme;
    public Theme SelectedTheme 
    {
        get => _selectedTheme;
        set
        {
            _selectedTheme = value;
            if (_selectedTheme != null)
                EnableButtons = true;
            else 
                EnableButtons = false;
            OnPropertyChanged(nameof(EnableButtons));
        }
    }

    private async void FilterThemes(int? year = null, int? stProgramId = null)
    {
        Themes.Clear();
        if (year.HasValue && stProgramId.HasValue)
           await LoadAllThemes($"https://localhost:7066/theme/themes?year={year}&stProgramId={stProgramId}");
        else if (year.HasValue && !stProgramId.HasValue)
            await LoadAllThemes($"https://localhost:7066/theme/themes?year={year}");
        else if (!year.HasValue && stProgramId.HasValue)
            await LoadAllThemes($"https://localhost:7066/theme/themes?stProgramId={stProgramId}");
        else if (!year.HasValue && !stProgramId.HasValue)
            await LoadAllThemes("https://localhost:7066/theme/themes");
    }

    private List<int> _themesYears;

    public List<int> ThemesYears
    {
        get => _themesYears;
        set 
        { 
            _themesYears = value; 
            OnPropertyChanged(nameof(ThemesYears));
        }
    }

    private ObservableCollection<Theme> _themes;
    public ObservableCollection<Theme> Themes
    {
        get => _themes;
        set
        {
            _themes = value;
            OnPropertyChanged(nameof(Themes));
        }
    }

    public int Count { get => _themes.Count; }
    public bool EnableButtons { get; set; }
    public ICommand ClearCommand { get; }
    public ICommand ShowDetailsCommand { get; }
    public ICommand GenereateToDocxCommand { get; }

    public FilterViewModel(MenuBarViewModel menuBarViewModel)
    {
        _studyPrograms = new List<string>();
        _themesYears = new List<int>();
        _themes = new ObservableCollection<Theme>();

        ClearCommand = new RelayCommand(() => { SelectedStudyProgram = null; SelectedYear = null; });
        ShowDetailsCommand = new RelayCommand(ShowDetails);
        GenereateToDocxCommand = new RelayCommand(() => SaveToDocx());
        EnableButtons = false;

        MenuBarViewModel = menuBarViewModel;
        InitializeAsync();
    }

    private void ShowDetails()
    {
        DetailViewModel detailViewModel = new DetailViewModel(SelectedTheme);
        DetailWindow detailWin = new DetailWindow()
        {
            DataContext = detailViewModel
        };
        detailViewModel.CloseWindowRequest += detailWin.CloseWindowHandler;
        detailWin.ShowDialog();
    }

    private async Task SaveToDocx()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "Word document (*.docx) | *.docx| All files (*.*) | *.*";
        sfd.CheckFileExists = false;
        sfd.CheckPathExists = true;
        sfd.ShowDialog();

        using (HttpClient httpClient = new HttpClient())
        {
            string apiURL = "https://localhost:7066/theme/theme2docx/11311";

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

    private async Task InitializeAsync()
    {
        await LoadStudyPrograms();
        await LoadThemseYears();
        await LoadAllThemes("https://localhost:7066/theme/themes");
    }

    private async Task LoadAllThemes(string apiURL)
    {

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(apiURL);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var themesResponse = JsonSerializer.Deserialize<List<Theme>>(jsonResponse);

            if (themesResponse != null)
            {
                foreach (var theme in themesResponse) 
                    Themes.Add(theme);
            }
        }
        OnPropertyChanged(nameof(Count));
    }

    private async Task LoadThemseYears()
    {
        string apiURL = "https://localhost:7066/theme/themesyears";

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(apiURL);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var yearsResponse = JsonSerializer.Deserialize<List<int>>(jsonResponse);
            
            if (yearsResponse != null)
                _themesYears.AddRange(yearsResponse);

        }
    }

    private async Task LoadStudyPrograms()
    {
        string apiUrl = "https://localhost:7066/stprograms";

        try
        {
            using (var httpClient = new HttpClient(/*handler*/))
            {
                var response = await httpClient.GetAsync(apiUrl);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var studyPrograms = JsonSerializer.Deserialize<List<StProgram>>(jsonResponse);

                _programList = studyPrograms;

                if (studyPrograms != null)
                {
                    _studyPrograms.Clear();
                    _studyPrograms.AddRange(studyPrograms.Select(s => s.Name));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba pri načítaní údajov: {ex.Message}");
        }
    }
}
