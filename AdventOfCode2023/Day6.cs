using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day6
    {
        public Day6()
        {
            Console.WriteLine("Day6");

            var lines = File.ReadLines(@"..\..\..\input\day6.txt");

            //Part 1

            var time = lines.First().Split(':')[1].Split(' ').Where(c => c.Trim() != "").Select(int.Parse).ToArray();
            var distance = lines.Last().Split(':')[1].Split(' ').Where(c => c.Trim() != "").Select(int.Parse).ToArray();

            var total = 1;

            for (var i = 0; i<time.Count(); i++)
            {
                var t = time[i];
                var d = distance[i];

                var waysToWin = 0;

                for (var j = 0; j<=t; j++)
                {
                    var speed = j;
                    var raceTime = t - j;
                    var dist = speed * raceTime;
                    if (dist > d)
                    {
                        waysToWin++;
                    }
                }

                total *= waysToWin;
            }

            Console.WriteLine(total);
            
            //Part 2

            var time1 = long.Parse(string.Join("", lines.First().Split(':')[1].Split(' ').Where(c => c.Trim() != "")));
            var distance1 = long.Parse(string.Join("",lines.Last().Split(':')[1].Split(' ').Where(c => c.Trim() != "")));

            total = 0;

            for (var j = 0; j <= time1; j++)
            {
                var speed = j;
                var raceTime = time1 - j;
                var dist = speed * raceTime;
                if (dist > distance1)
                {
                    total++;
                }
            }

            Console.WriteLine(total);
        }

        public class Race
        {
            public int Time;
            public int Distance;
        }
    }
}
