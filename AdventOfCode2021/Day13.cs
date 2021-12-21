using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day13
    {
        public Day13()
        {
            var lines = File.ReadLines(@"..\..\..\input\day13.txt");

            var coordinates = lines.TakeWhile(r => r != "").Select(t => Tuple.Create(int.Parse(t.Split(',')[0]), int.Parse(t.Split(',')[1]))).ToList();

            var foldInstructions = lines.Skip(coordinates.Count() + 1).Select(r => {
                var split = r.Split(" ")[2].Split("=");

                return new Fold { WayToFold = split[0], Number = int.Parse(split[1]) };
            });

            //Part 1
            var firstFold = foldInstructions.First();

            var maxX = coordinates.Max(c => c.Item1);
            var maxY = coordinates.Max(c => c.Item2);

            var coordinatesRemainining = new HashSet<Tuple<int, int>>();

            if (firstFold.WayToFold == "y")
            {
                coordinatesRemainining = coordinates.Where(r => r.Item2 < firstFold.Number).ToHashSet();
                var coordinatesBelowY = coordinates.Where(r => r.Item2 > firstFold.Number);
                var newCoordinates = coordinatesBelowY.Select(c => Tuple.Create(c.Item1, maxY - c.Item2));
                coordinatesRemainining.UnionWith(newCoordinates);
            }
            else if (firstFold.WayToFold == "x")
            {
                coordinatesRemainining = coordinates.Where(r => r.Item1 < firstFold.Number).ToHashSet();
                var coordinatesToLeftOfX = coordinates.Where(r => r.Item1 > firstFold.Number);
                var newCoordinates = coordinatesToLeftOfX.Select(c => Tuple.Create(maxX - c.Item1, c.Item2));
                coordinatesRemainining.UnionWith(newCoordinates);
            }

            Console.WriteLine(coordinatesRemainining.Count());

            //Part 2

            var updatedCoordinates = coordinates.ToHashSet();

            foreach (var fold in foldInstructions)
            {
                if (fold.WayToFold == "y")
                {
                    var coordinatesAboveY = updatedCoordinates.Where(r => r.Item2 < fold.Number).ToHashSet();
                    var coordinatesBelowY = updatedCoordinates.Where(r => r.Item2 > fold.Number);
                    var newCoordinates = coordinatesBelowY.Select(c => {
                        var toMiddle = c.Item2 - fold.Number;
                        var newY = fold.Number - toMiddle;
                        return Tuple.Create(c.Item1, newY);
                    }).ToList();
                    updatedCoordinates = coordinatesAboveY.Union(newCoordinates).ToHashSet();
                }
                else if (fold.WayToFold == "x")
                {
                    var coordinatesRightOfX = updatedCoordinates.Where(r => r.Item1 < fold.Number).ToHashSet();
                    var coordinatesToLeftOfX = updatedCoordinates.Where(r => r.Item1 > fold.Number);
                    var newCoordinates = coordinatesToLeftOfX.Select(c => {
                        var toMiddle = c.Item1 - fold.Number;
                        var newX = fold.Number - toMiddle;
                        return Tuple.Create(newX, c.Item2);
                    }).ToList();
                    updatedCoordinates = coordinatesRightOfX.Union(newCoordinates).ToHashSet();
                }
            }

            maxX = updatedCoordinates.Max(c => c.Item1);
            maxY = updatedCoordinates.Max(c => c.Item2);

            for (int i = 0; i<= maxY; i++)
            {
                var line = "";
                for (int j = 0; j<= maxX; j++)
                {
                    if (updatedCoordinates.Contains(Tuple.Create(j, i)))
                    {
                        line += "#";
                    }
                    else
                    {
                        line += ".";
                    }
                }
                Console.WriteLine(line);
            }
        }

        public struct Fold
        {
            public string WayToFold;
            public int Number;
        }
    }
}
