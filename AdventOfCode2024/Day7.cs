using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day7
    {

        public Day7()
        {
            Console.WriteLine("Day7");

            var lines = File.ReadLines(@"..\..\..\input\day7.txt").ToArray();

            var sum = 0l;

            foreach (var line in lines)
            {
                Console.WriteLine($"{line}");
                var lineSum = long.Parse(line.Split(':')[0]);
                var numbers = line.Split(':')[1].Split(' ')
                    .Select(t => t.Trim()).Where(t => t.Length > 0).Select(long.Parse);
                
                var possibleSums = new List<long>();
                var sumStrings = new List<string>();

                foreach (var num in numbers)
                {
                    if (!possibleSums.Any())
                    {
                        possibleSums.Add(num);
                        continue;
                    }

                    var newPossibleSums = new List<long>();
                    

                    foreach (var pos in possibleSums)
                    {
                        var newSum = pos * num;
                        var newSum2 = pos + num;
                        sumStrings.Add(pos.ToString() + num.ToString());
                        newPossibleSums.Add(newSum);
                        newPossibleSums.Add(newSum2);
                        newPossibleSums.Add(long.Parse(pos.ToString() + num.ToString()));
                    }

                    possibleSums = newPossibleSums;
                }


                if (possibleSums.Any(p => p == lineSum) ||
                    sumStrings.Any(p => long.Parse(p) == lineSum))
                {
                    sum += lineSum;
                }
            }

            Console.WriteLine(sum);

        }
    }
}
