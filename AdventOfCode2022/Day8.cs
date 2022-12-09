using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day8
    {

        public Day8()
        {
            var lines = File.ReadLines(@"..\..\..\input\day8.txt");

            var height = lines.Count();
            var width = lines.First().Length;

            var grid = new List<List<int>>();

            foreach (var line in lines)
            {
                var intLine = line.Select(l => int.Parse(l.ToString())).ToList();
                grid.Add(intLine);
            }

            var visibles = 0;
            var highestScenicScore = 0;

            for (int y = 1; y<width-1; y++)
            {
                for (int x = 1; x<height-1; x++) 
                {
                    var gridItem = grid[y][x];
                    var neighborsUp = new List<int>();
                    var neighborsDown = new List<int>();
                    var neighborsLeft = new List<int>();
                    var neighborsRight = new List<int>();
                    for (int a = y-1; a>=0; a--)
                    {
                        neighborsUp.Add(grid[a][x]);
                    }
                    for (int a = y+1; a<height; a++)
                    {
                        neighborsDown.Add(grid[a][x]);
                    }
                    for (int a = x-1; a>=0; a--)
                    {
                        neighborsLeft.Add(grid[y][a]);
                    }
                    for (int a = x+1; a<width; a++)
                    {
                        neighborsRight.Add(grid[y][a]);
                    }

                    //Part 1
                    if (neighborsUp.All(n => gridItem > n) ||
                        neighborsDown.All(n => gridItem > n) ||
                        neighborsLeft.All(n => gridItem > n) ||
                        neighborsRight.All(n => gridItem > n))
                    {
                        visibles++;
                    }

                    //Part 2
                    var seeingNeighborsUp = neighborsUp.TakeWhile(n => gridItem > n);
                    var up = seeingNeighborsUp.Count() == neighborsUp.Count() ? seeingNeighborsUp.Count() : seeingNeighborsUp.Count() + 1;
                    var seeingNeighborsDown = neighborsDown.TakeWhile(n => gridItem > n);
                    var down = seeingNeighborsDown.Count() == neighborsDown.Count() ? seeingNeighborsDown.Count() : seeingNeighborsDown.Count() + 1;
                    var seeingNeighborsLeft = neighborsLeft.TakeWhile(n => gridItem > n);
                    var left = seeingNeighborsLeft.Count() == neighborsLeft.Count() ? seeingNeighborsLeft.Count() : seeingNeighborsLeft.Count() + 1;
                    var seeingNeighborsRight = neighborsRight.TakeWhile(n => gridItem > n);
                    var right = seeingNeighborsRight.Count() == neighborsRight.Count() ? seeingNeighborsRight.Count() : seeingNeighborsRight.Count() + 1;

                    var score = up * down * left * right;

                    if (score > highestScenicScore)
                    {
                        highestScenicScore = score;
                    }
                }
            }

            visibles += (height * 2) + ((width-2)*2);

            Console.WriteLine(visibles);

            Console.WriteLine(highestScenicScore);
        }
    }
}