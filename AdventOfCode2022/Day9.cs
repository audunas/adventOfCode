using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day9
    {

        public Day9()
        {
            var lines = File.ReadLines(@"..\..\..\input\day9.txt");

            var headX = 0;
            var headY = 0;

            var tails = new Dictionary<int, Tuple<int, int>>() { 
                { 0, Tuple.Create(0, 0) } ,
                { 1, Tuple.Create(0, 0) },
                { 2, Tuple.Create(0, 0) },
                { 3, Tuple.Create(0, 0) },
                { 4, Tuple.Create(0, 0) },
                { 5, Tuple.Create(0, 0) },
                { 6, Tuple.Create(0, 0) },
                { 7, Tuple.Create(0, 0) },
                { 8, Tuple.Create(0, 0) },
            };

            var visitedTailPos = new Dictionary<int, List<Tuple<int, int>>>() { 
                { 0, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 1, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 2, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 3, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 4, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 5, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 6, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 7, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
                { 8, new List<Tuple<int, int>>() { new Tuple<int, int>(0,0) } },
            };

            foreach (var line in lines)
            {
                var split = line.Split(" ");
                var cmd = split[0];
                var steps = int.Parse(split[1]);

                for (int i = 1; i <= steps; i++)
                {
                    var newTails = tails.ToDictionary(t => t.Key, t => t.Value);
                    foreach (var tail in tails)
                    {
                        var tailX = tail.Value.Item1;
                        var tailY = tail.Value.Item2;
                        var newHeadX = tail.Key == 0 ? headX : newTails[tail.Key - 1].Item1;
                        var newHeadY = tail.Key == 0 ? headY : newTails[tail.Key - 1].Item2;
                        switch (cmd)
                        {
                            case "R":
                                if (tail.Key == 0)
                                {
                                    newHeadX++;
                                }
                                var dist = CalculateManhattanDistance(newHeadX, tailX, newHeadY, tailY);
                                if (dist > 1)
                                {
                                    if (newHeadY == tailY)
                                    {
                                        tailX++;
                                        UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                    }
                                    else if (newHeadY > tailY)
                                    {
                                        tailY++;
                                        if (newHeadX == tailX)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX > tailX)
                                        {
                                            tailX++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX < tailX)
                                        {
                                            tailX--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                    else if (newHeadY < tailY)
                                    {
                                        tailY--;
                                        if (newHeadX == tailX)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX > tailX)
                                        {
                                            tailX++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX < tailX)
                                        {
                                            tailX--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                }
                                break;
                            case "U":
                                if (tail.Key == 0)
                                {
                                    newHeadY--;
                                }
                                dist = CalculateManhattanDistance(newHeadX, tailX, newHeadY, tailY);
                                if (dist > 1)
                                {
                                    if (newHeadX == tailX)
                                    {
                                        tailY--;
                                        UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                    }
                                    else if (newHeadX > tailX)
                                    {
                                        tailX++;
                                        if (newHeadY == tailY)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY > tailY)
                                        {
                                            tailY++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY < tailY)
                                        {
                                            tailY--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                    else if (newHeadX < tailX)
                                    {
                                        tailX--;
                                        if (newHeadY == tailY)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY > tailY)
                                        {
                                            tailY++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY < tailY)
                                        {
                                            tailY--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                }
                                break;
                            case "L":
                                if (tail.Key == 0)
                                {
                                    newHeadX--;
                                }
                                dist = CalculateManhattanDistance(newHeadX, tailX, newHeadY, tailY);
                                if (dist > 1)
                                {
                                    if (newHeadY == tailY)
                                    {
                                        tailX--;
                                        UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                    }
                                    else if (newHeadY > tailY)
                                    {
                                        tailY++;
                                        if (newHeadX == tailX)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX > tailX)
                                        {
                                            tailX++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX < tailX)
                                        {
                                            tailX--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                    else if (newHeadY < tailY)
                                    {
                                        tailY--;
                                        if (newHeadX == tailX)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX > tailX)
                                        {
                                            tailX++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadX < tailX)
                                        {
                                            tailX--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                }
                                break;
                            case "D":
                                if (tail.Key == 0)
                                {
                                    newHeadY++;
                                }
                                dist = CalculateManhattanDistance(newHeadX, tailX, newHeadY, tailY);
                                if (dist > 1)
                                {
                                    if (newHeadX == tailX)
                                    {
                                        tailY++;
                                        UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                    }
                                    else if (newHeadX > tailX)
                                    {
                                        tailX++;
                                        if (newHeadY == tailY)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY > tailY)
                                        {
                                            tailY++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY < tailY)
                                        {
                                            tailY--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                    else if (newHeadX < tailX)
                                    {
                                        tailX--;
                                        if (newHeadY == tailY)
                                        {
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY > tailY)
                                        {
                                            tailY++;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                        else if (newHeadY < tailY)
                                        {
                                            tailY--;
                                            UpdateVisited(visitedTailPos, tail, tailX, tailY);
                                        }
                                    }
                                }
                                break;
                        }

                        if (tail.Key == 0)
                        {
                            headX = newHeadX;
                            headY = newHeadY;
                        }
                        newTails[tail.Key] = new Tuple<int, int>(tailX, tailY);

                    }
                    tails = newTails.ToDictionary(t => t.Key, t => t.Value);
                    
                }
            }

            var distinctVisited = visitedTailPos.ToDictionary(t => t.Key, t => t.Value.Distinct().ToList());
            
            // Part 1
            Console.WriteLine(distinctVisited[0].Count());

            // Part 2
            Console.WriteLine(distinctVisited[8].Count());
        }

        private static void UpdateVisited(Dictionary<int, List<Tuple<int, int>>> visitedTailPos, KeyValuePair<int, Tuple<int, int>> tail, int tailX, int tailY)
        {
            visitedTailPos[tail.Key].Add(new Tuple<int, int>(tailX, tailY));
        }

        public static void Print(Dictionary<int, Tuple<int, int>> currentTails, int currentHX, int currentHY)
        {
            var grid = new List<List<string>>();

            foreach(var i in Enumerable.Range(1, 29)){
                grid.Add(new List<string>(Enumerable.Range(1, 29).Select(x => ".")));
            }

            var midX = grid[0].Count / 2;
            var midY = grid.Count / 2;

            grid[midX][midY] = "s";

            foreach (var tail in currentTails.Reverse())
            {
                grid[midY - (tail.Value.Item2*-1)][midX + tail.Value.Item1] = (tail.Key + 1).ToString();
            }

            grid[midY - (currentHY * -1)][midX + currentHX] = "H";

            foreach (var row in grid){
                Console.WriteLine(string.Join("", row));
            }
        }


        public static int CalculateManhattanDistance(int x1, int x2, int y1, int y2)
        {
            if(Math.Abs(x1 - x2) == 1 && Math.Abs(y1 - y2) == 1)
            {
                return 1;
            }
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}