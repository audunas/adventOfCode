using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Day15
    {
        public Day15()
        {
            var lines = File.ReadLines(@"..\..\input\day15.txt").ToList();
            var startingNumbers = lines.First().Split('j').Select(l => int.Parse(l));
            var numbersSpoken = new List<int>();
            var counter = 0;
            var currentNumber = 0;

            while (counter < 2020)
            {

            }

            Console.WriteLine(currentNumber);
        }
    }
}
