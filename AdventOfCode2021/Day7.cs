using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
    class Day7
    {
        public Day7()
        {
            var line = File.ReadLines(@"..\..\..\input\day7.txt").First();

            var positions = line.Split(',').Select(n => int.Parse(n));

            //Part 1
            var median = positions.OrderBy(x => x).ElementAt(positions.Count()/2);
            var sum = positions.Sum(p => p == median ? 0 : p > median ? p-median : median - p);

            Console.WriteLine(sum);

            //Part 2
            var minCost = 0;
            var min = positions.Min();
            var max = positions.Max();
            for (int i = min; i < max; i++)
            {
                var s = positions.Sum(p =>
                {
                    var diff = p > i
                        ? p - i
                        : i - p;
                    var diffSum = (int) (diff > 0 ? (1 + diff) * decimal.Divide(diff, 2) : 1);
                    return p == i
                    ? 0
                    : diffSum;
                });
                if (minCost == 0 || s < minCost)
                {
                    minCost = s;
                }
            }

            Console.WriteLine(minCost);
        }
    }
}
