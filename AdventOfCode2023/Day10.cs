using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day10
    {

        public Day10() 
        {
            Console.WriteLine("Day10");

            var lines = File.ReadLines(@"..\..\..\input\day10.txt").ToArray();

            var coord = new Dictionary<(int, int), string>();

            var startX = 0;
            var startY = 0;

            for (var i = 0; i<lines.Count(); i++)
            {
                var line = lines[i];
                for (var j = 0; j<line.Length; j++)
                {
                    var value = line[j].ToString();
                    coord.Add((i, j), value);

                    if (value == "S")
                    {
                        startX = i;
                        startY = j;
                    }
                }
            }

            var mainLoop = new List<(int, int)>
            {
                (startX, startY)
            };

            var currentX = startX;
            var currentY = startY;

            var currentSymbol = "S";

            while (true)
            {
                var north = !mainLoop.Contains((currentX - 1, currentY)) && currentX - 1 >= 0 
                    ? coord[(currentX - 1, currentY)] 
                    : null; //Check if already visited
                var east = !mainLoop.Contains((currentX, currentY + 1)) && currentY + 1 < lines[0].Length
                    ? coord[(currentX, currentY + 1)] 
                    : null;
                var west = !mainLoop.Contains((currentX, currentY - 1)) && currentY - 1 >= 0 
                    ? coord[(currentX, currentY - 1)] 
                    : null;
                var south = !mainLoop.Contains((currentX + 1, currentY)) && currentX + 1 < lines.Length 
                    ? coord[(currentX + 1, currentY)] 
                    : null;


                if (north != null && 
                 (currentSymbol == "L" || currentSymbol == "J" || currentSymbol == "|" || currentSymbol == "S") &&
                 (north == "|" || north == "F" || north == "7"))
                {
                    currentSymbol = north;
                    currentX -= 1;
                }
                else if (east != null &&
                    (currentSymbol == "-" || currentSymbol == "L" || currentSymbol == "F" || currentSymbol == "S") &&
                    (east == "-" || east == "7" || east == "J"))
                {
                    currentSymbol = east;
                    currentY += 1;
                }
                else if (west != null &&
                    (currentSymbol == "-" || currentSymbol == "J" || currentSymbol == "7" || currentSymbol == "S") &&
                    (west == "-" || west == "L" || west == "F"))
                {
                    currentSymbol = west;
                    currentY -= 1;
                }
                else if (south != null &&
                    (currentSymbol == "7" || currentSymbol == "F" || currentSymbol == "|" || currentSymbol == "S") &&
                    (south == "|" || south == "L" || south == "J"))
                {
                    currentSymbol = south;
                    currentX += 1;
                }
                else
                {
                    break;
                }

                mainLoop.Add((currentX, currentY));
            }


            Console.WriteLine(mainLoop.Count/2);

            var length = lines.Count();
            var width = lines.First().Length;

            var nonLoopCoords = new List<(int, int)>();

            foreach (var c in coord)
            {
                if (mainLoop.Contains(c.Key))
                {
                    continue;
                }

                nonLoopCoords.Add(c.Key);
            }

            var areas = new List<Area>();

            foreach (var cand in nonLoopCoords)
            {
                var a = areas.FirstOrDefault(a => a.IsNextToArea(cand.Item1, cand.Item2));
                if (a != null)
                {
                    a.members.Add(cand);
                }
                else
                {
                    var newArea = new Area()
                    {
                        members = new List<(int, int)>()
                        {
                            cand
                        }
                    };
                    areas.Add(newArea);
                }
            }

            var areaList = areas.ToArray();

            var mergedAreas = new List<Area>();
            var removedAreas = new List<int>();

            for (var i = 0; i< areaList.Count(); i++)
            {
                if (removedAreas.Contains(i))
                {
                    continue;
                }
                var areaA = areaList[i];
                var newArea = areaA;
                for (var j = i+1; j< areaList.Count(); j++)
                {
                    var areaB = areaList[j];

                    if (newArea.members.Any(m => areaB.IsNextToArea(m.Item1, m.Item2)))
                    {
                        removedAreas.Add(j);
                        newArea.members.AddRange(areaB.members);
                    }
                }
                mergedAreas.Add(newArea);
            }

            var sum = 0;

            var mainLoopHorizontals = coord.Where(c => mainLoop.Contains(c.Key) && c.Value == "-");
            var mainLoopVerticals = coord.Where(c => mainLoop.Contains(c.Key) && c.Value == "|");


            foreach (var area in mergedAreas)
            {
                if (area.startX == 0 || area.startY == 0 ||
                   area.endX == length - 1 || area.endY == width - 1)
                {
                    //Is connected to edge
                    continue;
                }

                //Check if can squeeze out or not

                var allXs = Enumerable.Range(area.startX, area.endX - area.startX + 1);
                var allYs = Enumerable.Range(area.startY, area.endY - area.startY + 1);

                var allAboveCovered = allYs
                    .All(y => mainLoopHorizontals.Any(item => item.Key.Item2 == y && item.Key.Item1 < area.startX));

                var allBelowCovered = allYs
                    .All(y => mainLoopHorizontals.Any(item => item.Key.Item2 == y && item.Key.Item1 > area.startX));

                var allToLeftCovered = allXs
                    .All(x => mainLoopVerticals.Any(item => item.Key.Item1 == x && item.Key.Item2 < area.startY));

                var allToRightCovered = allXs
                    .All(x => mainLoopVerticals.Any(item => item.Key.Item1 == x && item.Key.Item2 > area.endY));

                var areaIsEnclosed = allAboveCovered && allBelowCovered && allToLeftCovered && allToRightCovered;

                if (areaIsEnclosed)
                {
                    sum += area.members.Count();
                }
            }

            Console.WriteLine(sum);

        }

        public record Area
        {
            public int startX => members.Min(x => x.Item1);
            public int endX => members.Max(x => x.Item1);
            public int startY => members.Min(y => y.Item2);
            public int endY => members.Max(y => y.Item2);

            public List<(int, int)> members;

            public bool IsNextToArea(int x, int y) 
                => members.Any(m => Math.Abs(m.Item1 - x) + Math.Abs(m.Item2 - y) == 1);
        }
    }
}
