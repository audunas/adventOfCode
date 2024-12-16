using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day2
    {

        public Day2()
        {
            Console.WriteLine("Day2");

            var lines = File.ReadLines(@"..\..\..\input\day2.txt").ToArray();

            var counter = 0;

            foreach (var line in lines)
            {
                var nums = line.Split(' ').Select(i => int.Parse(i));
                var zipped = nums.Zip(nums.Skip(1));

                if (zipped.All(z => (z.First - z.Second) > 0 && (z.First - z.Second) < 4))
                {
                    counter++;
                }
                else if (zipped.All(z => (z.Second - z.First) > 0 && (z.Second - z.First) < 4))
                {
                    counter++;
                }

            }

            Console.WriteLine(counter);

            counter = 0;

            //Part 2
            foreach (var line in lines)
            {
                var nums = line.Split(' ').Select(int.Parse).ToList();

                var listOfNums = new List<List<int>>
                {
                    nums
                };

                for (var i = 0; i<nums.Count; i++)
                {
                    var newList = new List<int>();
                    for (var j = 0; j < nums.Count; j++)
                    {
                        if (i != j)
                        {
                            newList.Add(nums[j]);
                        }
                    }
                    listOfNums.Add(newList);
                }

                var anyHit = false;

                foreach (var numList in listOfNums) {

                    var zipped = numList.Zip(numList.Skip(1));

                    if (zipped.All(z => (z.First - z.Second) > 0 && (z.First - z.Second) < 4))
                    {
                        anyHit = true;
                    }
                    else if (zipped.All(z => (z.Second - z.First) > 0 && (z.Second - z.First) < 4))
                    {
                        anyHit = true;
                    }
                }

                if (anyHit)
                {
                    counter++;
                }
                

            }

            Console.WriteLine(counter);
        }
    }
}
