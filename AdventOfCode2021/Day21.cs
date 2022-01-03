using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AdventOfCode2021
{
    class Day21
    {
        public Day21()
        {
            var liens = File.ReadLines(@"..\..\..\input\day21.txt");

            //Part 1
            var player1Score = 0;
            var player2Score = 0;

            var player1Position = 10;
            var player2Position = 2;

            var currentDiceRoll = 1;

            while (player1Score < 1000 && player2Score < 1000)
            {
                var player1Rolls = Enumerable.Range(currentDiceRoll, 3).Sum();
                var player1NextSpace = NextSpace(player1Position, player1Rolls);
                player1Score += player1NextSpace;
                player1Position = player1NextSpace;

                if (player1Score >= 1000)
                {
                    currentDiceRoll += 3;
                    break;
                }

                var player2Rolls = Enumerable.Range(currentDiceRoll + 3, 3).Sum();
                var player2NextSpace = NextSpace(player2Position, player2Rolls);
                player2Score += player2NextSpace;
                player2Position = player2NextSpace;

                if (player2Score >= 1000)
                {
                    currentDiceRoll += 3;
                    break;
                }

                currentDiceRoll += 6;
            }

            var losingScore = Math.Min(player1Score, player2Score);

            Console.WriteLine(losingScore * (currentDiceRoll - 1));

            //Part 2

            player1Score = 0;
            player2Score = 0;

            player1Position = 4;
            player2Position = 8;

            var player1Wins = 0l;
            var player2Wins = 0l;


            var sumCombos = Combinations().GroupBy(g => g.Sum()).ToDictionary(i => i.Key, j => j.Count());

            Dictionary<int, long> result = GetResult(player1Position, sumCombos);

            var numberOfWaysToReach21ForPlayer1 = result.Sum(r => r.Value);

            Dictionary<int, long> result2 = GetResult(player2Position, sumCombos);

            var numberOfWaysToReach21ForPlayer2 = result2.Sum(r => r.Value);

            var numberOfPlayer1Wins = 0l;
            foreach (var res in result)
            {
                var gamesWhere1Wins = result.Where(r => r.Key >= res.Key);
                var sumPlayer1 = gamesWhere1Wins.Sum(r => r.Value);
                var gamesWhere1WinsOver2 = result2.Where(r => r.Key >= res.Key);
                var sumPlayer2 = gamesWhere1WinsOver2.Sum(r => r.Value);
                numberOfPlayer1Wins += (sumPlayer1 + sumPlayer2);
            }

            var winningScore = player1Wins > player2Wins ? player1Wins : player2Wins;

            Console.WriteLine(numberOfPlayer1Wins);
        }

        private static Dictionary<int, long> GetResult(int player1Position, Dictionary<int, int> sumCombos)
        {
            var posMap = Enumerable.Range(1, 10).Select(r => r).ToDictionary(i => i, j => 0);
            posMap[player1Position] = 1;

            Dictionary<int, long> sumMap = Enumerable.Range(0, 35).Select(r => r).ToDictionary(i => i, j => 0l);

            var newPosMap = Enumerable.Range(1, 10).Select(r => r).ToDictionary(i => i, j => 0l);
            posMap[player1Position] = 1;
            foreach (var pos in posMap)
            {
                if (pos.Value == 0)
                {
                    continue;
                }
                var numPos = pos.Key;
                foreach (var combo1 in sumCombos)
                {
                    var nextSpace = NextSpace(numPos, combo1.Key);
                    newPosMap[nextSpace] = combo1.Value;
                    sumMap[nextSpace] = combo1.Value;
                }
            }

            var result = new Dictionary<int, long>();
            var counter = 2;

            while (sumMap.Any(r => r.Value > 0))
            {
                var newSumMap = Enumerable.Range(0, 35).Select(r => r).ToDictionary(i => i, j => 0l);
                foreach (var pos in sumMap)
                {
                    if (pos.Value == 0)
                    {
                        continue;
                    }
                    var numPos = pos.Key;
                    var numCount = pos.Value;
                    foreach (var combo1 in sumCombos)
                    {
                        var nextSpace = NextSpace(numPos, combo1.Key);
                        var newScore = numPos + nextSpace;
                        newSumMap[newScore] += (numCount * combo1.Value);
                    }
                }

                var sumOfAllAbove21 = newSumMap.Where(r => r.Key > 20).Sum(l => l.Value);
                result.Add(counter, sumOfAllAbove21);

                sumMap = newSumMap.Where(m => m.Key < 21).ToDictionary(r => r.Key, m => m.Value);
                counter++;
            }

            return result;
        }

        private static List<List<int>> Combinations()
        {
            var combos = new List<List<int>>();

            for (var i = 1; i<4; i++)
            {
                for (var j = 1; j < 4; j++)
                {
                    for (var k = 1; k < 4; k++)
                    {
                        combos.Add(new List<int>() {i, j, k });
                    }
                }
            }

            return combos;
        }

        private static int NextSpace(int currentSpace, int moves)
        {
            var rest = moves % 10;
            var sum = currentSpace + rest;
            var div = sum / 10;
            var sumRest = sum % 10;
            if (sumRest == 0)
            {
                div = div - 1;
            }
            return sum > 10 ? sum-(10*div): sum;
        }
    }
}
