using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day8
    {

        public Day8()
        {
            Console.WriteLine("Day8");

            var lines = File.ReadLines(@"..\..\..\input\day8.txt").ToArray();

            var coord = new Dictionary<(int, int), char>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    coord.Add((i, j), line[j]);
                }
            }

            var antinodes = new List<(int, int, char)>();

            var ants = coord.Where(c => c.Value != '.');

            foreach (var coo in ants)
            {
                var antenna = coo.Value;
                var otherAntennas = coord
                    .Where(c => c.Value == antenna && c.Key != coo.Key
                    && c.Key.Item1 > coo.Key.Item1);

                foreach (var o in otherAntennas)
                {
                    var x = Math.Abs(o.Key.Item1 - coo.Key.Item1);
                    var y = Math.Abs(o.Key.Item2 - coo.Key.Item2);

                    var isLeft = o.Key.Item2 < coo.Key.Item2;


                    var nextXDown = Math.Max(o.Key.Item1, coo.Key.Item1) + x;
                    var nextXUp = Math.Min(o.Key.Item1, coo.Key.Item1) - x;
                    var nextYLeft = Math.Min(o.Key.Item2, coo.Key.Item2) - y;
                    var nextYRight = Math.Max(o.Key.Item2, coo.Key.Item2) + y;

                    var multiplier = 1;

                    while (nextXDown < lines.Length ||
                        nextXUp >= 0 ||
                        nextYLeft >= 0 ||
                        nextYRight < lines[0].Length)
                    {
                        
                        var antiNode1 = (o.Key.Item1 + (x * multiplier),
                        isLeft ? o.Key.Item2 - (y * multiplier) : o.Key.Item2 + (y * multiplier),
                        antenna
                        );

                        var antiNode2 = (coo.Key.Item1 - (x * multiplier),
                           isLeft ? coo.Key.Item2 + (y * multiplier) : coo.Key.Item2 - (y * multiplier),
                           antenna
                           );

                        if (antiNode1.Item1 >= 0 && antiNode1.Item1 < lines.Length &&
                            antiNode1.Item2 >= 0 && antiNode1.Item2 < lines[0].Length)
                        {
                            antinodes.Add(antiNode1);
                        }

                        if (antiNode2.Item1 >= 0 && antiNode2.Item1 < lines.Length &&
                            antiNode2.Item2 >= 0 && antiNode2.Item2 < lines[0].Length)
                        {
                            antinodes.Add(antiNode2);
                        }

                        multiplier++;

                        nextXDown = Math.Max(o.Key.Item1, coo.Key.Item1) + (x * multiplier);
                        nextXUp = Math.Min(o.Key.Item1, coo.Key.Item1) - (x * multiplier);
                        nextYLeft = Math.Min(o.Key.Item2, coo.Key.Item2) - (y * multiplier);
                        nextYRight = Math.Max(o.Key.Item2, coo.Key.Item2) + (y * multiplier);
                    }

                    
                }
            }

            var conc = antinodes.Concat(ants.Select(a => (a.Key.Item1, a.Key.Item2, a.Value)));

            Console.WriteLine(conc.Select(a => (a.Item1, a.Item2)).Distinct().Count());
        }
    }
}
