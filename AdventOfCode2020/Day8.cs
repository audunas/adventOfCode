using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day8
    {
        public Day8()
        {
            var lines = File.ReadLines(@"..\..\input\day8.txt").ToList();

            // Part 1
            var instructions = lines.Select(l => 
            {
                var split = l.Split(' ');
                var operation = split[0];
                var argument = int.Parse(split[1]);
                return new Instruction { Argument = argument, Operation = operation };
            }
            );

            var visitedIndices = new List<int>();
            var currentIndex = 0;
            var accumulator = 0;

            while (!visitedIndices.Contains(currentIndex))
            {
                visitedIndices.Add(currentIndex);
                var currentInstruction = instructions.ElementAt(currentIndex);
                switch (currentInstruction.Operation)
                {
                    case "nop":
                        currentIndex++;
                        break;
                    case "acc":
                        accumulator += currentInstruction.Argument;
                        currentIndex++;
                        break;
                    case "jmp":
                        currentIndex += currentInstruction.Argument;
                        break;
                    default:
                        throw new Exception($"Unknown operation: {currentInstruction.Operation}");
                }

            }

            Console.WriteLine(accumulator);

            // Part 2

            var possibleSetsOfInstructions = new List<List<Instruction>>();

            for (var i = 0; i< instructions.Count(); i++)
            {
                var currentInstruction = instructions.ElementAt(i);
                var newInstruction = currentInstruction;
                switch (currentInstruction.Operation)
                {
                    case "nop":
                        newInstruction = new Instruction { Argument = currentInstruction.Argument, Operation = "jmp" };
                        break;
                    case "acc":
                        break;
                    case "jmp":
                        newInstruction = new Instruction { Argument = currentInstruction.Argument, Operation = "nop" };
                        break;
                    default:
                        throw new Exception($"Unknown operation: {currentInstruction.Operation}");
                }
                if (currentInstruction.Operation != "acc")
                {
                    var listOfInstructions = new List<Instruction>(instructions);
                    listOfInstructions.RemoveAt(i);
                    listOfInstructions.Insert(i, newInstruction);
                    possibleSetsOfInstructions.Add(listOfInstructions);
                }
            }

            var reachedEnd = false;

            foreach (var ins in possibleSetsOfInstructions)
            {
                visitedIndices = new List<int>();
                currentIndex = 0;
                accumulator = 0;

                while (!visitedIndices.Contains(currentIndex))
                {
                    visitedIndices.Add(currentIndex);
                    var currentInstruction = ins.ElementAt(currentIndex);
                    switch (currentInstruction.Operation)
                    {
                        case "nop":
                            currentIndex++;
                            break;
                        case "acc":
                            accumulator += currentInstruction.Argument;
                            currentIndex++;
                            break;
                        case "jmp":
                            currentIndex += currentInstruction.Argument;
                            break;
                        default:
                            throw new Exception($"Unknown operation: {currentInstruction.Operation}");
                    }
                    if (currentIndex >= ins.Count())
                    {
                        reachedEnd = true;
                        Console.WriteLine(accumulator);
                        Console.WriteLine("Reach end of index");
                        break;
                    }
                }
                if (reachedEnd)
                {
                    break;
                }
            }
        }

        internal struct Instruction
        {
            public string Operation;
            public int Argument;
        }
    }
}
