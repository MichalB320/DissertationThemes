using DissertationThemes.SharedLibrary;
using DissertationThemes.WebApi.DTOs;
using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers;

[ApiController]
[Route("theme/")]
public class ThemesController : ControllerBase
{
    private readonly ThemeService _service;

    public ThemesController()
    {
        _service = new ThemeService();
    }

    [HttpGet("{id}")]
    public ActionResult<ThemeDTO> GetTheme(int id)
    {
        ThemeDTO themeDto = _service.GetThemeById(id);
        return Ok(themeDto);
    }

    [HttpGet("theme2docx/{id}")]
    public ActionResult GetThemeDocx(int id)
    {
        var themeDocxService = new ThemeDocxService(id);
        byte[] fileBytes = themeDocxService.GetThemeDocx();

        return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "theme_233.docx");
    }

    [HttpGet("themes/")]
    public ActionResult<IEnumerable<Theme>> GetThemes([FromQuery] int? year, [FromQuery] int? stProgramId)
    {
        IEnumerable<ThemeSupDTO> themes = _service.GetThemes(year, stProgramId);
        return Ok(themes);
    }

    [HttpGet("themes2csv/")]
    public ActionResult GetThemes2csv([FromQuery] int? year, [FromQuery] int? stProgramId)
    {
        byte[] csvBytes = _service.GetAllThemes(year, stProgramId);
        string fileName = "themes.csv";

        return File(csvBytes, "text/csv", fileName);
    }

    [HttpGet("themesyears")]
    public ActionResult<IEnumerable<int>> ThemesYears()
    {
        IEnumerable<int> themes = _service.GetThemes().Select(p => p.Created.Year);
        return Ok(themes);
    }
}
