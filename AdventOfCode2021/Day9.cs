using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
    class Day9
    {
        public Day9()
        {
            var lines = File.ReadLines(@"..\..\..\input\day9.txt");

            var lowPoints = new Dictionary<Tuple<int, int>, int>();

            //Part 1
            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines.ElementAt(i).Select(n => int.Parse(n.ToString())).ToArray();
                var lineBelow = i < lines.Count()-1 ? lines.ElementAt(i + 1).Select(n => int.Parse(n.ToString())).ToArray() : null;
                var lineAbove = i > 0 ? lines.ElementAt(i - 1).Select(n => int.Parse(n.ToString())).ToArray() : null;
                for (int j = 0; j<line.Length; j++)
                {
                    var currentNumber = line[j];
                    var neighbours = new List<int>();
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            neighbours.Add(line[j+1]);
                            neighbours.Add(lineBelow[j]);
                            //neighbours.Add(lineBelow[j + 1]);
                        }
                        else if (j == line.Length - 1)
                        {
                            neighbours.Add(line[line.Length - 2]);
                            neighbours.Add(lineBelow[j]);
                            //neighbours.Add(lineBelow[line.Length - 2]);
                        }
                        else
                        {
                            neighbours.Add(line[j + 1]);
                            neighbours.Add(line[j - 1]);
                            neighbours.Add(lineBelow[j]);
                            //neighbours.Add(lineBelow[j + 1]);
                            //neighbours.Add(lineBelow[j - 1]);
                        }
                    }
                    else if (j == 0)
                    {
                        if (lineAbove != null)
                        {
                            neighbours.Add(lineAbove[j]);
                            //neighbours.Add(lineAbove[j+1]);
                        }

                        if (lineBelow != null)
                        {
                            neighbours.Add(lineBelow[j]);
                            //neighbours.Add(lineBelow[j + 1]);
                        }
                        neighbours.Add(line[j+1]);
                    }
                    else if (i == lines.Count()-1)
                    {
                        if (j == 0)
                        {
                            neighbours.Add(line[j + 1]);
                            neighbours.Add(lineAbove[j]);
                            //neighbours.Add(lineAbove[j + 1]);
                        }
                        else if (j == line.Length - 1)
                        {
                            neighbours.Add(line[line.Length -2]);
                            neighbours.Add(lineAbove[j]);
                            //neighbours.Add(lineAbove[line.Length - 2]);
                        }
                        else
                        {
                            neighbours.Add(line[j + 1]);
                            neighbours.Add(line[j - 1]);
                            neighbours.Add(lineAbove[j]);
                            //neighbours.Add(lineAbove[j + 1]);
                            //neighbours.Add(lineAbove[j - 1]);
                        }
                    }
                    else if (j == line.Length - 1)
                    {
                        if (lineAbove != null)
                        {
                            neighbours.Add(lineAbove[line.Length-1]);
                            //neighbours.Add(lineAbove[line.Length - 2]);
                        }
                        if (lineBelow != null)
                        {
                            neighbours.Add(lineBelow[j]);
                            //neighbours.Add(lineBelow[line.Length - 2]);
                        }
                        
                        neighbours.Add(line[line.Length - 2]);
                       
                    }
                    else
                    {
                        neighbours.Add(line[j + 1]);
                        neighbours.Add(line[j - 1]);
                        neighbours.Add(lineBelow[j]);
                        //neighbours.Add(lineBelow[j + 1]);
                        //neighbours.Add(lineBelow[j - 1]);
                        //neighbours.Add(lineAbove[j - 1]);
                        neighbours.Add(lineAbove[j]);
                        //neighbours.Add(lineAbove[j + 1]);
                    }

                    var isLowest = neighbours.All(n => n > currentNumber);
                    if (isLowest)
                        lowPoints.Add(Tuple.Create(i, j), currentNumber);

                }
                
            }

            Console.WriteLine(lowPoints.Sum(b => b.Value+1));

            //Part 2
            var locMap = lines.Select(l => l.Select(el => int.Parse(el.ToString())).ToArray()).ToArray();
            var basins = new List<List<int>>();

            foreach (var lowPoint in lowPoints)
            {
                var pos = lowPoint.Key;
                var posX = pos.Item2;
                var posY = pos.Item1;
                var basin = new List<int>();
                basin.Add(lowPoint.Value);
                var visitedPoints = new List<Tuple<int, int>>();
                visitedPoints.Add(pos);
                var inFourDirections = GetValuesInAllFourDirectionsForPoint(pos, locMap);
                var pointsToVisit = inFourDirections.Where(k => !visitedPoints.Contains(k.Key)).ToList();
                visitedPoints.AddRange(pointsToVisit.Select(k => k.Key));
                basin.AddRange(pointsToVisit.Select(k => k.Value));
                while (pointsToVisit.Any())
                {
                    var newPointsToVisit = pointsToVisit;
                    pointsToVisit = new List<KeyValuePair<Tuple<int, int>, int>>();
                    foreach (var point in newPointsToVisit)
                    {
                        inFourDirections = GetValuesInAllFourDirectionsForPoint(point.Key, locMap);
                        var newPoints = inFourDirections.Where(k => !visitedPoints.Contains(k.Key)).ToList();
                        pointsToVisit.AddRange(newPoints);
                        visitedPoints.AddRange(newPoints.Select(k => k.Key));
                        basin.AddRange(newPoints.Select(k => k.Value));
                    }
                }
                basins.Add(basin);
            }

            var largestBasins = basins.OrderByDescending(b => b.Count).Take(3);
            var basingProduct = largestBasins.Select(b => b.Count()).Aggregate((a, b) => a * b);
            Console.WriteLine(basingProduct);
        }

        private static Dictionary<Tuple<int, int>, int> GetValuesInAllFourDirectionsForPoint(Tuple<int, int> startPos, int[][] locMap)
        {
            var newDict = new Dictionary<Tuple<int, int>, int>();
            GetValuesToTheLeft(startPos, locMap)
                .Concat(GetValuesToTheRight(startPos, locMap))
                .Concat(GetValuesUp(startPos, locMap))
                .Concat(GetValuesDown(startPos, locMap))
                .ToList()
                .ForEach(x => newDict.Add(x.Key, x.Value));
            return newDict;
        }

        private static Dictionary<Tuple<int, int>, int> GetValuesToTheLeft(Tuple<int, int> startPos, int[][] locMap)
        {
            var list = new Dictionary<Tuple<int, int>, int>();
            var posX = startPos.Item2 - 1;
            var posY = startPos.Item1;
            while (posX >= 0)
            {
                var numberInPos = locMap[posY][posX];
                if (numberInPos == 9)
                {
                    return list;
                }
                list.Add(Tuple.Create(posY, posX), numberInPos);
                posX--;
            }
            return list;
        }

        private static Dictionary<Tuple<int, int>, int> GetValuesToTheRight(Tuple<int, int> startPos, int[][] locMap)
        {
            var list = new Dictionary<Tuple<int, int>, int>();
            var posX = startPos.Item2 + 1;
            var posY = startPos.Item1;
            while (posX < locMap[0].Length)
            {
                var numberInPos = locMap[posY][posX];
                if (numberInPos == 9)
                {
                    return list;
                }
                list.Add(Tuple.Create(posY, posX), numberInPos);
                posX++;
            }
            return list;
        }

        private static Dictionary<Tuple<int, int>, int> GetValuesUp(Tuple<int, int> startPos, int[][] locMap)
        {
            var list = new Dictionary<Tuple<int, int>, int>();
            var posX = startPos.Item2;
            var posY = startPos.Item1 - 1;
            while (posY >= 0)
            {
                var numberInPos = locMap[posY][posX];
                if (numberInPos == 9)
                {
                    return list;
                }
                list.Add(Tuple.Create(posY, posX), numberInPos);
                posY--;
            }
            return list;
        }

        private static Dictionary<Tuple<int, int>, int> GetValuesDown(Tuple<int, int> startPos, int[][] locMap)
        {
            var list = new Dictionary<Tuple<int, int>, int>();
            var posX = startPos.Item2;
            var posY = startPos.Item1 + 1;
            while (posY < locMap.Length)
            {
                var numberInPos = locMap[posY][posX];
                if (numberInPos == 9)
                {
                    return list;
                }
                list.Add(Tuple.Create(posY, posX), numberInPos);
                posY++;
            }
            return list;
        }
    }
}
