using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day16
    {
        public Day16()
        {
            var hex = File.ReadLines(@"..\..\..\input\day16.txt").First();

            var bytes = GetBytes(hex);

            var versions = new List<int>();
            var shouldContinue = true;
            
            while (shouldContinue)
            {
                var version = byteArrayToNumber(bytes.Take(3).ToArray());
                var typeId = byteArrayToNumber(bytes.Skip(3).Take(3).ToArray());
                versions.Add(version);
                if (typeId == 4)
                {
                    var groups = new List<List<byte>>();
                    var total = 0;
                    var groupSize = 5;
                    var restOfList = bytes.Skip(6).ToList();
                    while (total < restOfList.Count())
                    {
                        var nextGroup = restOfList.Skip(total).Take(groupSize);
                        if (nextGroup.Count() < groupSize)
                        {
                            break;
                        }
                        groups.Add(nextGroup.ToList());
                        total += groupSize;
                    }
                    shouldContinue = false;
                }
                else
                {
                    var lengthTypeId = bytes.Skip(6).Take(1).First();
                    var nextSectionLength = 0;
                    var totalLengthOfBits = 0;
                    var numberOfSubPackesImmediately = 0;
                    if (lengthTypeId == 0)
                    {
                        totalLengthOfBits = 15;
                        nextSectionLength = 15;
                    }
                    else
                    {
                        numberOfSubPackesImmediately = 11;
                        nextSectionLength = 11;
                    }
                    var nextSection = byteArrayToNumber(bytes.Skip(7).Take(nextSectionLength).ToArray());
                    if (lengthTypeId == 0)
                    {

                    }
                    else
                    {

                    }
                }
            }

            Console.WriteLine("");
        
        }

        private static int byteArrayToNumber(byte[] byteArray)
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

        public static List<byte> GetBytes(string hexString)
        {
            var ints = new List<byte>();
            foreach (var ch in hexString)
            {
                switch (ch)
                {
                    case '0':
                        ints.AddRange(new byte[] { 0,0,0,0});
                        break;
                    case '1':
                        ints.AddRange(new byte[] { 0, 0, 0, 1 });
                        break;
                    case '2':
                        ints.AddRange(new byte[] { 0, 0, 1, 0 });
                        break;
                    case '3':
                        ints.AddRange(new byte[] { 0, 0, 1, 1 });
                        break;
                    case '4':
                        ints.AddRange(new byte[] { 0, 1, 0, 0 });
                        break;
                    case '5':
                        ints.AddRange(new byte[] { 0, 1, 0, 1 });
                        break;
                    case '6':
                        ints.AddRange(new byte[] { 0, 1, 1, 0 });
                        break;
                    case '7':
                        ints.AddRange(new byte[] { 0, 1, 1, 1 });
                        break;
                    case '8':
                        ints.AddRange(new byte[] { 1, 0, 0, 0 });
                        break;
                    case '9':
                        ints.AddRange(new byte[] { 1, 0, 0, 1 });
                        break;
                    case 'A':
                        ints.AddRange(new byte[] { 1, 0, 1, 0 });
                        break;
                    case 'B':
                        ints.AddRange(new byte[] { 1, 0, 1, 1 });
                        break;
                    case 'C':
                        ints.AddRange(new byte[] { 1, 1, 0, 0 });
                        break;
                    case 'D':
                        ints.AddRange(new byte[] { 1, 1, 0, 1 });
                        break;
                    case 'E':
                        ints.AddRange(new byte[] { 1, 1, 1, 0 });
                        break;
                    case 'F':
                        ints.AddRange(new byte[] { 1, 1, 1, 1 });
                        break;
                }
            }
            return ints;
        }
    }
}
