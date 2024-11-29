using CommunityToolkit.Mvvm.Input;
using DissertationThemes.SharedLibrary;
using System.Windows.Input;

namespace DissertationThemes.ViewerApp.ViewModels;

public class DetailViewModel : ViewModelBase
{
    public event Action CloseWindowRequest;
    public ICommand CloseCommand { get; }

    public int Id { get; }
    public string Name { get; }
    public string Supervisor { get; }
    public string StudyProgram { get; }
    public bool IsFullTimeStudy { get; }
    public bool IsExternalStudy { get; }
    public string Description { get; set; }
    public DateTime Created { get; }

    public DetailViewModel(Theme theme)
    {
        CloseCommand = new RelayCommand(OnCloseWindows);
        Id = theme.Id;
        Name = theme.Name;
        //Supervisor = theme.Supervisor.FullName;
        //StudyProgram = theme.StProgram.Name;
        IsFullTimeStudy = theme.IsFullTimeStudy;
        IsExternalStudy = theme.IsExternalStudy;
        Description = theme.Description;
        Created = theme.Created;
    }

    private void OnCloseWindows()
    {
        CloseWindowRequest?.Invoke();
    }
}
