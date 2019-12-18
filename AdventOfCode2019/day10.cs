using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day10
    {

        public static char Empty = '.';
        public static char AsteroidSign = '#';
        public static decimal MagicVerticalSlope = -400000m;
        public static int AsteroidsToFind = 200; //Part2;

        public Day10()
        {
            var lines = File.ReadLines(@"..\..\..\input\day10.txt");

            var map = new List<List<int>>();

            foreach (var line in lines)
            {
                var splitLine = line.ToCharArray().Select(c => c == AsteroidSign ? 1 : 0).ToList();
                map.Add(splitLine);
            }

            //Part1
            var (x, y, numAsteroids) = GetBestLocation(map);
            
            Console.WriteLine(numAsteroids);

            //Part2
            var (vaporX, vaporY) = GetAsteroidToByVaporized(AsteroidsToFind, map);

            Console.WriteLine((vaporX*100) +vaporY);
        }

        public struct SlopeQuadrant
        {
            public int x;
            public int y;
            public Quadrant quadrant;
            public decimal slope;

            public SlopeQuadrant(int x, int y, Quadrant quadrant, decimal slope)
            {
                this.x = x;
                this.y = y;
                this.quadrant = quadrant;
                this.slope = slope;
            }
        }

        public static (int, int) GetAsteroidToByVaporized(int asteroidsToFind, List<List<int>> map)
        {
            List<Asteroid> allAsteroids = GetAllAsteroids(map);

            Dictionary<Asteroid, Dictionary<decimal, List<Asteroid>>> asteroidsDetected = GetAsteroidMapping(allAsteroids);
            var maxAsteroid = asteroidsDetected.OrderByDescending(r => r.Value.SelectMany(s => s.Value).Count()).First();

            var groupedAsteroids = maxAsteroid.Value.SelectMany(s => s.Value.Select(b => new SlopeQuadrant(b.x, b.y, b.quadrant, s.Key))).GroupBy(s => s.quadrant);

            var quadrantDirections = new[] {Quadrant.UpRight, Quadrant.DownRight, Quadrant.DownLeft, Quadrant.UpLeft };

            foreach (var quadrantDirection in quadrantDirections)
            {
                var group = groupedAsteroids.First(k => k.Key == quadrantDirection);
                if (group.Count() < asteroidsToFind)
                {
                    asteroidsToFind -= group.Count();
                }
                else
                {
                    var sortedBySlope = group.OrderBy(a => a.slope);
                    foreach (var ast in sortedBySlope)
                    {
                        asteroidsToFind--;
                        if (asteroidsToFind == 0)
                        {
                            return (ast.x, ast.y);
                        }
                    }
                }
            }

            return (0,0);
        }

        public static (int, int, int) GetBestLocation(List<List<int>> map)
        {
            List<Asteroid> allAsteroids = GetAllAsteroids(map);

            Dictionary<Asteroid, Dictionary<decimal, List<Asteroid>>> asteroidsDetected = GetAsteroidMapping(allAsteroids);

            var maxAsteroid = asteroidsDetected.OrderByDescending(r => r.Value.SelectMany(s => s.Value).Count()).First();

            return (maxAsteroid.Key.x, maxAsteroid.Key.y, maxAsteroid.Value.SelectMany(s => s.Value).Count());
        }

        private static List<Asteroid> GetAllAsteroids(List<List<int>> map)
        {
            var allAsteroids = new List<Asteroid>();
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[0].Count; x++)
                {
                    if (map[y][x] == 1)
                    {
                        allAsteroids.Add(new Asteroid(x, y));
                    }
                }
            }

            return allAsteroids;
        }

        private static Dictionary<Asteroid, Dictionary<decimal, List<Asteroid>>> GetAsteroidMapping(List<Asteroid> asteroids)
        {
            var asteroidsMapping = new Dictionary<Asteroid, Dictionary<decimal, List<Asteroid>>>();

            foreach (var asteroid in asteroids)
            {
                var slopes = new Dictionary<decimal, List<Asteroid>>();
                foreach (var other in asteroids.Where(a => a.x != asteroid.x || a.y != asteroid.y))
                {
                    var slope = 0m;
                    if ((other.x - asteroid.x) == 0)
                    {
                        slope = MagicVerticalSlope;
                    }
                    else
                    {
                        slope = decimal.Divide(other.y - (decimal)asteroid.y,
                                                other.x - (decimal)asteroid.x);
                    }
                    var quadrant = GetQuadrant(asteroid, other);
                    var otherWithQuadrant = other.WithQuadrant(quadrant);

                    if (slopes.ContainsKey(slope))
                    {
                        var existsOnSameQuadrant = slopes[slope].Any(a => GetQuadrant(asteroid, a) == quadrant);

                        if (existsOnSameQuadrant)
                        {
                            var existingAsteroid = slopes[slope].First();
                            var isCloser = IsOtherAsteroidCloser(other, existingAsteroid, asteroid);

                            if (isCloser)
                            {
                                slopes[slope].Remove(existingAsteroid);
                                slopes[slope].Add(otherWithQuadrant);
                            }
                        }

                        if (!existsOnSameQuadrant)
                        {
                            slopes[slope].Add(otherWithQuadrant);
                        }
                    }
                    else
                    {
                        slopes[slope] = new List<Asteroid>() { otherWithQuadrant };
                    }
                }
                asteroidsMapping.Add(asteroid, slopes);
            }

            return asteroidsMapping;
        }

        public static bool IsOtherAsteroidCloser(Asteroid other, Asteroid existing, Asteroid asteroid)
        {
            return Math.Abs(other.x - asteroid.x) + Math.Abs(other.y - asteroid.y)
                < Math.Abs(existing.x - asteroid.x) + Math.Abs(existing.y - asteroid.y);
        }

        public enum Quadrant
        {
            UpLeft,
            UpRight,
            DownLeft,
            DownRight,
            None
        }

        public static Quadrant GetQuadrant(Asteroid a, Asteroid b)
        {
            if (a.x <= b.x)
            {
                if (a.y < b.y)
                {
                    return Quadrant.DownRight;
                }
                else
                {
                    return Quadrant.UpRight;
                }
            }
            else
            {
                if (a.y < b.y)
                {
                    return Quadrant.DownLeft;
                }
                else
                {
                    return Quadrant.UpLeft;
                }
            }
        }

        public class Asteroid
        {
            public int x;
            public int y;
            public Quadrant quadrant = Quadrant.None;

            public Asteroid(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Asteroid(int x, int y, Quadrant quadrant)
            {
                this.x = x;
                this.y = y;
                this.quadrant = quadrant;
            }

            public Asteroid WithQuadrant(Quadrant quadrant)
            {
                return new Asteroid(x, y, quadrant);
            }
        }
    }
}
