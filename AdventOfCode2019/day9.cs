using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    public class Day9
    {
        public static long InputValue = 2;//part1=1;
        public static long RelativeBase = 0;

        public Day9()
        {
            var lines = File.ReadLines(@"..\..\..\input\day9.txt").First();
            var memory = new long[100000000];
            var index = (long)0;
            foreach(var inst in lines.Split(',')){
                memory[index] = long.Parse(inst);
                index++;
            }
            var endstate = FindEndState(memory, InputValue);

            
        }

        public static long GetValueBasedOnParameter(long[] numbers, long[] parameters, long index, long paramIndex)
        {
            var parameter = numbers[index];
            if (parameters[paramIndex] == 0)
            {
                return numbers[parameter];
            }
            else if (parameters[paramIndex] == 1)
            {
                return parameter;
            }
            else if (parameters[paramIndex] == 2)
            {
                return numbers[RelativeBase + parameter];
            }

            return parameter;
        }

        public static long GetSetAddress(long[] numbers, long[] parameters, long index, long paramIndex)
        {
            var parameter = numbers[index];
            if (parameters[paramIndex] == 0)
            {
                return parameter;
            }
            else if (parameters[paramIndex] == 1)
            {
                return parameter;
            }
            else if (parameters[paramIndex] == 2)
            {
                return RelativeBase + parameter;
            }

            return parameter;
        }

        public static long[] FindEndState(long[] numbers, long inputValue)
        {
            long instructionPointer = 0;

            var operation = numbers[instructionPointer];

            while (operation != 99)
            {
                var (opCode, parameters) = parseOperation(operation);

                var firstValue = GetValueBasedOnParameter(numbers, parameters, instructionPointer + 1, 0);
                var valueToStore = (long)0;
               
                switch (opCode)
                {
                    case 1:
                        var secondValue = GetValueBasedOnParameter(numbers, parameters, instructionPointer + 2, 1);
                        valueToStore = firstValue + secondValue;
                        var outputPos = GetSetAddress(numbers, parameters, instructionPointer + 3, 2);
                        numbers[outputPos] = valueToStore;
                        instructionPointer += 4;
                        break;
                    case 2:
                        secondValue = GetValueBasedOnParameter(numbers, parameters, instructionPointer + 2, 1);
                        valueToStore = firstValue * secondValue;
                        outputPos = GetSetAddress(numbers, parameters, instructionPointer + 3, 2);
                        numbers[outputPos] = valueToStore;
                        instructionPointer += 4;
                        break;
                    case 3:
                        outputPos = GetSetAddress(numbers, parameters, instructionPointer + 1, 0);
                        numbers[outputPos] = InputValue;
                        instructionPointer += 2;
                        break;
                    case 4:
                        Console.WriteLine(firstValue);
                        instructionPointer += 2;
                        break;
                    case 5:
                        secondValue = GetValueBasedOnParameter(numbers, parameters, instructionPointer + 2, 1);
                        instructionPointer = firstValue != 0 ? secondValue : instructionPointer + 3;
                        break;
                    case 6:
                        secondValue = GetValueBasedOnParameter(numbers, parameters, instructionPointer + 2, 1);
                        instructionPointer = firstValue == 0 ? secondValue : instructionPointer + 3;
                        break;
                    case 7:
                        secondValue = GetValueBasedOnParameter(numbers, parameters, instructionPointer + 2, 1);
                        outputPos = GetSetAddress(numbers, parameters, instructionPointer + 3, 2);
                        numbers[outputPos] = firstValue < secondValue ? 1 : 0;
                        instructionPointer += 4;
                        break;
                    case 8:
                        secondValue = GetValueBasedOnParameter(numbers, parameters, instructionPointer + 2, 1);
                        outputPos = GetSetAddress(numbers, parameters, instructionPointer + 3, 2);
                        numbers[outputPos] = firstValue == secondValue ? 1 : 0;
                        instructionPointer += 4;
                        break;
                    case 9:
                        RelativeBase += GetValueBasedOnParameter(numbers, parameters, instructionPointer + 1, 0);
                        instructionPointer += 2;
                        break;
                    default:
                        Console.WriteLine("Unknown opCode");
                        return numbers;
                }

                operation = numbers[instructionPointer];
            }

            return numbers;
        }

        private static (long, long[]) parseOperation(long operation)
        {
            var stringVersion = operation.ToString();
            var parameterModes = new long[3];
            if (stringVersion.Length == 1)
            {
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
