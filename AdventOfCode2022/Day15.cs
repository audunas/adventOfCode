using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode2022
{
    public class Day15
    {

        public Day15()
        {
            var lines = File.ReadLines(@"..\..\..\input\day15.txt");

            var sensors = new List<(long, long)>();
            var dict = new Dictionary<(long, long), (long, long, long)>();
            var beacons = new HashSet<(long, long)>();
            var noBeacons = new HashSet<(long, long)>();

            foreach (var line in lines)
            {
                var parts = line.Split(" ");
                var sensorX = long.Parse(parts[2].Split("=")[1].Trim(','));
                var sensorY = long.Parse(parts[3].Split("=")[1].Trim(':'));
                sensors.Add((sensorX, sensorY));

                var beaconX = long.Parse(parts[8].Split("=")[1].Trim(','));
                var beaconY = long.Parse(parts[9].Split("=")[1].Trim());
                beacons.Add((beaconX, beaconY));

                dict[(sensorX, sensorY)] = (beaconX, beaconY, ManhattanDistance(sensorX, beaconX, sensorY, beaconY));
            }

            // Part 1

            var maxX = sensors.Max(r => r.Item1) + 10000000;
            var minX = sensors.Min(r => r.Item1) - 10000000;

            var rowY = 2000000;
            var points = new HashSet<(long, long)>();

            for (var i = minX; i < maxX; i++)
            {
                foreach (var d in dict)
                {
                    var manhattanDistToBeacon = ManhattanDistance(d.Key.Item1, d.Value.Item1, d.Key.Item2, d.Value.Item2);
                    var manhattanDistToPoint = ManhattanDistance(d.Key.Item1, i, d.Key.Item2, rowY);

                    if (manhattanDistToBeacon >= manhattanDistToPoint && !beacons.Contains((i, rowY)))
                    {
                        points.Add((i, rowY));
                    }
                }
            }

            Console.WriteLine(points.Count());

            // Part 2

            var edgePoints = new HashSet<(long, long)>();
            foreach (var d in dict)
            {
                var x = d.Key.Item1;
                var y = d.Key.Item2;
                var mDist = d.Value.Item3;
                for (var h = 0; h < mDist; h++)
                {
                    var xOutsideLeft = (x - h) - 1;
                    var xOutsideRight = x + h + 1;
                    edgePoints.Add((xOutsideLeft, y-(mDist - h)));
                    edgePoints.Add((xOutsideRight, y-(mDist - h)));

                    edgePoints.Add((xOutsideLeft, y + (mDist - h)));
                    edgePoints.Add((xOutsideRight, y + (mDist - h)));
                }
                edgePoints.Add((x - mDist - 1, y));
                edgePoints.Add((x + mDist + 1, y));
            }

            var edgePointsInsideGrid = edgePoints.Where(s => s.Item1 >= 0 && s.Item1 <= 4000000 && s.Item2 >= 0 && s.Item2 <= 4000000).ToList();

            var possibleBeaconPoint = (0L,0L);

            foreach (var edgePoint in edgePointsInsideGrid)
            {
                var pointVisited = false;
                foreach (var d in dict)
                {
                    var manhattanDistToPoint = ManhattanDistance(d.Key.Item1, edgePoint.Item1, d.Key.Item2, edgePoint.Item2);
                    var manhattanDistToBeacon = d.Value.Item3;

                    if (manhattanDistToBeacon >= manhattanDistToPoint)
                    {
                        pointVisited = true;
                        break;
                    }
                }
                if (!pointVisited)
                {
                    possibleBeaconPoint = (edgePoint.Item1, edgePoint.Item2);
                    break;
                }
            }

            var sum = (possibleBeaconPoint.Item1 * 4000000) + possibleBeaconPoint.Item2;
            
            Console.WriteLine(sum);
        }

        public long ManhattanDistance(long x1, long x2, long y1, long y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}
