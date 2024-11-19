using System.CommandLine.NamingConventionBinder;
using System.CommandLine;

namespace DissertationThemes.ImporterApp;

internal class Program
{
    public static async Task Main(string[] args)
    {
        IS importer = new();

        var rootCommand = new RootCommand
            {
                new Argument<string>("argument", "path to the file.") { Arity = ArgumentArity.ZeroOrOne }, 
                new Option<bool>("-r", "remove previous data"),
                new Option<bool>("--remove-previous-data", "remove previous data"),
            };

        rootCommand.Description = "importer app for phd themes";

        rootCommand.Handler = CommandHandler.Create<string, bool, bool>(async (argument, r, removePreviousData) =>
        {
            if (string.IsNullOrEmpty(argument))
            {
                if (r || removePreviousData)
                {
                    Console.Write("Zadaj cestu k súboru: ");
                    argument = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Neboli zadané žiadne argumenty ani voliteľné flagy. Ukončujem program.");
                    return;
                }
            }

            if (r)
            {
                Console.WriteLine($"Flag -r použitý s argumentom: {argument}");
                importer.Delete();
                // Logika pre mazanie dát
            }
            else if (removePreviousData)
            {
                Console.WriteLine($"Flag --remove-previous-data použitý s argumentom: {argument}");
                importer.Delete();
                // Logika pre mazanie dát
            }
            else
            {
                Console.WriteLine($"Spracovávam súbor na ceste: {argument}");
                importer.ReadData(argument);
                // Logika pre spracovanie dát zo súboru
            }
        });

        await rootCommand.InvokeAsync(args);

        importer.ShowAll();
    }
}
