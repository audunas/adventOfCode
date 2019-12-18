using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day11
    {
        public static long InputValue = 0;
        public static long RelativeBase = 0;

        public Day11()
        {
            var lines = File.ReadLines(@"..\..\..\input\day11.txt").First();
            var memory = new long[100000000];
            var index = (long)0;
            foreach (var inst in lines.Split(','))
            {
                memory[index] = long.Parse(inst);
                index++;
            }
            //Part1
            FindEndState(memory);

            Console.WriteLine(DrawnPanels.Count);

            //Part2
            DrawnPanels = new HashSet<Point>();
            WhitePanels = new HashSet<Point>();
            var startPoint = new Point(robotX, robotY);
            WhitePanels.Add(startPoint);
            //DrawnPanels.Add(startPoint);
            InputValue = 1;
            FindEndState(memory);

            var maxX = DrawnPanels.Max(p => p.x);
            var maxY = DrawnPanels.Max(p => p.y);
            var minX = DrawnPanels.Min(p => p.x);
            var minY = DrawnPanels.Min(p => p.y);

            for (int y = maxY; y>= minY; y--)
            {
                for (int x = minX; x<= maxX; x++)
                {
                    var point = new Point(x, y);
                    if (WhitePanels.Contains(point))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }

        }

        public static HashSet<Point> WhitePanels = new HashSet<Point>();
        public static HashSet<Point> DrawnPanels = new HashSet<Point>();
        
        public struct Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public static int robotX = 0;
        public static int robotY = 0;
        public static Direction robotDirection = Direction.Up;

        public enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        public static Direction GetNextDirection(bool shouldTurnLeft)
        {
            switch (robotDirection)
            {
                case Direction.Up:
                    return shouldTurnLeft ? Direction.Left : Direction.Right;
                case Direction.Left:
                    return shouldTurnLeft ? Direction.Down : Direction.Up;
                case Direction.Down:
                    return shouldTurnLeft ? Direction.Right : Direction.Left;
                case Direction.Right:
                    return shouldTurnLeft ? Direction.Up : Direction.Down;
                default:
                    return Direction.Up;
            };
        }
       
        public static long[] FindEndState(long[] numbers)
        {
            long instructionPointer = 0;

            var operation = numbers[instructionPointer];

            var isFirstOutputValue = true;

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
                        //Console.WriteLine(firstValue);

                        var currentPosition = new Point(robotX, robotY);
                        //Paint
                        if (isFirstOutputValue)
                        {
                            if (firstValue == 0)
                            {
                                //Paint black
                                WhitePanels.Remove(currentPosition);
                            }
                            else if(firstValue == 1)
                            {
                                //Paint white
                                WhitePanels.Add(currentPosition);
                               
                            }
                            DrawnPanels.Add(currentPosition);
                        }
                        //Turn robot
                        else
                        {
                            var nextDirection = GetNextDirection(firstValue == 0);
                            robotDirection = nextDirection;
                            switch (nextDirection)
                            {
                                case Direction.Up:
                                    robotY++;
                                    break;
                                case Direction.Left:
                                    robotX--;
                                    break;
                                case Direction.Down:
                                    robotY--;
                                    break;
                                case Direction.Right:
                                    robotX++;
                                    break;
                                default:
                                    break;

                            }
                            currentPosition = new Point(robotX, robotY);
                            var isOnAWhitePanel = WhitePanels.Contains(currentPosition);
                            InputValue = isOnAWhitePanel ? 1 : 0;
                        }
                      
                        isFirstOutputValue = !isFirstOutputValue;
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

    }
}
