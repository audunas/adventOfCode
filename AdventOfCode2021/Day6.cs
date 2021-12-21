using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day6
    {

        private static Dictionary<int, long> GetEmptyDict()
        {
            var counterDict = new Dictionary<int, long>();
            for (var i = 0; i < 9; i++)
            {
                counterDict.Add(i, 0);
            }
            return counterDict;
        }
        public Day6()
        {
            var lines = File.ReadLines(@"..\..\..\input\day6.txt").First();

            var startingNumbers = lines.Split(",").Select(l => int.Parse(l));

            var fish = startingNumbers.ToArray();

            //Part 1
            var counter = 80;
            //part 2
            counter = 256;

            var counterDict = GetEmptyDict();
            foreach (var st in startingNumbers)
            {
                counterDict[st] += 1;
            }

            while (counter > 0)
            {
                var newDict = GetEmptyDict();
                for (var i = 8; i >= 0; i--)
                {
                    var dictValue = counterDict[i];
                    if (i == 0)
                    {
                        newDict[8] += dictValue;
                        newDict[6] += dictValue;
                    }
                    else
                    {
                        newDict[i - 1] = dictValue;
                    }
                }
                counterDict = newDict;
                counter--;
            }

            var numberOfFish = counterDict.Sum(r => r.Value);

            Console.WriteLine(numberOfFish);
        }
    }
}
