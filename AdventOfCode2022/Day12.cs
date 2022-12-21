using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2022
{
    public class Day12
    {

        public Day12()
        {
            var lines = File.ReadLines(@"..\..\..\input\day12.txt");

            var grid = lines.Select(s => s.Select(m => m).ToList()).ToList();
            var x = 0;
            var y = 0;

            var sourceX = 0;
            var sourceY = 0;

            var targetX = 0;
            var targetY = 0;

            var stepDict = new Dictionary<(int, int), int>();
            var q = new HashSet<(int, int, char)>();
            var startingPoints = new List<(int, int)>();
            for (var i = 0; i < grid.Count(); i++)
            {
                for (var j = 0; j < grid[i].Count(); j++)
                {
                    stepDict.Add((i, j), int.MaxValue);
                    q.Add((i, j, grid[i][j]));
                    if (grid[i][j] == 'E')
                    {
                        targetX = i;
                        targetY = j;
                    }
                    else if (grid[i][j] == 'S')
                    {
                        sourceX = i;
                        sourceY = j;
                        startingPoints.Add((i, j));
                    }
                    else if (grid[i][j] == 'a')
                    {
                        startingPoints.Add((i,j));
                    }
                }
            }

            //Part 1
            //startingPoints = new List<(int, int)>() { (sourceX, sourceY) };

            var minList = new List<int>();

            var counter = 0;

            foreach (var starts in startingPoints)
            {
                Console.WriteLine(counter);
                int min = GetMin(grid, starts.Item1, starts.Item2, targetX, targetY, 
                    stepDict.ToDictionary(t => t.Key, t => t.Value), new HashSet<(int, int, char)>(q));
                minList.Add(min);
                counter++;
            }

            Console.WriteLine(minList.Min()+1);
        }

        private int GetMin(List<List<char>> grid, int sourceX, int sourceY, int targetX, int targetY, 
            Dictionary<(int, int), int> stepDict, HashSet<(int, int, char)> q)
        {
            stepDict[(sourceX, sourceY)] = 0;

            while (q.Any())
            {
                var u = q.Aggregate((l, r) => stepDict[(l.Item1, l.Item2)] < stepDict[(r.Item1, r.Item2)] ? l : r);
                var element = grid[u.Item1][u.Item2];
                q.Remove((u.Item1, u.Item2, element));
                var neighbors = getNeighbors(grid, u.Item1, u.Item2).Where(n => q.Contains((n.Item1, n.Item2, n.Item3))).ToList();
                var uValue = stepDict[(u.Item1, u.Item2)];
                foreach (var n in neighbors)
                {
                    if (element == 'S' || (n.Item3 - element) < 2)
                    {
                        if (stepDict[(n.Item1, n.Item2)] > uValue)
                        {
                            stepDict[(n.Item1, n.Item2)] = uValue + 1;
                        }
                    }
                }
            }

            var min = int.MaxValue;

            var targetNeigh = getNeighbors(grid, targetX, targetY);
            var targetValue = 'z';

            foreach (var d in targetNeigh)
            {
                var value = stepDict[(d.Item1, d.Item2)];
                if (value > 0 && value < min && (targetValue - grid[d.Item1][d.Item2]) < 2)
                {
                    min = value;
                }
            }

            return min;
        }

        public List<(int, int, char)> getNeighbors(List<List<char>> grid, int x, int y)
        {
            var neigh = new List<(int, int, char)>();
            if (x > 0)
            {
                neigh.Add((x-1, y, grid[x-1][y]));
            }
            if (y > 0)
            {
                neigh.Add((x, y-1, grid[x][y-1]));
            }

            if (x < grid.Count()-1)
            {
                neigh.Add((x+1, y, grid[x+1][y]));
            }
            if (y < grid[0].Count()-1)
            {
                neigh.Add((x, y+1, grid[x][y+1]));
            }
            return neigh;
        }
    }
}
