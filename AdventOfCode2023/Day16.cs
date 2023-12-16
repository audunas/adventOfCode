using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day16
    {
        public Day16()
        {
            Console.WriteLine("Day16");

            var lines = File.ReadLines(@"..\..\..\input\day16.txt").ToArray();

            var coord = new Dictionary<(int, int), char>();

            for (var i = 0; i<lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    coord.Add((i, j), lines[i][j]);
                }
            }

            var length = lines.Length;
            var width = lines.First().Length;

            //Part 1
            var startingPoint = (0,0);
            var firstType = coord[startingPoint];
            var startingDirection = Direction.East;
            if (firstType == '\\' || firstType == '|')
            {
                startingDirection = Direction.South;
            }

            var tilesPassedThrough = GetPassedThroughTiles(coord, length, width, startingPoint, startingDirection);

            Console.WriteLine(tilesPassedThrough.Count);

            //Part 2
            var allStartingPoints = new List<(int, int, Direction)>();
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j< width; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        allStartingPoints.Add((i, j, Direction.South));
                        allStartingPoints.Add((i, j, Direction.East));
                    }
                    else if (i == 0 && j == width - 1)
                    {
                        allStartingPoints.Add((i, j, Direction.South));
                        allStartingPoints.Add((i, j, Direction.West));
                    }
                    else if (i == length - 1 && j == 0)
                    {
                        allStartingPoints.Add((i, j, Direction.North));
                        allStartingPoints.Add((i, j, Direction.East));
                    }
                    else if (i == length - 1 && j == width - 1)
                    {
                        allStartingPoints.Add((i, j, Direction.North));
                        allStartingPoints.Add((i, j, Direction.West));
                    }
                    else if (i == 0)
                    {
                        allStartingPoints.Add((i, j, Direction.South));
                    }
                    else if (i == length-1)
                    {
                        allStartingPoints.Add((i, j, Direction.North));
                    }
                    else if (j == 0)
                    {
                        allStartingPoints.Add((i, j, Direction.East));
                    }
                    else if (j == width - 1)
                    {
                        allStartingPoints.Add((i, j, Direction.West));
                    }
                }
            }

            var max = 0;

            foreach (var st in allStartingPoints)
            {
                var passedThrough = GetPassedThroughTiles(coord, length, width, (st.Item1, st.Item2), st.Item3);
                if (passedThrough.Count > max)
                {
                    max = passedThrough.Count;
                }
            }

            Console.WriteLine(max);
        }

        private static HashSet<(int, int)> GetPassedThroughTiles(Dictionary<(int, int), char> coord, int length,
            int width, (int, int) startingPoint, Direction startingDirection)
        {
            var tilesPassedThrough = new HashSet<(int, int)>()
            {
                startingPoint
            };

          
            var beams = new List<(int, int, Direction)>()
            {
                (startingPoint.Item1, startingPoint.Item2, startingDirection)
            };

            var allBeams = new HashSet<(int, int, Direction)>();

            while (beams.Any())
            {
                var newBeams = new List<(int, int, Direction)>();
                foreach (var beam in beams)
                {
                    var nextItem = (0, 0);
                    switch (beam.Item3)
                    {
                        case Direction.East:
                            nextItem = (beam.Item1, beam.Item2 + 1);
                            break;
                        case Direction.South:
                            nextItem = (beam.Item1 + 1, beam.Item2);
                            break;
                        case Direction.West:
                            nextItem = (beam.Item1, beam.Item2 - 1);
                            break;
                        case Direction.North:
                            nextItem = (beam.Item1 - 1, beam.Item2);
                            break;
                        default:
                            break;
                    }

                    if (nextItem.Item1 < 0 || nextItem.Item1 > length - 1 ||
                        nextItem.Item2 < 0 || nextItem.Item2 > width - 1)
                    {
                        //Do nothing - beam is moving outside grid
                        continue;
                    }

                    tilesPassedThrough.Add(nextItem);

                    var type = coord[nextItem];

                    if (type == '\\')
                    {
                        switch (beam.Item3)
                        {
                            case Direction.East:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.South));
                                break;
                            case Direction.South:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.East));
                                break;
                            case Direction.West:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.North));
                                break;
                            case Direction.North:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.West));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (type == '/')
                    {
                        switch (beam.Item3)
                        {
                            case Direction.East:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.North));
                                break;
                            case Direction.South:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.West));
                                break;
                            case Direction.West:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.South));
                                break;
                            case Direction.North:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.East));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (type == '|')
                    {
                        switch (beam.Item3)
                        {
                            case Direction.East:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.North));
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.South));
                                break;
                            case Direction.South:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.South));
                                break;
                            case Direction.West:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.North));
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.South));
                                break;
                            case Direction.North:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.North));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (type == '-')
                    {
                        switch (beam.Item3)
                        {
                            case Direction.East:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.East));
                                break;
                            case Direction.South:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.East));
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.West));
                                break;
                            case Direction.West:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.West));
                                break;
                            case Direction.North:
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.East));
                                newBeams.Add((nextItem.Item1, nextItem.Item2, Direction.West));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        newBeams.Add((nextItem.Item1, nextItem.Item2, beam.Item3));
                    }


                }
                beams = newBeams.Where(t => !allBeams.Contains(t)).ToList();
                allBeams.UnionWith(newBeams);

            }
            return tilesPassedThrough;
        }


        public enum Direction
        {
            North,
            South,
            West,
            East
        }
    }
}
