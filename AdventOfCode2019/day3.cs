using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day3
    {

        public Day3()
        {
            var lines = File.ReadLines(@"..\..\..\input\day3.txt").ToList();

            var firstInstructions = lines[0].Split(',');
            var secondInstructions = lines[1].Split(',');

            //Part1
            var distance = GetDistance(firstInstructions, secondInstructions);

            Console.WriteLine(distance);

            //Part2
            var numberOfSteps = GetNumberOfSteps(firstInstructions, secondInstructions);

            Console.WriteLine(numberOfSteps);
        }

        public static int GetDistance(string[] firstInstructions, string[] secondInstructions)
        {
            List<Point> pointsCoveredByFirstInstructions = GetPointsCoveredByInstructions_Part1(firstInstructions);
            List<Point> pointsCoveredBySecondInstructions = GetPointsCoveredByInstructions_Part1(secondInstructions);

            List<Point> intersectingPoints = pointsCoveredByFirstInstructions.Intersect(pointsCoveredBySecondInstructions, new CustomComparer()).Where(p => p.x != 0 && p.y != 0).ToList();

            var closestDistance = intersectingPoints.Min(p => p.manhattanDistance);

            return closestDistance;
        }

        public static int GetNumberOfSteps(string[] firstInstructions, string[] secondInstructions)
        {
            List<Point> pointsCoveredByFirstInstructions = GetPointsCoveredByInstructions_Part2(firstInstructions);
            List<Point> pointsCoveredBySecondInstructions = GetPointsCoveredByInstructions_Part2(secondInstructions);

            List<Point> intersectingPoints = pointsCoveredByFirstInstructions.Intersect(pointsCoveredBySecondInstructions, new CustomComparer()).Where(p => p.x != 0 && p.y != 0).ToList();

            var smallestSum = int.MaxValue;

            foreach (var point in intersectingPoints)
            {
                var firstInstructionPoint = pointsCoveredByFirstInstructions.First(p => p.x == point.x && p.y == point.y);
                var secondInstructionPoint = pointsCoveredBySecondInstructions.First(p => p.x == point.x && p.y == point.y);

                var sum = firstInstructionPoint.numberOfSteps + secondInstructionPoint.numberOfSteps;

                if( sum < smallestSum)
                {
                    smallestSum = sum;
                }
            }

            return smallestSum;
        }

        private static List<Point> GetPointsCoveredByInstructions_Part2(string[] instructions)
        {
            List<Point> points = new List<Point>();
            Point currentPoint = new Point(0,0);
            int numberOfSteps = 1;
            foreach (string instruction in instructions)
            {
                var direction = instruction.Substring(0, 1);
                var distance = int.Parse(instruction.Substring(1, instruction.Length-1));

                switch (direction)
                {
                    case "R":
                        points.AddRange(Enumerable.Range(1, distance).Select(i => new Point(currentPoint.x, currentPoint.y + i, numberOfSteps++)));
                        currentPoint = new Point(currentPoint.x, currentPoint.y + distance);
                        break;
                    case "L":
                        points.AddRange(Enumerable.Range(1, distance).Select(i => new Point(currentPoint.x, currentPoint.y - i, numberOfSteps++)));
                        currentPoint = new Point(currentPoint.x, currentPoint.y - distance);
                        break;
                    case "U":
                        points.AddRange(Enumerable.Range(1, distance).Select(i => new Point(currentPoint.x - i, currentPoint.y, numberOfSteps++)));
                        currentPoint = new Point(currentPoint.x - distance, currentPoint.y);
                        break;
                    case "D":
                        points.AddRange(Enumerable.Range(1, distance).Select(i => new Point(currentPoint.x + i, currentPoint.y, numberOfSteps++)));
                        currentPoint = new Point(currentPoint.x + distance, currentPoint.y);
                        break;
                    default:
                        break;
                }
            }
            return points.Distinct().ToList();
        }


        private static List<Point> GetPointsCoveredByInstructions_Part1(string[] instructions)
        {
            List<Point> points = new List<Point>();
            Point currentPoint = new Point(0, 0);
            foreach (string instruction in instructions)
            {
                var direction = instruction.Substring(0, 1);
                var distance = int.Parse(instruction.Substring(1, instruction.Length - 1));

                switch (direction)
                {
                    case "R":
                        points.AddRange(Enumerable.Range(0, distance + 1).Select(i => new Point(currentPoint.x, currentPoint.y + i)));
                        currentPoint = new Point(currentPoint.x, currentPoint.y + distance);
                        break;
                    case "L":
                        points.AddRange(Enumerable.Range(0, distance + 1).Select(i => new Point(currentPoint.x, currentPoint.y - i)));
                        currentPoint = new Point(currentPoint.x, currentPoint.y - distance);
                        break;
                    case "U":
                        points.AddRange(Enumerable.Range(0, distance + 1).Select(i => new Point(currentPoint.x - i, currentPoint.y)));
                        currentPoint = new Point(currentPoint.x - distance, currentPoint.y);
                        break;
                    case "D":
                        points.AddRange(Enumerable.Range(0, distance + 1).Select(i => new Point(currentPoint.x + i, currentPoint.y)));
                        currentPoint = new Point(currentPoint.x + distance, currentPoint.y);
                        break;
                    default:
                        break;
                }
            }
            return points.Distinct().ToList();
        }

        public class Point
        {
            public int x;
            public int y;
            public int manhattanDistance;
            public int numberOfSteps = 0;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
                manhattanDistance = Math.Abs(x) + Math.Abs(y);
            }

            public Point(int x, int y, int numberOfSteps)
            {
                this.x = x;
                this.y = y;
                this.numberOfSteps = numberOfSteps;
                manhattanDistance = Math.Abs(x) + Math.Abs(y);
            }
        }

        class CustomComparer : IEqualityComparer<Point>
        {

            public bool Equals(Point p1, Point p2)
            {
                return p1.x == p2.x && p1.y == p2.y;
            }

         
            public int GetHashCode(Point obj)
            {
                return obj.x * 12348;
            }
        }
    }
}
