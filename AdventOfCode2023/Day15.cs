using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day15
    {
        public Day15() 
        {
            Console.WriteLine("Day15");

            var lines = File.ReadLines(@"..\..\..\input\day15.txt").ToArray();

            var h = lines.First().Split(',');

            var sum = 0;

            foreach (var j in h)
            {
                var s = 0;
                foreach (var c in j)
                {
                    var asci = (int)c;
                    s += asci;
                    s *= 17;
                    s = s % 256;
                }
                sum += s;
            }

            Console.WriteLine(sum);

            //Part 2

            var dict = new Dictionary<int, List<(string, int)>>();


            foreach (var j in h)
            {
                var label = j.Contains("=") ? j.Split('=')[0].Trim() : j.Split("-")[0].Trim();

                var box = 0;
                foreach (var c in label)
                {
                    var asci = (int)c;
                    box += asci;
                    box *= 17;
                    box = box % 256;
                }

                if (j.Contains("="))
                {
                    var s = j.Split('=');
                    var focalLength = int.Parse(s[1].Trim());
                    var item = (label, focalLength);
                    if (dict.ContainsKey(box))
                    {
                        if (dict[box].Any(i => i.Item1 == label))
                        {
                            var it = dict[box].First(t => t.Item1 == label);
                            var index = dict[box].IndexOf(it);
                            dict[box][index] = item;
                        }
                        else
                            dict[box].Add(item);
                    }
                    else
                    {
                        dict.Add(box, new List<(string, int)>() { item });
                    }
                }
                else if (j.Contains("-"))
                {
                    var spl = j.Split('-');
                    if (dict.ContainsKey(box))
                    {
                        if (dict[box].Any(t => t.Item1 == label))
                        {
                            dict[box] = dict[box].Where(c => c.Item1 != label).ToList();
                        }
                    }
                }

            }

            var sumPerLabel = new Dictionary<string, int>();

            foreach (var b in dict)
            {
                for (var j = 0; j < b.Value.Count; j++)
                {
                    var label = b.Value[j].Item1;
                    var focalLength = b.Value[j].Item2;
                    var power = ((b.Key + 1) * (j + 1) * focalLength);
                    if (sumPerLabel.ContainsKey(label))
                    {
                        sumPerLabel[label] += power;
                    }
                    else
                    {
                        sumPerLabel.Add(label, power);
                    }
                }
            }

            sum = sumPerLabel.Sum(t => t.Value);

            Console.WriteLine(sum);
        }
    }
}
