using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day10
    {

        public Day10()
        {
            Console.WriteLine("Day10");

            var lines = File.ReadLines(@"..\..\..\input\day10.txt").ToArray();

            var coord = new Dictionary<(int, int), int>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    coord.Add((i, j), int.Parse(line[j].ToString()));
                }
            }

            var zeroes = coord.Where(c => c.Value == 0);

            var sum = 0;

            foreach (var zero in zeroes)
            {
                var x = zero.Key.Item1;
                var y = zero.Key.Item2;

                var coordsToCheck = new List<(int, int)>
                {
                    (x, y)
                };

                var nextValue = 1;

                while (nextValue < 10 && coordsToCheck.Any())
                {

                    var nextCoordsToCheck = new List<(int, int)>();

                    foreach (var toCheck in coordsToCheck)
                    {
                        var up = (toCheck.Item1 - 1, toCheck.Item2);
                        var down = (toCheck.Item1 + 1, toCheck.Item2);
                        var left = (toCheck.Item1, toCheck.Item2 - 1);
                        var right = (toCheck.Item1, toCheck.Item2 + 1);

                        if (coord.ContainsKey(up) && coord[up] == nextValue)
                        {
                            nextCoordsToCheck.Add(up);
                        }
                        if (coord.ContainsKey(down) && coord[down] == nextValue)
                        {
                            nextCoordsToCheck.Add(down);
                        }
                        if (coord.ContainsKey(left) && coord[left] == nextValue)
                        {
                            nextCoordsToCheck.Add(left);
                        }
                        if (coord.ContainsKey(right) && coord[right] == nextValue)
                        {
                            nextCoordsToCheck.Add(right);
                        }
                    }

                    coordsToCheck = nextCoordsToCheck;

                    nextValue++;

                }

                var matches = coordsToCheck.Where(c => coord.ContainsKey(c) && coord[c] == 9);//.Distinct(); //Part 1 = With Distinct();

                sum += matches.Count();

            }

            Console.WriteLine(sum);
        }
    }
}
