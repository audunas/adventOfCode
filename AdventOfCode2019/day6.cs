using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day6
    {

        public Day6()
        {
            var lines = File.ReadLines(@"..\..\..\input\day6.txt").ToList();

            var parsed = lines.Select(l => new Orbit(l.Split(")")[0], l.Split(")")[1])).ToList();

            //Part1
            var numberOfOrbits = FindNumberOfOrbits(parsed);

            Console.WriteLine(numberOfOrbits);

            //Part2
            var numberOfOrbitTransfers = FindNumberOfOrbitTranfers(parsed);

            Console.WriteLine(numberOfOrbitTransfers);
        }

        public static int FindNumberOfOrbitTranfers(List<Orbit> orbits)
        {
            var (fromTo, toFrom) = GetDicts(orbits);

            var whatYouAreObiting = toFrom.First(t => t.Key == "YOU").Value.First();
            var whatSantaIsOrbiting = toFrom.First(t => t.Key == "SAN").Value.First();

            var lengthOfShortesPath = getShortestPathBetween(whatYouAreObiting, whatSantaIsOrbiting, toFrom);

            return lengthOfShortesPath;
        }

        private static int getShortestPathBetween(string whatYouAreObiting, string whatSantaIsOrbiting, 
            Dictionary<string, List<string>> toFrom)
        {
            var allOrbitsForYou = getListOfOrbits(whatYouAreObiting, toFrom, new List<string>());
            var allOrbitsForSanta = getListOfOrbits(whatSantaIsOrbiting, toFrom, new List<string>());

            string firstCommonOrbit = null;

            foreach (var orbitForYou in allOrbitsForYou)
            {
                if (allOrbitsForSanta.Contains(orbitForYou))
                {
                    firstCommonOrbit = orbitForYou;
                    break;
                }
            }

            return allOrbitsForYou.FindIndex(o => o == firstCommonOrbit)
                + allOrbitsForSanta.FindIndex(o => o == firstCommonOrbit);
        }

        private static List<string> getListOfOrbits(string whatYouAreOrbiting, 
            Dictionary<string, List<string>> toFrom, List<string> orbits)
        {
            var from = toFrom.FirstOrDefault(t => t.Key == whatYouAreOrbiting);
            if (from.Value == null)
            {
                return orbits.Append(from.Key).ToList();
            }
            return getListOfOrbits(from.Value.First(), toFrom, orbits.Append(from.Key).ToList());
        }

        public static (Dictionary<string, List<string>>, Dictionary<string, List<string>>) GetDicts(List<Orbit> orbits)
        {
            var fromTo = new Dictionary<string, List<string>>();
            var toFrom = new Dictionary<string, List<string>>();
            foreach (var orbit in orbits)
            {
                var values = new List<string>();
                if (fromTo.TryGetValue(orbit.before, out values))
                {
                    values.Add(orbit.after);
                    fromTo[orbit.before] = values;
                }
                else
                {
                    fromTo.Add(orbit.before, new List<string>() { orbit.after });
                }
                values = new List<string>();
                if (toFrom.TryGetValue(orbit.after, out values))
                {
                    values.Add(orbit.before);
                    toFrom[orbit.after] = values;
                }
                else
                {
                    toFrom.Add(orbit.after, new List<string>() { orbit.before });
                }
            }

            return (fromTo, toFrom);
        }

        public static int FindNumberOfOrbits(List<Orbit> orbits)
        {
            var (fromTo, toFrom) = GetDicts(orbits);

            var allObjects = fromTo.Select(t => t.Key).Concat(toFrom.Select(t => t.Key));

            foreach (var ob in allObjects)
            {
                var relationships = TraverseDict(ob, fromTo);
                FoundRelationships[ob] = relationships;
            }

            return FoundRelationships.Sum(f => f.Value);
        }

        public static Dictionary<string, int> FoundRelationships = new Dictionary<string, int>();

        public static int TraverseDict(string ob, Dictionary<string, List<string>> fromTo)
        {
            if (FoundRelationships.ContainsKey(ob))
            {
                return FoundRelationships[ob];
            }

            var direct = fromTo.Where(f => f.Value.Contains(ob));
            if (!direct.Any())
            {
                FoundRelationships[ob] = 0; 
                return 0;
            }

            foreach (var rel in direct)
            {
                if (FoundRelationships.ContainsKey(rel.Key))
                {
                    return 1 + FoundRelationships[rel.Key];
                }
                else
                {
                    var res = 1 + TraverseDict(rel.Key, fromTo);
                    FoundRelationships[ob] = res;
                    return res;
                }
            }
            return 0;
        }

        public class Orbit
        {
            public string before;
            public string after;

            public Orbit(string before, string after)
            {
                this.before = before;
                this.after = after;
            }
        }
    }
}
