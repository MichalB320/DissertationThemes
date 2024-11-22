using DissertationThemes.ImporterApp;
using DissertationThemes.SharedLibrary;
using DissertationThemes.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DissertationThemes.WebApi.Services;

public class ThemeDocxService
{
    private readonly Theme _theme;
    private readonly StProgram _stProgram;
    private readonly Supervisor _supervisor;

    public ThemeDocxService(int id) 
    {
        using (var db = new DisertationThemesDbContext())
        {
            var theme = db.Themes.Include(t => t.StProgram).Include(t => t.Supervisor).FirstOrDefault(p => p.Id == id);

            _theme = theme;
            _supervisor = theme.Supervisor;
            _stProgram = theme.StProgram;
        }
    }

    public ThemeDTO GetTheme()
    {
        return new ThemeDTO()
        {
            Created = _theme.Created,
            Description = _theme.Description,
            Id = _theme.Id,
            IsExternalStudy = _theme.IsExternalStudy,
            IsFullTimeStudy = _theme.IsFullTimeStudy,
            Name = _theme.Name
        };
    }

    public StProgramDTO GetStProgram()
    {
        return new StProgramDTO()
        {
            Id = _theme.StProgram.Id,
            Name = _theme.StProgram.Name,
            FieldOfStudy = _theme.StProgram.FieldOfStudy
        };
    }

    public SupervisorDTO GetSupervisor()
    {
        return new SupervisorDTO()
        {
            Id = _theme.Supervisor.Id,
            FullName = _theme.Supervisor.FullName
        };
    }
}
