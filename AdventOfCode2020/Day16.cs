using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    class Day16
    {
        public Day16()
        {
            var lines = File.ReadLines(@"..\..\input\day16.txt").ToList();
            var rules = lines.TakeWhile(r => r != string.Empty).Select(l => CreateRule(l));
            var myTicket = lines.Skip(rules.Count()+2).Take(1).First().Split(',').Select(r => int.Parse(r));
            var nearbyTickets = lines.Skip(rules.Count()+5).Select(l => l.Split(',').Select(m => int.Parse(m)));

            var intervals = rules.SelectMany(r => r.ValidIntervals);

            var errorRate = 0;
            var validTickets = new List<List<int>>();
            // Part 1
            foreach (var ticket in nearbyTickets)
            {
                var invalidNumbers = ticket.Where(value => !intervals.Any(interval => interval.Start <= value && value <= interval.End));
                if (invalidNumbers.Any())
                {
                    errorRate += invalidNumbers.Sum();
                }
                else
                {
                    validTickets.Add(ticket.ToList());
                }
                
            }

            Console.WriteLine(errorRate);

            var determinedFields = new Dictionary<int, Rule>();
            var rulesToConsider = new List<Rule>(rules);

            // Part 2

            while (rulesToConsider.Any())
            {
                for (var i = 0; i < myTicket.Count(); i++)
                {
                    var ticketNumbers = validTickets.Select(t => t.ElementAt(i));
                    var matchingRule = rulesToConsider.Where(r => 
                        ticketNumbers.All(value => r.ValidIntervals.Any(interval => interval.Start <= value && value <= interval.End)));
                    if (matchingRule.Count() == 1)
                    {
                        determinedFields[i] = matchingRule.First();
                        rulesToConsider = rulesToConsider.Except(matchingRule).ToList();
                    }
                }
            }
            

            var departureFields = determinedFields.Where(d => d.Value.Name.StartsWith("departure")).Select(f => f.Key);
            var myTicketNumbers = myTicket.Where((i, index) => departureFields.Contains(index));
            var multiPlied = myTicketNumbers.Aggregate(1l, (a, b) => a*b);

            Console.WriteLine(multiPlied);
        }

        private Rule CreateRule(string l)
        {
            var split = l.Split(':');
            var name = split[0].Trim();
            var intervals = split[1].Split(new string[] { "or" }, StringSplitOptions.None);
            var ints = new List<Interval>();

            foreach (var i in intervals)
            {
                var r = i.Trim().Split('-');
                var createdInterval = new Interval { Start = int.Parse(r[0]), End = int.Parse(r[1]) };
                ints.Add(createdInterval);
            }

            return new Rule { Name = name, ValidIntervals = ints };
        }
    }

    internal struct Rule
    {
        public string Name;
        public List<Interval> ValidIntervals;
    }

    internal struct Interval
    {
        public int Start;
        public int End;
    }
}
