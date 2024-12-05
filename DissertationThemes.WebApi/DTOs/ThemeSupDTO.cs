using DissertationThemes.SharedLibrary;
using System.Text.Json.Serialization;

namespace DissertationThemes.WebApi.DTOs;

public class ThemeSupDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }
    [JsonPropertyName("stProgramId")]
    public int StProgramId { get; set; }
    [JsonPropertyName("isFullTimeStudy")]
    public bool IsFullTimeStudy { get; set; }
    [JsonPropertyName("isExternalStudy")]
    public bool IsExternalStudy { get; set; }
    [JsonPropertyName("researchType")]
    public ResearchType ResearchType { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("created")]
    public DateTime Created { get; set; }
    
    
}
