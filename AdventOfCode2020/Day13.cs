using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day13
    {
        public Day13()
        {
            var lines = File.ReadLines(@"..\..\input\day13.txt").ToList();
            var earliestTimestamp = int.Parse(lines.First());
            var busIds = lines.Last().Split(',').Where(s => s != "x").Select(r => int.Parse(r));

            // Part 1
            long foundTimestamp = 0;
            long currentTimestamp = earliestTimestamp;
            var busIdOfMatchingBus = 0;
            while (foundTimestamp == 0)
            {
                var matchingBuses = busIds.Where(bus => currentTimestamp % bus == 0);
                if (matchingBuses.Any())
                {
                    foundTimestamp = currentTimestamp;
                    busIdOfMatchingBus = matchingBuses.First();
                }

                currentTimestamp++;
            }

            var waitingTime = foundTimestamp - earliestTimestamp;

            Console.WriteLine(waitingTime * busIdOfMatchingBus);

            // Part 2
            var buses = lines.Last().Split(',').Select((r, index) => new { id = r, position = index}).Where(r => r.id != "x").Select(r =>  new BusId { id = long.Parse(r.id), position = r.position}).ToList();
            var firstBus = buses.First().id;
            var maxBus = buses.OrderByDescending(b => b.id).First();
            var firstPossible = maxBus.id - maxBus.position;
            var product = buses.Aggregate(1l, (acc, bus) => acc*(bus.id+bus.position));
            var matching = new List<long>();

            for (long i = 0; i < 10000000000; i++)
            {
                var m = firstPossible + (maxBus.id * i);
                if ((m % firstBus == 0))
                {
                    matching.Add(m);
                }
            }
            currentTimestamp = 0;
            foundTimestamp = 0;

            foreach (var match in matching)
            {
                var allBusesMatch = buses.All(bus => ((match + bus.position) % bus.id) == 0);
                if (allBusesMatch)
                {
                    foundTimestamp = match;
                    break;
                }

            }

            //while (foundTimestamp == 0)
            //{
            //    var allBusesMatch = buses.All(bus => ((currentTimestamp+bus.position) % bus.id) == 0);
            //    if (allBusesMatch)
            //    {
            //        foundTimestamp = currentTimestamp;
            //    }

            //    currentTimestamp += firstBus;
            //}


            Console.WriteLine(foundTimestamp);
        }

        internal struct BusId
        {
            public long id;
            public int position;
        }
    }
}
