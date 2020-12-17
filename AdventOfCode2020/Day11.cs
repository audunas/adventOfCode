using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Day11
    {
        public Day11()
        {
            var lines = File.ReadLines(@"..\..\input\day11.txt").ToList();
            var seatLayout = lines.Select(l => l.ToCharArray()).ToList();
            var originalLayout = new List<char[]>(seatLayout);

            // Part 1
            var changes = 1;

            while (changes > 0)
            {
                changes = 0;
                var newLayout = new List<char[]>(seatLayout);
                for (var x = 0; x < seatLayout.Count(); x++)
                {
                    var line = seatLayout[x];
                    var newLine = new char[line.Count()];
                    Array.Copy(line, newLine, line.Count());
                    for (var y = 0; y < line.Count(); y++)
                    {
                        var c = line[y];
                        if (c == '.')
                        {
                            continue;
                        }

                        var neighbours = getNeighbours(seatLayout, x, y);
                        if (c == 'L')
                        {
                            if (!neighbours.Any(r => r == '#'))
                            {
                                newLine[y] = '#';
                                changes++;
                            }
                        }
                        else if (c == '#')
                        {
                            if (neighbours.Count(r => r == '#') >= 4)
                            {
                                newLine[y] = 'L';
                                changes++;
                            }
                        }
                    }
                    newLayout.RemoveAt(x);
                    newLayout.Insert(x, newLine);
                }
                seatLayout = newLayout;
            }

            Console.WriteLine(seatLayout.SelectMany(r => r.Where(c => c == '#')).Count());

            // Part 2

            seatLayout = originalLayout;
            changes = 1;

            while (changes > 0)
            {
                changes = 0;
                var newLayout = new List<char[]>(seatLayout);
                for (var x = 0; x < seatLayout.Count(); x++)
                {
                    var line = seatLayout[x];
                    var newLine = new char[line.Count()];
                    Array.Copy(line, newLine, line.Count());
                    for (var y = 0; y < line.Count(); y++)
                    {
                        var c = line[y];
                        if (c == '.')
                        {
                            continue;
                        }

                        var neighbours = getVisibleSeats(seatLayout, x, y);
                        if (c == 'L')
                        {
                            if (!neighbours.Any(r => r == '#'))
                            {
                                newLine[y] = '#';
                                changes++;
                            }
                        }
                        else if (c == '#')
                        {
                            if (neighbours.Count(r => r == '#') >= 5)
                            {
                                newLine[y] = 'L';
                                changes++;
                            }
                        }
                    }
                    newLayout.RemoveAt(x);
                    newLayout.Insert(x, newLine);
                }
                seatLayout = newLayout;
            }

            Console.WriteLine(seatLayout.SelectMany(r => r.Where(c => c == '#')).Count());
        }

        private List<char> getVisibleSeats(List<char[]> seatLayout, int x, int y)
        {
            var neighbours = new List<char>();
            var checkedOutDirection = new List<Tuple<int, int>>();

            foreach (var perm in Permutations)
            {
                var moreToCheck = true;
                var counter = 1;
                while (moreToCheck)
                {
                    var dX = x + (perm.Item1 * counter);
                    var dY = y + (perm.Item2 * counter);
                    if (dX >= 0 && dY >= 0 && dX < seatLayout.Count() && dY < seatLayout[0].Count())
                    {
                        var neighbour = seatLayout[dX][dY];
                        if (neighbour == '#' || neighbour == 'L')
                        {
                            neighbours.Add(neighbour);
                            moreToCheck = false;
                        }
                    }
                    else
                    {
                        moreToCheck = false;
                    }
                    counter++;
                }
            }

            return neighbours;
        }


        private List<Tuple<int, int>> Permutations = new List<Tuple<int, int>>
        {
            Tuple.Create(0, 1),
            Tuple.Create(0, -1),
            Tuple.Create(1, 0),
            Tuple.Create(1, 1),
            Tuple.Create(1, -1),
            Tuple.Create(-1, 0),
            Tuple.Create(-1, -1),
            Tuple.Create(-1, 1)
        };

        private List<char> getNeighbours(List<char[]> seatLayout, int x, int y)
        {
            var neighbours = new List<char>();

            foreach (var perm in Permutations)
            {
                var dX = x + perm.Item1;
                var dY = y + perm.Item2;
                if (dX >= 0 && dY >= 0 && dX < seatLayout.Count() && dY < seatLayout[0].Count())
                {
                    var neighbour = seatLayout[dX][dY];
                    neighbours.Add(neighbour);
                }
               
            }
            return neighbours;
        }
    }
}
