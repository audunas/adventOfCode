using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day8
    {

        public Day8() 
        {
            Console.WriteLine("Day8");

            var lines = File.ReadLines(@"..\..\..\input\day8.txt");

            var instruction = lines.First();

            var maps = lines.Skip(2);

            var node = new Dictionary<string, (string, string)>();

            foreach(var i in maps)
            {
                var s = i.Split('=');

                var nodeName = s[0].Trim();

                var left = s[1].Trim().Split(',')[0].Trim('(');
                var right = s[1].Trim().Split(',')[1].Trim(')').Trim();

                node.Add(nodeName, (left, right));
            }

            //Part 1

            //var next = "AAA";
            var steps = 0;
            var insCounter = 0;

            //while (next != "ZZZ")
            //{
            //    var inst = instruction[insCounter].ToString();

            //    var no = node[next];

            //    next = inst == "L" ? no.Item1 : no.Item2;

            //    steps++;

            //    insCounter++;
            //    if (insCounter >= instruction.Length)
            //    {
            //        insCounter = 0;
            //    }
            //}

            //Console.WriteLine(steps);

            //Part 2

            var nextNodes = node.Keys.Where(k => k.EndsWith("A"));
            var zSets = new Dictionary<string, List<int>>();
            steps = 0;
            insCounter = 0;

            while (!nextNodes.All(n => n.EndsWith("Z")) && steps < 100000)
            {
                var inst = instruction[insCounter].ToString();

                var nodes = new List<string>();

                foreach (var n in nextNodes)
                {
                    var no = node[n];
                    var nextNode = inst == "L" ? no.Item1 : no.Item2;
                    nodes.Add(nextNode);

                    if (nextNode.EndsWith("Z"))
                    {
                        if (!zSets.ContainsKey(nextNode))
                        {
                            zSets.Add(nextNode, new List<int>() { steps});
                        }
                        else
                        {
                            zSets[nextNode].Add(steps);
                        }
                    }
                }

                nextNodes = nodes;

                steps++;

                insCounter++;
                if (insCounter >= instruction.Length)
                {
                    insCounter = 0;
                }
            }

            var zSetStep = new Dictionary<string, long>();

            foreach (var zSet in zSets)
            {
                var z = zSet.Value.Zip(zSet.Value.Skip(1)).Select(r => r.Second - r.First);

                if (z.Distinct().Count() == 1)
                {
                    zSetStep.Add(zSet.Key, z.First());
                }
            }

            var commonStep = LCM(zSetStep.Values.ToArray());

            Console.WriteLine(commonStep);
        }

        static long LCM(long[] numbers)
        {
            return numbers.Aggregate(lcm);
        }
        static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}
