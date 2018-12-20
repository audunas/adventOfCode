//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{
//    using System;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Linq;

//    namespace AdventOfCode2018
//    {
//        class Program
//        {
//            static void Main(string[] args)
//            {
//                var lines = File.ReadLines(@"..\..\..\input\day2.txt").ToList();

//                //Part 1

//                var numberOfLinesWithLetterAppearingTwice = 0;
//                var numberOfLinesWithLetterAppearingTrice = 0;

//                foreach (var line in lines)
//                {
//                    var charactersWithCount = line.GroupBy(x => x).Select(x => new { c = x.Key, count = x.Count() });
//                    numberOfLinesWithLetterAppearingTwice += charactersWithCount.Any(x => x.count == 2) ? 1 : 0;
//                    numberOfLinesWithLetterAppearingTrice += charactersWithCount.Any(x => x.count == 3) ? 1 : 0;
//                }

//                Console.WriteLine(numberOfLinesWithLetterAppearingTwice * numberOfLinesWithLetterAppearingTrice);
//                Console.ReadLine();


//                //Part 2

//                for (int i = 0; i < lines.Count; i++)
//                {
//                    for (int j = i + 1; j < lines.Count; j++)
//                    {
//                        var lineA = lines[i];
//                        var lineB = lines[j];

//                        var numOfDifferences = 0;
//                        int diffCharIndex = -1;

//                        for (int k = 0; k < lineA.Length; k++)
//                        {
//                            var lineAChar = lineA[k];
//                            var lineBChar = lineB[k];

//                            if (lineAChar != lineBChar)
//                            {
//                                diffCharIndex = k;
//                                numOfDifferences++;
//                            }

//                            if (numOfDifferences > 1) break;
//                        }
//                        if (numOfDifferences == 1)
//                        {
//                            Console.WriteLine(lineA.Remove(diffCharIndex, 1));
//                            Console.ReadLine();
//                        }
//                    }
//                }
//            }
//        }
//    }

//}
