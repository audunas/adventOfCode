using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day3
    {
        public Day3()
        {
            Console.WriteLine("Day3");

            var lines = File.ReadLines(@"..\..\..\input\day3.txt");

            var numbers = new List<Number>();
            var symbolPos = new Dictionary<Position, string>();

            var xIndex = 0;

            foreach (var line in lines)
            {
                var currentNumber = "";
                var yInidex = 0;
                foreach (var c in line)
                {
                    if (!int.TryParse(c.ToString(), out int _) && c != '.')
                    {
                        symbolPos.Add( new Position() { x = xIndex, y = yInidex}, c.ToString());
                        if (currentNumber != "")
                        {
                            numbers.Add(new Number()
                            {
                                start = new Position()
                                {
                                    x = xIndex,
                                    y = yInidex - currentNumber.Length,
                                },
                                end = new Position()
                                {
                                    x = xIndex,
                                    y = yInidex - 1
                                },
                                value = int.Parse(currentNumber)
                            });
                            currentNumber = "";
                        }
                        
                    }
                    else if (int.TryParse(c.ToString(), out int _))
                    {
                        currentNumber += c;
                    }

                    else if (currentNumber != "" && c == '.')
                    {
                        numbers.Add(new Number()
                        {
                            start = new Position()
                            {
                                x = xIndex,
                                y = yInidex - currentNumber.Length,
                            },
                            end = new Position()
                            {
                                x = xIndex,
                                y = yInidex - 1
                            },
                            value = int.Parse(currentNumber)
                        });
                        currentNumber = "";
                    }
                    yInidex++;
                }
                if (currentNumber != "")
                {
                    numbers.Add(new Number()
                    {
                        start = new Position()
                        {
                            x = xIndex,
                            y = yInidex - currentNumber.Length,
                        },
                        end = new Position()
                        {
                            x = xIndex,
                            y = yInidex - 1
                        },
                        value = int.Parse(currentNumber)
                    });
                    currentNumber = "";
                }
                xIndex++;
            }

            //Part 1
            var sum = 0;

            foreach (var number in numbers)
            {
                var anyOnRowAbove = symbolPos.Keys.Any(v => number.start.x - 1 == v.x && number.start.y -1 <= v.y && v.y <= number.end.y + 1);
                var anyOnRowBelow = symbolPos.Keys.Any(v => number.start.x + 1 == v.x && number.start.y - 1 <= v.y && v.y <= number.end.y + 1);

                var anyOnSameRow = symbolPos.Keys.Any(v => number.start.x == v.x && number.start.y - 1 <= v.y && v.y <= number.end.y + 1);


                if (anyOnRowAbove || anyOnRowBelow || anyOnSameRow)
                {
                    sum += number.value;
                }
            }

            Console.WriteLine(sum);

            var gearSum = 0;

            foreach (var gear in symbolPos.Where(k => k.Value == "*"))
            {
                var rowAbove = numbers.Where(v => {
                    var isRowAbove = gear.Key.x - 1 == v.start.x;
                    var endIsInRange = gear.Key.y - 1 <= v.end.y && v.end.y <= gear.Key.y + 1;
                    var startIsInRange = gear.Key.y - 1 <= v.start.y && v.start.y <= gear.Key.y + 1;
                    return isRowAbove && (endIsInRange || startIsInRange);
                }).ToList();
                var rowBelow = numbers.Where(v => {
                    var isRowBelow = gear.Key.x + 1 == v.start.x;
                    var endIsInRange = gear.Key.y - 1 <= v.end.y && v.end.y <= gear.Key.y + 1;
                    var startIsInRange = gear.Key.y - 1 <= v.start.y && v.start.y <= gear.Key.y + 1;
                    return isRowBelow && (endIsInRange || startIsInRange);
                }).ToList();

                var sameRow = numbers.Where(v => gear.Key.x == v.start.x && (gear.Key.y - 1 == v.end.y ||  gear.Key.y + 1 == v.start.y)).ToList();


                var numbersCloseToGear = rowAbove.Concat(rowBelow).Concat(sameRow);

                if (numbersCloseToGear.Count() == 2)
                {
                    gearSum += numbersCloseToGear.Select(t => t.value).Aggregate((n, g) => n * g);
                }
            }

            Console.WriteLine(gearSum);

        }

        public class Number
        {
            public Position start;
            public Position end;
            public int value;
            public override string ToString() => $"{value}: {start.x},{start.y} - {end.x},{end.y}";
        }

        public struct Position
        {
            public int x;
            public int y;
        }
    }
}
