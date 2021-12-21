using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var allPossibleTiles = tiles.SelectMany(t => GetAllPossibleTileVariations(t));

            var topLeftCorner = tiles.First(t => t.Id == corners.First().Key);

            topLeftCorner.TilePosition = new Position { x = 0, y = 0};
            var availableTiles = new List<int>(tiles.Select(t => t.Id));
            availableTiles.Remove(topLeftCorner.Id);

            

            var neighDirections = new HashSet<Direction>(matchingPairs.Where(r => r.TileId1 == topLeftCorner.Id).Select(t => t.TileId1InputDirection)
                              .Concat(matchingPairs.Where(r => r.TileId2 == topLeftCorner.Id).Select(t => t.TileId2InputDirection)));


            //if (neighDirections.SetEquals(new HashSet<Direction> { Direction.Top, Direction.Right }))
            //{
            //    var tempRight = topLeftCorner.Right;
            //    topLeftCorner.Right = new HashSet<int>(topLeftCorner.Top);
            //    topLeftCorner.Bottom = new HashSet<int>(topLeftCorner.RotatedRight);
            //    topLeftCorner.Left = new HashSet<int>();
            //    topLeftCorner.Top = new HashSet<int>();
            //}
            //else if (neighDirections.SetEquals(new HashSet<Direction> { Direction.Top, Direction.Left }))
            //{
            //}
            //else if (neighDirections.SetEquals(new HashSet<Direction> { Direction.Left, Direction.Bottom }))
            //{
            //}

            image.Add(topLeftCorner);

            var currentTile = topLeftCorner;
            var expectedNoNeighbors = 4;

            for (var x = 0; x < lengthOfImage; x++)
            {
                if (x > 0)
                {
                    currentTile = image.First(t => t.TilePosition.x == x - 1 && t.TilePosition.y == 0);
                }
                for (var y = 0; y < lengthOfImage; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    else if (x == 0 || x == lengthOfImage - 1)
                    {
                        expectedNoNeighbors = 3;

                        if (y == lengthOfImage - 1 || y == 0)
                        {
                            expectedNoNeighbors = 2;
                        }
                        
                    }
                    else
                    {
                        expectedNoNeighbors = 4;
                        if (y == lengthOfImage - 1 || y == 0)
                        {
                            expectedNoNeighbors = 3;
                        }
                    }

                    var neigh = matchingPairs.Where(r => r.TileId1 == currentTile.Id)
                              .Concat(matchingPairs.Where(r => r.TileId2 == currentTile.Id));

                    var neighbours = matchingPairs.Where(r => r.TileId1 == currentTile.Id).Select(t => t.TileId2)
                              .Concat(matchingPairs.Where(r => r.TileId2 == currentTile.Id).Select(t => t.TileId1))
                              .Distinct()
                              .Where(t => availableTiles.Contains(t)).ToList();
                    var possibleNeighbors = matchingDict.Where(k => neighbours.Contains(k.Key) && k.Value.Count == (expectedNoNeighbors * 2));
                    var next = tiles.First(t => t.Id == possibleNeighbors.First().Key);
                    next.TilePosition = new Position { x = x, y = y };
                    availableTiles.Remove(next.Id);
                    image.Add(next);
                    currentTile = next;

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

            tilesMatch = tilesMatch.GroupBy(t => new { t.TileId1, t.TileId2}).Select(t => t.First()).ToList();

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

        private List<Tile> GetAllPossibleTileVariations(Tile tile)
        {
            var tiles = new List<Tile>();
            tiles.Add(tile);
            var rotatedRight = tile.RotateRight();
            tiles.Add(rotatedRight);
            var rotatedRightTwice = rotatedRight.RotateRight();
            tiles.Add(rotatedRightTwice);
            var rotatedRightThrice = rotatedRightTwice.RotateRight();
            tiles.Add(rotatedRightThrice);

            var rotatedLeft = tile.RotateLeft();
            tiles.Add(rotatedLeft);
            var rotatedLefttwice = rotatedLeft.RotateLeft();
            tiles.Add(rotatedLefttwice);
            var rotatedLeftThrice = rotatedLefttwice.RotateLeft();
            tiles.Add(rotatedLeftThrice);

            var flippedUpsideDown = tile.FlipUpsideDown();
            tiles.Add(flippedUpsideDown);
            var flippedLeftToRight = tile.FlipLeftToRight();
            tiles.Add(flippedLeftToRight);

            return tiles;
        }

        internal class Tile
        {
            public int Id;
            public Position TilePosition;
            public List<Position> Positions;
            public int MaxLength;
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
                MaxLength = maxLength;
                Left = new HashSet<int>(Positions.Where(p => p.y == 0).Select(r => r.x));
                RotatedLeft = new HashSet<int>(Left.Select(m => maxLength - m));
                Right = new HashSet<int>(Positions.Where(p => p.y == maxLength).Select(r => r.x));
                RotatedRight = new HashSet<int>(Right.Select(m => maxLength - m));
                Top = new HashSet<int>(Positions.Where(p => p.x == 0).Select(r => r.y));
                RotatedTop = new HashSet<int>(Top.Select(m => maxLength - m));
                Bottom = new HashSet<int>(Positions.Where(p => p.x == maxLength).Select(r => r.y));
                RotatedBottom = new HashSet<int>(Bottom.Select(m => maxLength - m));
            }

            public Tile RotateRight()
            {
                return new Tile(Id, TilePosition, Positions.Select(p => new Position {x = p.y,  y = MaxLength - p.x}).ToList(), MaxLength);
            }

            public Tile RotateLeft()
            {
                return new Tile(Id, TilePosition, Positions.Select(p => new Position {x = MaxLength - p.y, y = p.x}).ToList(), MaxLength);
            }

            public Tile FlipLeftToRight()
            {
                return new Tile(Id, TilePosition, Positions.Select(p => new Position { x = p.x, y = MaxLength - p.y }).ToList(), MaxLength);
            }

            public Tile FlipUpsideDown()
            {
                return new Tile(Id, TilePosition, Positions.Select(p => new Position { x = MaxLength - p.x, y = p.y }).ToList(), MaxLength);
            }

            public void Print()
            {
                for (int x = 0; x <= MaxLength; x++)
                {
                    for(int y = 0; y <= MaxLength; y++)
                    {
                        if (Positions.Contains(new Position { x = x, y = y}))
                        {
                            Console.Write('#');
                        }
                        else
                        {
                            Console.Write('.');
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        [DebuggerDisplay("x = {x}, y = {y}")]
        internal struct Position
        {
            public int x;
            public int y;
        }
    }
}
