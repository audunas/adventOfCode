using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day6
    {

        public Day6()
        {
            Console.WriteLine("Day6");

            var lines = File.ReadLines(@"..\..\..\input\day6.txt").ToArray();

            var coord = new Dictionary<(int, int), char>();

            for(int i = 0; i<lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j <line.Length; j++)
                {
                    coord.Add((i, j), line[j]);
                }
            }

            var startPoint = coord.First(c => c.Value == '^');

            var visited = new List<(int, int)>()
            {
                startPoint.Key
            };

            var currentDir = Dir.North;

            var nextX = startPoint.Key.Item1 - 1;
            var nextY = startPoint.Key.Item2;

            var hitObstructions = new HashSet<(int, int)>();   

            while (nextX >= 0 && nextX < lines.Length && 
                nextY >= 0 && nextY < lines[0].Length)
            {
                visited.Add((nextX, nextY));

                var tempX = nextX;
                var tempY = nextY;

                switch (currentDir)
                {
                    case Dir.North:
                        tempX = tempX - 1;
                        break;
                    case Dir.South:
                        tempX = tempX + 1;
                        break;
                    case Dir.West:
                        tempY = tempY - 1;
                        break;
                    case Dir.East:
                        tempY = tempY + 1;
                        break;
                }
                var exists = coord.TryGetValue((tempX, tempY), out char v);

                if (exists && v == '#')
                {
                    hitObstructions.Add((tempX, tempY));
                    switch (currentDir)
                    {
                        case Dir.North:
                            currentDir = Dir.East;
                            tempX = tempX + 1;
                            tempY = tempY + 1;
                            break;
                        case Dir.South:
                            currentDir = Dir.West;
                            tempX = tempX - 1;
                            tempY = tempY - 1;
                            break;
                        case Dir.West:
                            currentDir = Dir.North;
                            tempX = tempX - 1;
                            tempY = tempY + 1;
                            break;
                        case Dir.East:
                            currentDir = Dir.South;
                            tempX = tempX + 1;
                            tempY = tempY - 1;
                            break;
                    }
                }
                nextX = tempX;
                nextY = tempY;

            }

            Console.WriteLine(visited.Distinct().Count());


            var loopObs = new HashSet<(int, int)>();

            for (var i = 0; i<lines.Length; i++)
            {
                Console.WriteLine(i);
                for (var j = 0; j < lines[i].Length; j++)
                {
                    //Console.WriteLine((i,j));
                    if (coord[(i, j)] == '#' || (i,j) == startPoint.Key)
                    {
                        continue;
                    }
                    
                    var visited2 = new List<(int, int, Dir)>()
                    {
                        (startPoint.Key.Item1, startPoint.Key.Item2, Dir.North)
                    };

                    var newCoord = new Dictionary<(int, int), char>(coord);
                    newCoord[(i, j)] = '#';
                    currentDir = Dir.North;

                    nextX = startPoint.Key.Item1 - 1;
                    nextY = startPoint.Key.Item2;

                    var isLoop = false;

                    while ((nextX >= 0 && nextX < lines.Length &&
                        nextY >= 0 && nextY < lines[0].Length) || isLoop)
                    {
                        visited2.Add((nextX, nextY, currentDir));

                        var tempX = nextX;
                        var tempY = nextY;

                        switch (currentDir)
                        {
                            case Dir.North:
                                tempX = tempX - 1;
                                break;
                            case Dir.South:
                                tempX = tempX + 1;
                                break;
                            case Dir.West:
                                tempY = tempY - 1;
                                break;
                            case Dir.East:
                                tempY = tempY + 1;
                                break;
                        }
                        var exists = newCoord.TryGetValue((tempX, tempY), out char v);
                        var ch = v;

                        if (exists && ch == '#')
                        {
                            while (exists && ch == '#')
                            {
                                switch (currentDir)
                                {
                                    case Dir.North:
                                        currentDir = Dir.East;
                                        tempX = tempX + 1;
                                        tempY = tempY + 1;
                                        break;
                                    case Dir.South:
                                        currentDir = Dir.West;
                                        tempX = tempX - 1;
                                        tempY = tempY - 1;
                                        break;
                                    case Dir.West:
                                        currentDir = Dir.North;
                                        tempX = tempX - 1;
                                        tempY = tempY + 1;
                                        break;
                                    case Dir.East:
                                        currentDir = Dir.South;
                                        tempX = tempX + 1;
                                        tempY = tempY - 1;
                                        break;
                                }
                                exists = newCoord.TryGetValue((tempX, tempY), out char c);
                                ch = c;
                            }

                           
                            if (visited2.Contains((nextX, nextY, currentDir)))
                            {
                                isLoop = true;
                                break;
                            }
                            visited2.Add((nextX, nextY, currentDir));
                            
                        }
                        nextX = tempX;
                        nextY = tempY;

                        if (visited2.Contains((nextX, nextY, currentDir)))
                        {
                            isLoop = true;
                            break;
                        }
                    }

                    if (isLoop)
                    {
                        loopObs.Add((i, j));
                    }

                }
            }

            Console.WriteLine(loopObs.Count);

        }

        public enum Dir
        {
            North,
            South,
            East,
            West
        }
    }
}
