using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day5
    {
        public Day5()
        {
            var lines = File.ReadLines(@"..\..\..\input\day5.txt");

            var vents = lines.Select(l =>
            {
                var split = l.Split("->");
                var xSplit = split[0].Trim().Split(",");
                var x1 = int.Parse(xSplit[0]);
                var y1 = int.Parse(xSplit[1]);

                var ySplit = split[1].Trim().Split(",");
                var x2 = int.Parse(ySplit[0]);
                var y2 = int.Parse(ySplit[1]);

                return new Vent { x1 = x1, x2 = x2, y1 = y1, y2 = y2 };
            });

            //Part 1
            var posCounter1 = new Dictionary<Tuple<int, int>, int>();
            var posCounter2 = new Dictionary<Tuple<int, int>, int>();

            foreach (var vent in vents)
            {
                if (vent.x1 == vent.x2)
                {
                    var startY = Math.Min(vent.y1, vent.y2);
                    var endY = Math.Max(vent.y1, vent.y2);
                    for (int i = startY; i <= endY; i++)
                    {
                        var pos = Tuple.Create(vent.x1, i);
                        if (posCounter1.ContainsKey(pos))
                        {
                            posCounter1[pos] += 1;
                        }
                        else
                        {
                            posCounter1.Add(pos, 1);
                        }
                        if (posCounter2.ContainsKey(pos))
                        {
                            posCounter2[pos] += 1;
                        }
                        else
                        {
                            posCounter2.Add(pos, 1);
                        }
                    }
                }
                else if (vent.y1 == vent.y2)
                {
                    var startX = Math.Min(vent.x1, vent.x2);
                    var endX = Math.Max(vent.x1, vent.x2);
                    for (int i = startX; i <= endX; i++)
                    {
                        var pos = Tuple.Create(i, vent.y1);
                        if (posCounter1.ContainsKey(pos))
                        {
                            posCounter1[pos] += 1;
                        }
                        else
                        {
                            posCounter1.Add(pos, 1);
                        }
                        if (posCounter2.ContainsKey(pos))
                        {
                            posCounter2[pos] += 1;
                        }
                        else
                        {
                            posCounter2.Add(pos, 1);
                        }
                    }
                }
                else
                {
                    var startX = Math.Min(vent.x1, vent.x2);
                    var endX = Math.Max(vent.x1, vent.x2);
                    var firstY = startX == vent.x1 ? vent.y1 : vent.y2;
                    var shouldAdd = startX == vent.x1 && vent.y2 > vent.y1
                        || startX == vent.x2 && vent.y1 > vent.y2;
                    var counter = 0;
                    for (int i = startX; i <= endX; i++)
                    {
                        var y = shouldAdd 
                            ? firstY + counter 
                            : firstY - counter;
                        var pos = Tuple.Create(i, y);
                        if (posCounter2.ContainsKey(pos))
                        {
                            posCounter2[pos] += 1;
                        }
                        else
                        {
                            posCounter2.Add(pos, 1);
                        }
                        counter++;
                    }
                }
            }

            Console.WriteLine(posCounter1.Where(p => p.Value > 1).Count());

            Console.WriteLine(posCounter2.Where(p => p.Value > 1).Count());

        }

        public struct Vent
        {
            public int x1;
            public int x2;
            public int y1;
            public int y2;
        }
    }
}
