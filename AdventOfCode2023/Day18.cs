using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day18
    {
        public Day18() 
        {
            Console.WriteLine("Day18");

            var lines = File.ReadLines(@"..\..\..\input\day18.txt").ToArray();

            var digPlan = new List<DigIns>();

            foreach (var line in lines)
            {
                var sp = line.Split(' ');
                var dir = sp[0].Trim();
                var meters = int.Parse(sp[1].Trim());
                var color = sp[2].Trim().Trim('(').Trim(')');
                digPlan.Add(new DigIns
                {
                    Dir = dir,
                    Meters = meters,
                    Color = color
                });
            }

            var start = (0, 0, digPlan.First().Color);
            var next = start;
            var edge = new List<(int, int, string)>() { start };

            foreach (var dig in digPlan)
            {
                for(var i = 0; i< dig.Meters; i++)
                {
                    switch (dig.Dir)
                    {
                        case "R":
                            next = (next.Item1, next.Item2 + 1, dig.Color);
                            break;
                        case "D":
                            next = (next.Item1 + 1, next.Item2, dig.Color);
                            break;
                        case "U":
                            next = (next.Item1 - 1, next.Item2, dig.Color);
                            break;
                        case "L":
                            next = (next.Item1, next.Item2 - 1, dig.Color);
                            break;
                    }
                    edge.Add(next);
                }
            }

            var minX = edge.Min(e => e.Item1);
            var maxX = edge.Max(e => e.Item1);
            var minY = edge.Min(e => e.Item2);
            var maxY = edge.Max(e => e.Item2);
            var edges = edge.Select(c => (c.Item1, c.Item2));

            //var edgesAdjusted = edge.Select(e => (e.Item1 + (minX < 0 ? Math.Abs(minX) : 0), e.Item2 + (minY < 0 ? Math.Abs(minY) : 0)));

            //var minX2 = edgesAdjusted.Min(e => e.Item1);
            //var maxX2 = edgesAdjusted.Max(e => e.Item1);
            //var minY2 = edgesAdjusted.Min(e => e.Item2);
            //var maxY2 = edgesAdjusted.Max(e => e.Item2);

            var liens = new List<string>();

            for (var x = minX; x<=maxX; x++)
            {
                var line = "";
                for (var y = minY; y<=maxY; y++)
                {
                    if (edges.Contains((x, y)))
                    {
                        line += "#";
                    }
                    else
                    {
                        line += ".";
                    }
                }
                liens.Add(line);
            }

            File.WriteAllLines(@"..\..\..\input\day18output.txt", liens);

            var allPoints = Enumerable.Range(minX, Math.Abs(maxX - minX)+1)
                .SelectMany(x => Enumerable.Range(minY, Math.Abs(maxY - minY)+1).Select(y => (x, y)));

            var totalArea = (Math.Abs(maxX - minX) + 1) * (Math.Abs(maxY - minY) + 1);

            

            var outside = new HashSet<(int, int)>();

            var areas = new List<Area>();

            foreach (var cand in allPoints)
            {
                if (edges.Contains(cand))
                {
                    continue;
                }
                //Console.WriteLine(cand);
                var a = areas.FirstOrDefault(a => a.IsNextToArea(cand.Item1, cand.Item2));
                if (a != null)
                {
                    a.members.Add(cand);
                }
                else
                {
                    var newArea = new Area()
                    {
                        members = new HashSet<(int, int)>()
                        {
                            cand
                        }
                    };
                    areas.Add(newArea);
                }
            }

            var areaList = areas.ToArray();

            var mergedAreas = new HashSet<Area>();
            var removedAreas = new HashSet<int>();

            for (var i = 0; i < areaList.Count(); i++)
            {
                if (removedAreas.Contains(i))
                {
                    continue;
                }
                var areaA = areaList[i];
                var newArea = areaA;
                for (var j = i + 1; j < areaList.Count(); j++)
                {
                    if (removedAreas.Contains(j))
                    {
                        continue;
                    }
                    var areaB = areaList[j];

                    if (newArea.members.Any(m => areaB.IsNextToArea(m.Item1, m.Item2)))
                    {
                        removedAreas.Add(j);
                        newArea.members.UnionWith(areaB.members);
                    }
                }
                mergedAreas.Add(newArea);
            }

            var sum = 0;

            foreach (var a in mergedAreas)
            {
                if (a.members.Any(m => m.Item1 == minX || m.Item1 == maxX ||
                        m.Item2 == minY || m.Item2 == maxY))
                {
                    sum += a.members.Count();
                }
            }

            //foreach (var point in allPoints)
            //{
            //    if (edgesAdjusted.Contains(point))
            //    {
            //        continue;
            //    }
                
            //    var edgesToLeft = edgesAdjusted.Where(edge => edge.Item2 < point.y && edge.Item1 == point.x).Count();
            //    var edgesToRight = edgesAdjusted.Where(edge => edge.Item2 > point.y && edge.Item1 == point.x).Count();
            //    var edgesAbove = edgesAdjusted.Where(edge => edge.Item1 < point.x && edge.Item2 == point.y).Count();
            //    var edgesBelow = edgesAdjusted.Where(edge => edge.Item1 > point.x && edge.Item2 == point.y).Count();

            //    if (edgesToLeft == 0 || edgesToRight == 0 || edgesAbove == 0 || edgesBelow == 0)
            //    {
            //        outside.Add(point);
            //    }

            //}
            
            //var totalArea = (maxX - minX + 1) * (maxY - minY + 1);

            Console.WriteLine(totalArea - sum);

        }

        public record Area
        {
            public int startX => members.Min(x => x.Item1);
            public int endX => members.Max(x => x.Item1);
            public int startY => members.Min(y => y.Item2);
            public int endY => members.Max(y => y.Item2);

            public HashSet<(int, int)> members;

            public bool IsNextToArea(int x, int y)
                => members.Any(m => Math.Abs(m.Item1 - x) + Math.Abs(m.Item2 - y) == 1);
        }

        public class DigIns
        {
            public string Dir;
            public int Meters;
            public string Color;
        }
    }
}
