using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    class Day14
    {
        public Day14()
        {
            var lines = File.ReadLines(@"..\..\input\day14.txt").ToList();


            //Part 1
            var mask = "";
            var memory = new Dictionary<long, string>();

            foreach (var line in lines)
            {
                var splitted = line.Split('=');
                if (splitted[0].Trim() == "mask")
                {
                    mask = splitted[1].Trim();
                }
                else
                {
                    var r = splitted[0].Split(new[] { "mem" }, StringSplitOptions.None)[1].Trim();
                    var loc = r.TrimStart('[').TrimEnd(']');
                    var memLocation = int.Parse(loc);
                    var value = long.Parse(splitted[1].Trim());
                    var bitValue = GetBitValueFromValue(value);
                    bitValue = applyBitMask(bitValue, mask);
                    memory[memLocation] = bitValue;
                }
               
            }

            var sum = memory.Sum(m => GetValueFromBitValue(m.Value));

            //Console.WriteLine(sum);

            // Part 2

            mask = "";
            memory = new Dictionary<long, string>();

            foreach (var line in lines)
            {
                var splitted = line.Split('=');
                if (splitted[0].Trim() == "mask")
                {
                    mask = splitted[1].Trim();
                }
                else
                {
                    var r = splitted[0].Split(new[] { "mem" }, StringSplitOptions.None)[1].Trim();
                    var loc = r.TrimStart('[').TrimEnd(']');
                    var memLocation = int.Parse(loc);
                    var value = long.Parse(splitted[1].Trim());
                    var bitValue = GetBitValueFromValue(value);
                    var memLocBitValue = GetBitValueFromValue(memLocation);
                    var memoryAddresses = applyBitMaskPart2(memLocBitValue, mask);
                    foreach (var mem in memoryAddresses)
                    {
                        var memLocation2 = GetValueFromBitValue(mem);
                        memory[memLocation2] = bitValue;
                    }
                }

            }

            sum = memory.Sum(m => GetValueFromBitValue(m.Value));

            Console.WriteLine(sum);


        }

        private List<string> applyBitMaskPart2(string memoryAddress, string mask)
        {
            var memoryAddresses = new Dictionary<int, string>();
            for (var i = 0; i < mask.Length; i++)
            {
                var c = mask[i];
                switch (c)
                {
                    case '0':
                        break;
                    case '1':
                        memoryAddress = memoryAddress.Remove(i, 1).Insert(i, c.ToString());
                        break;
                    case 'X':
                        memoryAddress = memoryAddress.Remove(i, 1).Insert(i, c.ToString());
                        break;
                    default:
                        throw new Exception("invalid character!");
                }
            }
            var numberOfFloatingOnes = memoryAddress.Where(r => r == 'X');
            var currentMems = new List<string>();
            for (var j = 0; j < memoryAddress.Length; j++)
            {
                var ch = memoryAddress[j];
                if (ch == 'X')
                {
                    if (currentMems.Any())
                    {
                        var tempMems = new List<string>();
                        foreach (var mem in currentMems)
                        {
                            var withZero = mem.Remove(j, 1).Insert(j, "0");
                            var withOne = mem.Remove(j, 1).Insert(j, "1");
                            tempMems.Add(withZero);
                            tempMems.Add(withOne);
                        }
                        currentMems.AddRange(tempMems);
                    }
                    else
                    {
                        var withZero = memoryAddress.Remove(j, 1).Insert(j, "0");
                        var withOne = memoryAddress.Remove(j, 1).Insert(j, "1");
                        currentMems.Add(withZero);
                        currentMems.Add(withOne);
                    }
                }
            }
            var memWithoutXes = currentMems.Where(k => !k.Any(j => j == 'X'));

            return memWithoutXes.ToList();
        }

        private string applyBitMask(string bitValue, string mask)
        {
            for (var i = 0; i< mask.Length; i++)
            {
                var c = mask[i];
                if (c != 'X')
                {
                    bitValue = bitValue.Remove(i, 1).Insert(i, c.ToString());
                }
            }
            return bitValue;
        }

        private string GetBitValueFromValue(long value)
        {
            var bitValue = Convert.ToString(value, 2);
            if (bitValue.Length < 36)
            {
                var leadingZeros = string.Concat(Enumerable.Repeat("0", 36 - bitValue.Length));
                return leadingZeros + bitValue;
            }
            return bitValue;
        }

        private long GetValueFromBitValue(string value)
        {
            return Convert.ToInt64(value, 2);
        }
    }
}
