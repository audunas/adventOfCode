//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace AdventOfCode
//{
//    class day19
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Start");

//            List<string> instr = File.ReadLines(@"..\..\input\day19.txt").ToList();

//            int lineNum = 0;
//            int maxCols = instr[lineNum].Length;
//            int maxRows = instr.Count();

//            int colNum = instr[lineNum].IndexOf("|");
//            lineNum++;
//            var cont = true;
//            string letters = "";
//            var currentDir = '|';
//            var down = true;
//            bool left = false;

//            var alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

//            while (cont)
//            {
//                var l = colNum - 1;
//                var r = colNum + 1;
//                var u = lineNum - 1;
//                var d = lineNum + 1;
//                char lefts, right, up, downwards;
//                if (l > 0)
//                {
//                    lefts = instr[lineNum][l];
//                }
//                if (r <= maxCols)
//                {
//                    right = instr[lineNum][r];
//                }
//                if (u > 0)
//                {
//                    up = instr[u][colNum];
//                }
//                if (d < maxRows)
//                {
//                    downwards = instr[d][colNum];
//                }



//                if (currentDir.Equals('|'))
//                {
//                    if (down)
//                    {
//                        lineNum++;
//                    }
//                    else
//                    {
//                        lineNum--;
//                    }
//                    currentDir = '|';
//                }
//                else if (currentDir.Equals('+'))
//                {
                        
//                            if (alph.Contains(lefts) || lefts.Equals('-'))
//                            {
//                                colNum--;
//                                left = true;
//                            }
                        
                        
//                            if (alph.Contains(right) || right.Equals('-'))
//                            {
//                                colNum++;
//                                left = false;
//                            }
                        
//                        currentDir = '-';
//                                                if (alph.Contains(up) || up.Equals('|'))
//                            {
//                                lineNum--;
//                                down = false;
//                            }
//                        }
                       
//                            if (alph.Contains(downwards) || downwards.Equals('|'))
//                            {
//                                lineNum++;
//                                down = true;
//                            }
//                        }
//                          currentDir = '-';
//                    }
                    
//                }
//                else if (next.Equals('-'))
//                {
//                    if (left)
//                    {
//                        colNum--;
//                    }
//                    else
//                    {
//                        colNum++;
//                    }

//                    currentDir = '-';
//                }
//                else
//                {
//                    if (currentDir.Equals('|'))
//                    {

//                        if (down)
//                        {
//                            lineNum++;
//                        }
//                        else
//                        {
//                            lineNum--;
//                        }
//                    }
//                    else if (currentDir.Equals('-'))
//                    {
//                        if (left)
//                        {
//                            colNum--;
//                        }
//                        else
//                        {
//                            colNum++;
//                        }
//                    }
//                    letters += next;
//                    Console.WriteLine(letters);
//                }
//            }


//            Console.ReadLine();
//        }
//    }
//}
