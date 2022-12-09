using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day2
    {

        public Day2()
        {
            var lines = File.ReadLines(@"..\..\..\input\day2.txt");

            //Part 1
            var sum = 0;

            foreach (var line in lines)
            {
                var plays = line.Split(" ");
                var playerA = plays[0];
                var playerB = plays[1];
                sum += getShapeScore(playerB);

                var result = getResult(playerA, playerB);

                sum += getResultScore(result);
            }
            Console.WriteLine(sum);


            sum = 0;
            //Part 2
            foreach (var line in lines)
            {
                var plays = line.Split(" ");
                var playerA = plays[0];
                var playerB = plays[1];

                var preferredResult = getPreferredResult(playerB);

                var adjustedB = getAdjustedB(playerA, preferredResult);

                sum += getShapeScore(adjustedB);

                var result = getResult(playerA, adjustedB);

                sum += getResultScore(result);
            }

            Console.WriteLine(sum);
        }

        public string getAdjustedB(string player1, Result preferredResult)
        {
            return player1 switch
            {
                //Rock
                "A" => preferredResult switch
                {
                    //Rock
                    Result.Draw => "X",
                    //Paper
                    Result.Player2 => "Y",
                    //Scissors
                    Result.Player1 => "Z",
                    _ => player1,
                },
                //Paper
                "B" => preferredResult switch
                {
                    //Rock
                    Result.Player1 => "X",
                    //Paper
                    Result.Draw => "Y",
                    //Scissors
                    Result.Player2 => "Z",
                    _ => player1,
                },
                //Scissors
                "C" => preferredResult switch
                {
                    //Rock
                    Result.Player2 => "X",
                    //Paper
                    Result.Player1 => "Y",
                    //Scissors
                    Result.Draw => "Z",
                    _ => player1,
                },
                _ => player1,
            };
        }

        public Result getPreferredResult(string playerB)
        {
            return playerB switch
            {
                //Rock
                "X" => Result.Player1,
                //Paper
                "Y" => Result.Draw,
                //Scissors
                "Z" => Result.Player2,
                _ => Result.Draw,
            };
        }

        public int getShapeScore(string shape)
        {
            return shape switch
            {
                //Rock
                "X" => 1,
                //Paper
                "Y" => 2,
                //Scissors
                "Z" => 3,
                _ => 0,
            };
        }

        public int getResultScore(Result result)
        {
            return result switch
            {
                Result.Draw => 3,
                Result.Player2 => 6,
                _ => 0,
            };
        }

        public Result getResult(string player1, string player2)
        {
            return player1 switch
            {
                //Rock
                "A" => player2 switch
                {
                    //Rock
                    "X" => Result.Draw,
                    //Paper
                    "Y" => Result.Player2,
                    //Scissors
                    "Z" => Result.Player1,
                    _ => Result.Draw,
                },
                //Paper
                "B" => player2 switch
                {
                    //Rock
                    "X" => Result.Player1,
                    //Paper
                    "Y" => Result.Draw,
                    //Scissors
                    "Z" => Result.Player2,
                    _ => Result.Draw,
                },
                //Scissors
                "C" => player2 switch
                {
                    //Rock
                    "X" => Result.Player2,
                    //Paper
                    "Y" => Result.Player1,
                    //Scissors
                    "Z" => Result.Draw,
                    _ => Result.Draw,
                },
                _ => Result.Draw,
            };
        }

        public enum Result
        {
            Player1,
            Player2,
            Draw
        }
    }
}