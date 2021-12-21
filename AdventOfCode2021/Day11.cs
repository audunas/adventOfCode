using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
    class Day11
    {
        public Day11()
        {
            var lines = File.ReadLines(@"..\..\..\input\day11.txt");

            var grid = lines.Select(l => l.Select(el => int.Parse(el.ToString())).ToArray()).ToArray();

            var numberOfFlashes = 0;

            var newGrid = grid;

            for (var i = 0; i<100/* Part2:400*/; i++)
            {
                newGrid = newGrid.Select(l => l.Select(octopus => octopus + 1).ToArray()).ToArray();
                var flashingOctopuses = new HashSet<Tuple<int, int>>();
                var newFlashingOctopuses = newGrid.SelectMany((l, yIndex) => l.Select((oct, xIndex) => {
                            if (oct > 9)
                            {
                                return Tuple.Create(yIndex, xIndex);
                            }
                            return null;
                        })).Where(b => b != null).ToHashSet();
                flashingOctopuses.UnionWith(newFlashingOctopuses);
                while (newFlashingOctopuses.Count() > 0)
                {
                   
                    //Increase all neighbors by 1
                    foreach (var octo in newFlashingOctopuses)
                    {
                        if (octo.Item2 < newGrid[0].Length - 1)
                        {
                           newGrid[octo.Item1][octo.Item2 + 1] += 1;
                        }
                        if (octo.Item2 > 0)
                        {
                           newGrid[octo.Item1][octo.Item2 - 1] += 1;
                        }
                        if (octo.Item1 < newGrid[0].Length - 1)
                        {
                            newGrid[octo.Item1 + 1][octo.Item2] += 1;
                            if (octo.Item2 < newGrid[0].Length - 1)
                            {
                                newGrid[octo.Item1 + 1][octo.Item2 + 1] += 1;
                            }
                            if (octo.Item2 > 0)
                            {
                               newGrid[octo.Item1 + 1][octo.Item2 - 1] += 1;
                            }
                        }
                        if (octo.Item1 > 0)
                        {
                            newGrid[octo.Item1 - 1][octo.Item2] += 1;
                            if (octo.Item2 < newGrid[0].Length - 1)
                            {
                                newGrid[octo.Item1 - 1][octo.Item2 + 1] += 1;
                            }
                            if (octo.Item2 > 0)
                            {
                                newGrid[octo.Item1 - 1][octo.Item2 - 1] += 1;
                            }
                        }
                    }

                    //Check for 9
                    newFlashingOctopuses = newGrid
                        .SelectMany((l, yIndex) => l.Select((oct, xIndex) =>
                        {
                            if (oct > 9)
                            {
                                return Tuple.Create(yIndex, xIndex);
                            }
                            return null;
                        })).Where(b => b != null)
                        .Except(flashingOctopuses).ToHashSet();
                   
                    flashingOctopuses.UnionWith(newFlashingOctopuses);

                }
                if (flashingOctopuses.Count() == (grid.Length*grid[0].Length))
                {
                    //Part 2
                    Console.WriteLine(i+1);
                }
                foreach (var octo in flashingOctopuses)
                {
                    newGrid[octo.Item1][octo.Item2] = 0;
                }
                numberOfFlashes += flashingOctopuses.Count();
            }

            Console.WriteLine(numberOfFlashes);
        }
    }
}
