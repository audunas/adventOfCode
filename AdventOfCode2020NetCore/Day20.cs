using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020NetCore
{
    class Day20
    {
        public Day20()
        {
            var lines = File.ReadAllText(@"..\..\..\input\day20.txt");
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

            // Part1

            var corners = matchingDict.Where(r => r.Value.Count == 4);

            Console.WriteLine(corners.Aggregate(1L, (acc, corner) => acc * corner.Key));

            // Part 2
            var image = new List<Tile>();
            var topLeftCorner = tiles.First(t => t.Id == corners.First().Key);
            topLeftCorner.TilePosition = new Position { x = 0, y = 0};
            var availableTiles = new List<int>(tiles.Select(t => t.Id));
            availableTiles.Remove(topLeftCorner.Id);
            image.Add(topLeftCorner);

            var currentTile = topLeftCorner;

            for (var x = 0; x < lengthOfImage; x++)
            {
                for (var y = 0; y < lengthOfImage; y++)
                {
                }
            }
            
            Console.WriteLine("42");
        }

       

        private static (List<MatchingPair>, Dictionary<int, HashSet<Direction>>) GetMatchingPairs(List<Tile> tiles)
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
                            var set1 = GetSetFromDirection(firstTile, direction);
                            var set2 = GetSetFromDirection(secondTile, direction2);
                            if (set1.SetEquals(set2))
                            {
                                SetDirection(tilesMatch, tilesDictionary, firstTile, secondTile,
                                    direction, direction2);
                            }
                        }
                    }

                }
            }

            return (tilesMatch, tilesDictionary);
        }

        private static void SetDirection(List<MatchingPair> tilesMatch, Dictionary<int, HashSet<Direction>> tilesDictionary, 
            Tile firstTile, Tile secondTile, Direction tile1Direction, Direction tile2Direction)
        {
            tilesMatch.Add(new MatchingPair
            {
                TileId1 = firstTile.Id,
                TileId2 = secondTile.Id,
                TileId1InputDirection = tile1Direction,
                TileId2InputDirection = tile2Direction
            });

            if (tilesDictionary.ContainsKey(firstTile.Id))
            {
                tilesDictionary[firstTile.Id].Add(tile1Direction);
            }
            else
            {
                tilesDictionary[firstTile.Id] = new HashSet<Direction>() { tile1Direction };
            }
            if (tilesDictionary.ContainsKey(secondTile.Id))
            {
                tilesDictionary[secondTile.Id].Add(tile2Direction);
            }
            else
            {
                tilesDictionary[secondTile.Id] = new HashSet<Direction>() { tile2Direction };
            }
        }

        public class MatchingPair
        {
            public int TileId1;
            public int TileId2;
            public Direction TileId1InputDirection;
            public Direction TileId2InputDirection;
            public Direction TileId1OutputDirection;
            public Direction TileId2OutputDirection;
        }

        public enum Direction
        {
            Top,
            TopRotated,
            Bottom,
            BottomRotated,
            Left,
            LeftRotated,
            Right,
            RightRotated
        }

        private static HashSet<int> GetSetFromDirection(Tile tile, Direction direction)
        {
            switch (direction)
            {
                case Direction.Bottom:
                    return tile.Bottom;
                case Direction.BottomRotated:
                    return tile.RotatedBottom;
                case Direction.Left:
                    return tile.Left;
                case Direction.LeftRotated:
                    return tile.RotatedLeft;
                case Direction.Right:
                    return tile.Right;
                case Direction.RightRotated:
                    return tile.RotatedRight;
                case Direction.Top:
                    return tile.Top;
                case Direction.TopRotated:
                    return tile.RotatedTop;
                default:
                    throw new Exception("Unknown direction! : " + direction);
            }
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
            public HashSet<int> RotatedLeft;
            public HashSet<int> RotatedRight;
            public HashSet<int> RotatedTop;
            public HashSet<int> RotatedBottom;

            public Tile(int id, Position tilePosition, List<Position> positions, int maxLength)
            {
                Id = id;
                TilePosition = tilePosition;
                Positions = positions;
                Left = new HashSet<int>(Positions.Where(p => p.y == 0).Select(r => r.x));
                RotatedLeft = new HashSet<int>(Left.Select(m => maxLength - m));
                Right = new HashSet<int>(Positions.Where(p => p.y == maxLength).Select(r => r.x));
                RotatedRight = new HashSet<int>(Right.Select(m => maxLength - m));
                Top = new HashSet<int>(Positions.Where(p => p.x == 0).Select(r => r.y));
                RotatedTop = new HashSet<int>(Top.Select(m => maxLength - m));
                Bottom = new HashSet<int>(Positions.Where(p => p.x == maxLength).Select(r => r.y));
                RotatedBottom = new HashSet<int>(Bottom.Select(m => maxLength - m));
            }
        }

        internal struct Position
        {
            public int x;
            public int y;
        }
    }
}
