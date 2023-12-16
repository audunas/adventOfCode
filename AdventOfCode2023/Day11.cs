using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day11
    {
        public Day11()
        {
            Console.WriteLine("Day11");

            var lines = File.ReadLines(@"..\..\..\input\day11.txt").ToArray();

            var rowExpanded = new List<string>();
            var rowsToExpand = new List<int>();

            for (var i = 0; i<lines.Length; i++)
            {
                var line = lines[i];
                rowExpanded.Add(line);
                if (line.All(l => l == '.'))
                {
                    rowExpanded.Add(line);
                    rowsToExpand.Add(i);
                }
            }

            var colToExpand = new List<int>();
            for (var i = 0; i < rowExpanded.First().Length; i++)
            {
                var allRowIsExpanded = rowExpanded.Select(t => t.ToArray()[i]).All(t => t == '.');
                if (allRowIsExpanded)
                {
                    colToExpand.Add(i);
                }
            }
            var final = new List<string>();

            foreach (var line in rowExpanded)
            {
                var l = line;
                var c = 0;
                foreach (var col in colToExpand)
                {
                    l = l.Insert(col + c, ".");
                    c++;
                }
                final.Add(l);
            }

            var galaxies = new HashSet<(int, int)>();

            for(int x = 0; x< lines.Length; x++)
            {
                var line = lines.ToArray()[x];
                for (int y = 0; y< line.Length; y++)
                {
                    if (line.ToCharArray()[y] == '#')
                    {
                        galaxies.Add((x, y));
                    }
                }
            }

            var galArr = galaxies.ToArray();

            var sum = 0l;

            for (int i = 0; i < galArr.Length; i++)
            {
                for (int x = i+1; x < galArr.Length; x++)
                {
                    var gal1 = galArr[i];
                    var gal2 = galArr[x];

                    var maxX = Math.Max(gal1.Item1, gal2.Item1);
                    var maxY = Math.Max(gal1.Item2, gal2.Item2);

                    var minX = Math.Min(gal1.Item1, gal2.Item1);
                    var minY = Math.Min(gal1.Item2, gal2.Item2);

                    var extraRowsBetween = rowsToExpand.Where(r => minX < r && r < maxX);
                    var extraColumnsBetween = colToExpand.Where(c => minY < c && c < maxY);

                    //part 1 = replace 999999 with 1
                    var length = Math.Abs(gal2.Item2 - gal1.Item2) + (extraRowsBetween.Count() * 999999l)  
                        + Math.Abs(gal2.Item1 - gal1.Item1) + (extraColumnsBetween.Count() * 999999l);

                    sum += length;
                }
            }

            Console.WriteLine(sum);
        }
    }
}
