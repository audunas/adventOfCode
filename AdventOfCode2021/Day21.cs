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

            currentDiceRoll = 1;

            while (player1Score < 21 && player2Score < 21)
            {
                var player1Rolls = Enumerable.Range(currentDiceRoll, 3).Sum();
                var player1NextSpace = NextSpace(player1Position, player1Rolls);
                player1Score += player1NextSpace;
                player1Position = player1NextSpace;

                if (player1Score >= 21)
                {
                    currentDiceRoll += 3;
                    break;
                }

                var player2Rolls = Enumerable.Range(currentDiceRoll + 3, 3).Sum();
                var player2NextSpace = NextSpace(player2Position, player2Rolls);
                player2Score += player2NextSpace;
                player2Position = player2NextSpace;

                currentDiceRoll += 6;
            }

            losingScore = Math.Min(player1Score, player2Score);

            Console.WriteLine(losingScore * (currentDiceRoll - 1));
        }

        private static int NextSpace(int currentSpace, int moves)
        {
            var rest = moves % 10;
            var sum = currentSpace + rest;
            return sum > 10 ? sum-10: sum;
        }
    }
}
