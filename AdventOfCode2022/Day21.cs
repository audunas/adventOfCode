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
    public class Day21
    {

        public Day21()
        {
            var lines = File.ReadLines(@"..\..\..\input\day21.txt").ToList();

            var allValues = new List<int>();
            var uniqueValues = new HashSet<int>();
            var initialPos = new List<(string, int)>();
            var valuePos = new Dictionary<int, string>();
            var m = new Dictionary<string, int>();

            for (var i = 0; i<lines.Count; i++)
            {
                var value = int.Parse(lines[i]);
                initialPos.Add(($"{i}/{value}", i));
                allValues.Add(value);
                uniqueValues.Add(value);
                valuePos.Add(i, $"{i}/{value}");
                m.Add($"{i}/{value}", i);
            }

            var length = lines.Count();

            foreach (var v in initialPos)
            {
                var newValuePos = valuePos.ToDictionary(t => t.Key, t => t.Value);
                var currentPos = newValuePos.Single(k => k.Value == v.Item1).Key;
                var currentValue = int.Parse(v.Item1.Split("/")[1]);

                
                if (currentValue == 0)
                {
                    continue;
                }

                var newPos = currentPos + currentValue;
                if (newPos > length)
                {
                    var cycle = (int)Math.Ceiling((decimal)newPos / length);
                    newPos = (newPos-((cycle-1) * length));
                }
                else if (newPos < 0)
                {
                    var cycle = (int)Math.Ceiling((decimal)Math.Abs(newPos) / length);
                    newPos = newPos + (cycle * length) - 1;
                }
                else if (newPos == 0)
                {
                    newPos = length - 1;
                }

                if (newPos == length)
                {
                    newPos = 0;
                }

                Console.WriteLine($"Pos: {v.Item1} - Current Pos: {currentPos} - NewPos: {newPos}");
                //Thread.Sleep(1000);

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

                if (newValuePos.Count != length || newValuePos.Max(r => r.Key) > length - 1 || newValuePos.Min(r => r.Key) < 0 )
                {
                    throw new Exception("Something is wrong");
                }

                var pos = newValuePos.Select(r => int.Parse(r.Value.Split("/")[1])).ToList();
                if (pos.Count() != allValues.Count() || pos.Intersect(allValues).Count() != uniqueValues.Count())
                {
                    throw new Exception("Something is wrong");
                }

                valuePos = newValuePos.ToDictionary(t => t.Key, t => t.Value);
            }

            var posOf0 = valuePos.Single(k => int.Parse(k.Value.Split("/")[1]) == 0).Key;

            var pos1000 = 1000 - ((int)Math.Floor((decimal)(1000) / length) * length);
            var h = pos1000 + posOf0;
            pos1000 = h > length ? h - length : h;
            var valueAtPos1000 = int.Parse(valuePos[pos1000].Split("/")[1]);

            var pos2000 = 2000 - ((int)Math.Floor((decimal)2000 / length) * length);
            h = pos2000 + posOf0;
            pos2000 = h > length ? h - length : h;
            var valueAtPos2000 = int.Parse(valuePos[pos2000].Split("/")[1]);

            var pos3000 = 3000 - ((int)Math.Floor((decimal)3000 / length) * length);
            h = pos3000 + posOf0;
            pos3000 = h > length ? h - length : h;
            var valueAtPos3000 = int.Parse(valuePos[pos3000].Split("/")[1]);

            var sum = valueAtPos1000 + valueAtPos2000 + valueAtPos3000;


            Console.WriteLine(sum);

        }
    }
}
