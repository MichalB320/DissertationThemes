using System.Text.Json.Serialization;

namespace DissertationThemes.SharedLibrary;

public class Theme
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }


    [JsonPropertyName("isFullTimeStudy")]
    public bool IsFullTimeStudy { get; set; }

    [JsonPropertyName("isExternalStudy")]
    public bool IsExternalStudy { get; set; }

    [JsonPropertyName("researchType")]
    public ResearchType ResearchType { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    
    public StProgram StProgram { get; set; }

    [JsonPropertyName("stProgramId")]
    public int StProgramId { get; set; }

    public Supervisor Supervisor { get; set; }
    public int SupervisorId { get; set; }
}
