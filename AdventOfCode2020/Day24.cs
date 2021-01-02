using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Day24
    {
        public Day24()
        {
            var lines = File.ReadLines(@"..\..\input\day24.txt").ToList();

            var directionList = new List<List<string>>();
            foreach (var line in lines)
            {
                var ins = string.Empty;
                var directions = new List<string>();
                foreach (var c in line)
                {
                    ins += c;
                    if (isDirection(ins))
                    {
                        directions.Add(ins);
                        ins = string.Empty;
                    }
                }
                directionList.Add(directions);
            }

            HashSet<Position> blackTiles = GetTilesToFlip(directionList);

            Console.WriteLine(blackTiles.Count());

            var positions = directionList.Select(dl => FindPosition(dl));

            var counter = 0;
            while (counter < 100)
            {
                var tilesToCheck = blackTiles.Concat(new HashSet<Position>(blackTiles.SelectMany(b => GetNeighbours(b))));
                var tempBlackTiles = new HashSet<Position>(blackTiles);
                foreach (var tile in tilesToCheck)
                {
                    var isBlack = blackTiles.Contains(tile);
                    var tileNeighbours = GetNeighbours(tile);
                    var blackNeighbours = blackTiles.Intersect(tileNeighbours);

                    if (isBlack)
                    {
                        if (blackNeighbours.Count() == 0 || blackNeighbours.Count() > 2)
                        {
                            tempBlackTiles.Remove(tile);
                        }
                    }
                    else
                    {
                        if (blackNeighbours.Count() == 2)
                        {
                            tempBlackTiles.Add(tile);
                        }
                    }

                }
                blackTiles = tempBlackTiles;
                counter++;

            }

            Console.WriteLine(blackTiles.Count());
        }

        private HashSet<Position> GetNeighbours(Position pos)
        {
            var neighbours = new HashSet<Position>();
            neighbours.Add(new Position { x = pos.x - 2, y = pos.y });
            neighbours.Add(new Position { x = pos.x - 1, y = pos.y + 1 });
            neighbours.Add(new Position { x = pos.x + 1, y = pos.y + 1 });
            neighbours.Add(new Position { x = pos.x + 2, y = pos.y });
            neighbours.Add(new Position { x = pos.x + 1, y = pos.y - 1 });
            neighbours.Add(new Position { x = pos.x - 1, y = pos.y - 1 });
            return neighbours;
        }

        private static HashSet<Position> GetTilesToFlip(List<List<string>> directionList)
        {
            var blackTiles = new HashSet<Position>();

            foreach (var directions in directionList)
            {
                Position currentPosition = FindPosition(directions);

                if (blackTiles.Contains(currentPosition))
                {
                    blackTiles.Remove(currentPosition);
                }
                else
                {
                    blackTiles.Add(currentPosition);
                }
            }

            return blackTiles;
        }

        private static Position FindPosition(List<string> directions)
        {
            var currentPosition = new Position { x = 0, y = 0 };
            foreach (var dir in directions)
            {
                switch (dir)
                {
                    case "w":
                        currentPosition = new Position { x = currentPosition.x - 2, y = currentPosition.y };
                        break;
                    case "nw":
                        currentPosition = new Position { x = currentPosition.x - 1, y = currentPosition.y + 1 };
                        break;
                    case "ne":
                        currentPosition = new Position { x = currentPosition.x + 1, y = currentPosition.y + 1 };
                        break;
                    case "e":
                        currentPosition = new Position { x = currentPosition.x + 2, y = currentPosition.y };
                        break;
                    case "se":
                        currentPosition = new Position { x = currentPosition.x + 1, y = currentPosition.y - 1 };
                        break;
                    case "sw":
                        currentPosition = new Position { x = currentPosition.x - 1, y = currentPosition.y - 1 };
                        break;
                    default:
                        throw new Exception("");

                }
            }

            return currentPosition;
        }

        internal struct Position
        {
            public int x;
            public int y;
        }

        private bool isDirection(string ins)
        {
            if (ins == "w" || ins == "nw" || ins == "ne" || ins == "e" 
                || ins == "se" || ins == "sw")
            {
                return true;
            }
            return false;
        }

    }
}
