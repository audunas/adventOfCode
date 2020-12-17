using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day10
    {
        public Day10()
        {
            var lines = File.ReadLines(@"..\..\input\day10.txt").ToList();
            var numbers = lines.Select(s => int.Parse(s)).OrderBy(s => s).ToList();

            // Part 1
            numbers.Insert(0, 0);
            numbers.Add(numbers.Max() + 3);

            var listOfDiffs = numbers.Select((a, index) => {
                if (index > 0)
                {
                    return numbers.ElementAt(index) - numbers.ElementAt(index - 1);
                }
                return (long?)null;
            }).Where(r => r != null);


            Console.WriteLine(listOfDiffs.Where(n => n == 1).Count()* listOfDiffs.Where(n => n == 3).Count());

            // Part 2
            numbers = lines.Select(s => int.Parse(s)).OrderBy(s => s).ToList();
            numbers.Add(numbers.Max() + 3);

            long numberOfPossibilities = 1;

            var indicesOfNumbersOnEdgeof3Difference = new List<int>();
            var numbersOnEdgeof3Difference = new List<int>();

            for (var i = numbers.Count()-1; i > 0; i--)
            {
                var last = numbers.ElementAt(i);
                var before = numbers.ElementAt(i - 1);
                var diff = last - before;
                if (diff == 3)
                {
                    indicesOfNumbersOnEdgeof3Difference.Add(i);
                    indicesOfNumbersOnEdgeof3Difference.Add(i - 1);
                    numbersOnEdgeof3Difference.Add(last);
                    numbersOnEdgeof3Difference.Add(before);
                }
            }

            var optionalIndices = Enumerable.Range(0, numbers.Count()).Except(indicesOfNumbersOnEdgeof3Difference);
            var optionalNumbers = numbers.Except(numbersOnEdgeof3Difference);
            var groups = new List<List<int>>();
            var group = new List<int>();
            for (var j = optionalNumbers.Count() - 1; j > 0; j--)
            {
                var numberA = optionalNumbers.ElementAt(j);
                var numberB = optionalNumbers.ElementAt(j-1);
                group.Add(numberA);
                group.Add(numberB);
                if (numberA - numberB > 1)
                {
                    group.Remove(numberB);
                    groups.Add(group.OrderBy(r => r).Distinct().ToList());
                    group = new List<int>();
                }
            }

            groups.Add(group.Distinct().ToList());

            foreach (var g in groups)
            {
                var sequence = new List<int>(g);
                var allPermutations = getPerms(sequence);
                var min = sequence.Min() - 1;
                var max = sequence.Max() + 1;
                var allValidPermuations = allPermutations.Where(a => IsValid(a, min, max));
                if (allValidPermuations.Any())
                {
                    numberOfPossibilities *= allValidPermuations.Count();
                }

            }

            Console.WriteLine(numberOfPossibilities);
        }

        private static bool IsValid(int[] a, int min, int max)
        {
            if (max-min <= 3)
            {
                return true;
            }
            if (!a.Any() && ((max-min) > 3) )
            {
                return false;
            }

            var aMin = a.Min();
            var aMax = a.Max();
            return (a.Min() - min <= 3) && (max - a.Max() <= 3);
        }

        private static IEnumerable<int[]> getPerms(List<int> sequence)
        {
            int[] data = sequence.ToArray();

            return Enumerable
              .Range(0, 1 << (data.Length))
              .Select(index => data
                 .Where((v, i) => (index & (1 << i)) != 0)
                 .ToArray());
        }

    }
}
