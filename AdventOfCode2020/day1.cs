using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day1
    {
        public Day1()
        {
            var lines = File.ReadLines(@"..\..\input\day1.txt").Select(l => int.Parse(l));

            var product = FindProduct(lines.ToList());

            Console.WriteLine(product);
        }

        private int FindProduct(List<int> numbers)
        {
            const int wantedSum = 2020;

            for (int num=0; num < numbers.Count; num++)
            {
                var firstNum = numbers[num];
                for (int num2=0; num2 < numbers.Count(); num2++)
                {
                    var secondNum = numbers[num2];
                    for (int num3 = 0; num3 < numbers.Count(); num3++)
                    {
                        var thirdNum = numbers[num3];
                        if (num != num2 && num != num3)
                        {
                            var isWantedEqualSum = (firstNum + secondNum + thirdNum) == wantedSum;
                            if (isWantedEqualSum)
                            {
                                return firstNum * secondNum * thirdNum;
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }
}
