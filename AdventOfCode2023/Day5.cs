using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day5
    {
        public Day5()
        {
            Console.WriteLine("Day5");

            var lines = File.ReadLines(@"..\..\..\input\day5.txt");

            var seeds = lines.First().Split(':')[1].Split(' ').Where(c => c.Trim() != "").Select(c => long.Parse(c.Trim()));
            
            var maps = new Dictionary<string, List<SourceDestMap>>();
            var currentMap = "";

            foreach (var line in lines.Skip(2))
            {
                if (line.Trim() == string.Empty)
                {
                    continue;
                }
                else if(!int.TryParse(line.First().ToString(), out int _))
                {
                    var nameSplit = line.Split(':')[0].Split('-');
                    currentMap = $"{nameSplit[0]}-{nameSplit[2]}";
                    maps.Add(currentMap, new List<SourceDestMap>());
                }
                else
                {
                    var sourceDest = line.Split(' ').Where(c => c.Trim() != "").ToArray();
                    maps[currentMap].Add(new SourceDestMap()
                    {
                         DestRangeStart = long.Parse(sourceDest[0].Trim()),
                         SourceRangeStart = long.Parse(sourceDest[1].Trim()),
                         RangeLength = long.Parse(sourceDest[2].Trim())
                    });
                }
            }
            var order = new List<string>()
            {
                "seed-soil map",
                "soil-fertilizer map",
                "fertilizer-water map",
                "water-light map",
                "light-temperature map",
                "temperature-humidity map",
                "humidity-location map"
            };

            //Part 1

            var seedLocations = new List<long>();

            foreach (var seed in seeds)
            {
                var loc = seed;

                foreach (var o in order)
                {
                    var map = maps[o];
                    var match = map.Where(m => m.SourceRangeStart <= loc && (m.SourceRangeStart + m.RangeLength >= loc));
                    if (match.Any())
                    {
                        loc = match.First().DestRangeStart + (loc - match.First().SourceRangeStart);
                    }
                }

                seedLocations.Add(loc);
            }

            Console.WriteLine(seedLocations.Min());

            //Part 2
            var lowestSeedsLocation = -1l;

            var t = seeds.Zip(seeds.Skip(1)).Where((i, t) => t % 2 == 0);

            var counter = 72263000l; //Funnet ved prøving og feiling. Starte med stort counter steg (100000) og så gradvis minske

            order.Reverse();

            while (lowestSeedsLocation < 0)
            {
                var tmp = counter;
                foreach (var o in order)
                {
                    var map = maps[o];
                    var match = map.Where(m => m.IsInDestRange(tmp));
                    if (match.Any())
                    {
                        tmp = match.First().SourceRangeStart + (tmp - match.First().DestRangeStart);
                    }
                }

                if (t.Any(t => t.First <= tmp && tmp <= (t.First+t.Second)))
                {
                    lowestSeedsLocation = counter;
                }

                counter += 1;
            }

            Console.WriteLine(lowestSeedsLocation);
        }

        public class SourceDestMap
        {
            public long DestRangeStart;
            public long SourceRangeStart;
            public long RangeLength;

            public long SourceEnd => SourceRangeStart + RangeLength;
            public long DestEnd => DestRangeStart + RangeLength;

            public (long, long) Range => (SourceRangeStart, SourceEnd);

            public bool IsInRange(long input) => SourceRangeStart <= input && SourceEnd >= input;

            public bool IsInDestRange(long input) => DestRangeStart <= input && input <= DestEnd;

            public override string ToString()
            {
                return $"{DestRangeStart}-{SourceRangeStart}-{RangeLength}";
            }

        }
    }
}
