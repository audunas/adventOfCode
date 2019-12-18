using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day13
    {
        public static long InputValue = 0;
        public static long RelativeBase = 0;

        public Day13()
        {
            var lines = File.ReadLines(@"..\..\..\input\day13.txt").First();

            var memory = new long[100000000];
            var index = (long)0;
            foreach (var inst in lines.Split(','))
            {
                memory[index] = long.Parse(inst);
                index++;
            }
            var endstate = FindEndState(memory, InputValue);

            //Part1
            var numOfBlockTiles = Tiles.Where(t => t.tileId == TileId.Block).Count();

            Console.WriteLine(numOfBlockTiles);

            memory[0] = 2;
            endstate = FindEndState(memory, InputValue);
            //Part2
            Console.WriteLine(Score);
        }

        public class Tile
        {
            public int x;
            public int y;
            public TileId tileId;

            public Tile(int x, int y, TileId tileId)
            {
                this.x = x;
                this.y = y;
                this.tileId = tileId;
            }
        }

        public enum TileId
        {
            Empty = 0,
            Wall = 1,
            Block = 2,
            HorizontalPaddle = 3,
            Ball = 4,
        }

        public static List<Tile> Tiles = new List<Tile>();
        public static int InputCounter = 0;
        public static int CurrentX;
        public static int CurrentY;
        public static long Score;

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
                        //Console.WriteLine(firstValue);
                        if (InputCounter == 0)
                        {
                            CurrentX = (int)firstValue;
                            InputCounter++;
                        }
                        else if (InputCounter == 1)
                        {
                            CurrentY = (int)firstValue;
                            InputCounter++;
                        }
                        else if (InputCounter == 2)
                        {
                            if (CurrentX == -1 && CurrentY == 0)
                            {
                                Score = firstValue;
                            }
                            else
                            {
                                var tile = new Tile(CurrentX, CurrentY, (TileId)firstValue);
                                Tiles.Add(tile);
                            }
                            InputCounter = 0;
                        }
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
