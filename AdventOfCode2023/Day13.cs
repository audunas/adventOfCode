using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day13
    {
        public Day13()
        {
            Console.WriteLine("Day13");

            var lines = File.ReadLines(@"..\..\..\input\day13.txt");

            var patterns = new List<List<string>>();

            var c = new List<string>();

            foreach (var line in lines)
            {
                if (line.Trim() == "")
                {
                    patterns.Add(c);
                    c = new List<string>();
                }
                else
                {
                    c.Add(line);
                }
            }
            patterns.Add(c);

            var sum = 0;

            foreach (var p in patterns)
            {
                //Look for horizontal
                var row = RowThatCanReflect(p);
                if (row != null)
                {
                    sum += (100 * (row.Value + 1));
                    continue;
                }

                //Look for vertical
                //Transpose from column to row
                var t = Enumerable.Range(0, p[0].Count())
                    .Select(i => new string(p.Select(lst => lst[i]).ToArray())).ToList();

                var g = t.Select(g => {
                    char[] charArray = g.ToCharArray();
                    Array.Reverse(charArray);
                    return new string(charArray);}
                ).ToList();

                row = RowThatCanReflect(g);
                if (row != null)
                {
                    sum += (row.Value + 1);
                    continue;
                }
            }

            Console.WriteLine(sum);
        }

        private static int? RowThatCanReflect(List<string> p)
        {
            var length = p.Count();
            var equalRows = new List<(int, int)>();
            var almostEqualRows = new List<(int, int)>();
            for (var i = 0; i < length; i++)
            {
                var line1 = p[i];
                for (var j = i + 1; j < length; j++)
                {
                    var line2 = p[j];
                    if (line1 == line2)
                    {
                        equalRows.Add((i, j));
                    }
                    else
                    {
                        var mismatches = 0;
                        for (var k = 0; k<line1.Length; k++)
                        {
                            var line1k = line1[k];
                            var line2k = line2[k];
                            if(line1k != line2k)
                            {
                                mismatches++;
                            }
                        }
                        if (mismatches == 1)
                        {
                            almostEqualRows.Add((i, j));
                        }
                    }
                }
            }

            var almostEqualRowMatches = new List<int>();

            foreach (var almostEqualRow in almostEqualRows)
            {
                var equ1 = new List<(int, int)>(equalRows);
                //Remove any row matches on the row we are replaced
                equ1 = equ1.Where(e => e.Item1 != almostEqualRow.Item1 && e.Item2 != almostEqualRow.Item2).ToList();
                equ1.Add(almostEqualRow);


                var matches = GetMatches(equ1, length);
                almostEqualRowMatches.AddRange(matches);
            }

            var equalMatches = GetMatches(equalRows, length);

            var m = almostEqualRowMatches.Except(equalMatches);


            return m.Any() ? m.Max(): null;
        }

        private static List<int> GetMatches(List<(int, int)> equ1, int length)
        {
            var startPoints = equ1.Where(t => t.Item2 - t.Item1 == 1);

            var matches = new List<int>();

            foreach (var st in startPoints)
            {
                var end = true;
                var current = st;
                while (end != false && (current.Item1 != 0 && current.Item2 != length - 1))
                {
                    var next = equ1.FirstOrDefault(f => f.Item1 == current.Item1 - 1 && f.Item2 == current.Item2 + 1);
                    if (next == (default, default))
                    {
                        end = false;
                    }
                    current = next;
                }
                //Reached one of the ends
                if (end)
                {
                    matches.Add(st.Item1);
                }
            }
            return matches;
        }

        //Part 1

        //private static int? RowThatCanReflect(List<string> p)
        //{
        //    var length = p.Count();
        //    var equalRows = new List<(int, int)>();
        //    for (var i = 0; i < length; i++)
        //    {
        //        var line1 = p[i];
        //        for (var j = i + 1; j < length; j++)
        //        {
        //            var line2 = p[j];
        //            if (line1 == line2)
        //            {
        //                equalRows.Add((i, j));
        //            }
        //        }
        //    }
        //    if (!equalRows.Any(c => c.Item2 - c.Item1 == 1))
        //    {
        //        return null;
        //    }

        //    var startPoints = equalRows.Where(t => t.Item2 - t.Item1 == 1);

        //    var matches = new List<int>();

        //    foreach (var st in startPoints)
        //    {
        //        var end = true;
        //        var current = st;
        //        while (end != false && (current.Item1 != 0 && current.Item2 != length - 1))
        //        {
        //            var next = equalRows.FirstOrDefault(f => f.Item1 == current.Item1 - 1 && f.Item2 == current.Item2 + 1);
        //            if (next == (default, default))
        //            {
        //                end = false;
        //            }
        //            current = next;
        //        }
        //        //Reached one of the ends
        //        if (end)
        //        {
        //            matches.Add(st.Item1);
        //        }
        //    }

        //    return matches.Any() ? matches.Max() : null;

        //}
    }
}
