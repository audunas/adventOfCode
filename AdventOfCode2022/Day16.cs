using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace AdventOfCode2022
{
    public class Day16
    {

        public Day16()
        {
            var lines = File.ReadLines(@"..\..\..\input\day16.txt");

            var dict = new Dictionary<string, (long, List<string>)>();
            var valveState = new Dictionary<string, int>();

            foreach (var line in lines)
            {
                var a = line.Split(';');
                var firstPart = a[0].Split(" ");
                var valveName = firstPart[1];
                var flowRate = long.Parse(firstPart[4].Split("=")[1]);


                var tunnel = a[1].Trim();
                var split = tunnel.Split(" ");
                var valves = split.Skip(4).Select(r => r.Trim(',')).ToList();
                dict[valveName] = (flowRate, valves);
                valveState[valveName] = 0;
            }

            var closedValves = dict.Where(r => r.Value.Item1 != 0).Select(r => r.Key);

            var allPaths = new Dictionary<string, List<List<string>>>();
            var pathWithPressure = new Dictionary<List<string>, long>();

            foreach (var d in dict)
            {
                var paths = GetAllPaths(dict, d.Key, new List<List<string>>(), new List<string>() { d.Key });
                allPaths[d.Key] = paths;
                foreach (var p in paths)
                {
                    var currentPressure = 0L;
                    var totalPressure = 0L;
                    var nodesWithFlow = 0;
                    for (var i = 0; i< p.Count; i++)
                    {
                        var c = p[i];
                        currentPressure = dict[c].Item1;
                        if (currentPressure > 0)
                        {
                            nodesWithFlow++;
                        }
                        totalPressure += currentPressure;
                    }
                    totalPressure = totalPressure - p.Count - nodesWithFlow;
                    pathWithPressure[p] = totalPressure;
                }
            }

            var pairs = new HashSet<List<string>>();

            foreach (var t in dict)
            {
                foreach (var p in t.Value.Item2)
                {
                    var l = new List<string>() { t.Key, p };
                    l.Sort();
                    if (!pairs.Any(r => r.Intersect(l).Count() == l.Count()))
                    {
                        //if (!l.Any(k => dict[k].Item1 == 0))
                        //{
                            pairs.Add(l);
                        //}
                    }
                }
            }

            var pairOrder = new List<(string, string)>();

            foreach (var p in pairs)
            {
                var v1 = p.First();
                var v2 = p.Last();

                var v1FlowRate = dict[v1].Item1;
                var v2FlowRate = dict[v2].Item1;

                var openV1First = (28 * v1FlowRate) + (26 * v2FlowRate);
                var openV2First = (28 * v2FlowRate) + (26 * v1FlowRate);

                if (openV1First >= openV2First)
                {
                    pairOrder.Add((v1, v2));
                }
                else
                {
                    pairOrder.Add((v2, v1));
                }

            }


            var currentValve = "AA";
            var currentPath = new List<string>() { currentValve };

            var openValves = dict.Where(t => t.Value.Item1 == 0).Select(k => k.Key).ToHashSet();
            MaxPressure = dict.Sum(d => d.Value.Item1);

            // Part 1
            //GetAllPathsUpToLength(dict, currentValve,
            //    new List<PathElement>() { new PathElement { Element = currentValve, OpenValves = openValves } }, 1, 0, openValves);

            //Console.WriteLine(Part1Max.Item2);

            // part 2
            Part2GetAllPathsUpToLength(dict, currentValve, currentValve,
                new List<PathElement>() { new PathElement { Element = currentValve, OpenValves = openValves } },
                new List<PathElement>() { new PathElement { Element = currentValve, OpenValves = openValves } }, 1, 0, 
                openValves);

            Console.WriteLine(Part2Max.Item3);
        }


        public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                yield break;
            }

            var list = sequence.ToList();

            if (!list.Any())
            {
                yield return Enumerable.Empty<T>();
            }
            else
            {
                var startingElementIndex = 0;

                foreach (var startingElement in list)
                {
                    var index = startingElementIndex;
                    var remainingItems = list.Where((e, i) => i != index);

                    foreach (var permutationOfRemainder in Permute(remainingItems))
                    {
                        yield return permutationOfRemainder.Prepend(startingElement);
                    }

                    startingElementIndex++;
                }
            }
        }

        public static List<List<string>> GetAllPaths(Dictionary<string, (long, List<string>)> dict, string currentNode, 
            List<List<string>> allPaths, List<string> currentPath)
        {
            var neighbors = dict[currentNode].Item2;
            var neighborsNotAlreadyInPath = neighbors.Except(currentPath);
            if (!neighborsNotAlreadyInPath.Any())
            {
                allPaths.Add(currentPath);
                return allPaths;
            }

            foreach (var n in neighborsNotAlreadyInPath)
            {
                allPaths.Concat(GetAllPaths(dict, n, allPaths, currentPath.Append(n).ToList()));
            }

            return allPaths;
        }

        public static long MaxPressure;
        public static (List<PathElement>, long) Part1Max;

        public static (List<PathElement>, List<PathElement>, long) Part2Max = 
            new(new List<PathElement>(), new List<PathElement>(), 2114);

        public const int Part1MaxMinutes = 30;
        public const int Part2MaxMinutes = 26;

        public struct PathElement
        {
            public string Element;
            public HashSet<string> OpenValves;
            public long CurrentPressure;
            public int Minute;

            public override string ToString() => $"Minute {Minute} - {Element} - Pressure: {CurrentPressure}";
        }

        public static void GetAllPathsUpToLength(Dictionary<string, (long, List<string>)> dict, string currentNode,
           List<PathElement> currentPath, int currentLength, long currentWeight, HashSet<string> openValves, bool justAppended = false)
        {
            if (currentLength > Part1MaxMinutes)
            {
                if (currentWeight > Part1Max.Item2)
                {
                    Console.WriteLine(currentWeight);
                    Part1Max = (currentPath, currentWeight);
                }
                return;
            }
            
            var currentPressureRelease = openValves.Sum(p => dict[p].Item1);

            if (justAppended)
            {
                var newCurrentPath = currentPath.Append(new PathElement
                {
                    Element = currentNode,
                    OpenValves = openValves,
                    CurrentPressure = currentPressureRelease,
                    Minute = currentLength
                }).ToList();
                GetAllPathsUpToLength(dict, currentNode, newCurrentPath,
                        currentLength + 1, currentWeight + currentPressureRelease, new HashSet<string>(openValves.Append(currentNode)));
                return;
            }

            if ((currentWeight + (MaxPressure * (Part1MaxMinutes-currentLength+1))) < Part1Max.Item2)
            {
                return;
            }

            if (openValves.Count() == dict.Count())
            {
                var calcWeight = currentWeight + (currentPressureRelease * (Part1MaxMinutes-currentLength+1));
                if (calcWeight > Part1Max.Item2)
                {
                    Console.WriteLine(calcWeight);
                    Part1Max = (currentPath, calcWeight);
                }
                return;
            }

            if (currentPath.Take(currentPath.Count() - 1).GroupBy(r => new { r.Element, r.CurrentPressure }).Any(g => g.Count() > 2))            {
                return;
            }

            var pathLength = currentPath.Count();
            if (pathLength > 2)
            {
                for (int i = pathLength; i > 2; i--)
                {
                    // D -> x -> D
                    if (currentPath[i - 1].Element == currentPath[i - 3].Element)
                    {
                        return;
                    }
                }
               
            }

            var neighbors = dict[currentNode].Item2;
          
            var newWeight = currentWeight + currentPressureRelease;

            foreach (var n in neighbors)
            {
                var newCurrentPath = currentPath.Append(new PathElement { 
                    Element = n, 
                    OpenValves = openValves, 
                    CurrentPressure = currentPressureRelease,
                    Minute = currentLength
                }).ToList();

                if (!openValves.Contains(n))
                {
                    GetAllPathsUpToLength(dict, n, newCurrentPath,
                        currentLength + 1, newWeight, openValves, true);
                }

                GetAllPathsUpToLength(dict, n, newCurrentPath,
                    currentLength+1, newWeight, new HashSet<string>(openValves));
            }

            if (newWeight > Part1Max.Item2)
            {
                Part1Max = (currentPath, newWeight);
            }

            return;
        }

        public static void Part2GetAllPathsUpToLength(Dictionary<string, (long, List<string>)> dict, string currentNode, string currentElephant,
           List<PathElement> currentPath, List<PathElement> currentElephantPath, int currentLength, long currentWeight, HashSet<string> openValves,
           bool justAppended = false, bool justAppendedElephant = false)
        {
            if (currentLength > Part2MaxMinutes)
            {
                if (currentWeight > Part2Max.Item3)
                {
                    Console.WriteLine(currentWeight);
                    Part2Max = (currentPath, currentElephantPath, currentWeight);
                }
                return;
            }
            var currentPressureRelease = openValves.Sum(p => dict[p].Item1);

            if (openValves.Count() == dict.Count())
            {
                var calcWeight = currentWeight + (currentPressureRelease * (Part2MaxMinutes - currentLength + 1));
                if (calcWeight > Part2Max.Item3)
                {
                    Console.WriteLine(calcWeight);
                    Part2Max = (currentPath, currentElephantPath, calcWeight);
                }
                return;
            }


            if (justAppended)
            {
                openValves.Add(currentNode);
            }

            if (justAppendedElephant)
            {
                openValves.Add(currentElephant);
            }

            var unopened = dict.Select(f => f.Key).Except(openValves);
            var maxValvesToOpen = (int) Math.Ceiling((double)(Part2MaxMinutes - currentLength) / 2);
            var maxUnopened = unopened.OrderByDescending(uo => dict[uo].Item1).Take(maxValvesToOpen);
            var maxUnopenedPressure = maxUnopened.Sum(r => dict[r].Item1);

            if ((currentWeight + (maxUnopenedPressure * (Part2MaxMinutes - currentLength + 1))) < Part2Max.Item3)
            {
                return;
            }

            var pathLength = currentPath.Count();
            if (pathLength > 2)
            {
                for (int i = pathLength; i > 2; i--)
                {
                    // D -> x -> D
                    if (currentPath[i - 1].Element == currentPath[i - 3].Element)
                    {
                        return;
                    }
                }

            }

            pathLength = currentElephantPath.Count();
            if (pathLength > 2)
            {
                for (int i = pathLength; i > 2; i--)
                {
                    // D -> x -> D
                    if (currentElephantPath[i - 1].Element == currentElephantPath[i - 3].Element)
                    {
                        return;
                    }
                }

            }

            if (currentPath.Take(currentPath.Count() - 1).GroupBy(r => new { r.Element, r.CurrentPressure }).Any(g => g.Count() > 1 && (g.Max(t => t.Minute) - g.Min(r => r.Minute)) > 1) ||
              currentElephantPath.Take(currentElephantPath.Count() - 1).GroupBy(r => new { r.Element, r.CurrentPressure }).Any(g => g.Count() > 1 && (g.Max(t => t.Minute) - g.Min(r => r.Minute)) > 1))
            {
                return;
            }

            var neighbors = dict[currentNode].Item2.OrderByDescending(r => dict[r].Item1);
            var elephantNeighbors = dict[currentElephant].Item2.OrderByDescending(r => dict[r].Item1);

            var neighborCombos = from a in neighbors
                                 from b in elephantNeighbors
                                 select (a, b);

            var newWeight = currentWeight + currentPressureRelease;

            foreach (var nCombo in neighborCombos)
            {
                var newCElement = justAppended ? currentPath.Last().Element : nCombo.a;
                var newElephantElement = justAppendedElephant ? currentElephantPath.Last().Element : nCombo.b;

                var newCurrentPath = currentPath.Append(new PathElement
                {
                    Element = newCElement,
                    OpenValves = openValves,
                    CurrentPressure = currentPressureRelease,
                    Minute = currentLength
                }).ToList();

                var newCurrentElephantPath = currentElephantPath.Append(new PathElement
                {
                    Element = newElephantElement,
                    OpenValves = openValves,
                    CurrentPressure = currentPressureRelease,
                    Minute = currentLength
                }).ToList();

                if (nCombo.a == nCombo.b && openValves.Contains(nCombo.a))
                {
                    Part2GetAllPathsUpToLength(dict, newCElement, newElephantElement, newCurrentPath, newCurrentElephantPath,
                        currentLength + 1, newWeight, new HashSet<string>(openValves), false, false);
                    continue;
                }

                if (!openValves.Contains(newCElement))
                {
                    if (!openValves.Contains(newElephantElement))
                    {
                        Part2GetAllPathsUpToLength(dict, newCElement, newElephantElement, newCurrentPath, newCurrentElephantPath,
                        currentLength + 1, newWeight, new HashSet<string>(openValves), true, true);
                    }
                    Part2GetAllPathsUpToLength(dict, newCElement, newElephantElement, newCurrentPath, newCurrentElephantPath,
                        currentLength + 1, newWeight, new HashSet<string>(openValves), true, false);
                }
                else if (!openValves.Contains(newElephantElement))
                {
                    Part2GetAllPathsUpToLength(dict, newCElement, newElephantElement, newCurrentPath, newCurrentElephantPath,
                        currentLength + 1, newWeight, new HashSet<string>(openValves), false, true);
                }

                Part2GetAllPathsUpToLength(dict, newCElement, newElephantElement, newCurrentPath, newCurrentElephantPath,
                    currentLength + 1, newWeight, new HashSet<string>(openValves), false, false);
            }

            return;
        }
    }
}
