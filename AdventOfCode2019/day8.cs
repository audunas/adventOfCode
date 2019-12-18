using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day8
    {

        public Day8()
        {
            var lines = File.ReadLines(@"..\..\..\input\day8.txt").First();

            var digits = lines.ToCharArray().Select(c => (int)c - '0').ToList();

            var wide = 25;
            var tall = 6;

            var layers = GetLayers(digits, wide, tall);

            var lowestNumberOfZeros = int.MaxValue;
            var layerWithLowestNumberOfZeros = layers.First();

            foreach (var layer in layers)
            {
                var numOfZeros = numberOfDigitsOfType(layer, 0);

                if (numOfZeros < lowestNumberOfZeros)
                {
                    lowestNumberOfZeros = numOfZeros;
                    layerWithLowestNumberOfZeros = layer;
                }
            }

            var result = numberOfDigitsOfType(layerWithLowestNumberOfZeros, 1) * numberOfDigitsOfType(layerWithLowestNumberOfZeros, 2);

            Console.WriteLine(result);

            //Part2

            var finalLayer = GetFinalLayer(layers, wide, tall);

            foreach (var imageLayer in finalLayer.layers)
            {
                imageLayer.ForEach(t => Console.Write($"{t} "));
                Console.WriteLine();
            }

        }

        public static Layer GetFinalLayer(List<Layer> layers, int wide, int tall)
        {
            var finalLayers = new List<List<int>>();

            for (int i = 0; i < tall; i++)
            {
                var wideLayer = new List<int>();

                for (int j = 0; j < wide; j++)
                {
                    var pixel = 0;
                    foreach (var layer in layers)
                    {
                        pixel = layer.layers[i][j];
                        if (pixel == 0 || pixel == 1)
                        {
                            break;
                        }
                    }

                    wideLayer.Add(pixel);
                }
                finalLayers.Add(wideLayer);
            }

            return new Layer(100, finalLayers);
        }

        public static int numberOfDigitsOfType(Layer layer, int targetDigit)
        {
            return layer.layers.SelectMany(s => s.Where(d => d == targetDigit)).Count();
        }
      
        public static List<Layer> GetLayers(List<int> digits, int wide, int tall)
        {
            var wideCounter = 0;
            var tallCounter = 0;

            var layers = new List<Layer>();
            var digitList = new List<int>();
            var digitLayer = new List<List<int>>();
            var id = 0;

            foreach (var digit in digits)
            {
                wideCounter++;
                digitList.Add(digit);
                if (wideCounter == wide)
                {
                    digitLayer.Add(digitList);
                    digitList = new List<int>();
                    tallCounter++;
                    wideCounter = 0;
                }

                if (tallCounter == tall)
                {
                    var layer = new Layer(id, digitLayer);
                    layers.Add(layer);
                    digitList = new List<int>();
                    digitLayer = new List<List<int>>();
                    tallCounter = 0;
                    id++;
                }
            }

            return layers;
        }


        public class Layer
        {
            public int id;
            public List<List<int>> layers;

            public Layer(int id, List<List<int>> layers)
            {
                this.id = id;
                this.layers = layers;
            }
        }
    }
}
