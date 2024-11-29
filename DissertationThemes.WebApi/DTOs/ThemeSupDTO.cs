using DissertationThemes.SharedLibrary;

namespace DissertationThemes.WebApi.DTOs;

public class ThemeSupDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public int StProgramId { get; set; }
    public bool IsFullTimeStudy { get; set; }
    public bool IsExternalStudy { get; set; }
    public ResearchType ResearchType { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    
    
}
