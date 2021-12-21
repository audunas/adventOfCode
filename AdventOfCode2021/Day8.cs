using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
    class Day8
    {
        public Day8()
        {
            var lines = File.ReadLines(@"..\..\..\input\day8.txt");

            var outputDigits = lines.SelectMany(l => l.Split('|')[1].Trim().Split(" "));

            //Part 1
            Console.WriteLine(outputDigits.Where(d => new[] { 2, 3, 4, 7 }.Contains(d.Length)).Count());

            //Part 2
            var sum = 0;
            foreach (var line in lines)
            {
                var signals = GetSignals(line);
                var output = line.Split('|')[1].Trim().Split(" ");
                var outputValue = "";
                foreach (var entry in output)
                {
                    var digit = signals.Where(r => r.Value.Union(entry.ToCharArray()).Count()
                                == entry.ToCharArray().Count() 
                                && r.Value.Count() == entry.ToCharArray().Count()).First().Key;
                    outputValue += digit;
                }
                var number = int.Parse(outputValue);
                sum += number;
            }

            Console.WriteLine(sum);
        }

        public static Dictionary<int, char[]> GetSignals(string line)
        {
            var inputDigits = line.Split('|')[0].Trim().Split(" ");
            var config = new Dictionary<int, int[]>()
            {
                { 0, new int[] {0, 1, 2, 4, 5, 6 } },
                { 1, new int[] {2, 5} },
                { 2, new int[] {0, 2, 3, 4, 6 } },
                { 3, new int[] {0, 2, 3, 5, 6 } },
                { 4, new int[] {1, 2, 3, 5 } },
                { 5, new int[] {0, 1, 3, 5, 6 } },
                { 6, new int[] {0, 1, 3, 4, 5, 6 } },
                { 7, new int[] {0, 2, 5 } },
                { 8, new int[] {0, 1, 2, 3, 4, 5, 6 } },
                { 9, new int[] {0, 1, 2, 3, 4, 6 } },
            };

            var positionConfig = new List<Position>();
            positionConfig.Add(new Position()
            {
                PositionConfig = 1,
                PossibleChars = inputDigits.Where(d => d.Length == 2).First().ToCharArray()
            });
            positionConfig.Add(new Position()
            {
                PositionConfig = 4,
                PossibleChars = inputDigits.Where(d => d.Length == 4).First().ToCharArray()
            });
            positionConfig.Add(new Position()
            {
                PositionConfig = 7,
                PossibleChars = inputDigits.Where(d => d.Length == 3).First().ToCharArray()
            });
            positionConfig.Add(new Position()
            {
                PositionConfig = 8,
                PossibleChars = inputDigits.Where(d => d.Length == 7).First().ToCharArray()
            });

            var pos7Letters = positionConfig.Where(m => m.PositionConfig == 7).First().PossibleChars;
            var pos0 = pos7Letters
                .Except(positionConfig.Where(m => m.PositionConfig == 1).First().PossibleChars);

            var allChars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

            var finalList = new Signal[]
            {
                new Signal {Position = 0, PossibleLetters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
                new Signal {Position = 1, PossibleLetters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
                new Signal {Position = 2, PossibleLetters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
                new Signal {Position = 3, PossibleLetters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
                new Signal {Position = 4, PossibleLetters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
                new Signal {Position = 5, PossibleLetters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
                new Signal {Position = 6, PossibleLetters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
            };
            // Finding pos 0
            finalList[0] = new Signal() { Position = 0, PossibleLetters = pos0.ToArray() };
            finalList[1] = new Signal() { Position = 1, PossibleLetters = allChars.Except(pos7Letters).ToArray() };
            finalList[2] = new Signal() { Position = 2, PossibleLetters = pos7Letters.Except(pos0).ToArray() };
            finalList[3] = new Signal() { Position = 3, PossibleLetters = allChars.Except(pos7Letters).ToArray() };
            finalList[4] = new Signal() { Position = 4, PossibleLetters = allChars.Except(pos7Letters).ToArray() };
            finalList[5] = new Signal() { Position = 5, PossibleLetters = pos7Letters.Except(pos0).ToArray() };
            finalList[6] = new Signal() { Position = 6, PossibleLetters = allChars.Except(pos7Letters).ToArray() };

            var remainingPos4Letters = inputDigits.Where(d => d.Length == 4).First()
                .Except(pos7Letters);
            finalList[1] = new Signal() { Position = 1, PossibleLetters = remainingPos4Letters.ToArray() };
            finalList[3] = new Signal() { Position = 3, PossibleLetters = remainingPos4Letters.ToArray() };
            finalList[2] = new Signal() { Position = 2, PossibleLetters = finalList[2].PossibleLetters.Except(remainingPos4Letters).ToArray() };
            finalList[4] = new Signal() { Position = 4, PossibleLetters = finalList[4].PossibleLetters.Except(remainingPos4Letters).ToArray() };
            finalList[5] = new Signal() { Position = 5, PossibleLetters = finalList[5].PossibleLetters.Except(remainingPos4Letters).ToArray() };
            finalList[6] = new Signal() { Position = 6, PossibleLetters = finalList[6].PossibleLetters.Except(remainingPos4Letters).ToArray() };
            var charPositions = new Dictionary<int, char>();
            charPositions.Add(0, pos0.Single());

            //Finding pos 4 and 6
            var signal9 = inputDigits.Where(r => r.Length == 6
                && r.ToCharArray().Except(inputDigits.Where(d => d.Length == 4).First().ToCharArray()).Count() == 2);

            var remainingLetter = signal9.First().ToCharArray()
                .Intersect(finalList[4].PossibleLetters);
            finalList[6] = new Signal() { Position = 6, PossibleLetters = remainingLetter.ToArray() };
            finalList[4] = new Signal() { Position = 4, PossibleLetters = allChars.Except(signal9.First().ToCharArray()).ToArray() };

            //Finding pos 1 and 3

            var signal0 = inputDigits.Where(r => r.Length == 6
                && r.ToCharArray()
                .Except(inputDigits.Where(d => d.Length == 3).First().ToCharArray())
                .Except(finalList[4].PossibleLetters)
                .Except(finalList[6].PossibleLetters)
                .Count() == 1);

            var charAt1 = signal0.First().ToCharArray()
                .Except(finalList[0].PossibleLetters)
                .Except(finalList[2].PossibleLetters)
                .Except(finalList[4].PossibleLetters)
                .Except(finalList[6].PossibleLetters);
            finalList[1] = new Signal() { Position = 1, PossibleLetters = charAt1.ToArray() };
            finalList[3] = new Signal() { Position = 3, PossibleLetters = finalList[3].PossibleLetters.Except(charAt1).ToArray() };

            //Finding pos 2 and 5
            var signal2 = inputDigits.Where(r => r.Length == 5
                && signal9.First()
                .Intersect(r.ToCharArray())
                .Count() == 4);

            var charAt2 = signal2.First().ToCharArray()
                .Except(finalList[0].PossibleLetters)
                .Except(finalList[3].PossibleLetters)
                .Except(finalList[4].PossibleLetters)
                .Except(finalList[6].PossibleLetters);
            finalList[2] = new Signal() { Position = 2, PossibleLetters = charAt2.ToArray() };
            finalList[5] = new Signal() { Position = 5, PossibleLetters = finalList[5].PossibleLetters.Except(charAt2).ToArray() };

            var signals = new Dictionary<int, char[]>
            {
                {0, new char[] {finalList[0].PossibleLetters.First(), finalList[1].PossibleLetters.First(),
                    finalList[2].PossibleLetters.First(),
                    finalList[4].PossibleLetters.First(), finalList[5].PossibleLetters.First(),
                    finalList[6].PossibleLetters.First()} },
                {1, new char[] { finalList[2].PossibleLetters.First(), finalList[5].PossibleLetters.First()} },
                {2, new char[] {finalList[0].PossibleLetters.First(),
                    finalList[2].PossibleLetters.First(), finalList[3].PossibleLetters.First(),
                    finalList[4].PossibleLetters.First(),
                    finalList[6].PossibleLetters.First()} },
                {3, new char[] {finalList[0].PossibleLetters.First(),
                    finalList[2].PossibleLetters.First(), finalList[3].PossibleLetters.First(),
                    finalList[5].PossibleLetters.First(),
                    finalList[6].PossibleLetters.First()} },
                {4, new char[] { finalList[1].PossibleLetters.First(),
                    finalList[2].PossibleLetters.First(), finalList[3].PossibleLetters.First(),
                    finalList[5].PossibleLetters.First(),
                    } },
                {5, new char[] {finalList[0].PossibleLetters.First(), finalList[1].PossibleLetters.First(),
                    finalList[3].PossibleLetters.First(),
                    finalList[5].PossibleLetters.First(),
                    finalList[6].PossibleLetters.First()} },
                {6, new char[] {finalList[0].PossibleLetters.First(), finalList[1].PossibleLetters.First(),
                    finalList[3].PossibleLetters.First(),
                    finalList[4].PossibleLetters.First(), finalList[5].PossibleLetters.First(),
                    finalList[6].PossibleLetters.First()} },
                {7, new char[] {finalList[0].PossibleLetters.First(),
                    finalList[2].PossibleLetters.First(),
                    finalList[5].PossibleLetters.First(),
                    } },
                {8, new char[] {finalList[0].PossibleLetters.First(), finalList[1].PossibleLetters.First(),
                    finalList[2].PossibleLetters.First(), finalList[3].PossibleLetters.First(),
                    finalList[4].PossibleLetters.First(), finalList[5].PossibleLetters.First(),
                    finalList[6].PossibleLetters.First()} },
                {9, new char[] {finalList[0].PossibleLetters.First(), finalList[1].PossibleLetters.First(),
                    finalList[2].PossibleLetters.First(), finalList[3].PossibleLetters.First(),
                    finalList[5].PossibleLetters.First(),
                    finalList[6].PossibleLetters.First()} }
            };

            return signals;
        }

        public struct Signal
        {
            public int Position;
            public char[] PossibleLetters;
        }

        public struct Position
        {
            public int PositionConfig;
            public char[] PossibleChars;
        }
    }
}
