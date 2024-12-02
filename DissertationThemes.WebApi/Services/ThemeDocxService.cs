using DissertationThemes.ImporterApp;
using DissertationThemes.SharedLibrary;
using DissertationThemes.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;
using Xceed.Words.NET;

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

    private ThemeDTO GetTheme()
    {
        return new ThemeDTO()
        {
            Created = _theme.Created,
            Description = _theme.Description,
            Id = _theme.Id,
            IsExternalStudy = _theme.IsExternalStudy,
            IsFullTimeStudy = _theme.IsFullTimeStudy,
            ResearchType = _theme.ResearchType,
            Name = _theme.Name
        };
    }

    private StProgramDTO GetStProgram()
    {
        return new StProgramDTO()
        {
            Id = _theme.StProgram.Id,
            Name = _theme.StProgram.Name,
            FieldOfStudy = _theme.StProgram.FieldOfStudy
        };
    }

    private SupervisorDTO GetSupervisor()
    {
        return new SupervisorDTO()
        {
            Id = _theme.Supervisor.Id,
            FullName = _theme.Supervisor.FullName
        };
    }

    public byte[] GetThemeDocx()
    {
        var themeDTO = GetTheme();
        var supervisorDTO = GetSupervisor();
        var stProgramDTO = GetStProgram();

        string outputPath = Path.GetTempFileName();

        System.IO.File.Copy("C:\\Users\\micha\\Downloads\\PhD_temy_sablona.docx", outputPath, true);

        using (DocX document = DocX.Load(outputPath))
        {
            document.ReplaceText("#=ThemeName=#", themeDTO.Name);
            document.ReplaceText("#=Supervisor=#", supervisorDTO.FullName);
            document.ReplaceText("#=StProgram=#", stProgramDTO.Name);
            document.ReplaceText("#=FieldOfStudy=#", stProgramDTO.FieldOfStudy);
            switch (themeDTO.ResearchType)
            {
                case ResearchType.AppliedResearch:
                    document.ReplaceText("#=ResearchType=#", "aplikovaný výskum");
                    break;
                case ResearchType.AppliedResearchExpDevelopment:
                    document.ReplaceText("#=ResearchType=#", "aplikovaný výskum a experimentálny vývoj");
                    break;
                case ResearchType.BasicResearch:
                    document.ReplaceText("#=ResearchType=#", "základný výskum");
                    break;
            }
            document.ReplaceText("#=ResearchType=#", themeDTO.ResearchType.ToString());
            document.ReplaceText("#=Description=#", themeDTO.Description);

            string modifiedFilePath = Path.Combine(Path.GetTempPath(), $"modified_theme_{themeDTO.Id}.docx");
            document.SaveAs(modifiedFilePath);

            byte[] fileBytes = System.IO.File.ReadAllBytes(modifiedFilePath);

            return fileBytes;
        } 
    }
}
