using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day14
    {
        public Day14() 
        {
            Console.WriteLine("Day14");

            var lines = File.ReadLines(@"..\..\..\input\day14.txt").ToArray();

            var coord = new Dictionary<(int, int), char>();

            for (var i = 0; i<lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j<line.Length; j++)
                {
                    coord.Add((i, j), line[j]);
                }
            }

            var newCoord = Move(coord, Direction.North);

            var sum = 0;
            for (var i = 0; i<lines.Count(); i++)
            {
                var allOnLine = newCoord.Where(c => c.Key.Item1 == i && c.Value == 'O').Count();
                sum += (allOnLine * (lines.Count() - i));
            }


            Console.WriteLine(sum);

            //Part 2

            var c = 1;

            var next = new Dictionary<(int, int), char>(coord);
            var endstates = new List<string>();

            var max = 1000000000;

            while (c <= max)
            {
                foreach (var dir in new[] {Direction.North, Direction.West, Direction.South, Direction.East})
                {
                    next = Move(next, dir); 
                }
                var nextEndState = new string(next.Select(c => c.Value).ToArray());
                
                //Find repeating pattern
                if (endstates.Contains(nextEndState))
                {
                    var reversed = new List<string>(endstates)
                    {
                        nextEndState
                    };
                    reversed.Reverse();
                    var secondToLastRepeatedEndState = reversed.Skip(1).TakeWhile(e => e != nextEndState).Concat(new[] { nextEndState });
                    var count = secondToLastRepeatedEndState.Count();
                    var repatedPattern = new List<string>(secondToLastRepeatedEndState);
                    repatedPattern.Reverse();

                    var g = (max - c) % count;

                    var nextString = repatedPattern.ElementAt(g);

                    var n = nextString.Chunk(lines.First().Length).ToArray();

                    var co = new Dictionary<(int, int), char>();

                    for (var i = 0; i< n.Length; i++)
                    {
                        var f = n[i];
                        for (var j = 0; j<f.Length; j++)
                        {
                            co.Add((i, j), f[j]);
                        }
                    }

                    next = co;

                    break;
                    
                }
                endstates.Add(nextEndState);
                Console.WriteLine(c);
                c++;
            }

            sum = 0;
            for (var i = 0; i < lines.Count(); i++)
            {
                var allOnLine = next.Where(c => c.Key.Item1 == i && c.Value == 'O').Count();
                sum += (allOnLine * (lines.Count() - i));
            }


            Console.WriteLine(sum);

        }

        public enum Direction
        {
            North,
            South, 
            East, 
            West
        }

        private static Dictionary<(int,int), char> Move(Dictionary<(int, int), char> coord, Direction dir)
        {
            var newCoord = new Dictionary<(int, int), char>(coord);

            var rocks = coord.Where(c => c.Value == 'O');

            foreach (var rock in rocks)
            {
                var (x, y) = rock.Key;
                var n = (x, y);
                switch (dir)
                {
                    case Direction.North:
                        n = North(newCoord, x, y);
                        break;
                    case Direction.South:
                        n = South(newCoord, x, y);
                        break;
                    case Direction.East:
                        n = East(newCoord, x, y);
                        break;
                    case Direction.West:
                        n = West(newCoord, x, y);
                        break;
                    default:
                        break;
                }

                if (n == (x, y))
                {
                    continue;
                }

                newCoord[(n.Item1, n.Item2)] = 'O';
                newCoord[(x, y)] = '.';

            }
            return newCoord;
        }

        private static (int, int) North(Dictionary<(int, int), char> newCoord, int x, int y)
        {
            var allAbove = newCoord.Where(c => c.Key.Item1 < x && c.Key.Item2 == y && c.Value != 'O');
            var allAboveNotCubes = allAbove.OrderByDescending(c => c.Key.Item1)
                .TakeWhile(c => c.Value != '#');

            if (!allAboveNotCubes.Any())
            {
                return (x, y);
            }

            var newX = allAboveNotCubes.Min(c => c.Key.Item1);
            return (newX, y);
        }

        private static (int, int) South(Dictionary<(int, int), char> newCoord, int x, int y)
        {
            var allBelow = newCoord.Where(c => c.Key.Item1 > x && c.Key.Item2 == y && c.Value != 'O');
            var allBelowNotCubes = allBelow.OrderBy(c => c.Key.Item1)
                .TakeWhile(c => c.Value != '#');

            if (!allBelowNotCubes.Any())
            {
                return (x, y);
            }

            var newX = allBelowNotCubes.Max(c => c.Key.Item1);
            return (newX, y);
        }

        private static (int, int) East(Dictionary<(int, int), char> newCoord, int x, int y)
        {
            var allRight = newCoord.Where(c => c.Key.Item1 == x && c.Key.Item2 > y && c.Value != 'O');
            var allRightNotCubes = allRight.OrderBy(c => c.Key.Item2)
                .TakeWhile(c => c.Value != '#');

            if (!allRightNotCubes.Any())
            {
                return (x, y);
            }

            var newY = allRightNotCubes.Max(c => c.Key.Item2);
            return (x, newY);
        }

        private static (int, int) West(Dictionary<(int, int), char> newCoord, int x, int y)
        {
            var allLeft = newCoord.Where(c => c.Key.Item1 == x && c.Key.Item2 < y && c.Value != 'O');
            var allLeftNotCubes = allLeft.OrderByDescending(c => c.Key.Item2)
                .TakeWhile(c => c.Value != '#');

            if (!allLeftNotCubes.Any())
            {
                return (x, y);
            }

            var newY = allLeftNotCubes.Min(c => c.Key.Item2);
            return (x, newY);
        }
    }
}
