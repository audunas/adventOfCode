using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2022
{
    public class Day11
    {

        public Day11()
        {
            var lines = File.ReadLines(@"..\..\..\input\day11.txt");

            var monkeys = lines.Where(l => l.Length > 0).Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 6)
                .Select(x => x.Select(v => v.Value).ToList())
                .Select(m => new Monkey {
                    Id = int.Parse(m[0].Split(" ")[1].Split(":")[0]),
                    Items = m[1].Split(":")[1].Split(",").Select(item => BigInteger.Parse(item)).ToList(),
                    Operation = m[2].Split(":")[1].Split("=")[1].Trim(),
                    DivisibleBy = int.Parse(m[3].Split(" ").Last()),
                    TrueMonkeyId = int.Parse(m[4].Split(" ").Last()),
                    FalseMonkeyId = int.Parse(m[5].Split(" ").Last()),
                    InspectCounter = 0,
                })
                .ToList();

            var roundCounter = 0;

            var numberOfRounds = 20;
            numberOfRounds = 10000;

            while (roundCounter < 20)
            {
                Console.WriteLine(roundCounter);
                foreach (var monkey in monkeys)
                {
                    foreach (var item in monkey.Items)
                    {
                        monkey.InspectCounter++;
                        var operation = monkey.Operation.Split(" ");
                        var bigIntItem = item;
                        BigInteger worryLevel = item;
                        var number = bigIntItem;
                        if (operation[2].Trim() == "old")
                        {
                            number = bigIntItem;
                        }
                        else
                        {
                            var multiplier = int.Parse(operation[2].Trim());
                            number = multiplier;
                            
                        }
                        if (operation[1] == "+")
                        {
                            worryLevel = bigIntItem + number;
                        }
                        else
                        {
                            worryLevel = bigIntItem * number;
                        }
                        //Part 1: worryLevel = (BigInteger)Math.Floor(((decimal)worryLevel / 3));

                        if (isDivisble(worryLevel, monkey.DivisibleBy))
                        {
                            monkeys[monkey.TrueMonkeyId].Items.Add(worryLevel);
                        }
                        else
                        {
                            monkeys[monkey.FalseMonkeyId].Items.Add(worryLevel);
                        }
                    }
                    monkey.Items = new List<BigInteger>();
                }
                roundCounter++;
            }

            var twoMostActive = monkeys.OrderByDescending(m => m.InspectCounter).Take(2).ToList();

            var result = twoMostActive[0].InspectCounter * twoMostActive[1].InspectCounter;

            Console.WriteLine(result);
        }

        public bool isDivisble(BigInteger number, int divideBy)
        {
            //var allDigits = number.ToString().Select(n => int.Parse(n.ToString())).ToList();
            //var lastDigit = allDigits.Last();
            //if (divideBy == 2)
            //{
            //    return new List<int>() { 0, 2, 4, 6, 8 }.Contains(lastDigit);
            //}
            //else if (divideBy == 3)
            //{
            //    var sum = allDigits.Sum();
            //    return sum % 3 == 0;
            //}
            //else if (divideBy == 5)
            //{
            //    return new List<int>() { 0, 5 }.Contains(lastDigit);
            //}
            //else if (divideBy == 7)
            //{
            //    var digitCopy = number;
            //    while (digitCopy.ToString().Count() > 1)
            //    {
            //        var start = BigInteger.Parse(digitCopy.ToString().Substring(0, digitCopy.ToString().Count() - 1));
            //        var lastD = BigInteger.Parse(digitCopy.ToString().Last().ToString());

            //        var newValue = start - (lastD * 2);

            //        digitCopy = newValue;
            //    }

            //    return new List<int>() { 0, 7 }.Contains((int)digitCopy);
            //}
            //else if (divideBy == 11)
            //{
            //    var altSum = 0;
            //    for (var i = 1; i<=allDigits.Count; i++)
            //    {
            //        if (i % 2 == 0)
            //        {
            //            altSum += allDigits[i];
            //        }
            //        else
            //        {
            //            altSum -= allDigits[i];
            //        }
            //    }
            //    return altSum % 11 == 0;
            //}
            //else if (divideBy == 13)
            //{
            //    var reversed = number.ToString().Select(n => int.Parse(n.ToString())).ToList();
            //    reversed.Reverse();
            //    var chunks = reversed.Select((x, i) => new { Index = i, Value = x })
            //                        .GroupBy(x => x.Index / 3);

            //    var chunksOf3 = new List<string>();
            //    foreach (var g in chunks)
            //    {
            //        var l = g.ToList().Select(d => d.Value);
            //        l = l.Reverse();
            //        var s = l.Aggregate("", (a, b) => a + b);
            //        chunksOf3.Add(s);
            //    }

            //    var altSum = 0;
            //    for (var i = 1; i <= chunksOf3.Count; i++)
            //    {
            //        if (i % 2 == 0)
            //        {
            //            altSum += int.Parse(chunksOf3[i-1]);
            //        }
            //        else
            //        {
            //            altSum -= int.Parse(chunksOf3[i-1]);
            //        }
            //    }
            //    return altSum % 13 == 0;
            //}
            //else if (divideBy == 17)
            //{
            //    var digitCopy = number;
            //    while (digitCopy.ToString().Count() > 2 && digitCopy > divideBy)
            //    {
            //        var start = BigInteger.Parse(digitCopy.ToString().Substring(0, digitCopy.ToString().Count() - 1));
            //        var lastD = BigInteger.Parse(digitCopy.ToString().Last().ToString());

            //        var newValue = start - (lastD * 5);

            //        digitCopy = newValue;
            //    }

            //    return new List<int>() { 0, 17 }.Contains((int)digitCopy) || digitCopy % divideBy == 0;
            //}
            //else if (divideBy == 19)
            //{
            //    var digitCopy = number;
            //    while (digitCopy.ToString().Count() > 2 && digitCopy > divideBy)
            //    {
            //        var start = BigInteger.Parse(digitCopy.ToString().Substring(0, digitCopy.ToString().Count() - 1));
            //        var lastD = BigInteger.Parse(digitCopy.ToString().Last().ToString());

            //        var newValue = start + (lastD * 2);

            //        digitCopy = newValue;
            //    }

            //    return new List<int>() { 0, 19 }.Contains((int)digitCopy) || digitCopy % divideBy == 0;
            //}
            //else if (divideBy == 23)
            //{
            //    var digitCopy = number;
            //    while (digitCopy.ToString().Count() > 2 && digitCopy > divideBy)
            //    {
            //        var start = BigInteger.Parse(digitCopy.ToString().Substring(0, digitCopy.ToString().Count()- 1));
            //        var lastD = BigInteger.Parse(digitCopy.ToString().Last().ToString());

            //        var newValue = start + (lastD * 7);

            //        digitCopy = newValue;
            //    }

            //    return new List<int>() { 0, 23 }.Contains((int)digitCopy) || digitCopy % divideBy == 0;
            //}

            return number % divideBy == 0;
        }

        public BigInteger gcd(BigInteger a, BigInteger b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        public class Monkey
        {
            public int Id { get; set; }
            public List<BigInteger> Items { get; set; }
            public string Operation { get; set; }
            public int DivisibleBy { get; set; }
            public int TrueMonkeyId { get; set; }
            public int FalseMonkeyId { get; set; }
            public long InspectCounter { get; set; }
        }
    }
}