using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day15
    {
        public Day15()
        {
            var lines = File.ReadLines(@"..\..\..\input\day15.txt");

            var map = lines.Select(l => l.Select(r => int.Parse(r.ToString())).ToArray()).ToArray();

            var start = new Position { x = 0, y = 0 };
            var end = new Position { x = map[0].Length - 1,  y =  map.Length - 1};

            //Part 1
            var nodesDict = Dijkstra(map, start, end);
           
            Console.WriteLine(nodesDict[end]);

            //Part 2
            var tileLength = map[0].Length;
            var newMap = map.Select(r => r.ToList()).ToList();
            for (var j = 0; j < 4; j++)
            {
                var newMapValues = newMap.GetRange((j*map[0].Length), map[0].Length).Select(row => row.Select(cell => {
                    var newValue = cell + 1;
                    if (newValue > 9)
                        newValue = 1;
                    return newValue;
                }).ToList()).ToList();
                newMap = newMap.Concat(newMapValues).ToList();
            }
            for (var j = 0; j < 5; j++)
            {
                var startX = j * tileLength;
                for (var i = 0; i < 4; i++)
                {
                    var newMapValues = newMap.GetRange((j * tileLength), tileLength).Select(row => row.GetRange((i*tileLength), tileLength).Select(cell =>
                    {
                        var newValue = cell + 1;
                        if (newValue > 9)
                            newValue = 1;
                        return newValue;
                    }).ToList()).ToList();
                    for (var x = startX; x < (startX+newMapValues.Count()); x++)
                    {
                        newMap.ElementAt(x).AddRange(newMapValues.ElementAt(x-startX));
                    }
                }
            }
            var newMapArray = newMap.Select(r => r.ToArray()).ToArray();
            end = new Position { x = newMapArray[0].Length - 1, y = newMapArray.Length - 1 };
            var nodesDict2 = Dijkstra(newMapArray, start, end);

            Console.WriteLine(nodesDict2[end]);
        }

        public static Dictionary<Position, int> Dijkstra(int[][] map, Position start, Position end)
        {
            Vec3Comparer comparer = new Vec3Comparer();
            var nodesDictionary = new Dictionary<Position, int>(comparer);
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    var position = new Position { x = i, y = j };
                    nodesDictionary.Add(position, int.MaxValue);
                }
            }
            nodesDictionary[start] = 0;

            var unvisitedNodes = nodesDictionary.Select(n => n.Key).ToHashSet();
            var nodesInPath = new HashSet<Position>();
            var finishedNodes = new HashSet<Position>();

            unvisitedNodes.Remove(start);
            nodesInPath.Add(start);
            while (unvisitedNodes.Any())
            {
                var nodesToVisit = new HashSet<Position>(nodesInPath);
                var shortestDistance = int.MaxValue;
                var nodeWithShortestDistance = start;
                foreach (var visitedNode in nodesToVisit)
                {
                    var neighborNodes = Neighbors(visitedNode, end.x, end.y).Where(p => unvisitedNodes.Contains(p));
                    if (!neighborNodes.Any())
                    {
                        finishedNodes.Add(visitedNode);
                        nodesInPath.Remove(visitedNode);
                        continue;
                    }

                    foreach (var neighbor in neighborNodes)
                    {
                        var distance = nodesDictionary[visitedNode] + map[neighbor.x][neighbor.y];

                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            nodeWithShortestDistance = neighbor;
                        }
                        if (distance < nodesDictionary[neighbor])
                        {
                            nodesDictionary[neighbor] = distance;
                        }
                    }
                }
                if (nodeWithShortestDistance.Equals(end))
                {
                    return nodesDictionary;
                }
                unvisitedNodes.Remove(nodeWithShortestDistance);
                nodesInPath.Add(nodeWithShortestDistance);
            }

            return nodesDictionary;
        }

        public static Position[] Neighbors(Position node, int maxX, int maxY)
        {
            return new Position[]
            {
                new Position {x = node.x+1, y = node.y},
                new Position {x = node.x-1, y = node.y},
                new Position {x = node.x, y = node.y-1},
                new Position {x = node.x, y = node.y+1}
            }.Where(p => p.x <= maxX && p.x >= 0 && p.y <= maxY && p.y >= 0).ToArray();
        }

        class Vec3Comparer : IEqualityComparer<Position>
        {
            public bool Equals(Position a, Position b)
            {
                return a.x == b.x && a.y == b.y;
            }

            public int GetHashCode(Position obj)
            {
                return ((IntegerHash(obj.x)
                        ^ (IntegerHash(obj.y) << 1)) >> 1);
            }

            static int IntegerHash(int a)
            {
                // fmix32 from murmurhash
                uint h = (uint)a;
                h ^= h >> 16;
                h *= 0x85ebca6bU;
                h ^= h >> 13;
                h *= 0xc2b2ae35U;
                h ^= h >> 16;
                return (int)h;
            }
        }


        [DebuggerDisplay("x = {x}, y = {y}")]
        public struct Position
        {
            public int x;
            public int y;
        }
    }
}
