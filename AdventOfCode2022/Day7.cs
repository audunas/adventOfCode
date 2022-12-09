using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day7
    {

        public Day7()
        {
            var lines = File.ReadLines(@"..\..\..\input\day7.txt");

            var allFiles = lines.Where(l => Char.IsDigit(l.First()));
            var directories = new Dictionary<string, List<string>>();
            var fileMap = new Dictionary<string, int>();
            var currentDirectory = "";

            foreach (var line in lines)
            {
                if (line.StartsWith("$"))
                {
                    var splitLine = line.Split(" ");
                    var cmd = splitLine[1];
                    if (cmd == "ls")
                    {

                    }
                    else if (cmd == "cd")
                    {
                        var cmd2 = splitLine[2];
                        if (cmd2 == "..")
                        {
                            var split = currentDirectory.Split("/");
                            var nODirs = split.Length;
                            currentDirectory = string.Join("/", split.Take(nODirs - 1));
                        }
                        else
                        {
                            if (cmd2 != "/")
                            {
                                currentDirectory = currentDirectory + "/" + cmd2;
                            }
                        }
                    }
                }
                else if (line.StartsWith("dir"))
                {
                    var dir = line.Split(" ")[1];
                    if (directories.ContainsKey(currentDirectory))
                    {
                        directories[currentDirectory].Add(dir);
                    }
                    else
                    {
                        directories[currentDirectory] = new List<string>() { dir };
                    }
                }
                else if (Char.IsDigit(line.First())){
                    var fileName = line.Split(" ")[1];
                    var fileSize = int.Parse(line.Split(" ")[0]);
                    if (directories.ContainsKey(currentDirectory))
                    {
                        directories[currentDirectory].Add(fileName);
                    }
                    else
                    {
                        directories[currentDirectory] = new List<string>() { fileName };
                    }
                    fileMap.Add(currentDirectory +"/"+ fileName, fileSize);
                }
            }

            var totalSum = 0;

            var sumPerDir = new Dictionary<string, int>();

            foreach(var dir in directories)
            {
                var sum = getDirSum(directories, fileMap, dir);

                sumPerDir.Add(dir.Key, sum);

                if (sum <= 100000)
                {
                    totalSum += sum;
                }
            }

            // Part 1
            Console.WriteLine(totalSum);

            // Part 2

            var totalDiskSpace = 70000000;
            var neededUnusedSpace = 30000000;

            var totalSpace = sumPerDir.Single(r => r.Key == "").Value;
            var currentUnusedSpace = totalDiskSpace - totalSpace;
            var missingUnusedSpace = neededUnusedSpace - currentUnusedSpace;

            var smallestSum = sumPerDir.Select(r => r.Value).OrderBy(r => r).First(r => r > missingUnusedSpace);

            Console.WriteLine(smallestSum);

        }

        public int getDirSum(Dictionary<string, List<string>> dict, Dictionary<string, int> fileMap, KeyValuePair<string, List<string>> currentDir)
        {
            var fileContent = currentDir.Value.Where(c => fileMap.Any(f => currentDir.Key + "/" + c == f.Key));
            var fileSum = fileContent.Sum(c => fileMap[currentDir.Key + "/" + c]);

            var dirContent = currentDir.Value.Where(c => dict.ContainsKey(currentDir.Key + "/" + c)).ToList();
            if (!dirContent.Any())
            {
                return fileSum;
            }

            var dirSum = 0;
            foreach (var di in dirContent)
            {
                var newList = dict[currentDir.Key + "/" + di];
                dirSum += getDirSum(dict, fileMap, new KeyValuePair<string, List<string>>(currentDir.Key + "/" + di, newList));
            }

            return dirSum + fileSum;
        }
    }
}