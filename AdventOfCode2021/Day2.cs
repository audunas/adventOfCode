using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day2
    {
        public Day2()
        {
            var lines = File.ReadLines(@"..\..\..\input\day2.txt");

            var ins = lines.Select(l =>
            {
                var parse = l.Split(' ');
                return new Instruction { command = parse[0].Trim(), number = int.Parse(parse[1].Trim()) };
            });

            //Part 1
            var horizontal = 0;
            var depth = 0;

            foreach (var instruc in ins)
            {
                if (instruc.command == "forward") 
                {
                    horizontal += instruc.number;
                }
                else if (instruc.command == "down")
                {
                    depth += instruc.number;
                }
                else if (instruc.command == "up")
                {
                    depth -= instruc.number;
                }
            }
            var result = horizontal * depth;
            Console.WriteLine(result);

            //Part 2
            horizontal = 0;
            depth = 0;
            var aim = 0;

            foreach (var instruc in ins)
            {
                if (instruc.command == "forward")
                {
                    horizontal += instruc.number;
                    depth += (aim * instruc.number);
                }
                else if (instruc.command == "down")
                {
                    aim += instruc.number;
                }
                else if (instruc.command == "up")
                {
                    aim -= instruc.number;
                }
            }
            result = horizontal * depth;
            Console.WriteLine(result);
        }

        public struct Instruction
        {
            public string command { get; set; }
            public int number { get; set; }
        }
    }
}
