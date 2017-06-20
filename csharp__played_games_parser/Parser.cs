using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace csharp__played_games_parser
{
    /// <summary>
    /// Класс для парсинга списка игр.
    /// </summary> 
    public class Parser
    {
        public const string FINISHED_GAME = "FINISHED_GAME";
        public const string NOT_FINISHED_GAME = "NOT_FINISHED_GAME";
        public const string FINISHED_WATCHED = "FINISHED_WATCHED";
        public const string NOT_FINISHED_WATCHED = "NOT_FINISHED_WATCHED";

        /// Регулярка вытаскивает выражения вида: 1, 2, 3 или 1-3, или римские цифры: III, IV
//        private const Pattern PARSE_GAME_NAME_PATTERN = Pattern.compile("(\\d+(, *?\\d+)+)|(\\d+ *?- *?\\d+)|([MDCLXVI]+(, ?[MDCLXVI]+)+)", Pattern.CASE_INSENSITIVE);
        private static Regex PARSE_GAME_NAME_PATTERN = new Regex(@"(\d+(, *?\d+)+)|(\d+ *?- *?\d+)|([MDCLXVI]+(, ?[MDCLXVI]+)+)", RegexOptions.Compiled);
        
        /// <summary>
        /// Функция парсит переданный текст с списком игр.
        /// </summary>         
        /// <param name="text">Текст с списком игр</param>
        /// <returns>Словарь платформ с словарем категорий с списком игр.</returns>
        public static Dictionary<string, IDictionary<string, List<string>>> Parse(string text)
        {
            var lines = text.Split('\n');

            var platforms = new Dictionary<string, IDictionary<string, List<string>>>();
            IDictionary<string, List<string>> platform = null;

            foreach (var l in lines)
            {
                var line = l.TrimEnd();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var hasFlag1 = " -@".Contains(line[0].ToString());
                var hasFlag2 = " -@".Contains(line[1].ToString());

                if (!hasFlag1 && !hasFlag2 && line.EndsWith(":"))
                {
                    var platformName = line.Substring(0, line.Length - 1);

                    platform = new Dictionary<string, List<string>>
                    {
                        [FINISHED_GAME] = new List<string>(),
                        [NOT_FINISHED_GAME] = new List<string>(),
                        [FINISHED_WATCHED] = new List<string>(),
                        [NOT_FINISHED_WATCHED] = new List<string>()
                    };

                    platforms[platformName] = platform;

                    continue;
                }

                if (platform == null)
                {
                    continue;
                }

                var flag = line.Substring(0, 2);

                string categoryName = null;

                switch (flag)
                {
                    case "  ":
                        categoryName = FINISHED_GAME;
                        break;
                        
                    case " -":
                    case "- ":
                        categoryName = NOT_FINISHED_GAME;
                        break;
                        
                    case " @":
                    case "@ ":
                        categoryName = FINISHED_WATCHED;
                        break;
                        
                    case "@-":
                    case "-@":
                        categoryName = NOT_FINISHED_WATCHED;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid line format: \"{0}\"", line);
                        continue;
                }

                var category = platform[categoryName];

                var gameName = line.Substring(2);
                foreach (var game in ParseGameName(gameName))
                {
                    if (category.Contains(game))
                    {
                        Console.WriteLine("Preventing the addition of a duplicate game \"{0}\"", game);
                        continue;
                    }

                    category.Add(game);
                }
            }

            return platforms;
        }

        /// <summary>
        /// Функция принимает название игры и пытается разобрать его, после возвращает список названий.
        /// У некоторых игр в названии может указываться ее части или диапазон частей, поэтому для правильного
        /// составления списка игр такие случаи нужно обрабатывать.
        /// </summary>
        /// <example>
        /// <code>
        /// "Resident Evil 4, 5, 6" -> ["Resident Evil 4", "Resident Evil 5", "Resident Evil 6"]
        /// "Resident Evil 1-3"     -> ["Resident Evil", "Resident Evil 2", "Resident Evil 3"]
        /// "Resident Evil 4"       -> ["Resident Evil 4"]
        /// </code>
        /// </example>
        /// <param name="gameName">Название игры</param>
        /// <returns>Список названий игр.</returns>
        private static List<string> ParseGameName(string gameName)
        {
            var match = PARSE_GAME_NAME_PATTERN.Match(gameName);
            if (!match.Success)
            {
                return new List<string>() { gameName };
            }

            var seqStr = match.Value;
            
            // "Resident Evil 4,  5,   6" -> "Resident Evil"
            // For not valid "Trollface Quest 1-7-8" -> "Trollface Quest"
            var index = gameName.IndexOf(seqStr);
            var baseName = gameName.Substring(0, index).Trim();

            // "4,  5,   6" -> "4,5,6"
            seqStr = seqStr.Replace(" ", "");
            
            var seq = new List<string>();

            if (seqStr.Contains(",")) {
                seq.AddRange(seqStr.Split(','));

            } else if (seqStr.Contains("-")) {
                // "1-7" -> ["1", "7"] -> ["1", "2", "3", "4", "5", "6", "7"]
                var parts = seqStr.Split('-');

                var a = int.Parse(parts[0]);
                var b = int.Parse(parts[1]);

                for (; a <= b; a++) {
                    seq.Add(a.ToString());
                }

            } else {
                return new List<string>() { gameName };
            }
            
            
            var games = new List<string>();
            
            foreach (var num in seq) {
                if (num == "1") {
                    games.Add(baseName);
                } else {
                    // Example: "Resident Evil" + " " + "4"
                    games.Add(baseName + " " + num);
                }
            }

            return games;
        }
    }
}