using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day6
    {
        public Day6()
        {
            var lines = File.ReadLines(@"..\..\input\day6.txt").ToList();

            var answers = new List<List<string>>();
            var answer = new List<string>();
            foreach (var line in lines)
            {
                answer.Add(line);
                if (line == string.Empty)
                {
                    answer.RemoveAt(answer.Count() - 1);
                    answers.Add(answer);
                    answer = new List<string>();
                }
            }
            answers.Add(answer);

            var sum = 0;

            //Part 1

            foreach (var ans in answers)
            {
                var num = ans.SelectMany(r => r).Distinct().Count();
                sum += num;
            }

            Console.WriteLine(sum);

            //Part 2

            sum = 0;

            foreach (var ans in answers)
            {
                var distinctAnswers = ans.SelectMany(r => r).Distinct();
                foreach (var letter in distinctAnswers)
                {
                    if (ans.All(a => a.Contains(letter)))
                    {
                        sum++;
                    }
                }
            }

            Console.WriteLine(sum);

        }
    }
}
