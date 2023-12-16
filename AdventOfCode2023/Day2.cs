using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2023
{
    public class Day2
    {

        public Day2()
        {
            Console.WriteLine("Day2");

            var lines = File.ReadLines(@"..\..\..\input\day2.txt");


            const int MaxRed = 12;
            const int MaxGreen = 13;
            const int MaxBlue = 14;

            const string red = "red";
            const string blue = "blue";
            const string green = "green";

            // Part 1
            var sumOfGameIds = 0;

            foreach (var line in lines)
            {
                var split1 = line.Split(':');
                var gameId = int.Parse(split1[0].Split(' ')[1]);

                var valid = true;

                var sets = split1[1].Split(";");
                
                foreach (var set in sets)
                {
                    var cubeSet = set.Split(',');
                    foreach(var cube in cubeSet)
                    {
                        var split2 = cube.Trim().Split(" ");
                        var color = split2[1].Trim();
                        var number = int.Parse(split2[0].Trim());

                        if (color == red && number > MaxRed)
                        {
                            valid = false; 
                            break;
                        }
                        else if (color == green && number > MaxGreen) { valid = false; break; }
                        else if (color == blue && number > MaxBlue) { valid = false; break; }
                    }

                }


                if (valid)
                {
                    sumOfGameIds += gameId;
                }
            }


            Console.WriteLine(sumOfGameIds);

            //Part 2

            var totalSum = 0;

            foreach (var line in lines)
            {
                var maxBlue = int.MinValue;
                var maxRed = int.MinValue;
                var maxGreen = int.MinValue;

                var split1 = line.Split(':');
                var gameId = int.Parse(split1[0].Split(' ')[1]);

                var sets = split1[1].Split(";");

                foreach (var set in sets)
                {
                    var cubeSet = set.Split(',');
                    foreach (var cube in cubeSet)
                    {
                        var split2 = cube.Trim().Split(" ");
                        var color = split2[1].Trim();
                        var number = int.Parse(split2[0].Trim());

                        if (color == red && number > maxRed)
                        {
                            maxRed = number;
                        }
                        else if (color == green && number > maxGreen) 
                        { 
                            maxGreen = number;
                        }
                        else if (color == blue && number > maxBlue) 
                        { 
                            maxBlue = number;
                        }
                    }

                }

                var power = maxBlue * maxGreen * maxRed;

                totalSum += power;

            }

            Console.WriteLine(totalSum);
        }
    }
}
