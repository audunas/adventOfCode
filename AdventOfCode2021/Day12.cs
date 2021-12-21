using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day12
    {
        public Day12()
        {
            var lines = File.ReadLines(@"..\..\..\input\day12.txt");

            var cavePaths = lines.SelectMany(l => {
                var from = l.Split('-')[0];
                var to = l.Split('-')[1];
                if (from == "start" || to == "end")
                {
                    return new[] { new CavePath { From = from, To = to } };
                }
                else if (to == "start" || from == "end")
                {
                    return new[] { new CavePath { From = to, To = from } };
                }
                return new[] {
                    new CavePath { From = from, To = to },
                    new CavePath { From = to, To = from}
                };
                }).ToList();

            var startPoints = cavePaths.Where(c => c.IsStart).ToList();

            // Part 1
            var paths = startPoints.SelectMany(s => CreatePaths(cavePaths, s, new List<string>() { s.From },
                new List<string>(), new HashSet<List<string>>())).ToList();


            Console.WriteLine(paths.Count());

            // Part 2

            paths = startPoints.SelectMany(s => CreatePathsSmallCavesTwice(cavePaths, s, new List<string>() { s.From },
               new HashSet<string>(), new Dictionary<string, int>(), new HashSet<List<string>>())).ToList();


            Console.WriteLine(paths.Count());

        }

        public static HashSet<List<string>> CreatePaths(List<CavePath> allCaves, CavePath currentCave, 
            List<string> currentPath, List<string> visitedSmallCaves, HashSet<List<string>> allPaths)
        {
            currentPath.Add(currentCave.To);
            if (currentCave.IsEnd)
            {
                //Console.WriteLine(string.Join(',', currentPath));
                allPaths.Add(currentPath);
                return allPaths;
            }

            if (!currentCave.ToIsUpper)
            {
                visitedSmallCaves.Add(currentCave.To);
            }

            var allNextSteps = allCaves.Where(c => c.From == currentCave.To).ToList();

            foreach (var nextCave in allNextSteps)
            {
                if (!visitedSmallCaves.Contains(nextCave.To))
                {
                    var newCurrent = currentPath.Select(r => r).ToList();
                    var newVisitedSmallCaves = visitedSmallCaves.Select(v => v).ToList();
                    var paths = CreatePaths(allCaves, nextCave, newCurrent, newVisitedSmallCaves, allPaths);
                    allPaths.UnionWith(paths);
                }
            }

            return allPaths;
        }

        public static HashSet<List<string>> CreatePathsSmallCavesTwice(List<CavePath> allCaves, CavePath currentCave,
            List<string> currentPath, HashSet<string> visitedSmallCaves, Dictionary<string, int> visitedCounter,  HashSet<List<string>> allPaths)
        {
            currentPath.Add(currentCave.To);
            var currentPaths = new HashSet<List<string>>();
            if (currentCave.IsEnd)
            {
                //Console.WriteLine(string.Join(',', currentPath));
                allPaths.Add(currentPath);
                return allPaths;
            }

            if (!currentCave.ToIsUpper)
            {
                if (visitedCounter.ContainsKey(currentCave.To))
                {
                    visitedCounter[currentCave.To] += 1;
                }
                else
                {
                    visitedCounter.Add(currentCave.To, 1);
                }
                var hasAnySmallTwice = visitedCounter.Any(p => p.Value > 1);
                if (hasAnySmallTwice)
                {
                    visitedSmallCaves.UnionWith(visitedCounter.Select(r => r.Key));
                }
            }

            var allNextSteps = allCaves.Where(c => c.From == currentCave.To).ToList();

            foreach (var nextCave in allNextSteps)
            {
                if (!visitedSmallCaves.Contains(nextCave.To))
                {
                    var newCurrent = currentPath.Select(r => r).ToList();
                    var newVisitedSmallCaves = visitedSmallCaves.Select(v => v).ToHashSet();
                    var newHasVisistedSmallCaves = visitedCounter.Select(v => v).ToDictionary(x => x.Key, y => y.Value) ;
                    var paths = CreatePathsSmallCavesTwice(allCaves, nextCave, newCurrent, newVisitedSmallCaves, newHasVisistedSmallCaves, currentPaths);
                    currentPaths.UnionWith(paths);
                }
            }

            allPaths.UnionWith(currentPaths);

            return allPaths;
        }
    }

    public struct CavePath
    {
        public string From;
        public bool FromIsUpper => From == From.ToUpper();
        public string To;
        public bool ToIsUpper => To == To.ToUpper();
        public bool IsStart => From == "start";
        public bool IsEnd => To == "end";
    }
}
