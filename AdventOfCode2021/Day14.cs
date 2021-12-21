using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace AdventOfCode2021
{
    class Day14
    {
        public Day14()
        {
            var lines = File.ReadLines(@"..\..\..\input\day14.txt");

            var template = lines.First().ToCharArray();

            var insertionRules = lines.Skip(2).Select(l => new InsertionRules { From = l.Split("->")[0].Trim(), To = l.Split("->")[1].Trim() }).ToList();

            var dict = new Dictionary<string, List<char>>();
            var foundCombos = new HashSet<string>();

            var pairs = template.Zip(template.Skip(1)).Select(b => Tuple.Create(b.First, b.Second))
                .GroupBy(p => p)
                .Select(p => new { p = p, count = p.LongCount()})
                .ToDictionary(x => x.p.Key, y => y.count);

            var counter = new Dictionary<char, long>();

            foreach (var c in template)
            {
                if (counter.ContainsKey(c))
                {
                    counter[c] += 1;
                }
                else
                {
                    counter.Add(c, 1);
                }
            }

            for (var i = 0; i<40; i++)
            {
                var newPairs = new Dictionary<Tuple<char, char>, long>();
                foreach (var pair in pairs)
                {
                    var nextChar = insertionRules.First(r => r.From == new string(new[] { pair.Key.Item1, pair.Key.Item2 })).To.ToCharArray()[0];
                    var newPair1 = Tuple.Create(pair.Key.Item1, nextChar);
                    var newPair2 = Tuple.Create(nextChar, pair.Key.Item2);
                    if (newPairs.ContainsKey(newPair1))
                    {
                        newPairs[newPair1] += pair.Value;
                    }
                    else
                    {
                        newPairs.Add(newPair1, pair.Value);
                    }
                    if (newPairs.ContainsKey(newPair2))
                    {
                        newPairs[newPair2] += pair.Value;
                    }
                    else
                    {
                        newPairs.Add(newPair2, pair.Value);
                    }

                    if (counter.ContainsKey(nextChar))
                    {
                        counter[nextChar] += pair.Value;
                    }
                    else
                    {
                        counter.Add(nextChar, pair.Value);
                    }
                }

                pairs = newPairs;
            }

            var max = counter.Max(g => g.Value);
            var min = counter.Min(g => g.Value);

            Console.WriteLine(max-min);

        }

        [DebuggerDisplay("From = {From}, To = {To}")]
        public struct InsertionRules
        {
            public string From;
            public string To;
        }
    }
}
