using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day5
    {

        public Day5()
        {
            var lines = File.ReadLines(@"..\..\..\input\day5.txt");

            var stacks = getStack();

            var startRow = 10; //Test = 5, Actual = 10

            var instructions = lines.Skip(startRow);

            foreach (var ins in instructions)
            {
                var numberOfMoves = int.Parse(ins.Split("from")[0].Trim().Split(" ")[1].Trim());
                var moveFrom = int.Parse(ins.Split("from")[1].Trim().Split("to")[0].Trim());
                var moveTo = int.Parse(ins.Split("from")[1].Trim().Split("to")[1].Trim());

                for(int i = 0; i<numberOfMoves; i++)
                {
                    var charToMove = stacks[moveFrom].Pop();
                    stacks[moveTo].Push(charToMove);
                }
            }

            var message = "";
            foreach (var stack in stacks)
            {
                message += stack.Value.First();
            }

            Console.WriteLine(message);

            //Part 2
            stacks = getStack();

            foreach (var ins in instructions)
            {
                var numberOfMoves = int.Parse(ins.Split("from")[0].Trim().Split(" ")[1].Trim());
                var moveFrom = int.Parse(ins.Split("from")[1].Trim().Split("to")[0].Trim());
                var moveTo = int.Parse(ins.Split("from")[1].Trim().Split("to")[1].Trim());

                var charsToMove = new List<string>();

                for (int i = 0; i < numberOfMoves; i++)
                {
                    var charToMove = stacks[moveFrom].Pop();
                    charsToMove.Add(charToMove);
                }

                charsToMove.Reverse();

                foreach (var ch in charsToMove)
                {
                    stacks[moveTo].Push(ch);
                }
            }

            message = "";
            foreach (var stack in stacks)
            {
                message += stack.Value.First();
            }

            Console.WriteLine(message);
        }

        public Dictionary<int, Stack<string>> getStack()
        {
            var testStacks = new Dictionary<int, Stack<string>>()
            {
                {1, new Stack<string>(new []{"Z", "N"})},
                {2, new Stack<string>(new []{"M", "C", "D"}) },
                {3, new Stack<string>(new []{"P"}) },
            };

            //Part 1
            var myStacks = new Dictionary<int, Stack<string>>()
            {
                {1, new Stack<string>(new []{"N", "B", "D", "T", "V", "G", "Z", "J"})},
                {2, new Stack<string>(new []{"S", "R", "M", "D", "W", "P", "F"}) },
                {3, new Stack<string>(new []{"V", "C", "R", "S", "Z"}) },
                {4, new Stack<string>(new []{"R", "T", "J", "Z", "P", "H", "G"}) },
                {5, new Stack<string>(new []{"T", "C", "J", "N", "D", "Z", "Q", "F"}) },
                {6, new Stack<string>(new []{"N", "V", "P", "W", "G", "S", "F", "M"}) },
                {7, new Stack<string>(new []{"G", "C", "V", "B", "P", "Q"}) },
                {8, new Stack<string>(new []{"Z", "B", "P", "N"}) },
                {9, new Stack<string>(new []{"W", "P", "J"}) },
            };

            return myStacks;
        }
    }
}