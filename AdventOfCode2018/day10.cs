//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{
//    class Program
//    {
//        public static int lowestX;
//        public static int lowestY;
//        public static int highestX;
//        public static int highestY;
//        public static int height;
//        public static int width;

//        static void Main(string[] args)
//        {
//            var lines = File.ReadLines(@"..\..\..\input\day10.txt").ToList();
//            //lines = new List<string>() {
//            //    "position =< 9,  1 > velocity =< 0,  2 >",
//            //    "position =< 7,  0 > velocity =< -1,  0 >",
//            //    "position =< 3, -2 > velocity =< -1,  1 >",
//            //    "position =< 6, 10 > velocity =< -2, -1 >",
//            //    "position =< 2, -4 > velocity =< 2,  2 >",
//            //    "position =< -6, 10 > velocity =< 2, -2 >",
//            //    "position =< 1,  8 > velocity =< 1, -1 >",
//            //    "position =< 1,  7 > velocity =< 1,  0 >",
//            //    "position =< -3, 11 > velocity =< 1, -2 >",
//            //    "position =< 7,  6 > velocity =< -1, -1 >",
//            //    "position =< -2,  3 > velocity =< 1,  0 >",
//            //    "position =< -4,  3 > velocity =< 2,  0 >",
//            //    "position =< 10, -3 > velocity =< -1,  1 >",
//            //    "position =< 5, 11 > velocity =< 1, -2 >",
//            //    "position =< 4,  7 > velocity =< 0, -1 >",
//            //    "position =< 8, -2 > velocity =< 0,  1 >",
//            //    "position =< 15,  0 > velocity =< -2,  0 >",
//            //    "position =< 1,  6 > velocity =< 1,  0 >",
//            //    "position =< 8,  9 > velocity =< 0, -1 >",
//            //    "position =< 3,  3 > velocity =< -1,  1 >",
//            //    "position =< 0,  5 > velocity =< 0, -1 >",
//            //    "position =< -2,  2 > velocity =< 2,  0 >",
//            //    "position =< 5, -2 > velocity =< 1,  2 >",
//            //    "position =< 1,  4 > velocity =< 2,  1 >",
//            //    "position =< -2,  7 > velocity =< 2, -2 >",
//            //    "position =< 3,  6 > velocity =< -1, -1 >",
//            //    "position =< 5,  0 > velocity =< 1,  0 >",
//            //    "position =< -6,  0 > velocity =< 2,  0 >",
//            //    "position =< 5,  9 > velocity =< 1, -2 >",
//            //    "position =< 14,  7 > velocity =< -2,  0 >",
//            //    "position =< -3,  6 > velocity =< 2, -1 >"
//            //};

//            Dictionary<int, Point> points = ParseLinesToPoints(lines);

//            lowestX = points.Values.Min(p => p.x);
//            highestX = points.Values.Max(p => p.x);
//            width = Math.Abs(highestX) + Math.Abs(lowestX);
//            lowestY = points.Values.Min(p => p.y);
//            highestY = points.Values.Max(p => p.y);
//            height = Math.Abs(highestY) + Math.Abs(lowestY);

//            lastLocalWidth = width;
//            lastLocalHeight = height;

//            var counter = 0;

//            while (true)
//            {
//                try
//                {
//                    points = UpdatePoints(points);
//                    var containsPoints = PrintArea(points);
//                    if (!containsPoints)
//                    {
//                        break;
//                    }
//                }
//                catch (Exception e)
//                {
//                    break;
//                }
//                counter++;
//            }


//            Console.WriteLine(counter);

//            Console.ReadLine();

//        }

//        public static int lastLocalWidth;
//        public static int lastLocalHeight;
//        public static int lastLowestY;
//        public static int lastLowestX;
//        public static int lastHighestX;
//        public static int lastHighestY;


//        public static Dictionary<int, Point> UpdatePoints(Dictionary<int, Point> points)
//        {
//            Dictionary<int, Point> newList = new Dictionary<int, Point>();
//            foreach (var entry in points)
//            {
//                newList[entry.Key] = entry.Value.UpdatePointBasedOnVelocity();
//            }
//            return newList;
//        }

