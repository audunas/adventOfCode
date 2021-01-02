using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Day18
    {
        public Day18()
        {
            var lines = File.ReadLines(@"..\..\input\day18.txt").ToList();
            var sum = lines.Sum(l => GetValue(new string(l.Where(r => !char.IsWhiteSpace(r)).ToArray())));
            Console.WriteLine(sum);
        }

        private static long GetValue(string valuePart)
        {
            if (!(valuePart.Contains('(') || valuePart.Contains(')')))
            {
                // Part 1
                //return GetValueOfStringWithoutParenthesis(valuePart);
                // Part 2
                return GetValueOfStringWithoutParenthesisAddFirst(valuePart);
            }

            var parts = SplitIntoParts(valuePart);

            GetValuesAndOperators(parts, out List<long> values, out List<char> operators);

            // Part 1
            //var sum = values.First();
            //for (var i = 1; i < values.Count(); i++)
            //{
            //    var firstValue = values.ElementAt(i);
            //    var op = operators.ElementAt(i - 1);
            //    sum = op == '*'
            //        ? sum *= firstValue
            //        : sum += firstValue;
            //}

            // Part 2
            var valueString = string.Empty;

            for(var i = 0; i< values.Count() - 1; i++)
            {
                var value = values.ElementAt(i);
                var op = operators.ElementAt(i);
                valueString += value;
                valueString += op;
            }
            valueString += values.Last();

            return GetValueOfStringWithoutParenthesisAddFirst(valueString);

        }

        private static void GetValuesAndOperators(List<string> parts, out List<long> values, out List<char> operators)
        {
            values = new List<long>();
            operators = new List<char>();
            foreach (var part in parts)
            {
                var lastLetter = part.Last();
                var vp = part;
                if (new List<char> { '+', '*' }.Contains(lastLetter))
                {
                    operators.Add(lastLetter);
                    vp = new string(part.Take(part.Length - 1).ToArray());
                }
                if (vp.EndsWith(")"))
                {
                    vp = vp.Substring(0, vp.Length - 1);
                }
                if (vp.StartsWith("("))
                {
                    vp = vp.Substring(1);
                }
                values.Add(GetValue(vp));
            }
        }

        private static List<string> SplitIntoParts(string valuePart)
        {
            var numberOfLeftParenthesis = 0;
            var numberOfRightParenthesis = 0;
            var currentPart = string.Empty;
            var parts = new List<string>();
            var skipAdding = false;
            for (var i = 0; i < valuePart.Length; i++)
            {
                if (skipAdding)
                {
                    skipAdding = false;
                    continue;
                }
                skipAdding = false;
                var h = valuePart.ElementAt(i);
                if (char.IsDigit(h) && numberOfLeftParenthesis == 0 && numberOfRightParenthesis == 0 && i > 0
                    && currentPart != "")
                {
                    parts.Add(currentPart);
                    currentPart = string.Empty;
                }
                if (h == '(' && numberOfLeftParenthesis == 0)
                {
                    if (currentPart != "")
                    {
                        parts.Add(currentPart);
                    }
                    currentPart = string.Empty;
                    numberOfLeftParenthesis = 1;
                    numberOfRightParenthesis = 0;
                }
                else if (h == ')' && numberOfLeftParenthesis == (numberOfRightParenthesis + 1))
                {
                    if (valuePart.Length > i + 1)
                    {
                        currentPart = currentPart + h + valuePart.ElementAt(i + 1);
                        skipAdding = true;
                    }
                    else
                    {
                        currentPart += h;
                    }
                    parts.Add(currentPart);
                    currentPart = string.Empty;
                    numberOfLeftParenthesis = 0;
                    numberOfRightParenthesis = 0;
                }

                else if (h == '(')
                {
                    numberOfLeftParenthesis++;
                }

                else if (h == ')')
                {
                    numberOfRightParenthesis++;
                }

                if (!skipAdding)
                {
                    currentPart += h;
                }

            }
            if (valuePart.Last() != ')')
            {
                parts.Add(currentPart);
            }

            return parts;
        }

        private static long GetValueOfStringWithoutParenthesisAddFirst(string valuePart)
        {
            var addIndices = valuePart.Select((i, index) =>
            {
                if (i == '+')
                    return index;
                return int.MaxValue;
            }).Where(r => r != int.MaxValue);

            var tempValuePart = new string(valuePart.ToCharArray());

            while (addIndices.Any())
            {
                var addIndex = addIndices.First();
                var reverseStartIndex = tempValuePart.Length - addIndex;
                var firstValue = long.Parse(new string(tempValuePart.Reverse().Skip(reverseStartIndex).TakeWhile(r => char.IsDigit(r)).Reverse().ToArray()));
                var secondValue = long.Parse(new string(tempValuePart.Skip(addIndex + 1).TakeWhile(r => char.IsDigit(r)).ToArray()));
                var addSum = firstValue + secondValue;
                var sumLength = firstValue.ToString().Length + secondValue.ToString().Length;
                tempValuePart = tempValuePart.Remove(addIndex - firstValue.ToString().Length, 1 + sumLength);
                tempValuePart = tempValuePart.Insert(addIndex - firstValue.ToString().Length, addSum.ToString());

                addIndices = tempValuePart.Select((i, index) =>
                {
                    if (i == '+')
                        return index;
                    return int.MaxValue;
                }).Where(r => r != int.MaxValue);
            }

            return tempValuePart.Split('*').Aggregate(1l, (a, b) => a*long.Parse(b));
        }


        private static long GetValueOfStringWithoutParenthesis(string valuePart)
        {
            var sum = 0l;
            var opera = string.Empty;
            foreach (var c in valuePart.Trim())
            {
                if (c == ' ')
                {
                    continue;
                }
                if (char.IsDigit(c))
                {
                    var cValue = long.Parse(c.ToString());
                    if (opera != string.Empty)
                    {
                        switch (opera)
                        {
                            case "*":
                                sum *= cValue;
                                break;
                            case "+":
                                sum += cValue;
                                break;
                        }
                    }
                    else
                    {
                        sum = cValue;
                    }
                }
                else if (c == '*')
                {
                    opera = "*";
                }
                else if (c == '+')
                {
                    opera = "+";
                }

            }
            return sum;
        }
    }
}
