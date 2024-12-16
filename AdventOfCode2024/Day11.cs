using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day11
    {

        public Day11()
        {
            Console.WriteLine("Day11");

            var input = "3279 998884 1832781 517 8 18864 28 0";

            var numbers = input.Split(' ').Select(long.Parse).ToDictionary(t => t, t => 1l);

            var newNums = GetNumbers(numbers);
            
            Console.WriteLine(newNums.Select(t => t.Value).Sum());
        }

        public static Dictionary<long, long[]> FoundMap = new Dictionary<long, long[]>();

        private static Dictionary<long, long> GetNumbers(Dictionary<long, long> numbers)
        {
            for (int i = 0; i < 75; i++)
            {
                Console.WriteLine($"{i}: {numbers.Count()}, Stones: {numbers.Select(t => t.Value).Sum()}");
                var newNumbers = new Dictionary<long, long>();

                foreach (var n in numbers)
                {
                    if (FoundMap.ContainsKey(n.Key))
                    {
                        var newValues = FoundMap[n.Key];
                        foreach (var newValue in newValues)
                        {
                            if (newNumbers.ContainsKey(newValue))
                            {
                                newNumbers[newValue] = newNumbers[newValue] + n.Value;
                            }
                            else
                            {
                                newNumbers.Add(newValue, n.Value);
                            }
                        }
                       
                        continue;
                    }
                    if (n.Key == 0)
                    {
                        if (newNumbers.ContainsKey(1))
                        {
                            newNumbers[1] = newNumbers[1] + n.Value;
                        }
                        else
                        {
                            newNumbers.Add(1, n.Value);
                        }
                        FoundMap.Add(n.Key, [1]);
                    }
                    else if (n.Key.ToString().Length % 2 == 0)
                    {
                        var left = n.Key.ToString().Take(n.Key.ToString().Length / 2);
                        var right = n.Key.ToString().Skip(n.Key.ToString().Length / 2);

                        var leftNum = long.Parse(new string(left.ToArray()));
                        var rightNum = long.Parse(new string(right.ToArray()));

                        if (newNumbers.ContainsKey(leftNum))
                        {
                            newNumbers[leftNum] = newNumbers[leftNum] + n.Value;
                        }
                        else
                        {
                            newNumbers.Add(leftNum, n.Value);
                        }

                        if (newNumbers.ContainsKey(rightNum))
                        {
                            newNumbers[rightNum] = newNumbers[rightNum] + n.Value;
                        }
                        else
                        {
                            newNumbers.Add(rightNum, n.Value);
                        }

                        FoundMap.Add(n.Key,
                            [
                                long.Parse(new string(left.ToArray())),
                                long.Parse(new string(right.ToArray()))
                            ]
                            );
                    }
                    else
                    {
                        var num = n.Key * 2024;
                        if (newNumbers.ContainsKey(num))
                        {
                            newNumbers[num] = newNumbers[num] + n.Value;
                        }
                        else
                        {
                            newNumbers.Add(num, n.Value);
                        }
                        FoundMap.Add(n.Key, [num]);
                    }
                }

                numbers = newNumbers;
            }

            return numbers;
        }
    }
}
