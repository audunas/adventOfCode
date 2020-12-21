using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Day15
    {
        public Day15()
        {
            var lines = File.ReadLines(@"..\..\input\day15.txt").ToList();
            var startingNumbers = lines.First().Split(',').Select(l => int.Parse(l));
            var numbersSpoken = new Dictionary<int, NumberObject>();
            for (var i = 0; i < startingNumbers.Count(); i++)
            {
                var number = startingNumbers.ElementAt(i);
                numbersSpoken[number] = new NumberObject { Number = 0, LastTurnSpoken = i + 1, SecondToLastTurnSpoken = 0, NumberOfTimesSpoken = 1 };
            }

            // Part 1
            var currentNumber = GetCurrentNumber(new Dictionary<int, NumberObject>(numbersSpoken), startingNumbers.Count() + 1, startingNumbers.Last(), new HashSet<int>(startingNumbers), 2020);

            Console.WriteLine(currentNumber);

            // Part 2
            currentNumber = GetCurrentNumber(new Dictionary<int, NumberObject>(numbersSpoken), startingNumbers.Count() + 1, startingNumbers.Last(), new HashSet<int>(startingNumbers), 30000000);

            Console.WriteLine(currentNumber);
        }

        private static int GetCurrentNumber(Dictionary<int, NumberObject> numbersSpoken, int counter, int currentNumber, HashSet<int> numbersFound, int iterationStop)
        {
            while (counter <= iterationStop)
            {
                if (numbersFound.Contains(currentNumber))
                {
                    var item = numbersSpoken[currentNumber];
                    if (item.SecondToLastTurnSpoken != 0)
                    {
                        currentNumber = item.LastTurnSpoken - item.SecondToLastTurnSpoken;
                        if (numbersFound.Contains(currentNumber))
                        {
                            item = numbersSpoken[currentNumber];
                            numbersSpoken[currentNumber] = new NumberObject
                            {
                                Number = currentNumber,
                                LastTurnSpoken = counter,
                                SecondToLastTurnSpoken = numbersSpoken[currentNumber].LastTurnSpoken,
                                NumberOfTimesSpoken = item.NumberOfTimesSpoken + 1
                            };
                        }
                        else
                        {
                            numbersSpoken[currentNumber] = new NumberObject
                            {
                                Number = currentNumber,
                                LastTurnSpoken = counter,
                                SecondToLastTurnSpoken = 0,
                                NumberOfTimesSpoken = 1
                            };
                            numbersFound.Add(currentNumber);
                        }
                    }
                    else
                    {
                        currentNumber = 0;
                        numbersSpoken[currentNumber] = new NumberObject
                        {
                            Number = currentNumber,
                            LastTurnSpoken = counter,
                            SecondToLastTurnSpoken = numbersSpoken[currentNumber].LastTurnSpoken,
                            NumberOfTimesSpoken = item.NumberOfTimesSpoken + 1
                        };

                    }
                }


                counter++;
            }

            return currentNumber;
        }

        internal struct NumberObject
        {
            public int Number;
            public int LastTurnSpoken;
            public int SecondToLastTurnSpoken;
            public int NumberOfTimesSpoken;
        }
    }
}
