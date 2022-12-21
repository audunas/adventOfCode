using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day13
    {

        public Day13()
        {
            var lines = File.ReadLines(@"..\..\..\input\day13.txt");

            var chunksOf2 = lines.Where(l => l != "").Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 2)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            var inRightOrder = new HashSet<int>();
            var counter = 1;

            foreach (var ch in chunksOf2)
            {
                var left = new string(ch.First().Skip(1).Reverse().Skip(1).Reverse().ToArray());
                var right = new string(ch.Last().Skip(1).Reverse().Skip(1).Reverse().ToArray());
                Console.WriteLine();
                Console.WriteLine("left: "+left+" right:"+right);
                var rightOrder = true;

                while (left.Any())
                {
                    if (!left.Contains('[') && !left.Contains(']') && !right.Contains(']') && !right.Contains('['))
                    {
                        var leftWasActualList = (ch.First().TakeWhile(l => l == '[').Count() > 1 &&
                            ch.First().Reverse().TakeWhile(l => l == ']').Count() > 1) ||
                            ch.First().Where(r => r == '[').Count() == 1;
                        var rightWasActualList = (ch.Last().TakeWhile(l => l == '[').Count() > 1 &&
                            ch.Last().Reverse().TakeWhile(l => l == ']').Count() > 1) ||
                            ch.Last().Where(r => r == '[').Count() == 1;
                        rightOrder = isInRightOrder(left.Split(',').Select(int.Parse).ToArray(), 
                            right.Split(',').Select(int.Parse).ToArray(), leftWasActualList, rightWasActualList);
                        break;
                    }
                    var lArray = new List<string>();
                    var rArray = new List<string>();
                    var skipLeft = 1;
                    var skipRight = 1;
                    var leftIsActualList = true;
                    var rightIsActualList = true;
                    if (left.StartsWith('['))
                    {
                        var m = new string(left.Skip(1).TakeWhile(l => l != ']').ToArray());
                        lArray = m.Split(',').Where(r => r != "").ToList();
                        skipLeft += lArray.Count == 1 ? 1 : (lArray.Count * 2);
                    }
                    else
                    {
                        lArray = new List<string>() { new string(left.TakeWhile(r => r != ',').ToArray()) };
                        leftIsActualList = false;
                    }
                    if (right.StartsWith('['))
                    {
                        var m = new string(right.Skip(1).TakeWhile(l => l != ']').ToArray());
                        rArray = m.Split(',').Where(r => r != "").ToList();
                        skipRight += rArray.Count == 1 ? 1 : (rArray.Count * 2);
                    }
                    else
                    {
                        rArray = new List<string>() { new string(right.TakeWhile(r => r != ',').ToArray()) };
                        rightIsActualList = false;
                    }
                    if (lArray.Any(r => int.TryParse(r, out int v)) &&
                        rArray.Any(r => int.TryParse(r, out int y)))
                    {
                        rightOrder = isInRightOrder(lArray.Select(int.Parse).ToArray(), rArray.Select(int.Parse).ToArray(), 
                            leftIsActualList, rightIsActualList);
                    }
                    if (!rightOrder)
                    {
                        break;
                    }
                    if (left.Length > skipLeft + 2)
                    {
                        if (left[skipLeft] == ',' || left[skipLeft + 1] == ',' || left[skipLeft + 1] == ']')
                        {
                            if (left[skipLeft + 1] == ']' && left[skipLeft + 2] == ',')
                            {
                                skipLeft++;
                            }
                            skipLeft++;
                        }
                    }
                    if (right.Length > skipRight + 2)
                    {
                        if (right[skipRight] == ',' || right[skipRight + 1] == ',' || right[skipRight + 1] == ']')
                        {
                            if (right[skipRight + 1] == ']' && right[skipRight + 2] == ',')
                            {
                                skipRight++;
                            }
                            skipRight++;
                        }
                    }
                    left = new string(left.Skip(skipLeft).ToArray()).TrimStart(',');
                    right = new string(right.Skip(skipRight).ToArray()).TrimStart(',');
                    if (left.Any() && !right.Any())
                    {
                        rightOrder = false;
                        break;
                    }
                }
                if (rightOrder)
                {
                    inRightOrder.Add(counter);
                    Console.WriteLine("RightOrder Indexes: "+string.Join(',',inRightOrder));
                    Console.WriteLine("");
                }

                counter++;
            }

            Console.WriteLine(inRightOrder.Sum());
        }

        private static bool isInRightOrder(int[] leftList, int[] rightList, bool leftIsActualList, bool rightIsActualList)
        {
            Console.WriteLine("leftList:" + string.Join(',', leftList) + " -- rightList: "+ string.Join(',', rightList));

            if (leftIsActualList && rightIsActualList)
            {
                if (leftList.Length > rightList.Length)
                {
                    return false;
                }
                else if (rightList.Length > leftList.Length)
                {
                    return true;
                }
            }

            var rightOrder = true;
            for (var j = 0; j < leftList.Length; j++)
            { 
                if (j >= rightList.Length)
                {
                    if (rightIsActualList)
                    {
                        rightOrder = false;
                    }
                    break;
                }
                var lValue = leftList[j];
                var rValue = rightList[j];
               
                if (lValue > rValue)
                {
                    rightOrder = false;
                    break;
                }
            }
            //if (leftList.Length < rightList.Length && rightOrder)
            //{
            //    return false;
            //}

            return rightOrder;
        }
    }
}
