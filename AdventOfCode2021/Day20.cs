using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    class Day20
    {
        public Day20()
        {
            var lines = File.ReadLines(@"..\..\..\input\day20.txt");

            var algorithm = lines.First().Select(pix => pix == '#' ? (byte)1 : (byte)0).ToList();
            var infintyNumber = (byte)0;

            var image = lines.Skip(2).Select(l => l.Select(pixel => pixel == '#' ? (byte)1 : (byte)0).ToArray()).ToArray();
            var imageWithSpace = addEmptySpaceAround(image, infintyNumber, true);
            //Part 1 = 2, Part 2 = 50
            var numberOfSteps = 50;
            while (numberOfSteps > 0)
            {
                print(imageWithSpace);
                var newImage = new byte[imageWithSpace.Length][];
                newImage[0] = imageWithSpace[0];
                for (var i = 1; i< imageWithSpace.Length-1; i++)
                {
                    newImage[i] = new byte[imageWithSpace[i].Length];
                    for (var j = 1; j < imageWithSpace[i].Length-1; j++)
                    {
                        var byteArray = new List<byte>()
                        {
                            imageWithSpace[i-1][j-1],
                            imageWithSpace[i-1][j],
                            imageWithSpace[i-1][j+1],
                            imageWithSpace[i][j-1],
                            imageWithSpace[i][j],
                            imageWithSpace[i][j+1],
                            imageWithSpace[i+1][j-1],
                            imageWithSpace[i+1][j],
                            imageWithSpace[i+1][j+1]
                        }.ToArray();
                        var number = byteArrayToNumber(byteArray);
                        var algoByte = algorithm.ElementAt(number);
                        newImage[i][j] = algoByte;
                    }
                }
                newImage[imageWithSpace.Length-1] = imageWithSpace[imageWithSpace.Length-1];
                image = newImage;
                infintyNumber = infintyNumber == (byte)1 ? (byte)0 : (byte)1;
                imageWithSpace = addEmptySpaceAround(image, infintyNumber, false);
                numberOfSteps--;
                
            }

            print(imageWithSpace);
            Console.WriteLine(imageWithSpace.SelectMany(l => l.Where(l => l == 1)).Count());

        }

        private static void print(byte[][] image)
        {
            var lines = new List<string>();
            foreach (var row in image)
            {
                lines.Add(string.Join("", row.Select(p => p == 1 ? '#' : '.')));
                //Console.WriteLine(string.Join("", row.Select(p => p == 1 ? '#' : '.')));
            }
            lines.Add("");
            //File.AppendAllLines("../../../input/day20output.txt", lines);
            //Console.WriteLine();
        }

        private static byte[][] addEmptySpaceAround(byte[][] image, byte infityNumber,  bool addTwoRows = true)
        {
            var newImage = new List<List<byte>>();
            var imageWidth = image[0].Length;
            var imageHeight = image.Length;

            var newWidth = addTwoRows ? imageWidth + 4 : imageWidth + 2;
            var emptyRow = Enumerable.Range(0, newWidth).Select(r => infityNumber).ToList();
            var infintyBytes = new byte[] { 0, 0 };
            infintyBytes = infityNumber == (byte)1 ? new byte[] { 1, 1 } : infintyBytes;

            newImage.Add(emptyRow);
            //if (addTwoRows)
            //{
                newImage.Add(emptyRow);
            //}

            var imageToUpdate = addTwoRows ? image.ToList() : image.Skip(1).Take(image.Length-2).ToList();

            foreach (var row in imageToUpdate)
            {
                var newRow = addTwoRows 
                    ? infintyBytes.Concat(row).Concat(infintyBytes).ToList()
                    : infintyBytes.Concat(row.Skip(1).Take(row.Length-2)).Concat(infintyBytes).ToList();
                newImage.Add(newRow);
            }

            newImage.Add(emptyRow);
            //if (addTwoRows)
            //{
                newImage.Add(emptyRow);
            //}

            return newImage.Select(i => i.ToArray()).ToArray();
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
    }
}
