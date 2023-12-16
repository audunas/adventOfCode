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

            foreach (var line in lines)
            {
                var cond = line.Split(' ')[0].ToCharArray();
                var arrang = line.Split(" ")[1].Split(',').Select(int.Parse);

                var questionIndices = cond.Select((c, i) => c == '?' ? i : -1).Where(c => c > -1).ToList();

                var perms = Perms(questionIndices.Count());

                var numbersMatching = MatchCondition(perms, cond, questionIndices, arrang);
                sum += numbersMatching;
            }

            Console.WriteLine(sum);

            //Part 2
            sum = 0;

            foreach (var line in lines)
            {
                var original = line.Split(' ')[0].ToCharArray().ToList();
                var arrangOrg = line.Split(" ")[1].Split(',').Select(int.Parse).ToList();
                var cond = new List<char>(original);
                var arrang = new List<int>(arrangOrg);
                for (var i = 0; i<4; i++)
                {
                    cond.AddRange(new char[] { '?' }.Concat(original));
                    arrang.AddRange(arrangOrg);
                }
               
                var questionIndices = cond.Select((c, i) => c == '?' ? i : -1).Where(c => c > -1).ToList();

                var perms = Perms(questionIndices.Count());

                var numbersMatching = MatchCondition(perms, cond.ToArray(), questionIndices, arrang);
                sum += numbersMatching;
            }


            Console.WriteLine(sum);
        }

        private static List<char[]> Perms(int size)
        {
            var t = new List<string>();
            for (int i = 0; i <= ~(-1 << size); i++)
                t.Add(Convert.ToString(i, 2).PadLeft(size, '0'));

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
                var groups = new List<List<char>>();

                var g = new List<char>();
                foreach (var c in input)
                {
                    if (c == '#')
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

                if (groups.Select(c => c.Count()).SequenceEqual(arrang))
                    matching++;
            }
           
            return matching;
        }
    }
}
