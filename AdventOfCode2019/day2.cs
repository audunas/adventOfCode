using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day2
    {
        public Day2()
        {
            var line = File.ReadLines(@"..\..\..\input\day2.txt").First();

            //Part1
            var numbers = line.Split(',').Select(n => int.Parse(n)).ToArray();

            numbers[1] = 12;
            numbers[2] = 2;

            var endState = FindEndState(numbers);

            var position0 = endState[0];

            Console.WriteLine(position0);

            //Part2
            while (true)
            {
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        numbers = line.Split(',').Select(n => int.Parse(n)).ToArray();
                        numbers[1] = i;
                        numbers[2] = j;

                        endState = FindEndState(numbers);

                        position0 = endState[0];

                        if (position0 == 19690720)
                        {
                            Console.WriteLine(position0);
                            var res = 100 * i + j;
                            Console.WriteLine("Result:" + res);
                            Console.ReadLine();
                        }
                    }
                }
            }
        }

        public static int[] FindEndState(int[] numbers, int startIndex = 0)
        {
            if (startIndex >= numbers.Length) return numbers;

            var operation = numbers[startIndex];
            if (operation == 99) return numbers;

            var firstInputPos = numbers[startIndex+1];
            var secondInputPos = numbers[startIndex+2];

            var firstValue = numbers[firstInputPos];
            var secondValue = numbers[secondInputPos];

            var valueToStore = 0;
            switch (operation)
            {
                case 1:
                    valueToStore = firstValue + secondValue;
                    break;
                case 2:
                    valueToStore = firstValue * secondValue;
                    break;
                default:
                    return numbers;
            }

            var outputPos = numbers[startIndex+3];

            numbers[outputPos] = valueToStore;

            return FindEndState(numbers, startIndex+4);
        }

    }
}
