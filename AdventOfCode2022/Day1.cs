using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day1
    {

        public Day1()
        {
            var lines = File.ReadLines(@"..\..\..\input\day1.txt").Append("");

            var currentSum = 0;
            var maxSum = 0;
            //Part 1
            foreach (var line in lines)
            {
                if (line == "")
                {
                    if (currentSum > maxSum)
                    {
                        maxSum = currentSum;
                    }
                    currentSum = 0;
                }
                else
                {
                    currentSum += int.Parse(line);
                }
            }
            Console.WriteLine(maxSum);

            //Part 2
            currentSum = 0;
            var topThree = new List<int>() { 0, 0, 0 };
            foreach (var line in lines)
            {
                if (line == "")
                {
                    var min = topThree.Min();
                    if (currentSum > min)
                    {
                        topThree.Remove(min);
                        topThree.Add(currentSum);
                    }
                    currentSum = 0;
                }
                else
                {
                    currentSum += int.Parse(line);
                }
            }
            Console.WriteLine(topThree.Sum());
        }
    }
}
