using DissertationThemes.SharedLibrary;
using DissertationThemes.WebApi.DTOs;
using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DissertationThemes.WebApi.Controllers;

[ApiController]
public class ThemesController : ControllerBase
{

    public ThemesController()
    {

    }

    [HttpGet("theme/{id}")]
    public ActionResult<ThemeDTO> GetTheme(int id)
    {
        ThemeService service = new ThemeService();

        ThemeDTO themeDto = service.GetThemeById(id);
        return Ok(themeDto);
    }

    [HttpGet("themes/")]
    public ActionResult<IEnumerable<Theme>> GetThemes([FromQuery] int? year, [FromQuery] int? stProgramId)
    {
        ThemeService service = new ThemeService();
        IEnumerable<ThemeSupDTO> themes = service.GetThemes(year, stProgramId);

        return Ok(themes);
    }

    [HttpGet("themes2csv/")]
    public ActionResult GetThemes2csv([FromQuery] int? year, [FromQuery] int? stProgramId)
    {
        ThemeService service = new ThemeService();

        IEnumerable<ThemeFullDTO> themes = service.GetAllThemes(year, stProgramId);

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Name;Supervisor;StProgram;FieldOfStudy;IsFullTimeStudy;IsExternalStudy;ResearchType;Descrition;Created");
        foreach (var theme in themes)
        {
            sb.AppendLine($"{theme.Name};{theme.FullName};{theme.Name};{theme.FieldOfStudy};{theme.IsFullTimeStudy};{theme.IsExternalStudy};{theme.ResearchType};{theme.Description};{theme.Created}");
        }
        string csv = sb.ToString();

        Byte[] csvBytes = Encoding.UTF8.GetBytes(csv);
        string fileName = "themes.csv";

        return File(csvBytes, "text/csv", fileName);
    }

    [HttpGet("themesyears")]
    public ActionResult<IEnumerable<int>> ThemesYears()
    {
        ThemeService themeService = new ThemeService();
        IEnumerable<int> themes = themeService.GetThemes().Select(p => p.Created.Year);
        return Ok(themes);
    }
}
