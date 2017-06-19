using System;
using System.Collections.Generic;
using System.IO;

namespace csharp__played_games_parser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var readText = File.ReadAllText("../../../2017-06-08.txt");
            Console.WriteLine(readText);
        }
    }
}