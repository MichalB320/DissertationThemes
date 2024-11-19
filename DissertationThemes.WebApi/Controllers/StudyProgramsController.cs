using DissertationThemes.WebApi.DTOs;
using DissertationThemes.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DissertationThemes.WebApi.Controllers;

[ApiController]
[Route("stprograms")]
public class StudyProgramsController : ControllerBase
{
    private readonly StProgramService _stProgramService;

    public StudyProgramsController()
    {
        _stProgramService = new StProgramService();
    }

    [HttpGet]
    public ActionResult<IEnumerable<StProgramDTO>> GetStudyPrograms()
    {
        var studyPrograms = _stProgramService.GetAll();
        return Ok(studyPrograms);
    }

}
