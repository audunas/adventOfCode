using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day18
    {

        public Day18()
        {
            var lines = File.ReadLines(@"..\..\..\input\day18.txt").ToList();

            var cubes = new HashSet<(int, int, int)>();

            foreach (var line in lines)
            {
                var sp = line.Split(',');
                cubes.Add((int.Parse(sp[0]), int.Parse(sp[1]), int.Parse(sp[2])));
            }

            var count = 0;
            var allNotConnectedSides = new List<(int, int, int)>();

            foreach (var cube in cubes)
            {
                var possibleNeighbors = new HashSet<(int, int, int)>()
                {
                    (cube.Item1 + 1, cube.Item2, cube.Item3),
                    (cube.Item1 - 1, cube.Item2, cube.Item3),
                    (cube.Item1, cube.Item2 + 1, cube.Item3),
                    (cube.Item1, cube.Item2 - 1, cube.Item3),
                    (cube.Item1, cube.Item2, cube.Item3 + 1),
                    (cube.Item1, cube.Item2, cube.Item3 - 1)
                };

                var cubesExceptCurrent = cubes.Where(c => c != cube);

                var notConnectedSides = possibleNeighbors.Where(p => !cubesExceptCurrent.Contains(p));
                count += notConnectedSides.Count();
                allNotConnectedSides.AddRange(notConnectedSides);
            }

            Console.WriteLine(count);

            var part2Counter = 0;

            var c = new List<(int, int, int)>();
            //var maxX = cubes.Max(c => c.Item1);
            //var minX = cubes.Min(c => c.Item1);
            //var maxY = cubes.Max(c => c.Item2);
            //var minY = cubes.Min(c => c.Item2);
            //var maxZ = cubes.Max(c => c.Item3);
            //var minZ = cubes.Min(c => c.Item3);

            var interiors = new List<(int, int, int)>();

            foreach (var cube in allNotConnectedSides)
            {
                var cubesOnSameX = cubes
                    .Where(c => c.Item2 == cube.Item2 && c.Item3 == cube.Item3).ToList();
                var isOutsideX = true;
                if (cubesOnSameX.Any())
                {
                    var maxX = cubesOnSameX.Max(c => c.Item1);
                    var minX = cubesOnSameX.Min(c => c.Item1);
                    isOutsideX = cube.Item1 > maxX || cube.Item1 < minX;
                }

                var cubesOnSameY = cubes
                    .Where(c => c.Item1 == cube.Item1 && c.Item3 == cube.Item3).ToList();
                var isOutsideY = true;
                if (cubesOnSameY.Any())
                {
                    var maxY = cubesOnSameY.Max(c => c.Item2);
                    var minY = cubesOnSameY.Min(c => c.Item2);
                    isOutsideY = cube.Item2 > maxY || cube.Item2 < minY;
                }


                var cubesOnSameZ = cubes
                    .Where(c => c.Item1 == cube.Item1 && c.Item2 == cube.Item2).ToList();
                var isOutsideZ = true;
                if (cubesOnSameZ.Any())
                {
                    var maxZ = cubesOnSameZ.Max(c => c.Item3);
                    var minZ = cubesOnSameZ.Min(c => c.Item3);
                    isOutsideZ = cube.Item3 > maxZ || cube.Item3 < minZ;
                }

                var isExterior = isOutsideX || isOutsideY || isOutsideZ;

                if (isExterior)
                {
                    part2Counter++;
                    continue;
                }
                interiors.Add(cube);
            }

            var dist = interiors.Distinct().ToList().OrderBy(c => c.Item1).ThenBy(c => c.Item2);

            Console.WriteLine(count - (dist.Count() * 6));
        }
    }
}
