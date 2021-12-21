using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day4
    {
        public Day4()
        {
            var lines = File.ReadLines(@"..\..\..\input\day4.txt");

            var bingoNumbers = lines.First().Split(',').Select(n => int.Parse(n));

            var boards = new List<List<int[]>>();

            var currentBoard = new List<int[]>();

            foreach (var line in lines.Skip(1))
            {
                if (line == "")
                {
                    if (currentBoard.Count > 0)
                    {
                        boards.Add(currentBoard);
                        currentBoard = new List<int[]>();
                    }
                    continue;
                }
                var split = line.Split(' ').Where(s => s != ""); ;
                var boardNumbers = split.Select(n => int.Parse(n.Trim())).ToArray();
                currentBoard.Add(boardNumbers);
            }

            boards.Add(currentBoard);

            //Part 1
            var checkedNumbers = new List<int>();
            var winningBoard = new List<int[]>();

            foreach (var bingoNumber in bingoNumbers)
            {
                checkedNumbers.Add(bingoNumber);
                var possibleWinningBoards = CheckForWinningBoard(boards, checkedNumbers);
                if (possibleWinningBoards.Count > 0)
                {
                    winningBoard = possibleWinningBoards.First();
                    break;
                }
            }

            var sumOfUnmarkedNumbers = winningBoard.Sum(row => row.Where(r => !checkedNumbers.Contains(r)).Sum());
            var lastNumber = checkedNumbers.Last();
            var score = sumOfUnmarkedNumbers * lastNumber;

            Console.WriteLine(score);

            //Part 2
            checkedNumbers = new List<int>();
            winningBoard = new List<int[]>();

            foreach (var bingoNumber in bingoNumbers)
            {
                checkedNumbers.Add(bingoNumber);
                var possibleWinningBoards = CheckForWinningBoard(boards, checkedNumbers);
                if (possibleWinningBoards.Count > 0)
                {
                    if (boards.Count == 1)
                    {
                        winningBoard = possibleWinningBoards.First();
                        break;
                    }
                    possibleWinningBoards.ForEach(board => boards.Remove(board));
                }
            }

            sumOfUnmarkedNumbers = winningBoard.Sum(row => row.Where(r => !checkedNumbers.Contains(r)).Sum());
            lastNumber = checkedNumbers.Last();
            score = sumOfUnmarkedNumbers * lastNumber;

            Console.WriteLine(score);
        }

        private static List<List<int[]>> CheckForWinningBoard(List<List<int[]>> allBoards, List<int> numbers)
        {
            var boardsWithBingo = new List<List<int[]>>();
            foreach (var board in allBoards)
            {
                var hasBingo = isBingo(board, numbers);
                if (hasBingo)
                {
                    boardsWithBingo.Add(board);
                }
            }
            return boardsWithBingo;
        }

        private static bool isBingo(List<int[]> board, List<int> numbers)
        {
            foreach (var row in board)
            {
                var hasAll = row.All(numbers.Contains);
                if (hasAll) return true;
            }
            for (int i = 0; i < board.First().Length; i++)
            {
                var columnNumbers = board.Select(b => b[i]);
                var hasAll = columnNumbers.All(numbers.Contains);
                if (hasAll) return true;
            }

            return false;
        }
    }
}
