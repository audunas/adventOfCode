using AdventOfCode2019;
using NUnit.Framework;
using System.Collections.Generic;
using static AdventOfCode2019.Day8;

namespace AdventOfCode2019Tests
{
    class day8Tests
    {

        [Test]
        public void When_testing_part_1()
        {
            var layers = Day8.GetLayers(new List<int>() { 1,2,3,4,5,6,7,8,9,0,1,2}, 3, 2);
            Assert.That(layers[0].layers.Count, Is.EqualTo(2));
        }

        [Test]
        public void When_testing_part_2()
        {
            var layers = Day8.GetLayers(new List<int>() { 0, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 2, 0, 0, 0, 0 }, 
                2, 2);
            var finalLayer = Day8.GetFinalLayer(layers, 2, 2);
            Assert.That(finalLayer.layers[0][0], Is.EqualTo(0));
        }
    }
}
