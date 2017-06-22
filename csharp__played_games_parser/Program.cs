using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace csharp__played_games_parser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("From local file:");
            var textLocalFile = GetTextFromLocalFile();
            PrintText(textLocalFile);

            Console.WriteLine("\n\n");

            Console.WriteLine("From url file:");
            var textUrlFile = GetTextFromUrlFile();
            PrintText(textUrlFile);
        }

        static string GetTextFromLocalFile()
        {
            return File.ReadAllText("../../../2017-06-08.txt");
        }

        static string GetTextFromUrlFile()
        {
            using(var client = new WebClient())
            {
                const string urlGist = "https://gist.github.com/gil9red/2f80a34fb601cd685353";;
                var html = client.DownloadString(urlGist);
                
                var pattern = new Regex(@"/gil9red/2f80a34fb601cd685353/raw/.+/gistfile1.txt");
                var match = pattern.Match(html);
                if (!match.Success)
                {
                    return null;
                }

                var url = "https://gist.github.com" + match.Value;
                return client.DownloadString(url);
            }
        }

        static void PrintText(string text)
        {
            var platforms = Parser.Parse(text);
            Console.WriteLine("Platforms: " + platforms.Count);
                
            var totalGames = platforms.Values.SelectMany(categories => categories.Values).Sum(games => games.Count);
            Console.WriteLine("Games: " + totalGames);
            Console.WriteLine();
            Console.WriteLine(string.Join(", ", platforms.Keys.ToArray()));
            Console.WriteLine();
            Console.WriteLine("PS 2 / " + Parser.FINISHED_GAME);
            foreach (var game in platforms["PS 2"][Parser.FINISHED_GAME])
            {
                Console.WriteLine(game);
            }
            
            Console.WriteLine();
            Console.WriteLine("Sega / " + Parser.FINISHED_GAME);
            foreach (var game in platforms["Sega"][Parser.FINISHED_GAME])
            {
                Console.WriteLine(game);
            }
        }
    }
}