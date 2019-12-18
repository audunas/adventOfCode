using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day5
    {
        public static int InputValue = 5;//part1: 1;

        public Day5()
        {
            var lines = File.ReadLines(@"..\..\..\input\day5.txt").First();

            var instructions = lines.Split(',').Select(n => int.Parse(n));

            FindEndState(instructions.ToArray(), InputValue);

        }

        public static int[] FindEndState(int[] numbers, int inputValue, int index = 0)
        {
            if (index >= numbers.Length) return numbers;

            var operation = numbers[index];
            if (operation == 99) return numbers;

            var (opCode, parameters) = parseOperation(operation);

            var firstParameter = numbers[index + 1];
            var firstValue = parameters[0] == 0 ? numbers[firstParameter] : firstParameter;
            var valueToStore = 0;
            var instructionPointer = index + 4;
            switch (opCode)
            {
                case 1:
                    var secondParameter = numbers[index + 2];
                    var secondValue = parameters[1] == 0 ? numbers[secondParameter] : secondParameter;
                    valueToStore = firstValue + secondValue;
                    var outputPos = numbers[index + 3];
                    numbers[outputPos] = valueToStore;
                    break;
                case 2:
                    secondParameter = numbers[index + 2];
                    secondValue = parameters[1] == 0 ? numbers[secondParameter] : secondParameter;
                    valueToStore = firstValue * secondValue;
                    outputPos = numbers[index + 3];
                    numbers[outputPos] = valueToStore;
                    break;
                case 3:
                    numbers[firstParameter] = InputValue;
                    instructionPointer = index + 2;
                    break;
                case 4:
                    Console.WriteLine(firstValue);
                    instructionPointer = index + 2;
                    break;
                case 5:
                    secondParameter = numbers[index + 2];
                    secondValue = parameters[1] == 0 ? numbers[secondParameter] : secondParameter;
                    instructionPointer = firstValue != 0 ? secondValue : index + 3;
                    break;
                case 6:
                    secondParameter = numbers[index + 2];
                    secondValue = parameters[1] == 0 ? numbers[secondParameter] : secondParameter;
                    instructionPointer = firstValue == 0 ? secondValue : index + 3;
                    break;
                case 7:
                    secondParameter = numbers[index + 2];
                    secondValue = parameters[1] == 0 ? numbers[secondParameter] : secondParameter;
                    outputPos = numbers[index + 3];
                    numbers[outputPos] = firstValue < secondValue ? 1 : 0;
                    break;
                case 8:
                    secondParameter = numbers[index + 2];
                    secondValue = parameters[1] == 0 ? numbers[secondParameter] : secondParameter;
                    outputPos = numbers[index + 3];
                    numbers[outputPos] = firstValue == secondValue ? 1 : 0;
                    break;
                default:
                    return numbers;
            }

            Console.WriteLine($"OpCode: {operation}, instructionPointer: {instructionPointer}");

            return FindEndState(numbers, inputValue, instructionPointer);
        }

        private static (int, int[]) parseOperation(int operation)
        {
            var stringVersion = operation.ToString();
            var parameterModes = new int[3];
            if (stringVersion.Length == 1) {
                return (operation, parameterModes);
            }
            var opCode = int.Parse(stringVersion.Substring(stringVersion.Length - 2, 2));
            int pos = 0;
            for (int i = stringVersion.Length - 3; i > -1; i--)
            {
                var num = stringVersion.ElementAt(i).ToString();
                parameterModes[pos] = int.Parse(num);
                pos++;
            }

            return (opCode, parameterModes);
        }
    }
}
