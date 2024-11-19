using DissertationThemes.ImporterApp;
using DissertationThemes.SharedLibrary;
using DissertationThemes.WebApi.DTOs;

namespace DissertationThemes.WebApi.Services;

public class StProgramService
{
    private readonly List<StProgramDTO> _stPrograms; //= new List<StProgramDTO>
        //{
        //    new StProgramDTO { Id = 1, Name = "aplikovaná informatika", FieldOfStudy = "9.2.9 aplikovaná informatika" },
        //    new StProgramDTO { Id = 3, Name = "inteligentné informačné systémy", FieldOfStudy = "9.2.6 informačné systémy" },
        //    new StProgramDTO { Id = 4, Name = "manažment", FieldOfStudy = "3.3.15 manažment" }
        //};

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
    {
        return _stPrograms.OrderBy(sp => sp.Name);
    }
}
