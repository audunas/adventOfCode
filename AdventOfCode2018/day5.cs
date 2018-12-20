//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var line = File.ReadLines(@"..\..\..\input\day5.txt").ToList()[0];

//            var uniqueLetters = line.GroupBy(x => x);

//            var shortestLength = int.MaxValue;

//            foreach (var letter in uniqueLetters)
//            {
//                var lineWithoutLetter = line.Replace(letter.Key.ToString().ToLower(), "").Replace(letter.Key.ToString().ToUpper(), "");

//                var lengthOfLine = getLengthOfReducedLine(lineWithoutLetter);

//                if (lengthOfLine < shortestLength)
//                {
//                    shortestLength = lengthOfLine;
//                }
//            }

//            Console.WriteLine(shortestLength);
//            Console.ReadLine();
//        }

//        public static int getLengthOfReducedLine(string line)
//        {
//            var upperLine = line.ToUpper();
//            var moreToRemove = true;

//            while (moreToRemove)
//            {
//                List<int> indices = new List<int>();
//                for (int i = 1; i < line.Length; i++)
//                {
//                    if (upperLine[i] == upperLine[i - 1] && line[i] != line[i - 1])
//                    {
//                        if (!indices.Contains(i - 2))
//                        {
//                            indices.Add(i - 1);
//                        }
//                    }
//                }
//                foreach (var index in indices.OrderByDescending(i => i))
//                {
//                    line = line.Remove(index, 2);
//                    upperLine = upperLine.Remove(index, 2);
//                }
//                moreToRemove = indices.Any();
//            }

//            return line.Length;
//        }

//    }
//}
