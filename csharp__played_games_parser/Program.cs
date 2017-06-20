using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace csharp__played_games_parser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
//            * "Resident Evil 4, 5, 6" -> ["Resident Evil 4", "Resident Evil 5", "Resident Evil 6"]
//            * "Resident Evil 1-3"     -> ["Resident Evil", "Resident Evil 2", "Resident Evil 3"]
//            * "Resident Evil 4"       -> ["Resident Evil 4"]
            Console.WriteLine(string.Join("\t", Parser.ParseGameName("Resident Evil 4, 5, 6").ToArray()));
            Console.WriteLine(string.Join("\t", Parser.ParseGameName("Resident Evil 4,   5,    6").ToArray()));
            Console.WriteLine(string.Join("\t", Parser.ParseGameName("Resident Evil 1-3").ToArray()));
            Console.WriteLine(string.Join("\t", Parser.ParseGameName("Resident Evil 1 -  3").ToArray()));
            Console.WriteLine(string.Join("\t", Parser.ParseGameName("Resident Evil 4").ToArray()));
         
            
//            var readText = File.ReadAllText("../../../2017-06-08.txt");
////            Console.WriteLine(readText);
//            
//            Dictionary<string, IDictionary<string, List<string>>> platforms = Parser.Parse(readText);
//            Console.WriteLine("Platforms: " + platforms.Count);
//                
//            var totalGames = platforms.Values.SelectMany(categories => categories.Values).Sum(games => games.Count);
//            Console.WriteLine("Games: " + totalGames);
//            Console.WriteLine();
//            Console.WriteLine(string.Join(", ", platforms.Keys.ToArray()));
////            Console.WriteLine(platforms);
        }
    }
}