using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Program
    {
        static Register regs = new Register();

        //static void Main(string[] args)
        //{
        //    List<string> instr = File.ReadLines(@"..\..\input\day18.txt").ToList();

        //    int counter = 0;
            

        //    while (counter >= 0 && counter < instr.Count()-1)
        //    {
        //        var ins = instr.ElementAt(counter);
        //        string[] arguments = ins.Split(null);
        //        var op = arguments[0];
        //        var firstArg = arguments[1];
        //        int secondArg;
        //        if (arguments.Length > 2)
        //        {
        //            int res;
        //            var parseable = Int32.TryParse(arguments[2], out res);
        //            secondArg = res;
        //        }
        //        else
        //        {
        //            secondArg = int.MaxValue;
        //        }

        //        var result = doArg(op, firstArg, secondArg);
        //        if (result != int.MaxValue && result != int.MinValue)
        //        {
        //            counter += (int) result;
        //        }
               
        //        else
        //        {
        //            counter++;
        //        }
        //    }

        //}

        public static BigInteger doArg(string op, string firstArg, int secArg )
        {
            switch (op)
            {
                case "mul":
                    regs.Mul(firstArg, secArg);
                    return int.MaxValue;
                    break;
                case "add":
                    regs.Add(firstArg, secArg);
                    return int.MaxValue;
                    break;
                case "jgz":
                    return regs.Jgz(firstArg, secArg);
                    break;
                case "set":
                    regs.Set(firstArg, secArg);
                    return int.MaxValue;
                    break;
                case "mod":
                    if (secArg != 0)
                    {
                       regs.Mod(firstArg, secArg);
                    }
                    
                    return int.MaxValue;
                    break;
                case "snd":
                    regs.Snd(firstArg);
                    return int.MaxValue;
                    break;
                case "rcv":
                    regs.Rcv(firstArg);
                    return int.MaxValue;
                    break;
                default:
                    return int.MaxValue;
                    break;
            }
        }
    }
}
