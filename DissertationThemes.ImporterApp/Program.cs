using System.CommandLine.NamingConventionBinder;
using System.CommandLine;

namespace DissertationThemes.ImporterApp;

internal class Program
{
    public static async Task Main(string[] args)
    {
        try
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
                        Console.Write("Zadaj cestu k suboru: ");
                        argument = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Neboli zadane ziadne argumenty ani volitelne flagy. Ukoncujem program.");
                        return;
                    }
                }

                if (r)
                {
                    Console.WriteLine($"Flag -r pouzity s argumentom: {argument}");
                    importer.Delete();
                }
                else if (removePreviousData)
                {
                    Console.WriteLine($"Flag --remove-previous-data pouzity s argumentom: {argument}");
                    importer.Delete();
                }
                else
                {
                    Console.WriteLine($"Spracovavam subor na ceste: {argument}");
                    importer.ReadData(argument);
                }
            });

            await rootCommand.InvokeAsync(args);

            //importer.ShowAll();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
