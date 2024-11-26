using DissertationThemes.SharedLibrary;
using System.Net.Http;
using System.Text.Json;

namespace DissertationThemes.ViewerApp.ViewModels;

public class FilterViewModel : ViewModelBase
{
    private List<string> _studyPrograms;
    public List<string> StudyPrograms
    {
        get => _studyPrograms;
        set
        {
            _studyPrograms = value;
            OnPropertyChanged(nameof(StudyPrograms));
        }
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


    public FilterViewModel()
    {
        _studyPrograms = new List<string>();
        _themesYears = new List<int>();

        Task.Run(async () => await LoadStudyPrograms());
        Task.Run(async () => await LoadThemseYears());
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