//        public static Dictionary<int, Point> previousPoints;

//        public static bool PrintArea(Dictionary<int, Point> points)
//        {
//            var lines = new List<string[]>();
//            var localLowestX = points.Min(p => p.Value.x);
//            var localLowestY = points.Min(p => p.Value.y);
//            var localHighestX = points.Max(p => p.Value.x);
//            var localHighestY = points.Max(p => p.Value.y);

//            var localWidth = localHighestX - localLowestX;
//            var localHeight = localHighestY - localLowestY;
//            if (localWidth > lastLocalWidth || localHeight > lastLocalHeight)
//            {
//                for (int i = lastLowestY; i < lastHighestY + 1; i++)
//                {
//                    var line = new string[lastLocalWidth + 1];

//                    for (int j = lastLowestX; j < lastHighestX + 1; j++)
//                    {
//                        var symbol = ".";
//                        line[j - lastLowestX] = symbol;
//                    }
//                    lines.Add(line);
//                }

//                foreach (var point in previousPoints)
//                {
//                    lines.ElementAt(point.Value.y - lastLowestY)[point.Value.x - lastLowestX] = "#";
//                }
//                //lines = lines.Skip(lastLowestY + Math.Abs(lowestY)).Take(lastLocalHeight + 1).ToList();
//                using (StreamWriter file = new StreamWriter(@"..\..\..\day10Output.txt", true))
//                {
//                    file.WriteLine("***************************************");
//                    file.WriteLine();

//                    foreach (var line in lines)
//                    {
//                        file.WriteLine(string.Join(string.Empty, line));//.Skip(lastLowestX + Math.Abs(lowestX)).Take(lastLocalWidth + 1)));
//                    }
//                    file.WriteLine();
//                    file.WriteLine("***************************************");
//                    file.WriteLine();
//                }
//                return false;
//            }
//            lastLocalHeight = localHeight;
//            lastLocalWidth = localWidth;
//            lastLowestY = localLowestY;
//            lastLowestX = localLowestX;
//            lastHighestX = localHighestX;
//            lastHighestY = localHighestY;
//            previousPoints = points.ToDictionary(entry => entry.Key,
//                                               entry => new Point { x = entry.Value.x, y = entry.Value.y, x_Velocity = entry.Value.x_Velocity, y_Velocity = entry.Value.y_Velocity });

//            return true;
//        }

//        public static Dictionary<int, Point> ParseLinesToPoints(List<string> lines)
//        {
//            Dictionary<int, Point> points = new Dictionary<int, Point>();
//            var pointId = 1;

//            foreach (var line in lines)
//            {
//                var position = line.Split("velocity")[0];

//                var startIndex = position.IndexOf('<');
//                var endIndex = position.IndexOf(',');
//                var xString = position.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
//                var x = int.Parse(xString);

//                startIndex = position.IndexOf(',');
//                endIndex = position.IndexOf('>');
//                xString = position.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
//                var y = int.Parse(xString);

//                var velocity = line.Split("velocity")[1];

//                startIndex = velocity.IndexOf('<');
//                endIndex = velocity.IndexOf(',');
//                xString = velocity.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
//                var x_Velocity = int.Parse(xString);

//                startIndex = velocity.IndexOf(',');
//                endIndex = velocity.IndexOf('>');
//                xString = velocity.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
//                var y_Velocity = int.Parse(xString);

//                var point = new Point() { x = x, x_Velocity = x_Velocity, y = y, y_Velocity = y_Velocity };

//                points.Add(pointId, point);
//                pointId++;
//            }
//            return points;
//        }

//        public class Point
//        {
//            public int x;
//            public int y;
//            public int x_Velocity;
//            public int y_Velocity;

//            public Point UpdatePointBasedOnVelocity()
//            {
//                x = x + x_Velocity;
//                y = y + y_Velocity;
//                return this;
//            }
//        }


//    }

//}
