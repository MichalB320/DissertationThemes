using DissertationThemes.SharedLibrary;

namespace DissertationThemes.WebApi.DTOs;

public class ThemeDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }


    public bool IsFullTimeStudy { get; set; }
    public bool IsExternalStudy { get; set; }
    public ResearchType ResearchType { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    

    public StProgramDTO? StProgramDTO { get; set; }
    public SupervisorDTO? SupervisorDTO { get; set; }
}
