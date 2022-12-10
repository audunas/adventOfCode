using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day10
    {

        public Day10()
        {
            var lines = File.ReadLines(@"..\..\..\input\day10.txt");

            // Part 1
            var x = 1;
            var cycleCounter = 0;
            var nextCycle = 20;
            var signalStrength = 0;

            foreach (var line in lines)
            {
                var split = line.Split(" ");
                var cmd = split[0];
                if (cmd == "noop")
                {
                    cycleCounter++;
                    if (nextCycle == cycleCounter)
                    {
                        signalStrength += (cycleCounter * x);
                        nextCycle += 40;
                    }
                }
                else
                {
                    var add = int.Parse(split[1]);
                    for (var i = 0; i<2;  i++)
                    {
                        cycleCounter++;
                        if (nextCycle == cycleCounter)
                        {
                            signalStrength += (cycleCounter * x);
                            nextCycle += 40;
                        }
                    }
                    x += add;
                }
            }

            Console.WriteLine(signalStrength);

            // Part 2
            x = 1;
            cycleCounter = 0;
            var spritPos = new List<int>() { 0, 1, 2 };
            var row = new List<string>();

            foreach (var line in lines)
            {
                var split = line.Split(" ");
                var cmd = split[0];
                if (cmd == "noop")
                {
                    if (spritPos.Contains(cycleCounter))
                    {
                        row.Add("#");
                    }
                    else
                    {
                        row.Add(".");
                    }
                    cycleCounter++;
                    if (cycleCounter > 39)
                    {
                        cycleCounter -= 40;
                    }
                }
                else
                {
                    var add = int.Parse(split[1]);
                    for (var i = 0; i < 2; i++)
                    {
                        if (spritPos.Contains(cycleCounter))
                        {
                            row.Add("#");
                        }
                        else
                        {
                            row.Add(".");
                        }
                        cycleCounter++;
                        if (cycleCounter > 39)
                        {
                            cycleCounter -= 40;
                        }
                    }
                    x += add;
                    spritPos = new List<int>() { x - 1, x, x + 1 };
                }
            }

            var chunks = row.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 40)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            Print(chunks);
        }

        public static void Print(List<List<string>> grid)
        {
            foreach (var row in grid)
            {
                Console.WriteLine(string.Join("", row));
            }
        }
    }
}
