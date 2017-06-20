using System;
using System.IO;
using System.Linq;

namespace csharp__played_games_parser
{
    internal class Program
    {
        public static void Main(string[] args)
        {   
            var readText = File.ReadAllText("../../../2017-06-08.txt");
            
            var platforms = Parser.Parse(readText);
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