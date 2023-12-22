using System;
using System.Collections.Generic;
using System.Data;
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
                //Part 1
                //digPlan.Add(new DigIns
                //{
                //    Dir = dir,
                //    Meters = meters,
                //    Color = color
                //});
                //Part 2
                meters = Convert.ToInt32(new string(color.Skip(1).Take(5).ToArray()).ToString(), 16);
                var last = int.Parse(color.Last().ToString());
                dir = last == 0 ? "R" : last == 1 ? "D" : last == 2 ? "L" : "U";

                digPlan.Add(new DigIns
                {
                    Dir = dir,
                    Meters = meters,
                    Color = color
                });
            }

            var allMeters = digPlan.Select(t => t.Meters);

            var min = allMeters.Min();

            for (var i = Math.Ceiling((decimal)min/2); i > 0; i--)
            {
                var all = allMeters.All(t => t % i == 0);
                if (all)
                {
                    var div = i;
                }
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
            var edges = edge.Select(c => (c.Item1, c.Item2)).ToHashSet();

            edges = edges.Select(e => (e.Item1 + Math.Abs(minX), e.Item2 + Math.Abs(minY))).Order().ToHashSet();

            minX = edges.Min(e => e.Item1);
            maxX = edges.Max(e => e.Item1);
            minY = edges.Min(e => e.Item2);
            maxY = edges.Max(e => e.Item2);

            var endpoints = new HashSet<(int, int)>();

            foreach (var ed in edges)
            {
                var north = (ed.Item1 - 1, ed.Item2);
                var south = (ed.Item1 + 1, ed.Item2);
                var west = (ed.Item1, ed.Item2 - 1);
                var east = (ed.Item1, ed.Item2 + 1);
                if (!edges.Contains(north) && north.Item1 >= minX)
                {
                    endpoints.Add(north);
                }
                if(!edges.Contains(south) && south.Item1 <= maxX) 
                { 
                    endpoints.Add(south); 
                }
                if (!edges.Contains(west) && west.Item2 >= minY)
                {
                    endpoints.Add(west);
                }
                if (!edges.Contains(east) && east.Item2 <= maxY)
                {
                    endpoints.Add(east);
                }
            }

            var length = (long)(Math.Abs(maxX - minX) + 1);
            var width = (long)(Math.Abs(maxY - minY) + 1);

            var totalArea = length * width;

            for (var i = 0; i < length; i++)
            {
                if (!edges.Contains((i, maxY)))
                    endpoints.Add((i, maxY));
                if (!edges.Contains((i,0)))
                    endpoints.Add((i, 0));
            }
            for (var i = 0; i < width; i++)
            {
                if (!edges.Contains((0, i)))
                    endpoints.Add((0, i));
                if (!edges.Contains((maxX, i)))
                    endpoints.Add((maxX, i));
            }


            endpoints = endpoints.Order().ToHashSet();

            var insidePoints = new HashSet<(int, int)>();

            long inside = 0;
            long outside = 0;

            var groupedEndpointsByRow = endpoints.GroupBy(e => e.Item1);

            foreach (var rowGroup in groupedEndpointsByRow)
            {
                Console.WriteLine(rowGroup.Key);
                var r = insidePoints.RemoveWhere(r => r.Item1 == rowGroup.Key - 2);
                inside += r;
                var endpointsOnRow = rowGroup.OrderBy(r => r.Item2).ToArray();
                var allEdgesOnRow = edges.Where(e => e.Item1 == rowGroup.Key).OrderBy(r => r.Item2).ToArray();

                var indexOfFirstEdgeOnRow = allEdgesOnRow.Min(r => r.Item2);
                var current = endpointsOnRow.First().Item2 < indexOfFirstEdgeOnRow ? "outside" : "inside";

                if (endpointsOnRow.Length == 1)
                {
                    if (current == "outside")
                    {
                        outside++;
                    }
                    else
                    {
                        insidePoints.Add(endpointsOnRow.First());
                        //inside++;
                    }
                    continue;
                }


                var zip = endpointsOnRow.Zip(endpointsOnRow.Skip(1));

                if (rowGroup.Key == 0 || rowGroup.Key == maxX)
                {
                    var endZip = new List<((int, int), (int, int))>();
                    while (true)
                    {
                        if (!zip.Any())
                        {
                            break;
                        }
                        var zipT = zip.TakeWhile(z => z.Second.Item2 - z.First.Item2 == 1);
                        if (!zipT.Any())
                        {
                            endZip.Add(zip.First());
                            zip = zip.Skip(1);
                            continue;
                        }
                        var newZip = (zipT.First().First, zipT.Last().Second);
                        zip = zip.Skip(zipT.Count());
                        endZip.Add(newZip);
                    }
                    zip = endZip;
                }

                foreach (var tuple in zip)
                {
                    var prev = tuple.First;
                    var curr = tuple.Second;
                    var len = curr.Item2 - prev.Item2 - 1;
                    var edgesBetween = allEdgesOnRow.Where(t => t.Item2 > prev.Item2 && t.Item2 < curr.Item2);

                    var allBetweenAreEdges = edgesBetween.Count() > 1 && edgesBetween.Count() == len;
                    var rowAbove = (curr.Item1-1, curr.Item2);
                    var rowAboveIsInside = insidePoints.Contains(rowAbove) || (current == "outside" && edges.Contains(rowAbove));
                    var shouldSwitchOutsideInside = edgesBetween.Any();

                    shouldSwitchOutsideInside = shouldSwitchOutsideInside && 
                        ((current == "outside" && rowAboveIsInside) || (current == "inside" && !rowAboveIsInside)) 
                        ? true 
                        : false;

                    if (shouldSwitchOutsideInside)
                    {
                        if (current == "outside")
                        {
                            outside ++;
                        }
                        else
                        {
                            insidePoints.Add(prev);
                            //inside ++;
                        }
                        current = current == "outside" ? "inside" : "outside";
                    }
                    else
                    {
                        if (allBetweenAreEdges)
                        {
                            continue;
                        }
                        var l = (curr.Item2 - prev.Item2);
                        if (curr == endpointsOnRow.Last() || l > 1)
                        {
                            l++;
                        }
                        if (current == "outside")
                        {
                            outside += l;
                        }
                        else
                        {
                            insidePoints.UnionWith(Enumerable.Range(prev.Item2, l).Select(t => (rowGroup.Key, t)));
                            //inside += l;
                        }
                    }
                }
            }
            inside += insidePoints.Count();

            Console.WriteLine(inside + edges.Distinct().Count());

            Console.WriteLine(totalArea - outside);

            var allPoints = Enumerable.Range(minX, Math.Abs(maxX - minX) + 1)
              .SelectMany(x => Enumerable.Range(minY, Math.Abs(maxY - minY) + 1).Select(y => (x, y)));

            var areas = new List<Area>();

            //for (var i = minX; i < Math.Abs(maxX - minX) + 1;  i++)
            //{
            //    for (var j = minY; j < Math.Abs(maxY - minY) + 1; j++)
            //    {
            //        var cand = (i, j);
            //        if (edges.Contains(cand))
            //        {
            //            continue;
            //        }
            //        //Console.WriteLine(cand);
            //        var a = areas.FirstOrDefault(a => a.IsNextToArea(cand.Item1, cand.Item2));
            //        if (a != null)
            //        {
            //            a.members.Add(cand);
            //        }
            //        else
            //        {
            //            var newArea = new Area()
            //            {
            //                members = new HashSet<(int, int)>()
            //            {
            //                cand
            //            }
            //            };
            //            areas.Add(newArea);
            //        }
            //    }
            //}

            var liens = new List<string>();

            for (var x = minX; x <= maxX; x++)
            {
                var line = "";
                for (var y = minY; y <= maxY; y++)
                {
                    var point = (x, y);
                    if (edges.Contains(point) && insidePoints.Contains(point))
                    {
                        throw new Exception($"Cannot be in both lists: {point}");
                    }
                    if (edges.Contains(point))
                    {
                        line += "#";
                    }
                    else if (insidePoints.Contains(point))
                    {
                        line += "O";
                    }
                    else
                    {
                        line += ".";
                    }
                }
                liens.Add(line);
            }

            File.WriteAllLines(@"..\..\..\input\day18output-inside.txt", liens);

            var areaList = areas.ToArray();

            var mergedAreas = new HashSet<Area>();
            //var removedAreas = new HashSet<int>();

            var mergedInThisIteration = 1;

            while (mergedInThisIteration > 0)
            {
                mergedInThisIteration = 0;
                mergedAreas = new HashSet<Area>();
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
                            mergedInThisIteration++;
                            removedAreas.Add(j);
                            newArea.members.UnionWith(areaB.members);
                        }
                    }
                    mergedAreas.Add(newArea);
                }
                areaList = new List<Area>(mergedAreas).ToArray();
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
                => AllEndPoints().Any(m => Math.Abs(m.Item1 - x) + Math.Abs(m.Item2 - y) == 1);

            public IEnumerable<(int, int)> AllOnRow(int row)
                => members.Where(x => x.Item1 == row);
            
            public IEnumerable<(int, int)> AllEndPointsOnRows()
            {
                var rows = members.Select(x => x.Item1).Distinct();
                var endpoints = new List<(int, int)>();
                foreach (var r in rows)
                {
                    var allOnRow = AllOnRow(r).OrderBy(r => r);
                    var zipped = allOnRow.Zip(allOnRow.Skip(1));//.Select(r => r.Second.Item2 - r.First.Item2);
                    var gaps = zipped.Where(r => Math.Abs(r.Second.Item2) - Math.Abs(r.First.Item2) > 1);
                    endpoints.Add(allOnRow.First());
                    endpoints.AddRange(gaps.SelectMany(r => new[] {r.First, r.Second}));
                    endpoints.Add(allOnRow.Last());
                }
                return endpoints;
            }

            public IEnumerable<(int, int)> AllOnColumn(int column)
                => members.Where(x => x.Item2 == column);

            public IEnumerable<(int, int)> AllEndPointsOnColumn()
            {
                var columns = members.Select(x => x.Item2).Distinct();
                var endpoints = new List<(int, int)>();
                foreach (var r in columns)
                {
                    var allOnColumn = AllOnColumn(r).OrderBy(r => r);
                    var zipped = allOnColumn.Zip(allOnColumn.Skip(1));//.Select(r => r.Second.Item2 - r.First.Item2);
                    var gaps = zipped.Where(r => Math.Abs(r.Second.Item1) - Math.Abs(r.First.Item1) > 1);
                    endpoints.Add(allOnColumn.First());
                    endpoints.AddRange(gaps.SelectMany(r => new[] { r.First, r.Second }));
                    endpoints.Add(allOnColumn.Last());
                }
                return endpoints;
            }

            public HashSet<(int, int)> AllEndPoints()
            {
                var rowEndpoints = AllEndPointsOnRows();
                var columnEndpoints = AllEndPointsOnColumn();

                var set = new HashSet<(int, int)>(rowEndpoints);
                set.UnionWith(columnEndpoints);
                return set;
            }
        }

        public class DigIns
        {
            public string Dir;
            public int Meters;
            public string Color;
        }
    }
}
