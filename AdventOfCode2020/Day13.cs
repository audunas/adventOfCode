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
            var product = buses.Aggregate(1l, (acc, bus) => acc*bus.id);

            var sum = 0l;

            // Chinese remainder theorem: https://www.dave4math.com/mathematics/chinese-remainder-theorem/
            foreach (var bus in buses)
            {
                if (bus.position > 0)
                {
                    var n = product / bus.id;
                    var x = getX(n, bus.id, 1);
                    var s = (bus.id - bus.position) * n * x;
                    sum += s;
                }
                
            }

            var mod = sum % product;

            Console.WriteLine(mod);
        }

        public long getX(long first, long modNumber, long remainder)
        {
            var found = 0;
            var counter = 1;
            while (found == 0)
            {
                var t = first * counter % modNumber;
                if (t == remainder)
                {
                    found = counter;
                }

                counter++;
            }
            return found;
        }

        internal struct BusId
        {
            public long id;
            public int position;
        }
    }
}
