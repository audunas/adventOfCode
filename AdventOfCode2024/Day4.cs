using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day4
    {

        public Day4()
        {
            Console.WriteLine("Day4");

            var lines = File.ReadLines(@"..\..\..\input\day4.txt").ToArray();

            var coo = new string[lines.Count(), lines[0].Length];

            var verticalLines = new Dictionary<int, string>();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    coo[i,j] = lines[i][j].ToString();
                    if (verticalLines.ContainsKey(j))
                    {
                        verticalLines[j] = verticalLines[j] + lines[i][j].ToString();
                    }
                    else
                    {
                        verticalLines.Add(j, lines[i][j].ToString());
                    }
                }
            }

            var sum = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (i + 3 < lines.Length && j + 3 < lines[i].Length)
                    {
                        var word = coo[i, j] + coo[i + 1, j + 1] + coo[i + 2, j + 2] + coo[i + 3, j + 3];
                        if (word == "XMAS" || word == "SAMX")
                        {
                            sum++; 
                        }
                    }
                    if (i + 3 < lines.Length && j - 3 > -1)
                    {
                        var word = coo[i, j] + coo[i + 1, j -1] + coo[i + 2, j - 2] + coo[i + 3, j - 3];
                        if (word == "XMAS" || word == "SAMX")
                        {
                            sum++;
                        }
                    }
                }
            }

            

            foreach (var line in lines)
            {
                var xmasMatches = Regex.Matches(line, "XMAS");
                var samxMatches = Regex.Matches(line, "SAMX");
                sum += xmasMatches.Count;
                sum += samxMatches.Count;
            }

            foreach (var line in verticalLines)
            {
                var xmasMatches = Regex.Matches(line.Value, "XMAS");
                var samxMatches = Regex.Matches(line.Value, "SAMX");
                sum += xmasMatches.Count;
                sum += samxMatches.Count;
            }

            Console.WriteLine(sum);

            //Part 2
            sum = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if ((i + 2 < lines.Length && j + 2 < lines[i].Length))
                    {
                        var word1 = coo[i, j] + coo[i + 1, j + 1] + coo[i + 2, j + 2];
                        var word2 = coo[i, j + 2] + coo[i + 1, j + 1] + coo[i + 2, j];
                        if ((word1 == "MAS" || word1 == "SAM") &&
                            (word2 == "MAS" || word2 == "SAM"))
                        {
                            sum++;
                        }
                    }
                }
            }

            Console.WriteLine(sum);
        }
    }
}
