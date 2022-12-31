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

            var remainingMonkeys = lines.Select(l => new Monkey { Name = l.Split(":")[0].Trim(), 
                Operation = l.Split(":")[1].Trim() }); ;

            var dictionary = new Dictionary<string, long>();

            while (!dictionary.ContainsKey("root"))
            {
                var nextMonkeys = new List<Monkey>();
                foreach (var m in remainingMonkeys)
                {
                    long value;
                    if (long.TryParse(m.Operation, out value))
                    {
                        dictionary.Add(m.Name, value);
                    }
                    else
                    {
                        var ops = m.Operation.Split(" ");
                        var name1 = ops[0].Trim();
                        var name2 = ops[2].Trim();
                        if (dictionary.ContainsKey(name1) && dictionary.ContainsKey(name2))
                        {
                            var nextValue = 0L;
                            var operation = ops[1].Trim();
                            switch(operation)
                            {
                                case "+":
                                    nextValue = dictionary[name1] + dictionary[name2];
                                    break;
                                case "-":
                                    nextValue = dictionary[name1] - dictionary[name2];
                                    break;
                                case "*":
                                    nextValue = dictionary[name1] * dictionary[name2];
                                    break;
                                case "/":
                                    nextValue = dictionary[name1] / dictionary[name2];
                                    break;
                                default:
                                    break;
                            }

                            dictionary[m.Name] = nextValue;
                        }
                        else
                        {
                            nextMonkeys.Add(m);
                        }
                    }

                }
                remainingMonkeys = nextMonkeys;
            }

            Console.WriteLine(dictionary["root"]);

            //Part 2

            remainingMonkeys = lines.Select(l => new Monkey
            {
                Name = l.Split(":")[0].Trim(),
                Operation = l.Split(":")[1].Trim()
            }).ToList();

            var originalMonkeys = new List<Monkey>(remainingMonkeys);

            var root = remainingMonkeys.Single(t => t.Name == "root");
            var rootOps = root.Operation.Split(" ");
            var rootName1 = rootOps[0].Trim();
            var rootName2 = rootOps[2].Trim();

            var testValue = 3353687980000L; //Funnet ved manuell prøving og feiling

            while (true)
            {
                Console.WriteLine(testValue);
                var humn = originalMonkeys.Single(r => r.Name == "humn");
                var newHumn = new Monkey { Name= humn.Name, Operation = testValue.ToString()};
                remainingMonkeys = originalMonkeys.Where(t => t.Name != "humn");
                remainingMonkeys = remainingMonkeys.Append(newHumn);

                dictionary = new Dictionary<string, long>();

                while (!dictionary.ContainsKey("root"))
                {
                    var nextMonkeys = new List<Monkey>();
                    foreach (var m in remainingMonkeys)
                    {
                        long value;
                        if (long.TryParse(m.Operation, out value))
                        {
                            dictionary.Add(m.Name, value);
                        }
                        else
                        {
                            var ops = m.Operation.Split(" ");
                            var name1 = ops[0].Trim();
                            var name2 = ops[2].Trim();
                            if (dictionary.ContainsKey(name1) && dictionary.ContainsKey(name2))
                            {
                                var nextValue = 0L;
                                var operation = ops[1].Trim();
                                switch (operation)
                                {
                                    case "+":
                                        nextValue = dictionary[name1] + dictionary[name2];
                                        break;
                                    case "-":
                                        nextValue = dictionary[name1] - dictionary[name2];
                                        break;
                                    case "*":
                                        nextValue = dictionary[name1] * dictionary[name2];
                                        break;
                                    case "/":
                                        nextValue = dictionary[name1] / dictionary[name2];
                                        break;
                                    default:
                                        break;
                                }

                                dictionary[m.Name] = nextValue;
                            }
                            else
                            {
                                nextMonkeys.Add(m);
                            }
                        }

                    }
                    remainingMonkeys = nextMonkeys;
                }

                var n1 = dictionary[rootName1];
                var n2 = dictionary[rootName2];

                if (n1 == n2)
                {
                    break;
                }
                testValue++;
            }

            

            Console.WriteLine(testValue);
        }

        public struct Monkey
        {
            public string Name;
            public string Operation;
        }
    }
}
