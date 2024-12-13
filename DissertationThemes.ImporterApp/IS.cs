using DissertationThemes.SharedLibrary;
using System.Text.RegularExpressions;

namespace DissertationThemes.ImporterApp;

public class IS
{

    public void ReadData(string path)
    {
        using var db = new DisertationThemesDbContext();
        using StreamReader sr = new StreamReader(path);
        var zeroLine = sr.ReadLine();

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var colls = line.Split(';');

            StProgram stp = CreateStProgram(colls[3], colls[2]);
            Supervisor supervisor = CreateSupervisor(colls[1]);
            Theme theme = CreateTheme(colls[8], colls[7], bool.Parse(colls[5]), bool.Parse(colls[4]), colls[0], stp, supervisor, colls[6]);

            if (db.Themes.FirstOrDefault(t => t.Name == theme.Name && t.StProgram.Id == stp.Id && t.Supervisor.Id == supervisor.Id) == null)
                db.Themes.Add(theme);

            db.SaveChanges();
        }
    }

    private Supervisor CreateSupervisor(string fullName)
    {
        Supervisor supervisor = new Supervisor
        {
            FullName = fullName
        };

        return supervisor;
    }

    private StProgram CreateStProgram(string fieldOfStudy, string stProgram)
    {
        StProgram stp = new StProgram
        {
            FieldOfStudy = fieldOfStudy,
            Name = stProgram
        };

        return stp;
    }

    private Theme CreateTheme(string paCreated, string description, bool isExternalStudy, bool isFullTimeStudy, string paName, StProgram stp, Supervisor supervisor, string researchType)
    {
        var created = paCreated;
        var dateTime = created.Split(' ');
        var dateOnly = dateTime[0].Split('.');
        var timeOnly = dateTime[1].Split(':');
        int day = int.Parse(dateOnly[0]);
        int month = int.Parse(dateOnly[1]);
        int year = int.Parse(dateOnly[2]);
        int hour = int.Parse(timeOnly[0]);
        int minute = int.Parse(timeOnly[1]);

        Theme theme = new Theme
        {
            Created = new DateTime(new DateOnly(year, month, day), new TimeOnly(hour, minute)),
            Description = Regex.Replace(description, "<br>", Environment.NewLine), //description,
            IsExternalStudy = isExternalStudy,
            IsFullTimeStudy = isFullTimeStudy,
            Name = paName,
            StProgram = stp,
            Supervisor = supervisor
        };

        switch (researchType)
        {
            case "základný výskum":
                theme.ResearchType = ResearchType.BasicResearch;
                break;
            case "aplikovaný výskum":
                theme.ResearchType = ResearchType.AppliedResearch;
                break;
            case "aplikovaný výskum a experimentálny vývoj":
                theme.ResearchType = ResearchType.AppliedResearchExpDevelopment;
                break;
        }

        return theme;
    }

    public void Delete()
    {
        using (var dbContext = new DisertationThemesDbContext())
        {
            var stPrograms = dbContext.StPrograms;
            stPrograms.RemoveRange(stPrograms);

            var supervisors = dbContext.Supervisors;
            supervisors.RemoveRange(supervisors);
                
            var themes = dbContext.Themes;
            themes.RemoveRange(themes);

            dbContext.SaveChanges();

            //dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE StPrograms;");
            //dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Supervisors;");
            //dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Themes;");

            dbContext.SaveChanges();
        }
    }

    public void ShowAll()
    {
        using (var db = new DisertationThemesDbContext())
        {
            var stPrograms = db.StPrograms;
            foreach (var stProgram in stPrograms)
            {
                Console.WriteLine($"{stProgram.Name} {stProgram.Id}");
            }

            var supervisors = db.Supervisors;
            foreach (var supervisor in supervisors)
            {
                Console.WriteLine($"{supervisor.FullName} {supervisor.Id}");
            }

            var themes = db.Themes;
            foreach (var theme in themes)
            {
                Console.WriteLine($"{theme.Name} {theme.Id}");
            }

            Console.WriteLine(db.StPrograms.Count());
            Console.WriteLine(db.Supervisors.Count());
            Console.WriteLine(db.Themes.Count());

            var themesss = db.Themes.Where(p => p.StProgram.Name == "aplikovaná informatika").ToList().Count();
            Console.WriteLine(themesss);
        }
    }
}
