using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

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

                var player2Rolls = Enumerable.Range(currentDiceRoll+3, 3).Sum();
                var player2NextSpace = NextSpace(player2Position, player2Rolls);
                player2Score += player2NextSpace;
                player2Position = player2NextSpace;

                currentDiceRoll += 6;
            }

            var losingScore = Math.Min(player1Score, player2Score);

            Console.WriteLine(losingScore * (currentDiceRoll-1));

            //Part 2

            player1Score = 0;
            player2Score = 0;

            player1Position = 4;
            player2Position = 8;

            var player1Wins = 0l;
            var player2Wins = 0l;

            while (player1Score < 21 && player2Score < 21)
            {
                var allCombos = Combinations();

                foreach (var combo1 in allCombos)
                {
                    foreach (var combo2 in allCombos)
                    {
                        var player1Rolls = combo1.Sum();
                        var player1NextSpace = NextSpace(player1Position, player1Rolls);
                        player1Score += player1NextSpace;
                        player1Position = player1NextSpace;

                        if (player1Score >= 21)
                        {
                            player1Wins++;
                            break;
                        }

                        var player2Rolls = combo2.Sum();
                        var player2NextSpace = NextSpace(player2Position, player2Rolls);
                        player2Score += player2NextSpace;
                        player2Position = player2NextSpace;

                        if (player1Score >= 21)
                        {
                            player2Wins++;
                            break;
                        }

                    }
                }
            }

            var winningScore = player1Wins > player2Wins ? player1Wins : player2Wins;

            Console.WriteLine(winningScore);
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
            return sum > 10 ? sum-10: sum;
        }
    }
}
