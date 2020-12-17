using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day12
    {
        public Day12()
        {
            var lines = File.ReadLines(@"..\..\input\day12.txt").ToList();
            var instructions = lines.Select(s => new Action { ActionName = s.First(), Value = int.Parse(s.Substring(1))});

            // Part 1
            var tuple = Part1(instructions.ToList());

            Console.WriteLine(Math.Abs(tuple.Item1) + Math.Abs(tuple.Item2));

            // Part 2

            var currentEastWest = 0;
            var currentNorthSouth = 0;
            var wayPointEastWest = 10;
            var wayPointNorthSouth = 1;

            foreach (var ins in instructions)
            {
                switch (ins.ActionName)
                {
                    case 'N':
                        wayPointNorthSouth += ins.Value;
                        break;
                    case 'S':
                        wayPointNorthSouth -= ins.Value;
                        break;
                    case 'E':
                        wayPointEastWest += ins.Value;
                        break;
                    case 'W':
                        wayPointEastWest -= ins.Value;
                        break;
                    case 'L':
                        var tmpNorthSouth1 = wayPointNorthSouth;
                        switch (ins.Value)
                        {
                            case 90:
                                wayPointNorthSouth = wayPointEastWest;
                                wayPointEastWest = -tmpNorthSouth1;
                                break;
                            case 180:
                                wayPointNorthSouth = -tmpNorthSouth1;
                                wayPointEastWest = -wayPointEastWest;
                                break;
                            case 270:
                                wayPointNorthSouth = -wayPointEastWest;
                                wayPointEastWest = tmpNorthSouth1;
                                break;
                            default:
                                throw new Exception("Unknown degreee!");
                        }
                        break;
                    case 'R':
                        var tmpNorthSouth = wayPointNorthSouth;
                        switch (ins.Value)
                        {
                            case 90:
                                wayPointNorthSouth = -wayPointEastWest;
                                wayPointEastWest = tmpNorthSouth;
                                break;
                            case 180:
                                wayPointNorthSouth = -tmpNorthSouth;
                                wayPointEastWest = -wayPointEastWest;
                                break;
                            case 270:
                                wayPointNorthSouth = wayPointEastWest;
                                wayPointEastWest = -tmpNorthSouth;
                                break;
                            default:
                                throw new Exception("Unknown degreee!");
                        }
                        break;
                    case 'F':
                        currentEastWest += (wayPointEastWest * ins.Value);
                        currentNorthSouth += (wayPointNorthSouth * ins.Value);
                        break;
                }
            }
            Console.WriteLine(Math.Abs(currentEastWest) + Math.Abs(currentNorthSouth));
        }

        public enum Direction
        {
            North,
            South,
            East,
            West
        }

        public struct Action
        {
            public char ActionName;
            public int Value;
        }

        public Tuple<int, int> Part1(List<Action> instructions)
        {
            var currentEastWest = 0;
            var currentNorthSouth = 0;
            var currentDirection = Direction.East;

            foreach (var ins in instructions)
            {
                switch (ins.ActionName)
                {
                    case 'N':
                        currentNorthSouth += ins.Value;
                        break;
                    case 'S':
                        currentNorthSouth -= ins.Value;
                        break;
                    case 'E':
                        currentEastWest += ins.Value;
                        break;
                    case 'W':
                        currentEastWest -= ins.Value;
                        break;
                    case 'L':
                        switch (ins.Value)
                        {
                            case 90:
                                switch (currentDirection)
                                {
                                    case Direction.East:
                                        currentDirection = Direction.North;
                                        break;
                                    case Direction.North:
                                        currentDirection = Direction.West;
                                        break;
                                    case Direction.South:
                                        currentDirection = Direction.East;
                                        break;
                                    case Direction.West:
                                        currentDirection = Direction.South;
                                        break;
                                }
                                break;
                            case 180:
                                switch (currentDirection)
                                {
                                    case Direction.East:
                                        currentDirection = Direction.West;
                                        break;
                                    case Direction.North:
                                        currentDirection = Direction.South;
                                        break;
                                    case Direction.South:
                                        currentDirection = Direction.North;
                                        break;
                                    case Direction.West:
                                        currentDirection = Direction.East;
                                        break;
                                }
                                break;
                            case 270:
                                switch (currentDirection)
                                {
                                    case Direction.East:
                                        currentDirection = Direction.South;
                                        break;
                                    case Direction.North:
                                        currentDirection = Direction.East;
                                        break;
                                    case Direction.South:
                                        currentDirection = Direction.West;
                                        break;
                                    case Direction.West:
                                        currentDirection = Direction.North;
                                        break;
                                }
                                break;
                            default:
                                throw new Exception("Unknown degreee!");
                        }
                        break;
                    case 'R':
                        switch (ins.Value)
                        {
                            case 90:
                                switch (currentDirection)
                                {
                                    case Direction.East:
                                        currentDirection = Direction.South;
                                        break;
                                    case Direction.North:
                                        currentDirection = Direction.East;
                                        break;
                                    case Direction.South:
                                        currentDirection = Direction.West;
                                        break;
                                    case Direction.West:
                                        currentDirection = Direction.North;
                                        break;
                                }
                                break;
                            case 180:
                                switch (currentDirection)
                                {
                                    case Direction.East:
                                        currentDirection = Direction.West;
                                        break;
                                    case Direction.North:
                                        currentDirection = Direction.South;
                                        break;
                                    case Direction.South:
                                        currentDirection = Direction.North;
                                        break;
                                    case Direction.West:
                                        currentDirection = Direction.East;
                                        break;
                                }
                                break;
                            case 270:
                                switch (currentDirection)
                                {
                                    case Direction.East:
                                        currentDirection = Direction.North;
                                        break;
                                    case Direction.North:
                                        currentDirection = Direction.West;
                                        break;
                                    case Direction.South:
                                        currentDirection = Direction.East;
                                        break;
                                    case Direction.West:
                                        currentDirection = Direction.South;
                                        break;
                                }
                                break;
                            default:
                                throw new Exception("Unknown degreee!");
                        }
                        break;
                    case 'F':
                        switch (currentDirection)
                        {
                            case Direction.East:
                                currentEastWest += ins.Value;
                                break;
                            case Direction.North:
                                currentNorthSouth += ins.Value;
                                break;
                            case Direction.South:
                                currentNorthSouth -= ins.Value;
                                break;
                            case Direction.West:
                                currentEastWest -= ins.Value;
                                break;
                        }
                        break;
                }
            }
            return Tuple.Create(currentEastWest, currentNorthSouth);
        }
    }
}
