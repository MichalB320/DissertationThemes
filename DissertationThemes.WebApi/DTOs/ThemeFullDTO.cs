using DissertationThemes.SharedLibrary;

namespace DissertationThemes.WebApi.DTOs;

public class ThemeFullDTO
{
    public DateTime Created { get; set; }
    public string Description { get; set; }
    public int Id { get; set; }
    public bool IsExternalStudy { get; set; }
    public bool IsFullTimeStudy { get; set; }
    public string Name { get; set; }
    public ResearchType ResearchType { get; set; }
    public string FullName { get; set; }
    public string StProgram { get; set; }
    public string FieldOfStudy { get; set; }
}
