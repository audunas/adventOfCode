using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    public class Day14
    {

        public Day14()
        {
            var lines = File.ReadLines(@"..\..\..\input\day14.txt");

            (int, int) sand = (0, 500);

            var grid = new List<List<string>>();

            foreach (var i in Enumerable.Range(1, 1000))
            {
                grid.Add(new List<string>(Enumerable.Range(1, 1000).Select(x => ".")));
            }

            grid[0][500] = "+";

            foreach (var line in lines)
            {
                var split = line.Split("->");
                for (var i = 0; i<split.Length-1; i++)
                {
                    var a = split[i];
                    var b = split[i+1];
                    var aParts = a.Split(',').Select(int.Parse).ToArray();
                    var bParts = b.Split(",").Select(int.Parse).ToArray();
                    if (aParts[0] == bParts[0])
                    {
                        for (var j = Math.Min(aParts[1], bParts[1]); j <= Math.Max(aParts[1], bParts[1]); j++)
                        {
                            grid[j][aParts[0]] = "#";
                        }
                    }
                    else if (aParts[1] == bParts[1])
                    {
                        for (var j = Math.Min(aParts[0], bParts[0]); j <= Math.Max(aParts[0], bParts[0]); j++)
                        {
                            grid[bParts[1]][j] = "#";
                        }
                    }
                }
            }

            foreach (var row in grid.Take(15))
            {
                Console.WriteLine(string.Join("", row.Skip(490).Take(15)));
            }

            (int, int) sandPos = (int.MaxValue, int.MaxValue);

            var lowestRock = 0;

            for (var x = 0; x < grid.Count(); x++)
            {
                for (var y = 0; y < grid[x].Count(); y++)
                {
                    var t = grid[x][y];
                    if (t == "#")
                    {
                        if (lowestRock < x)
                        {
                            lowestRock = x;
                        }
                    }
                }
            }

            var counter = 0;

            //Part 2
            lowestRock = lowestRock + 2;
            grid[lowestRock] = Enumerable.Range(1, 1000).Select(x => "#").ToList();

            while (true)
            {
                var newSandPost = sand;

                while (newSandPost.Item1 <= lowestRock)
                {
                    var previousSandPos = newSandPost;
                    var elementRightBelow = grid[newSandPost.Item1 +1][newSandPost.Item2];
                    if (elementRightBelow == ".")
                    {
                        newSandPost = (newSandPost.Item1+1, newSandPost.Item2);
                        continue;
                    }
                    else if (elementRightBelow == "#" || elementRightBelow == "o")
                    {
                        var elementRightBelowToLeft = grid[newSandPost.Item1+1][newSandPost.Item2 - 1];
                        if (elementRightBelowToLeft == ".")
                        {
                            newSandPost = (newSandPost.Item1+1, newSandPost.Item2-1);
                        }
                        else if (elementRightBelowToLeft == "#" || elementRightBelowToLeft == "o")
                        {
                            var elementRightBelowToRight = grid[newSandPost.Item1 + 1][newSandPost.Item2 + 1];
                            if (elementRightBelowToRight == ".")
                            {
                                newSandPost = (newSandPost.Item1 + 1, newSandPost.Item2 + 1);
                            }
                        }
                    }
                    else
                    {
                        //if all below are rock or sand
                        newSandPost = (newSandPost.Item1, newSandPost.Item2);
                    }
                    
                    if (newSandPost == previousSandPos)
                    {
                        break;
                    }
                }
                //Part 1
                //if (newSandPost.Item1 >= lowestRock)
                //{
                //    break;
                //}
                //Part 2
                if (newSandPost == sand)
                {
                    break;
                }

                grid[newSandPost.Item1][newSandPost.Item2] = "o";

                Console.WriteLine();
                foreach (var row in grid.Take(15))
                {
                    Console.WriteLine(string.Join("", row.Skip(490).Take(15)));
                }

                counter++;
            }

            Console.WriteLine(counter+1);
        }
    }
}
