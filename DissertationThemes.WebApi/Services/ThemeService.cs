using DissertationThemes.ImporterApp;
using DissertationThemes.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;

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
                Created = theme.Created,
                Description = theme.Description,
                Id = theme.Id,
                IsExternalStudy = theme.IsExternalStudy,
                IsFullTimeStudy = theme.IsFullTimeStudy,
                Name = theme.Name
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

        using (var db = new DisertationThemesDbContext())
        {
            var themes = db.Themes.Include(p => p.Supervisor).AsQueryable();

            if (year.HasValue)
                themes = themes.Include(p => p.Supervisor).Where(p => p.Created.Year == year);

            if (stProgramId.HasValue)
                themes = themes.Include(p => p.Supervisor).Where(p => p.StProgramId == stProgramId);

            foreach (var theme in themes)
            {
                ThemeSupDTO themeSupDTO = new ThemeSupDTO()
                {
                    Created = theme.Created,
                    Description = theme.Description,
                    Id = theme.Id,
                    IsExternalStudy = theme.IsExternalStudy,
                    IsFullTimeStudy = theme.IsFullTimeStudy,
                    Name = theme.Name,
                    FullName = theme.Supervisor.FullName
                };

                list.Add(themeSupDTO);
            }

            return list;
        }
    }

    public IEnumerable<ThemeFullDTO> GetAllThemes(int? year, int? stProgramId)
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

            return list;
        }
    }
}
