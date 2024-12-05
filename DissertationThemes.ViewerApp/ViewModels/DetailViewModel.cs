using CommunityToolkit.Mvvm.Input;
using DissertationThemes.SharedLibrary;
using DissertationThemes.WebApi.DTOs;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;

namespace DissertationThemes.ViewerApp.ViewModels;

public class DetailViewModel : ViewModelBase
{
    public event Action CloseWindowRequest;
    public ICommand CloseCommand { get; }

    public int Id { get; }
    public string Name { get; }
    public string Supervisor { get; }
    public string StudyProgram { get; private set; }
    public bool IsFullTimeStudy { get; }
    public bool IsExternalStudy { get; }
    public ResearchType ResearchTypeP { get; }
    public string Description { get; set; }
    public DateTime Created { get; }

    public DetailViewModel(ThemeSupDTO theme)
    {
        CloseCommand = new RelayCommand(OnCloseWindows);
        Id = theme.Id;
        Name = theme.Name;
        Supervisor = theme.FullName;
        SetStudyProgram(theme.StProgramId);
        IsFullTimeStudy = theme.IsFullTimeStudy;
        IsExternalStudy = theme.IsExternalStudy;
        Description = theme.Description;
        Created = theme.Created;
        ResearchTypeP = theme.ResearchType;
    }

    private async void SetStudyProgram(int stId)
    {
        string apiURL = "https://localhost:7066/stprograms";

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(apiURL);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            List<StProgram> stPrograms = JsonSerializer.Deserialize<List<StProgram>>(jsonResponse);

            var stProgram = stPrograms.Where(c => c.Id == stId).FirstOrDefault();

            StudyProgram = $"{stProgram.Name} ({stProgram.FieldOfStudy})";
            OnPropertyChanged(nameof(StudyProgram));
        }
    }

    private void OnCloseWindows()
    {
        CloseWindowRequest?.Invoke();
    }
}
