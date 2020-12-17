using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day9
    {
        public Day9()
        {
            var lines = File.ReadLines(@"..\..\input\day9.txt").ToList();
            var numbers = lines.Select(s => long.Parse(s));

            var preambleLength = 25;

            long invalidNumber = 0;

            // Part 1
            for (var i = preambleLength; i < numbers.Count(); i++)
            {
                var currentPreamble = numbers.Skip(i-preambleLength).Take(preambleLength);
                var currentNumber = numbers.ElementAt(i);
                var possibleSums = currentPreamble.SelectMany(a => currentPreamble.Skip(1).Select(b => a+b));
                if (!possibleSums.Contains(currentNumber))
                {
                    invalidNumber = currentNumber;
                    break;
                }
            }

            Console.WriteLine(invalidNumber);

            var endReached = false;
            // Part 2
            for(var startIndex = 0; startIndex < numbers.Count(); startIndex++)
            {
                var numbersInRange = numbers.Skip(startIndex).Take(2).ToList();
                var sum = numbersInRange.Sum();
                var nextIndex = startIndex + 2;
                while (sum < invalidNumber)
                {
                    numbersInRange.Add(numbers.ElementAt(nextIndex));
                    sum = numbersInRange.Sum();
                    if (sum == invalidNumber)
                    {
                        endReached = true;
                        var smallestNumber = numbersInRange.Min();
                        var largestNumber = numbersInRange.Max();
                        Console.WriteLine($"Smallest number in range: {smallestNumber}");
                        Console.WriteLine($"Largest number in range: {largestNumber}");
                        Console.WriteLine($"Sum: {smallestNumber+largestNumber}");
                    }
                    nextIndex++;
                }
                if (endReached)
                {
                    break;
                }
            }

        }
    }
}
