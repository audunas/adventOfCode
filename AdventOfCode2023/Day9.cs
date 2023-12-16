using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day9
    {
        public Day9()
        {
            Console.WriteLine("Day9");

            var lines = File.ReadLines(@"..\..\..\input\day9.txt");

            var histories = lines.Select(l => l.Split(' ').Select(c => int.Parse(c)).ToList()).ToList();

            var sum = 0;

            //Part 1

            foreach (var history in histories)
            {
                var listOfSeq = new List<List<int>>() { history };
                var current = listOfSeq.First();

                while (!listOfSeq.Last().All(c => c == 0))
                {
                    var listOfDiffs = current.Zip(current.Skip(1)).Select(c => c.Second - c.First).ToList();
                    listOfSeq.Add(listOfDiffs);

                    current = listOfDiffs;
                }
                
                var arrayOfSeq = listOfSeq.ToArray(); ;

                for (var i = arrayOfSeq.Count(); i>1; i--)
                {
                    var previous = arrayOfSeq[i - 1].Last();
                    var currentValue = arrayOfSeq[i - 2].Last();
                    arrayOfSeq[i - 2].Add(previous + currentValue);
                }

                sum += arrayOfSeq.First().Last();
            }

            Console.WriteLine(sum);

            sum = 0;

            //Part 2

            foreach (var history in histories)
            {
                var listOfSeq = new List<List<int>>() { history };
                var current = listOfSeq.First();

                while (!listOfSeq.Last().All(c => c == 0))
                {
                    var listOfDiffs = current.Zip(current.Skip(1)).Select(c => c.Second - c.First).ToList();
                    listOfSeq.Add(listOfDiffs);

                    current = listOfDiffs;
                }

                var arrayOfSeq = listOfSeq.ToArray(); ;

                for (var i = arrayOfSeq.Count(); i > 1; i--)
                {
                    var previous = arrayOfSeq[i - 1].First();
                    var currentValue = arrayOfSeq[i - 2].First();
                    arrayOfSeq[i - 2] = arrayOfSeq[i - 2].Prepend(currentValue - previous).ToList();
                }

                sum += arrayOfSeq.First().First();
            }

            Console.WriteLine(sum);
        }
    }
}
