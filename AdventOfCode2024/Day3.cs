using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day3
    {

        public Day3()
        {
            Console.WriteLine("Day3");

            var lines = File.ReadLines(@"..\..\..\input\day3.txt").ToArray();

            var sum = 0;
            var mulEnabled = true;
            var curIndex = 0;
            var prevIndex = 0;

            foreach (var line in lines)
            {
                var matches = Regex.Matches(line, "(mul\\()([0-9]+,[0-9]+\\))");

                //Part 2
                var doMatches = Regex.Matches(line, "(do\\()(\\))");
                var dontMatches = Regex.Matches(line, "(don't\\()(\\))");


                foreach (Match item in matches)
                {
                    curIndex = item.Index;

                    if (mulEnabled && dontMatches.Any(d => d.Index > prevIndex && d.Index < curIndex))
                    {
                        mulEnabled = false;
                    }
                    else if (!mulEnabled && doMatches.Any(d => d.Index > prevIndex && d.Index < curIndex))
                    {
                        mulEnabled = true;
                    }

                    if (mulEnabled)
                    {
                        var x = int.Parse(item.Value.Split('(')[1].Split(',')[0]);
                        var y = int.Parse(item.Value.Split('(')[1].Split(',')[1].Trim(')'));
                        sum += (x * y);
                    }
                    prevIndex = item.Index;
                }
            }

            

            Console.WriteLine(sum);
        }
    }
}
