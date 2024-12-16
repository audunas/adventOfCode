using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day5
    {

        public Day5()
        {
            Console.WriteLine("Day5");

            var lines = File.ReadLines(@"..\..\..\input\day5.txt").ToArray();

            var rules = new List<List<int>>();

            var updates = new List<List<int>>();

            foreach (var line in lines)
            {
                if (line.Contains("|"))
                {
                    var sp = line.Split('|');
                    var l = new List<int>()
                    {
                        int.Parse(sp[0]),
                        int.Parse(sp[1])
                    };
                    rules.Add(l);
                }
                else if(line.Contains(","))
                {
                    updates.Add(line.Split(',').Select(int.Parse).ToList());
                }
            }

            var sum = 0;
            var sum2 = 0;

            foreach (var update in updates)
            {

                var relevantRules = rules.Where(r => r.Intersect(update).Count() == 2);

                var allMatch = relevantRules.Select(rule =>
                {
                    var index1 = update.Select((val, index) => val == rule.First() ? index : -1).Where(j => j != -1);
                    var index2 = update.Select((val, index) => val == rule.Last() ? index : -1).Where(j => j != -1);
                    if (index1.All(i => index2.All(i2 => i < i2)))
                    {
                        return true;
                    }
                    return false;
                });

                if (allMatch.All(a => a == true))
                {
                    var middleNumber = update[(int)Math.Floor((decimal)update.Count / 2)];

                    sum += middleNumber;
                }
                else
                {
                    var newList = new List<int>(update);
                    while (allMatch.Any(a => a == false))
                    {
                        foreach (var rule in relevantRules)
                        {
                            var index1 = newList.IndexOf(rule.First());
                            var index2 = newList.IndexOf(rule.Last());
                            if (index1 > index2)
                            {
                                newList[index1] = rule.Last();
                                newList[index2] = rule.First();
                            }

                        }
                        allMatch = relevantRules.Select(rule =>
                        {
                            var index1 = newList.Select((val, index) => val == rule.First() ? index : -1).Where(j => j != -1);
                            var index2 = newList.Select((val, index) => val == rule.Last() ? index : -1).Where(j => j != -1);
                            if (index1.All(i => index2.All(i2 => i < i2)))
                            {
                                return true;
                            }
                            return false;
                        });
                    }
                   

                    var middleNumber = newList[(int)Math.Floor((decimal)update.Count / 2)];

                    sum2 += middleNumber;
                }
               
            }

            Console.WriteLine(sum);

            Console.WriteLine(sum2);
        }
    }
}
