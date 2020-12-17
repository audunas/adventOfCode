using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day3
    {
        public Day3()
        {
            const int right = 3;
            const int down = 1;

            var lines = File.ReadLines(@"..\..\input\day3.txt").ToList();

            var hitTrees = FindHitTrees(lines, right, down);

            long trees = hitTrees.Count();

            var listOfSlope = new List<Tuple<int, int>>
            {
                Tuple.Create(1, 2),
                Tuple.Create(1, 1),
                Tuple.Create(5, 1),
                Tuple.Create(7, 1),
            };

            foreach (var slope in listOfSlope)
            {
                var t = FindHitTrees(lines, slope.Item1, slope.Item2);
                trees = trees * t.Count();
            }

            Console.WriteLine(hitTrees.Count());
            Console.WriteLine(trees);
        }

        private static IEnumerable<Position> FindHitTrees(List<string> lines, int right, int down)
        {
            var height = lines.Count();
            var numberOfIterations = height * down;
            var lengthOfLine = lines.First().Length;

            var treePositions = lines.SelectMany((line, lineIndex) =>
                line.Select((c, charIndex) =>
                {
                    if (c == '#')
                    {
                        return new Position { x = lineIndex, y = charIndex };
                    }
                    return null;
                }).Where(p => p != null)).ToList();

            var foundTrees = new List<Position>();
            var counter = 1;
            while ((down * counter) <= height)
            {
                var yPos = right * counter;
                var xPos = down * counter;

                if (yPos >= lengthOfLine)
                {
                    yPos = yPos - (int)(Math.Floor((decimal)yPos / lengthOfLine) * lengthOfLine);
                }
                var pos = new Position { x = xPos, y = yPos };

                if (treePositions.Any(p => p.x == pos.x && p.y == pos.y))
                {
                    foundTrees.Add(pos);
                }

                counter++;
            }
            return foundTrees;
        }


        internal class Position
        {
            public int x;
            public int y;
        }
    }
}
