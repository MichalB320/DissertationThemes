using DissertationThemes.ImporterApp;
using DissertationThemes.WebApi.DTOs;

namespace DissertationThemes.WebApi.Services;

public class StProgramService
{
    private readonly List<StProgramDTO> _stPrograms;

    public StProgramService()
    {
        _stPrograms = new List<StProgramDTO>();

        using (var db = new DisertationThemesDbContext())
        {
            var collection = db.StPrograms;
            foreach (var item in collection)
            {
                if (_stPrograms.Where(program => program.Name == item.Name).Count() == 0)
                {
                    StProgramDTO dto = new StProgramDTO()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        FieldOfStudy = item.FieldOfStudy
                    };

                    _stPrograms.Add(dto);
                }
            }
        }
    }

    public IEnumerable<StProgramDTO> GetAll()
        => _stPrograms.OrderBy(sp => sp.Name);
}
