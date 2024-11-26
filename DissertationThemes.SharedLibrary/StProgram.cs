using System.Text.Json.Serialization;

namespace DissertationThemes.SharedLibrary;

public class StProgram
{
    [JsonPropertyName("fieldOfStudy")]
    public string FieldOfStudy { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public List<Theme> Themes { get; set; }
}
