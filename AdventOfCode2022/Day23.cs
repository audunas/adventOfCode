using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2022
{
    public class Day23
    {

        public Day23()
        {
            var lines = File.ReadLines(@"..\..\..\input\day23.txt").ToList();

            var elves = new HashSet<(int, int)>();

            for (var i = 0; i<lines.Count; i++)
            {
                for (var j = 0; j < lines[i].Count(); j++)
                {
                    var letter = lines[i][j];
                    if (letter == '#')
                    {
                        elves.Add((i, j));
                    }
                }
            }

            var counter = 0;

            var directionPriorityList = new List<Direction>
            {
                Direction.North,
                Direction.South,
                Direction.West,
                Direction.East
            };

            var anyMoved = true;

            while (anyMoved)
            {
               
                Console.WriteLine(counter);
                var newElves = new HashSet<(int, int)>();
                var proposedDirections = new Dictionary<(int, int), (int,int)>();

                foreach (var elf in elves)
                {
                    var north = new HashSet<(int, int)>() { (elf.Item1 - 1, elf.Item2 - 1), (elf.Item1 - 1, elf.Item2), (elf.Item1 - 1, elf.Item2 + 1) };
                    var south = new HashSet<(int, int)>() { (elf.Item1 + 1, elf.Item2 - 1), (elf.Item1 + 1, elf.Item2), (elf.Item1 + 1, elf.Item2 + 1) };
                    var west = new HashSet<(int, int)>() { (elf.Item1 - 1, elf.Item2 - 1), (elf.Item1, elf.Item2 - 1), (elf.Item1 + 1, elf.Item2 - 1) };
                    var east = new HashSet<(int, int)>() { (elf.Item1 - 1, elf.Item2 + 1), (elf.Item1, elf.Item2 + 1), (elf.Item1 + 1, elf.Item2 + 1) };
                    var all = north.Union(south).Union(west).Union(east);
                    if (all.All(a => !elves.Contains(a)))
                    {
                        continue;
                    }

                    foreach (var dir in directionPriorityList)
                    {
                        switch(dir)
                        {
                            case Direction.North:
                                if (north.All(n => !elves.Contains(n)))
                                {
                                    proposedDirections.Add(elf, (elf.Item1 - 1, elf.Item2));
                                }
                                break;
                            case Direction.South:
                                if (south.All(s => !elves.Contains(s)))
                                {
                                    proposedDirections.Add(elf, (elf.Item1 + 1, elf.Item2));
                                }
                                break;
                            case Direction.West:
                                if (west.All(w => !elves.Contains(w)))
                                {
                                    proposedDirections.Add(elf, (elf.Item1, elf.Item2 - 1));
                                }
                                break;
                            case Direction.East:
                                if (east.All(e => !elves.Contains(e)))
                                {
                                    proposedDirections.Add(elf, (elf.Item1, elf.Item2 + 1));
                                }
                                break;
                            default:
                                break;
                        }
                        if (proposedDirections.ContainsKey(elf))
                        {
                            break;
                        }
                    }

                }

                anyMoved = false;

                var elvesWithProposals = elves.Where(proposedDirections.ContainsKey).ToList();

                foreach (var elf in elves)
                {
                    if (!proposedDirections.ContainsKey(elf))
                    {
                        newElves.Add(elf);
                        continue;
                    }

                    var proposedMove = proposedDirections[elf];
                    if (!elvesWithProposals.Except(new List<(int, int)>() { elf })
                        .Any(r => proposedDirections[r] == proposedMove))
                    {
                        newElves.Add(proposedMove);
                        anyMoved = true;
                    }
                    else
                    {
                        newElves.Add(elf);
                    }
                }

                var currentFirstDir = directionPriorityList.First();
                directionPriorityList.RemoveAt(0);
                directionPriorityList = directionPriorityList.Append(currentFirstDir).ToList();

                counter++;

                elves = newElves;

                //Part 1
                //if (counter > 9)
                //{
                //    anyMoved = false;
                //}

            }

            var minX = elves.Min(r => r.Item1) ;
            var maxX = elves.Max(r => r.Item1);
            var minY = elves.Min(r => r.Item2);
            var maxY = elves.Max(r => r.Item2);

            var height = Math.Abs(maxX - minX) + 1;
            var length = Math.Abs(maxY - minY) + 1;

            var numberOfElements = height * length;

            var emptyTiles = numberOfElements - elves.Count();

            Console.WriteLine(emptyTiles);

            Console.WriteLine(counter);

        }

        public enum Direction
        {
            North,
            South,
            East,
            West
        }
    }
}
