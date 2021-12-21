using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day1
    {
        public Day1()
        {
            var lines = File.ReadLines(@"..\..\..\input\day1.txt").Select(l => int.Parse(l));

            //Part 1
            var z = lines.Zip(lines.Skip(1).ToList());
            var numbersIncreasing = z.Where(z => z.Second > z.First);
            Console.WriteLine(numbersIncreasing.Count());

            //Part 2
            var zipped = lines.Zip(lines.Skip(1).ToList(), (x, y) => x+y).Zip(lines.Skip(2).ToList(), (x, y) => x+y);
            var compareZip = zipped.Zip(zipped.Skip(1).ToList());
            var increasing = compareZip.Where(z => z.Second > z.First);
            Console.WriteLine(increasing.Count());
        }
    }
}
