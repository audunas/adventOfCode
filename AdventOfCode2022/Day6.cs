using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day6
    {

        public Day6()
        {
            var lines = File.ReadLines(@"..\..\..\input\day6.txt");

            var line = lines.First();

            var currentString = new Queue<char>();
            var counter = 0;

            foreach (var ch in line)
            {
                counter++;
                currentString.Enqueue(ch);

                if (currentString.Count() > 13) // Part 1 = 3 and Part 2 = 13
                {
                    if (currentString.Distinct().Count() == currentString.Count())
                    {
                        Console.WriteLine(counter);
                        break;
                    }
                    currentString.Dequeue();
                }
            }
        }
    }
}