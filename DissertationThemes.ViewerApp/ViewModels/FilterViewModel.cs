using CommunityToolkit.Mvvm.Input;
using DissertationThemes.SharedLibrary;
using DissertationThemes.ViewerApp.Models;
using DissertationThemes.ViewerApp.Services;
using DissertationThemes.WebApi.DTOs;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace DissertationThemes.ViewerApp.ViewModels;

public class FilterViewModel : ViewModelBase
{
    private List<StProgram> _programList;
    private FilterModel _filterModel;
    private DataService _dataService;

    public MenuBarViewModel MenuBarViewModel { get; set; }

    public int? SelectedYear
    {
        get => _filterModel.SelectedYear;
        set
        {
            _filterModel.SelectedYear = value; OnPropertyChanged(nameof(SelectedYear)); FilterThemes(SelectedYear); 
        }
    }

    public string SelectedStudyProgram
    {
        get => _filterModel.SelectedStudyProgram;
        set
        {
            _filterModel.SelectedStudyProgram = value;
            OnPropertyChanged(nameof(SelectedStudyProgram));
            int stProgramId = _programList.Where(c => c.Name == SelectedStudyProgram).Select(s => s.Id).FirstOrDefault();
            FilterThemes(stProgramId: stProgramId);
            _filterModel.SelectedStudyProgramId = stProgramId;
        }
    }

    private ThemeSupDTO _selectedTheme;
    public ThemeSupDTO SelectedTheme 
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

    public List<string> StudyPrograms { get; set; }
    public List<int> ThemesYears { get; set; }
    public ObservableCollection<ThemeSupDTO> Themes { get; set; }
    public int Count { get => Themes.Count; }
    public bool EnableButtons { get; set; }
    public ICommand ClearCommand { get; }
    public ICommand ShowDetailsCommand { get; }
    public ICommand GenereateToDocxCommand { get; }

    public FilterViewModel(MenuBarViewModel menuBarViewModel, FilterModel filterModel, DataService dataService)
    {
        StudyPrograms = new List<string>();
        ThemesYears = new List<int>();
        Themes = new ObservableCollection<ThemeSupDTO>();

        ClearCommand = new RelayCommand(() => { SelectedStudyProgram = null; SelectedYear = null; });
        ShowDetailsCommand = new RelayCommand(ShowDetails);
        GenereateToDocxCommand = new RelayCommand(() => SaveToDocx());
        EnableButtons = false;

        MenuBarViewModel = menuBarViewModel;

        _filterModel = filterModel;
        _dataService = dataService;

        InitializeAsync();
    }

    private void ShowDetails()
    {
        var selectedST = SelectedStudyProgram;
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

        int themeId = SelectedTheme.Id;

        byte[] fileBytes = await _dataService.LoadDataAsync<byte[]>($"https://localhost:7066/theme/theme2docx/{themeId}");
        await File.WriteAllBytesAsync(sfd.FileName, fileBytes);

        Process.Start(new ProcessStartInfo
        {
            FileName = sfd.FileName,
            UseShellExecute = true
        });
    }

    private async Task InitializeAsync()
    {
        await LoadStudyPrograms();
        await LoadThemseYears();
        await LoadAllThemes("https://localhost:7066/theme/themes");
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

    private async Task LoadAllThemes(string apiURL)
    {
        var themesResponse = await _dataService.LoadDataAsync<List<ThemeSupDTO>>(apiURL);
        
        if (themesResponse != null)
        {
            foreach (var theme in themesResponse)
                Themes.Add(theme);            
        }

        OnPropertyChanged(nameof(Count));
    }

    private async Task LoadThemseYears()
    {
        var yearsResponse = await _dataService.LoadDataAsync<List<int>>("https://localhost:7066/theme/themesyears");
        ThemesYears.AddRange(yearsResponse);
    }

    private async Task LoadStudyPrograms()
    {
        var studyPrograms = await _dataService.LoadDataAsync<List<StProgram>>("https://localhost:7066/stprograms");
        _programList = studyPrograms;

        if (studyPrograms != null)
        {
            StudyPrograms.Clear();
            StudyPrograms.AddRange(studyPrograms.Select(s => s.Name));
        }
    }
}
