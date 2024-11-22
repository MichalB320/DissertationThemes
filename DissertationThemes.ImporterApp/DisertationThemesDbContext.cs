using DissertationThemes.SharedLibrary;
using Microsoft.EntityFrameworkCore;

namespace DissertationThemes.ImporterApp
{
    public class DisertationThemesDbContext : DbContext
    {
        public DbSet<Theme> Themes { get; set; }
        public DbSet<StProgram> StPrograms { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=C:\\Users\\micha\\source\\repos\\DissertationThemes\\DissertationThemes.ImporterApp\\DisertationThemes.db");
    }
}
