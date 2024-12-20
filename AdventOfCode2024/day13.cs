using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day13
    {
        public Day13()
        {
            Console.WriteLine("Day13");

            var lines = File.ReadLines(@"..\..\..\input\day13.txt").ToArray();

            var sum = 0l;

            Console.WriteLine($"Number of lines: {lines.Count()}");

            for (int i = 0; i<lines.Length; i = i+4)
            {
                var line = lines[i];
                var line2 = lines[i + 1];
                var line3 = lines[i + 2];

                var aX = long.Parse(line.Split(':')[1].Split(',')[0].Split('+')[1].Trim());
                var aY = long.Parse(line.Split(':')[1].Split(',')[1].Split('+')[1].Trim());

                var bX = long.Parse(line2.Split(':')[1].Split(',')[0].Split('+')[1].Trim());
                var bY = long.Parse(line2.Split(':')[1].Split(',')[1].Split('+')[1].Trim());

                var xTarget = long.Parse(line3.Split(':')[1].Split(',')[0].Split('=')[1].Trim());
                var yTarget = long.Parse(line3.Split(':')[1].Split(',')[1].Split('=')[1].Trim());

                //Part2
                //xTarget += 10000000000000;
                //yTarget += 10000000000000;

                //(x1 * aX) + (x2 * bX) = xTarget
                //(x1 * aY) + (x2 * bY) = yTarget

                var lcm = LCM([aX, aY]);

                var multA = lcm / aX;
                var multB = lcm / aY;

                var newAX = aX * multA;
                var newBX = bX * multA;
                var newXTarget = xTarget * multA;

                var newAY = aY * (-multB);
                var newBY = bY * (-multB);
                var newYTarget = yTarget * (-multB);

                var newY = newBX + newBY;
                var newTarget = newXTarget + newYTarget;
                var y = newTarget / newY;

                var bxWithReplacedY = bX * y;
                newXTarget = xTarget - bxWithReplacedY;
                var x = newXTarget / aX;

                var replacedAY = aY * x;
                var replacedBY = bY * y;
                if ((replacedAY + replacedBY) != yTarget)
                {
                    continue;
                }

                var replacedAX = aX * x;
                var replacedBX = bX * y;
                if ((replacedAX + replacedBX) != xTarget)
                {
                    continue;
                }

                var aCost = x * 3L;
                var bCost = y * 1L;
                var cost = aCost + bCost;
                sum += cost;
            }


            Console.WriteLine(sum);

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
