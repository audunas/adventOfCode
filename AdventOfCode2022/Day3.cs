using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day3
    {

        public Day3()
        {
            var lines = File.ReadLines(@"..\..\..\input\day3.txt");

            //Part 1
            var commonChars = new List<string>();
            foreach (var line in lines)
            {
                var part1 = line.Take((line.Length/2));
                var part2 = line.Skip(line.Length/2);

                var common = part1.First(p => part2.Contains(p)).ToString();
                commonChars.Add(common);
            }

            var sum = commonChars.Sum(c => {
                var ch = c.ToCharArray().First();
                if (ch.ToString() == ch.ToString().ToUpper())
                {
                    return ch - 38;
                }
                return ch - 96;
            });

            Console.WriteLine(sum);

            //Part 2
            var groups = new List<List<string>>();

            while (lines.Any())
            {
                var batch = lines.Take(3).ToList();
                groups.Add(batch);
                lines = lines.Skip(3);
            }

            commonChars = new List<string>();

            foreach(var group in groups)
            {
                var common = group[0].Where(g1 => group[1].Contains(g1)).Where(g2 => group[2].Contains(g2)).First().ToString();
                commonChars.Add(common);
            }

            sum = commonChars.Sum(c => {
                var ch = c.ToCharArray().First();
                if (ch.ToString() == ch.ToString().ToUpper())
                {
                    return ch - 38;
                }
                return ch - 96;
            });

            Console.WriteLine(sum);
        }
    }
}