using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    class Day20
    {
        public Day20()
        {
            var lines = File.ReadAllText(@"..\..\input\day20.txt");
            var inputTiles = lines.Split(new string[] { "\r\n\r\n" },
                               StringSplitOptions.RemoveEmptyEntries);
            var lengthOfImage = (int)Math.Sqrt(inputTiles.Length);

            var tiles = new List<Tile>();

            foreach (var inputTile in inputTiles)
            {
                var inputLines = inputTile.Split(new string[] { "\r\n" },
                               StringSplitOptions.RemoveEmptyEntries);
                var tileNumber = int.Parse(inputLines[0].Split(' ')[1].Split(':')[0].Trim());
                var positions = new List<Position>();
                for (var i = 1; i < inputLines.Length; i++)
                {
                    var line = inputLines[i];
                    for (var j = 0; j < line.Length; j++)
                    {
                        if (line[j] == '#')
                        {
                            var pos = new Position { x = i - 1, y = j };
                            positions.Add(pos);
                        }
                    }
                }
                var tile = new Tile(tileNumber, new Position { x = 0, y = 0 }, positions, inputLines[0].Length - 1);
                tiles.Add(tile);
            }

            var res = GetMatchingPairs(tiles);
            var matchingPairs = res.Item1;
            var matchingDict = res.Item2;

            var leftTopCorner = matchingDict.Where(r => r.Value.SetEquals(new HashSet<Direction> { Direction.Right, Direction.Bottom}));

            var corners = new List<Position>
            {
                new Position {x = 0, y = 0 },
                new Position {x = 0, y = lengthOfImage - 1 },
                new Position {x = lengthOfImage - 1, y = 0 },
                new Position {x = lengthOfImage - 1, y = lengthOfImage - 1 },
            };

            Console.WriteLine("Test");
        }

        private static Tuple<List<MatchingPair>, Dictionary<int, HashSet<Direction>>> GetMatchingPairs(List<Tile> tiles)
        {
            var tilesMatch = new List<MatchingPair>();
            var tilesDictionary = new Dictionary<int, HashSet<Direction>>();

            for (var i = 0; i < tiles.Count; i++)
            {
                var firstTile = tiles.ElementAt(i);
                for (var j = i + 1; j < tiles.Count; j++)
                {
                    var secondTile = tiles.ElementAt(j);

                    foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                    {
                        foreach (var direction2 in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                        {

                            if (firstTile.Left.SetEquals(secondTile.Right))
                            {
                                tilesMatch.Add(new MatchingPair
                                {
                                    TileId1 = firstTile.Id,
                                    TileId2 = secondTile.Id,
                                    TileId1Direction = Direction.Left,
                                    TileId2Direction = Direction.Right
                                });

                                if (tilesDictionary.ContainsKey(firstTile.Id))
                                {
                                    tilesDictionary[firstTile.Id].Add(Direction.Left);
                                }
                                else
                                {
                                    tilesDictionary[firstTile.Id] = new HashSet<Direction>() { Direction.Left };
                                }
                                if (tilesDictionary.ContainsKey(secondTile.Id))
                                {
                                    tilesDictionary[secondTile.Id].Add(Direction.Right);
                                }
                                else
                                {
                                    tilesDictionary[secondTile.Id] = new HashSet<Direction>() { Direction.Right };
                                }

                            }
                            if (firstTile.Right.SetEquals(secondTile.Left))
                            {
                                tilesMatch.Add(new MatchingPair
                                {
                                    TileId1 = firstTile.Id,
                                    TileId2 = secondTile.Id,
                                    TileId1Direction = Direction.Right,
                                    TileId2Direction = Direction.Left
                                });
                                if (tilesDictionary.ContainsKey(firstTile.Id))
                                {
                                    tilesDictionary[firstTile.Id].Add(Direction.Right);
                                }
                                else
                                {
                                    tilesDictionary[firstTile.Id] = new HashSet<Direction>() { Direction.Right };
                                }
                                if (tilesDictionary.ContainsKey(secondTile.Id))
                                {
                                    tilesDictionary[secondTile.Id].Add(Direction.Left);
                                }
                                else
                                {
                                    tilesDictionary[secondTile.Id] = new HashSet<Direction>() { Direction.Left };
                                }
                            }
                            if (firstTile.Top.SetEquals(secondTile.Bottom))
                            {
                                tilesMatch.Add(new MatchingPair
                                {
                                    TileId1 = firstTile.Id,
                                    TileId2 = secondTile.Id,
                                    TileId1Direction = Direction.Top,
                                    TileId2Direction = Direction.Bottom
                                });
                                if (tilesDictionary.ContainsKey(firstTile.Id))
                                {
                                    tilesDictionary[firstTile.Id].Add(Direction.Top);
                                }
                                else
                                {
                                    tilesDictionary[firstTile.Id] = new HashSet<Direction>() { Direction.Top };
                                }
                                if (tilesDictionary.ContainsKey(secondTile.Id))
                                {
                                    tilesDictionary[secondTile.Id].Add(Direction.Bottom);
                                }
                                else
                                {
                                    tilesDictionary[secondTile.Id] = new HashSet<Direction>() { Direction.Bottom };
                                }
                            }
                            if (firstTile.Bottom.SetEquals(secondTile.Top))
                            {
                                tilesMatch.Add(new MatchingPair
                                {
                                    TileId1 = firstTile.Id,
                                    TileId2 = secondTile.Id,
                                    TileId1Direction = Direction.Bottom,
                                    TileId2Direction = Direction.Top
                                });

                                if (tilesDictionary.ContainsKey(firstTile.Id))
                                {
                                    tilesDictionary[firstTile.Id].Add(Direction.Bottom);
                                }
                                else
                                {
                                    tilesDictionary[firstTile.Id] = new HashSet<Direction>() { Direction.Bottom };
                                }
                                if (tilesDictionary.ContainsKey(secondTile.Id))
                                {
                                    tilesDictionary[secondTile.Id].Add(Direction.Top);
                                }
                                else
                                {
                                    tilesDictionary[secondTile.Id] = new HashSet<Direction>() { Direction.Top };
                                }
                            }
                        }
                    }
                    
                }
            }

            return Tuple.Create(tilesMatch, tilesDictionary);
        }

        public class MatchingPair
        {
            public int TileId1;
            public int TileId2;
            public Direction TileId1Direction;
            public Direction TileId2Direction;
        }

        public enum Direction
        {
            Top,
            Bottom,
            Left,
            Right
        }

        internal class Tile
        {
            public int Id;
            public Position TilePosition;
            public List<Position> Positions;
            public HashSet<int> Left;
            public HashSet<int> Right;
            public HashSet<int> Top;
            public HashSet<int> Bottom;

            public Tile(int id, Position tilePosition, List<Position> positions, int maxLength)
            {
                Id = id;
                TilePosition = tilePosition;
                Positions = positions;
                Left = new HashSet<int>(Positions.Where(p => p.y == 0).Select(r => r.x));
                Right = new HashSet<int>(Positions.Where(p => p.y == maxLength).Select(r => r.x));
                Top = new HashSet<int>(Positions.Where(p => p.x == 0).Select(r => r.y));
                Bottom = new HashSet<int>(Positions.Where(p => p.x == maxLength).Select(r => r.y));
            }
        }

        internal struct Position
        {
            public int x;
            public int y;
        }
    }
}
