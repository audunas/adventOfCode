using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AdventOfCode2021
{
    class Day10
    {
        public Day10()
        {
            var lines = File.ReadLines(@"..\..\..\input\day10.txt");

            var illegalChars = new List<char>();
            var leftChars = new char[] { '{', '[', '(', '<' };
            var righChars = new char[] { '}', ']', ')', '>' };
            var map = new Dictionary<char, char>()
            {
                {'{', '}' },
                {'[', ']' },
                {'(', ')' },
                {'<', '>' }
            };

            var incompleteLines = new Dictionary<string, Stack<char>>();

            foreach (var line in lines)
            {
                var leftCharList = new Stack<char>();
                var hadIllegalChar = false;
                foreach (var letter in line)
                {
                    if (leftChars.Contains(letter))
                    {
                        leftCharList.Push(letter);
                    }
                    else if (righChars.Contains(letter))
                    {
                        var lastLeft = leftCharList.Pop();
                        var expectedRightChar = map[lastLeft];
                        if (expectedRightChar != letter)
                        {
                            illegalChars.Add(letter);
                            hadIllegalChar = true;
                            break;
                        }
                    }
                }
                if (!hadIllegalChar)
                {
                    incompleteLines.Add(line, leftCharList);
                }
            }

            // Part 1

            var sum = 0;
            foreach (var illegalChar in illegalChars)
            {
                if (illegalChar == '}')
                {
                    sum += 1197;
                }
                else if (illegalChar == ']')
                {
                    sum += 57;
                }
                else if (illegalChar == ')')
                {
                    sum += 3;
                }
                else if (illegalChar == '>')
                {
                    sum += 25137;
                }
            }

            Console.WriteLine(sum);

            // Part 2

            var addByLine = new List<char[]>();

            foreach (var incomplete in incompleteLines)
            {
                var stack = incomplete.Value;
                var charArray = new List<char>();
                foreach (var ch in stack)
                {
                    var rightChar = map[ch];
                    charArray.Add(rightChar);
                }
                addByLine.Add(charArray.ToArray());
            }

            var scores = new List<long>();

            foreach (var charLine in addByLine)
            {
                long scoreForLine = 0;
                foreach (var ch in charLine)
                {
                    if (ch == '}')
                    {
                        scoreForLine = (scoreForLine * 5) + 3;
                    }
                    else if (ch == ']')
                    {
                        scoreForLine = (scoreForLine * 5) + 2;
                    }
                    else if (ch == ')')
                    {
                        scoreForLine = (scoreForLine * 5) + 1;
                    }
                    else if (ch == '>')
                    {
                        scoreForLine = (scoreForLine * 5) + 4;
                    }
                }
                scores.Add(scoreForLine);
            }

            var sortedScores = scores.OrderBy(v => v);
            var middle = sortedScores.ElementAt(sortedScores.Count()/2);

            Console.WriteLine(middle);
        }
    }
}
