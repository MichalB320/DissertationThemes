using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Xceed.Words.NET;

namespace DissertationThemes.WebApi.Controllers;

[ApiController]
[Route("theme2docx")]
public class ThemeDocxController : ControllerBase
{



    [HttpGet("{id}")]
    public ActionResult GetThemeDocx(int id)
    {
        var themeDocxService = new ThemeDocxService(id);
        var themeDTO = themeDocxService.GetTheme();
        var supervisorDTO = themeDocxService.GetSupervisor();
        var stProgramDTO = themeDocxService.GetStProgram();

        string outputPath = Path.GetTempFileName();

        System.IO.File.Copy("C:\\Users\\micha\\Downloads\\PhD_temy_sablona.docx", outputPath, true);

        using (DocX document = DocX.Load(outputPath))
        {
            document.ReplaceText("#=ThemeName=#", themeDTO.Name);
            document.ReplaceText("#=Supervisor=#", supervisorDTO.FullName);
            document.ReplaceText("#=StProgram=#", stProgramDTO.Name);
            document.ReplaceText("#=FieldOfStudy=#", stProgramDTO.FieldOfStudy);
            //#=ResearchType=#
            document.ReplaceText("#=Description=#", themeDTO.Description);

            string modifiedFilePath = Path.Combine(Path.GetTempPath(), "modified_theme_233.docx");
            document.SaveAs(modifiedFilePath);

            byte[] fileBytes = System.IO.File.ReadAllBytes(modifiedFilePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "theme_233.docx");
        }
    }
} 
