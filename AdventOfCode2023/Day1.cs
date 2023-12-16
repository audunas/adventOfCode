using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day1
    {

        public Day1()
        {
            Console.WriteLine("Day1");

            var lines = File.ReadLines(@"..\..\..\input\day1.txt");

            //Part 1

            var sum = 0;
            //foreach (var line in lines)
            //{
            //    var j = "";
            //    foreach (var c in line)
            //    {
            //        if (int.TryParse(c.ToString(), out int res))
            //        {
            //            j += c;
            //        }
            //    }
            //    j = j.First().ToString() + j.Last().ToString();
            //    if (j != "")
            //    {
            //        sum += int.Parse(j);
            //    }
                
            //}

            //Console.WriteLine(sum);

            //Part 2

            var words = new Dictionary<string, int>() { { "one", 1 }, { "two" , 2}, { "three" , 3}, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } };

            List<string> matchingWords = words.Keys.ToList();
            string pattern = string.Join("|", matchingWords.Select(w => Regex.Escape(w)));
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Regex backReges = new Regex(pattern, RegexOptions.RightToLeft);

            sum = 0;
            foreach (var line in lines)
            {
                var j = "";
                var valIndices = line.Select((c, i) => int.TryParse(c.ToString(), out int v) ? i : -1).Where(t => t != -1);
                var matches = regex.Matches(line);
                var backMatches = backReges.Matches(line);
                var wordsIndices = matches.Select(t => t.Index);
                var backWordsIndices = backMatches.Select(t => t.Index);

                var firstVal = valIndices.Any() ? valIndices.First() : int.MaxValue;
                var firstWord = wordsIndices.Any() ? wordsIndices.First() : int.MaxValue;

                if (firstVal < firstWord)
                {
                    j += line[firstVal];
                }
                else
                {
                    j += words[matches.First().Value];
                }

                var lastVal = valIndices.Any() ? valIndices.Last() : int.MinValue;
                var lastWord = backWordsIndices.Any() ? backWordsIndices.First() : int.MinValue;

                if (lastVal > lastWord)
                {
                    j += line[lastVal];
                }
                else
                {
                    j += words[backMatches.First().Value];
                }

                j = j.First().ToString() + j.Last().ToString();
                if (j != "")
                {
                    sum += int.Parse(j);
                }

            }

            Console.WriteLine(sum);

        }
    }
}
