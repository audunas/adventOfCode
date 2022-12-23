using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2022
{
    public class Day20
    {

        public Day20()
        {
            var lines = File.ReadLines(@"..\..\..\input\day20.txt").ToList();

            var allValues = new List<long>();
            var uniqueValues = new HashSet<long>();
            var initialPos = new List<(string, long)>();
            var valuePos = new Dictionary<long, string>();
            var m = new Dictionary<string, long>();

            //Part 2
            var decryptionKey = 811589153;

            for (var i = 0; i < lines.Count; i++)
            {
                var value = long.Parse(lines[i]) * decryptionKey;
                initialPos.Add(($"{i}/{value}", i));
                allValues.Add(value);
                uniqueValues.Add(value);
                valuePos.Add(i, $"{i}/{value}");
                m.Add($"{i}/{value}", i);
            }

            var length = lines.Count();

            var numberOfPass = 10;

            while (numberOfPass > 0)
            {
                Console.WriteLine(numberOfPass);
                var next = valuePos.ToDictionary(t => t.Key, t => t.Value);

                valuePos = GetValuePos(allValues, uniqueValues, initialPos, valuePos, length);

                numberOfPass--;
            }


            var posOf0 = valuePos.Single(k => long.Parse(k.Value.Split("/")[1]) == 0).Key;

            var pos1000 = 1000 - ((long)Math.Floor((decimal)(1000) / length) * length);
            var h = pos1000 + posOf0;
            pos1000 = h > length ? h - length : h;
            var valueAtPos1000 = long.Parse(valuePos[pos1000].Split("/")[1]);

            var pos2000 = 2000 - ((long)Math.Floor((decimal)2000 / length) * length);
            h = pos2000 + posOf0;
            pos2000 = h > length ? h - length : h;
            var valueAtPos2000 = long.Parse(valuePos[pos2000].Split("/")[1]);

            var pos3000 = 3000 - ((long)Math.Floor((decimal)3000 / length) * length);
            h = pos3000 + posOf0;
            pos3000 = h > length ? h - length : h;
            var valueAtPos3000 = long.Parse(valuePos[pos3000].Split("/")[1]);

            var sum = valueAtPos1000 + valueAtPos2000 + valueAtPos3000;


            Console.WriteLine(sum);

        }

        private static Dictionary<long, string> GetValuePos(List<long> allValues, HashSet<long> uniqueValues, List<(string, long)> initialPos, Dictionary<long, string> valuePos, int length)
        {
            foreach (var v in initialPos)
            {
                var newValuePos = valuePos.ToDictionary(t => t.Key, t => t.Value);
                var currentPos = newValuePos.Single(k => k.Value == v.Item1).Key;
                var currentValue = long.Parse(v.Item1.Split("/")[1]);


                if (currentValue == 0)
                {
                    continue;
                }

                var newPos = currentPos + currentValue;

                if (newPos == 0)
                {
                    newPos = length - 1;
                }
                else if ((newPos) % (length-1) == 0 && currentValue > 0)
                {
                    newPos = length - 1;
                }
                else if (newPos >= length)
                {
                    newPos = newPos % (length - 1);
                    if (newPos == 0)
                    {
                        newPos = length - 1;
                    }
                }
                else if (newPos < 0)
                {
                    var k = newPos % (length-1);
                    newPos = length - 1 - Math.Abs(k);
                }

                newValuePos[newPos] = v.Item1;

                if (currentPos > newPos)
                {
                    for (var i = newPos; i < currentPos; i++)
                    {
                        newValuePos[i + 1] = valuePos[i];
                    }
                }
                else if (currentPos < newPos)
                {
                    for (var i = currentPos; i < newPos; i++)
                    {
                        newValuePos[i] = valuePos[i + 1];
                    }
                }

                if (newValuePos.Count != length || newValuePos.Max(r => r.Key) > length - 1 || newValuePos.Min(r => r.Key) < 0)
                {
                    throw new Exception("Something is wrong");
                }

                var pos = newValuePos.Select(r => long.Parse(r.Value.Split("/")[1])).ToList();
                if (pos.Count() != allValues.Count() || pos.Intersect(allValues).Count() != uniqueValues.Count())
                {
                    throw new Exception("Something is wrong");
                }

                valuePos = newValuePos.ToDictionary(t => t.Key, t => t.Value);
            }

            return valuePos;
        }
    }
}
