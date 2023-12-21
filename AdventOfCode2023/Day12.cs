using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2023
{
    internal class Day12
    {
        public Day12() 
        {
            Console.WriteLine("Day12");

            var lines = File.ReadLines(@"..\..\..\input\day12.txt").ToArray();


            //Part 1
            var sum = 0;
            //var origMatch = new Dictionary<string, int>();
            var origMatch = new Dictionary<int, (string, int)>();
            var id = 0;

            foreach (var line in lines)
            {
                var cond = line.Split(' ')[0].ToCharArray();
                var orig = new string(cond);
                var arrang = line.Split(" ")[1].Split(',').Select(int.Parse);

                var questionIndices = cond.Select((c, i) => c == '?' ? i : -1).Where(c => c > -1).ToList();

                var hashesInOrig = orig.Count(o => o == '#');
                var expectedHashes = arrang.Sum(l => l);
                var missingHashes = expectedHashes - hashesInOrig;

                var perms = Perms(questionIndices.Count(), missingHashes);

                var numbersMatching = MatchCondition(perms, cond, questionIndices, arrang);
                sum += numbersMatching;
                origMatch.Add(id, (orig, numbersMatching));
                id++;
            }

            Console.WriteLine(sum);

            //Part 2
            long sum2 = 0;
            id = 0;
            foreach (var line in lines)
            {
                Console.WriteLine(id);
                var original = line.Split(' ')[0].ToCharArray().ToList();
                var orig = origMatch.ContainsKey(id) 
                    ? origMatch[id].Item2 
                    : 1;
                id++;
                var arrangOrg = line.Split(" ")[1].Split(',').Select(int.Parse).ToList();
                var cond = new List<char>(original);
                var arrang = new List<int>(arrangOrg);
                var hashesInOrig = original.Count(o => o == '#');
                var expectedHashes = arrangOrg.Sum(l => l);
                var missingHashes = expectedHashes - hashesInOrig;
                var dots = original.Count(o => o == '.');
                var reversed = new List<char>(original);
                reversed.Reverse();

                var temp = orig;

                var groups = GroupBetween(cond.ToArray(), '.');

                var groupsThatCanSplit = groups.Where(g => g.Count() >= 3).ToList();

                var hashesIndices = cond.Select((c, i) => c == '#' ? i : -1).Where(c => c > -1).ToList();
                var questionIndices = cond.Select((c, i) => c == '?' ? i : -1).Where(c => c > -1).ToList();
                var requiredSpaces = arrangOrg.Count() - 1;

                var missingDots = cond.Count() - expectedHashes;

                var spare = cond.Count() - missingDots - expectedHashes;
                //spare == 0 -> only one way to arrange

                //1 group - expected 3. Need to add 3 dots

                var g = groups.Count() - spare;

                var perms = Perms(questionIndices.Count(), missingHashes);

                var numbersMatching = MatchCondition(perms, cond.ToArray(), questionIndices, arrang);
                temp *= (int)(Math.Pow(numbersMatching,4));
                sum2 += temp;
                
            }


            Console.WriteLine(sum2);
        }

        private static List<char[]> Perms(int size, int numberOfHashes)
        {
            var t = new List<string>();
            long numbers = (long)Math.Pow(2, size);
            var zeroes = "000000000000000000000000000000000000000000000000000000000000000000000000";
            for (int i = 0; i <numbers; i++)
            {
                string binary = Convert.ToString(i, 2);
                if (binary.Count(t => t == '1') != numberOfHashes)
                {
                    continue;
                }
                string leading_zeroes = zeroes.Substring(0, size - binary.Length);
                var s = leading_zeroes + binary;
                t.Add(s);
            }

            return t.Select(m => m.ToString().Replace('0', '.').Replace('1', '#').ToCharArray()).ToList();
        }

        private static int MatchCondition(List<char[]> perms, char[] input, List<int> questionIndices, IEnumerable<int> arrang)
        {
           

            var matching = 0;

            foreach (var per in perms)
            {
                for (var indx = 0; indx < questionIndices.Count; indx++)
                {
                    input[questionIndices[indx]] = per[indx];
                }

                var groups = Group(input, '#');

                if (groups.Select(c => c.Count()).SequenceEqual(arrang))
                    matching++;
            }
           
            return matching;
        }

        private static List<List<char>> Group(char[] input, char groupBy)
        {
            var groups = new List<List<char>>();

            var g = new List<char>();
            foreach (var c in input)
            {
                if (c == groupBy)
                {
                    g.Add(c);
                }
                else
                {
                    if (g.Any())
                    {
                        groups.Add(g);
                        g = new List<char>();
                    }
                }
            }
            if (g.Any())
            {
                groups.Add(g);
            }
            return groups;
        }

        private static List<List<char>> GroupBetween(char[] input, char groupBy)
        {
            var groups = new List<List<char>>();

            var g = new List<char>();
            foreach (var c in input)
            {
                if (c == groupBy)
                {
                    if (g.Any())
                    {
                        groups.Add(g);
                        g = new List<char>();
                    }
                }
                else
                {
                    g.Add(c);
                }
            }
            if (g.Any())
            {
                groups.Add(g);
            }
            return groups;
        }
    }
}
