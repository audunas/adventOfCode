using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day4
    {

        public Day4()
        {
            var lines = File.ReadLines(@"..\..\..\input\day4.txt");

            var numberOfFullContained = 0;
            var overLapsAtAll = 0;

            //Part 1
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                var part1 = parts[0].Split('-');
                var part2 = parts[1].Split('-');

                var part1Min = int.Parse(part1[0]);
                var part1Max = int.Parse(part1[1]);

                var part2Min = int.Parse(part2[0]);
                var part2Max = int.Parse(part2[1]);

                if ((part1Min <= part2Min && part1Max >= part2Max) ||
                    part2Min <= part1Min && part2Max >= part1Max)
                {
                    numberOfFullContained++;
                }

                if (part1Max >= part2Min && part1Min <= part2Max)
                {
                    overLapsAtAll++;
                }
            }

            Console.WriteLine(numberOfFullContained);

            //Part 2
            Console.WriteLine(overLapsAtAll);
        }
    }
}