using DissertationThemes.ImporterApp;
using DissertationThemes.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DissertationThemes.WebApi.Services;

public class ThemeService
{

    public ThemeService() 
    {
    
    }

    public ThemeDTO GetThemeById(int id)
    {
        using (var db = new DisertationThemesDbContext())
        {
            var theme = db.Themes.FirstOrDefault(p => p.Id == id);

            if (theme == null)
                return null;

            return new ThemeDTO()
            {
                Id = theme.Id,
                Name = theme.Name,
                //TODO: supervisor
                //TODO: supervisorId
                IsFullTimeStudy = theme.IsFullTimeStudy,
                IsExternalStudy = theme.IsExternalStudy,
                ResearchType = theme.ResearchType,
                Description = theme.Description,
                Created = theme.Created
            };
        }
    }

    public IEnumerable<ThemeDTO> GetThemes()
    {
        List<ThemeDTO> list = new List<ThemeDTO>();

        using (var db = new DisertationThemesDbContext())
        {
            var themes = db.Themes;
            foreach (var theme in themes)
            {
                if (list.Where(p => p.Created.Year == theme.Created.Year).Count() == 0)
                {
                    ThemeDTO themeDTO = new ThemeDTO()
                    {
                        Created = theme.Created,
                        Description = theme.Description,
                        Id = theme.Id,
                        IsExternalStudy = theme.IsExternalStudy,
                        IsFullTimeStudy = theme.IsFullTimeStudy,
                        Name = theme.Name
                    };
                    list.Add(themeDTO);
                }
            }
            return list.OrderByDescending(p => p.Created);
        }
    }

    public IEnumerable<ThemeSupDTO> GetThemes(int? year, int? stProgramId)
    {
        List<ThemeSupDTO> list = new List<ThemeSupDTO>();

        StProgramService stProgramService = new StProgramService();
        List<StProgramDTO> stPrograms = stProgramService.GetAll();
        string stProgramName = stPrograms.Where(c => c.Id == stProgramId).Select(s => s.Name).FirstOrDefault();

        using (var db = new DisertationThemesDbContext())
        {
            var themes = db.Themes.Include(p => p.Supervisor).Include(p => p.StProgram).AsQueryable();

            if (year.HasValue)
                themes = themes.Include(p => p.Supervisor).Include(p => p.StProgram).Where(p => p.Created.Year == year);

            if (stProgramId.HasValue)
                themes = themes.Include(p => p.Supervisor).Include(p => p.StProgram).Where(p => p.StProgram.Name == stProgramName);

            foreach (var theme in themes)
            {
                ThemeSupDTO themeSupDTO = new ThemeSupDTO()
                {
                    Id = theme.Id,
                    Name = theme.Name,
                    FullName = theme.Supervisor.FullName,
                    StProgramId = stPrograms.Where(c => c.Name == theme.StProgram.Name).Select(s => s.Id).FirstOrDefault(),
                    IsFullTimeStudy = theme.IsFullTimeStudy,
                    IsExternalStudy = theme.IsExternalStudy,
                    ResearchType = theme.ResearchType,
                    Description = theme.Description,
                    Created = theme.Created
                };

                list.Add(themeSupDTO);
            }

            return list;
        }
    }

    public byte[] GetAllThemes(int? year, int? stProgramId)
    {
        List<ThemeFullDTO> list = new List<ThemeFullDTO>();

        using (var db = new DisertationThemesDbContext())
        {
            var themes = db.Themes.Include(p => p.Supervisor).Include(p => p.StProgram).AsQueryable();

            if (year.HasValue)
                themes = themes.Include(p => p.Supervisor).Where(p => p.Created.Year == year);

            if (stProgramId.HasValue)
                themes = themes.Include(p => p.Supervisor).Where(p => p.StProgramId == stProgramId);

            foreach (var theme in themes)
            {
                ThemeFullDTO themeFullDTO = new ThemeFullDTO()
                {
                    Created = theme.Created,
                    Description = theme.Description,
                    Id = theme.Id,
                    IsExternalStudy = theme.IsExternalStudy,
                    IsFullTimeStudy = theme.IsFullTimeStudy,
                    Name = theme.Name,
                    FullName = theme.Supervisor.FullName,
                    StProgram = theme.StProgram.Name,
                    FieldOfStudy =theme.StProgram.FieldOfStudy
                };
                list.Add(themeFullDTO);
            }

            string csv = CreateCsv(list);
            byte[] csvBytes = Encoding.UTF8.GetBytes(csv);

            return csvBytes;
        }
    }

    private string CreateCsv(IEnumerable<ThemeFullDTO> themes)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Name;Supervisor;StProgram;FieldOfStudy;IsFullTimeStudy;IsExternalStudy;ResearchType;Descrition;Created");
        
        foreach (var theme in themes)
            sb.AppendLine($"{theme.Name};{theme.FullName};{theme.Name};{theme.FieldOfStudy};{theme.IsFullTimeStudy};{theme.IsExternalStudy};{theme.ResearchType};{theme.Description};{theme.Created}");

        string csv = sb.ToString();
        return csv;
    }
}
