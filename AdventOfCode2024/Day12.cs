using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day12
    {

        public Day12()
        {
            Console.WriteLine("Day12");

            var lines = File.ReadLines(@"..\..\..\input\day12.txt").ToArray();


            var coord = new Dictionary<(int, int), char>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    coord.Add((i, j), line[j]);
                }
            }

            var regions = new List<Region>();
            var idCounter = 0;

            foreach (var g in coord.GroupBy(g => g.Value))
            {
                var newRegions = new List<Region>();

                foreach (var point in g)
                {
                    newRegions.Add(new Region() { Id = idCounter, Points = [point.Key], Character = g.Key }); ;
                    idCounter++;
                }

                var nextRegions = newRegions;

                var removedRegions = new List<int>();

                var anyMerge = false;

                do
                {
                    removedRegions = new List<int>();
                    var regionsForNextIteration = new List<Region>();
                    anyMerge = false;
                    for (int i = 0; i < nextRegions.Count(); i++)
                    {
                        var r1 = nextRegions[i];
                        if (removedRegions.Contains(r1.Id))
                        {
                            continue;
                        }
                        var newRegion = new Region()
                        {
                            Id = r1.Id,
                            Character = r1.Character,
                            Points = new HashSet<(int, int)>(r1.Points)
                        };
                        for (int j = i + 1; j < nextRegions.Count(); j++)
                        {
                            var r2 = nextRegions[j];
                            if (r2.Character != r1.Character || removedRegions.Contains(r2.Id))
                            {
                                continue;
                            }
                            var anyNeighbors = 
                                r1.Points.Any(r1p => 
                                    r2.Points.Any(r2p => 
                                    (r1p.Item1 == r2p.Item1 
                                        && (r1p.Item2 + 1 == r2p.Item2 || r1p.Item2 - 1 == r2p.Item2))
                                    ||
                                    (r1p.Item2 == r2p.Item2
                                        && (r1p.Item1 + 1 == r2p.Item1 || r1p.Item1 - 1 == r2p.Item1)))
                                );
                            if (anyNeighbors)
                            {
                                newRegion.Points.UnionWith(r2.Points);
                                removedRegions.Add(r2.Id);
                                anyMerge = true;
                            }
                        }
                        regionsForNextIteration.Add(newRegion);
                    }
                    nextRegions = regionsForNextIteration;
                } while (anyMerge);



                regions.AddRange(nextRegions);
            }

            var sum = 0;
            var sumPart2 = 0;

            foreach (var re in regions)
            {
                var neighborsPoints = new HashSet<(int, int, int)>();
                foreach (var p in re.Points)
                {
                    var up = (p.Item1-1, p.Item2);
                    var down = (p.Item1+1, p.Item2);
                    var left = (p.Item1, p.Item2 - 1);
                    var right = (p.Item1, p.Item2 + 1);

                    if (!re.Points.Contains(up))
                        neighborsPoints.Add((up.Item1, up.Item2, 0));
                    if (!re.Points.Contains(down))
                        neighborsPoints.Add((down.Item1, down.Item2, 1));
                    if (!re.Points.Contains(left))
                        neighborsPoints.Add((left.Item1, left.Item2, 2));
                    if (!re.Points.Contains(right))
                        neighborsPoints.Add((right.Item1, right.Item2, 3));
                }

                var groupedHorizontal = neighborsPoints
                    .Where(n => new List<int>() { 0, 1}.Contains(n.Item3))
                    .GroupBy(t => new {t.Item1, t.Item3}, t => t);
                var groupedVertical = neighborsPoints
                    .Where(n => new List<int>() { 2, 3 }.Contains(n.Item3))
                    .GroupBy(t => new {t.Item2, t.Item3}, t => t);

                sum += (re.Points.Count * neighborsPoints.Count);

                var sides = 0;

                foreach (var g in groupedHorizontal)
                {
                    sides++;
                    var orderedG = g.OrderBy(g => g.Item2).ToArray();
                    for (var i = 1; i< orderedG.Count(); i++)
                    {
                        if (orderedG[i].Item2 - orderedG[i-1].Item2 > 1)
                        {
                            sides++;
                        }
                    }
                }

                foreach (var g in groupedVertical)
                {
                    sides++;
                    var orderedG = g.OrderBy(g => g.Item1).ToArray();
                    for (var i = 1; i < orderedG.Count(); i++)
                    {
                        if (orderedG[i].Item1 - orderedG[i - 1].Item1 > 1)
                        {
                            sides++;
                        }
                    }
                }

                sumPart2 += (re.Points.Count * sides);
            }

            Console.WriteLine(sum);
            Console.WriteLine(sumPart2);
        }

        public record Region
        {
            public int Id;
            public char Character;
            public HashSet<(int, int)> Points = [];
        }
    }
}
