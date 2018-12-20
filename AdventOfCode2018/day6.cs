//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var lines = File.ReadLines(@"..\..\..\input\day6.txt").ToList();

//            //lines = new List<string>
//            //{
//            //    "1, 1",
//            //    "1, 6",
//            //    "8, 3",
//            //    "3, 4",
//            //    "5, 5",
//            //    "8, 9"
//            //};

//            List<Coord> coords = lines.Select(cor => new Coord { x = int.Parse(cor.Split(',')[0]), y = int.Parse(cor.Split(',')[1]) }).ToList();

//            int maxHeight = 0;
//            int maxWidth = 0;

//            foreach (var coordinate in coords)
//            {
//                if (coordinate.y > maxHeight)
//                {
//                    maxHeight = coordinate.y;
//                }
//                if (coordinate.x > maxWidth)
//                {
//                    maxWidth = coordinate.x;
//                }
//            }

//            int[,] coordinateSystem = new int[maxWidth + 1, maxHeight + 1];
//            int counter = 0;

//            foreach (var coordinate in lines)
//            {
//                counter++;
//                var x = int.Parse(coordinate.Split(',')[0]);
//                var y = int.Parse(coordinate.Split(',')[1]);

//                coordinateSystem[x, y] = counter;

//            }

//            //Part 2
//            var maxDistance = 10000;
//            var regionSize = 0;

//            for (int x = 0; x < maxWidth + 1; x++)
//            {
//                for (int y = 0; y < maxHeight + 1; y++)
//                {
//                    var distanceToOtherCoordinates = coords.Sum(co => Math.Abs(x - co.x) + Math.Abs(y - co.y));
//                    if (distanceToOtherCoordinates < maxDistance)
//                    {
//                        coordinateSystem[x, y] = -1;
//                        regionSize++;
//                    }
//                }
//            }

//            Console.WriteLine(regionSize);
//            Console.ReadLine();

//            //Part 1
//            int[,] originalCoordinateSystem = (int[,])coordinateSystem.Clone();

//            for (int x = 0; x < maxWidth + 1; x++)
//            {
//                for (int y = 0; y < maxHeight + 1; y++)
//                {
//                    var curCor = coordinateSystem[x, y];
//                    if (curCor == 0)
//                    {
//                        var closestNumber = getClosestNumber(originalCoordinateSystem, x, y, maxHeight + 1, maxWidth + 1);
//                        coordinateSystem[x, y] = closestNumber;
//                    }
//                }
//            }
//            HashSet<int> borderCoords = new HashSet<int>();

//            for (int i = 0; i < maxHeight + 1; i++)
//            {
//                borderCoords.Add(coordinateSystem[0, i]);
//                borderCoords.Add(coordinateSystem[maxWidth, i]);
//            }

//            for (int i = 0; i < maxWidth + 1; i++)
//            {
//                borderCoords.Add(coordinateSystem[i, 0]);
//                borderCoords.Add(coordinateSystem[i, maxHeight]);
//            }
//            var numberList = Enumerable.Range(1, counter).ToHashSet();

//            numberList.ExceptWith(borderCoords);

//            var maxNum = 0;

//            foreach (var num in numberList)
//            {
//                var numberOccurences = getOccurencesOfNumber(coordinateSystem, num, maxHeight, maxWidth);

//                if (numberOccurences > maxNum)
//                {
//                    maxNum = numberOccurences;
//                }
//            }

//            Console.WriteLine(maxNum);
//            Console.ReadLine();


//        }

//        public static int getOccurencesOfNumber(int[,] coordinateSystem, int num, int height, int width)
//        {
//            var occurences = 0;
//            for (int i = 1; i < width; i++)
//            {
//                for (int j = 1; j < height; j++)
//                {
//                    if (coordinateSystem[i, j] == num)
//                    {
//                        occurences++;
//                    }
//                }
//            }
//            return occurences;
//        }

//        public static int getClosestNumber(int[,] originalCoordinateSystem, int x, int y, int maxHeight, int maxWidth)
//        {
//            for (int k = 1; k < maxWidth; k++)
//            {
//                var testCoordinates = getTestCoordinates(x, y, k);
//                var closestValue = -2;
//                foreach (var coord in testCoordinates)
//                {
//                    if (!(x + coord.x >= maxWidth || y + coord.y >= maxHeight)
//                        && !(x + coord.x < 0 || y + coord.y < 0))
//                    {
//                        var coorValue = originalCoordinateSystem[x + coord.x, y + coord.y];
//                        if (coorValue != 0)
//                        {
//                            if (closestValue == -2)
//                            {
//                                closestValue = coorValue;
//                            }
//                            else if (closestValue != coorValue)
//                            {
//                                closestValue = -1;
//                            }
//                        }
//                    }
//                    if (closestValue == -1)
//                    {
//                        break;
//                    }
//                }
//                if (closestValue > -2)
//                {
//                    return closestValue;
//                }
//            }
//            return originalCoordinateSystem[x, y];
//        }

//        public static HashSet<Coord> getTestCoordinates(int x, int y, int distance)
//        {
//            HashSet<Coord> testCoords = new HashSet<Coord>();

//            for (int i = 0; i < distance; i++)
//            {
//                testCoords.Add(new Coord { x = -i, y = distance - i });
//                testCoords.Add(new Coord { x = i, y = i - distance });
//                testCoords.Add(new Coord { x = distance - i, y = i });
//                testCoords.Add(new Coord { x = -distance + i, y = -i });
//            }

//            return testCoords;
//        }

//        public struct Coord
//        {
//            public int x;
//            public int y;
//        }
//    }
//}
