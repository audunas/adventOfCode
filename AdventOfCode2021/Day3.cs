using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day3
    {
        public Day3()
        {
            var lines = File.ReadLines(@"..\..\..\input\day3.txt").ToList();

            //Part 1
            var numberOfBits = lines.First().Length;
            var gamma = new byte[numberOfBits];
            var epsilon = new byte[numberOfBits];

            for (int i = 0; i<numberOfBits; i++)
            {
                var numberOfZeroes = 0;
                var numberOfOnes = 0;
                foreach (var line in lines)
                {
                    var v = int.Parse(line[i].ToString());
                    if (v == 0)
                    {
                        numberOfZeroes++;
                    }
                    else if (v == 1)
                    {
                        numberOfOnes++;
                    }
                }
                if (numberOfOnes > numberOfZeroes)
                {
                    gamma[i] = 1;
                    epsilon[i] = 0;
                }
                else
                {
                    gamma[i] = 0;
                    epsilon[i] = 1;
                }
            }

            var gammaValue = byteArrayToDecimal(gamma);
            var epsilonValue = byteArrayToDecimal(epsilon);

            var result = gammaValue * epsilonValue;

            Console.WriteLine(result);

            //Part 2
            var oxygenLine = getLastLine(lines, true, 1).Select(c => byte.Parse(c.ToString())).ToArray();
            var co2scrubber = getLastLine(lines, false, 0).Select(c => byte.Parse(c.ToString())).ToArray();

            var oxValue = byteArrayToDecimal(oxygenLine);
            var co2Value = byteArrayToDecimal(co2scrubber);

            result = oxValue * co2Value;

            Console.WriteLine(result);
        }

        private static string getLastLine(List<string> lines, bool getMostCommon, int numberToKeepIfEqual)
        {
            var numberOfBits = lines.First().Length;
            while (lines.Count > 1)
            {
                for (int i = 0; i < numberOfBits; i++)
                {
                    var numberOfZeroes = 0;
                    var numberOfOnes = 0;
                    foreach (var line in lines)
                    {
                        var v = int.Parse(line[i].ToString());
                        if (v == 0)
                        {
                            numberOfZeroes++;
                        }
                        else if (v == 1)
                        {
                            numberOfOnes++;
                        }
                    }
 
                    var isMostOnes = numberOfOnes > numberOfZeroes;
                    
                    var toKeep = getMostCommon 
                            ? isMostOnes 
                                ? 1 
                                : 0 
                            : isMostOnes 
                                ? 0
                                : 1;
                    var isEqual = numberOfOnes == numberOfZeroes;
                    if (isEqual)
                        toKeep = numberToKeepIfEqual;
                    lines = lines.Where(l => int.Parse(l[i].ToString()) == toKeep).ToList();
                    if (lines.Count == 1)
                    {
                        break;
                    }
                }
            }
            return lines.First();
        }

        private static int byteArrayToDecimal(byte[] byteArray)
        {
            var value = 0;
            var counter = 0;

            var first = byteArray.Length - 1;

            for (int i = first; i >= 0; i--)
            {
                var b = byteArray[i];
                var multiplier = (int)Math.Pow(2, counter);
                var result = b * multiplier;
                value += result;
                counter++;
            }

            return value;
        }
    }
}
