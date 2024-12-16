using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Day9
    {

        public Day9()
        {
            Console.WriteLine("Day9");

            var line = File.ReadLines(@"..\..\..\input\day9.txt").First();

            var isFile = true;

            var fileMap = new Dictionary<int, int>(); //index:value
            var valueToLengthMap = new List<(int, int)>();
            var spaceBlocks = new Dictionary<int, int>();
            var fileSpaceIndices = new List<int>();
            var freeSpace = new List<int>();

            var currentIndex = 0;

            var id = 0;

            foreach (var c in line)
            {
                var value = int.Parse(c.ToString());

                if (isFile)
                {
                    valueToLengthMap.Add((id, value));
                    foreach (var item in Enumerable.Range(0, value))
                    {
                        fileMap.Add(currentIndex, id);
                        fileSpaceIndices.Add(currentIndex);
                        currentIndex++;
                    }
                    id++;
                }
                else
                {
                    if (value > 0)
                    {
                        spaceBlocks.Add(currentIndex, value);
                        foreach (var item in Enumerable.Range(0, value))
                        {
                            freeSpace.Add(currentIndex);
                            currentIndex++;
                        }
                    }
                }

                isFile = !isFile;
            }

            var resultMap = new List<int>();

            

            var currentFileSpaceIndex = 0;
            fileSpaceIndices.Reverse();

            for (int i = 0; i<line.Length + freeSpace.Count; i++)
            {
                if (fileMap.ContainsKey(i))
                {
                    resultMap.Add(fileMap[i]);
                }
                else
                {
                    resultMap.Add(fileMap[fileSpaceIndices[currentFileSpaceIndex]]);
                    currentFileSpaceIndex++;
                }
                if (i >= fileSpaceIndices[currentFileSpaceIndex])
                {
                    break;
                }
            }

            var sum = 0l;

            for (int i = 0; i<resultMap.Count(); i++)
            {
                sum += (i * resultMap[i]);
            }

            Console.WriteLine(sum);

            var resultMap2 = new Dictionary<int, int>();

            var map = fileMap.Where(f => f.Value == 0);

            foreach (var m in map)
            {
                resultMap2[m.Key] = m.Value;
            }

            valueToLengthMap.Reverse();

            valueToLengthMap = valueToLengthMap.Where(v => v.Item1 != 0).ToList();

            foreach (var v in valueToLengthMap)
            {
                var firstIndex = fileMap.First(f => f.Value == v.Item1).Key;

                var f = spaceBlocks.Where(s => s.Value >= v.Item2 &&
                    firstIndex > s.Key
                );

                if (f.Any())
                {
                    var firstSpace = f.First();

                    for (int i = firstSpace.Key; i < firstSpace.Key + v.Item2; i++)
                    {
                        if (resultMap2.ContainsKey(i))
                        {
                            var err = 0;
                        }
                        resultMap2[i] = v.Item1;
                    }
                    spaceBlocks.Remove(firstSpace.Key);

                    var spaceLeft = firstSpace.Value - v.Item2;
                    if (spaceLeft > 0)
                    {
                        spaceBlocks.Add(firstSpace.Key + v.Item2, spaceLeft);
                        spaceBlocks = spaceBlocks.OrderBy(s => s.Key).ToDictionary();
                    }
                }
                else
                {
                    map = fileMap.Where(f => f.Value == v.Item1);

                    foreach (var m in map)
                    {
                        if (resultMap2.ContainsKey(m.Key))
                        {
                            var err = 0;
                        }
                        resultMap2[m.Key] = m.Value;
                    }
                }
            }

            sum = 0l;

            foreach (var m in resultMap2)
            {
                sum += (m.Key * m.Value);
            }

            Console.WriteLine(sum);

        }
    }
}
