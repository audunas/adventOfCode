using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day1
    {

        public Day1()
        {
            Console.WriteLine("Day1");

            var lines = File.ReadLines(@"..\..\..\input\day1.txt").ToArray();

            var leftList = new List<int>();
            var rightList = new List<int>();

            foreach (var line in lines)
            {
                var sp = line.Split(' ').Select(t => t.Trim()).Where(h => h.Length > 0).ToArray();
                leftList.Add(int.Parse(sp[0]));
                rightList.Add(int.Parse(sp[1]));
            }

            leftList.Sort();
            rightList.Sort(); 

            var sum = 0;

            for (var i = 0; i < leftList.Count; i++)
            {
                var l = leftList[i];
                var r = rightList[i];
                sum += Math.Abs(l - r);
            }

            Console.WriteLine(sum);

            //Part 2

            var simScore = 0;
            for (var i = 0; i < leftList.Count; i++)
            {
                var l = leftList[i];
                var count = rightList.Where(r => r== l).Count();
                simScore += (l*count);
            }

            Console.WriteLine(simScore);
        }
    }
}
