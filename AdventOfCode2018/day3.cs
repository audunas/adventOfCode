//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace AdventOfCode2018
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var lines = File.ReadLines(@"..\..\..\input\day3.txt").ToList();

//            int squaresWithinTwoOrMoreClaims = 0;
//            int maxWidth = 0;
//            int maxHeight = 0;

//            foreach (var line in lines)
//            {
//                var claim = line.Split(' ');
//                var xpos = int.Parse(claim[2].Split(',')[0]);
//                var ypos = int.Parse(claim[2].Split(',')[1].Remove(claim[2].Split(',')[1].Length - 1));
//                var wide = int.Parse(claim[3].Split('x')[0]);
//                var tall = int.Parse(claim[3].Split('x')[1]);

//                var width = xpos + wide;
//                if (width > maxWidth)
//                {
//                    maxWidth = width;
//                }

//                var height = ypos + tall;
//                if (height > maxHeight)
//                {
//                    maxHeight = height;
//                }
//            }

//            string[,] areas = new string[maxWidth, maxWidth];

//            HashSet<string> idsWithCollisions = new HashSet<string>();
//            HashSet<string> allIds = new HashSet<string>();

//            foreach (var line in lines)
//            {
//                var claim = line.Split(' ');
//                var id = claim[0].Split('#')[1];
//                allIds.Add(id);
//                var xpos = int.Parse(claim[2].Split(',')[0]);
//                var ypos = int.Parse(claim[2].Split(',')[1].Remove(claim[2].Split(',')[1].Length - 1));
//                var wide = int.Parse(claim[3].Split('x')[0]);
//                var tall = int.Parse(claim[3].Split('x')[1]);

//                var width = xpos + wide;
//                for (int i = xpos; i < (xpos + wide); i++)
//                {
//                    for (int j = ypos; j < (ypos + tall); j++)
//                    {
//                        if (areas[i, j] != null)
//                        {
//                            idsWithCollisions.Add(id);
//                            idsWithCollisions.Add(areas[i, j]);
//                            areas[i, j] = "X";
//                        }
//                        else areas[i, j] = id;
//                    }
//                }
//            }

//            for (int k = 0; k < areas.GetLength(0); k++)
//            {
//                for (int l = 0; l < areas.GetLength(1); l++)
//                {
//                    var val = areas[k, l];
//                    if (val != null && val.Equals("X"))
//                    {
//                        squaresWithinTwoOrMoreClaims++;
//                    }
//                }
//            }

//            // Part 1
//            Console.WriteLine(squaresWithinTwoOrMoreClaims);

//            // Part 2 
//            var idWithoutCollision = allIds.Except(idsWithCollisions);
//            Console.WriteLine(idWithoutCollision.First());
//            Console.ReadLine();

//        }
//    }
//}
