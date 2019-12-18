using AdventOfCode2019;
using NUnit.Framework;
using System.Collections.Generic;

namespace AdventOfCode2019Tests
{
    class day10Tests
    {

        [Test]
        public void When_testing_part_1()
        {
            var map = new List<List<int>>()
            {
                new List<int>(){0,1,0,0,1},
                new List<int>(){0,0,0,0,0},
                new List<int>(){1,1,1,1,1},
                new List<int>(){0,0,0,0,1},
                new List<int>(){0,0,0,1,1},
            };

            var (locX, locY, numOfAsteroids) = Day10.GetBestLocation(map);
            Assert.That(locX, Is.EqualTo(3));
            Assert.That(locY, Is.EqualTo(4));
            Assert.That(numOfAsteroids, Is.EqualTo(8));
        }

        [Test]
        public void When_testing_part_1_ex_2()
        {
            var map = new List<List<int>>()
            {
                new List<int>(){0,0,0,0,0,0,1,0,1,0},
                new List<int>(){1,0,0,1,0,1,0,0,0,0},
                new List<int>(){0,0,1,1,1,1,1,1,1,0},
                new List<int>(){0,1,0,1,0,1,1,1,0,0},
                new List<int>(){0,1,0,0,1,0,0,0,0,0},
                new List<int>(){0,0,1,0,0,0,0,1,0,1},
                new List<int>(){1,0,0,1,0,0,0,0,1,0},
                new List<int>(){0,1,1,0,1,0,0,1,1,1},
                new List<int>(){1,1,0,0,0,1,0,0,1,0},
                new List<int>(){0,1,0,0,0,0,1,1,1,1}
            };

            var (locX, locY, numOfAsteroids) = Day10.GetBestLocation(map);
            Assert.That(locX, Is.EqualTo(5));
            Assert.That(locY, Is.EqualTo(8));
            Assert.That(numOfAsteroids, Is.EqualTo(33));
        }

        [Test]
        public void When_testing_part_2_ex_2()
        {
            var map = new List<List<int>>()
            {
                new List<int>(){0,0,0,0,0,0,1,0,1,0},
                new List<int>(){1,0,0,1,0,1,0,0,0,0},
                new List<int>(){0,0,1,1,1,1,1,1,1,0},
                new List<int>(){0,1,0,1,0,1,1,1,0,0},
                new List<int>(){0,1,0,0,1,0,0,0,0,0},
                new List<int>(){0,0,1,0,0,0,0,1,0,1},
                new List<int>(){1,0,0,1,0,0,0,0,1,0},
                new List<int>(){0,1,1,0,1,0,0,1,1,1},
                new List<int>(){1,1,0,0,0,1,0,0,1,0},
                new List<int>(){0,1,0,0,0,0,1,1,1,1}
            };

            var (locX, locY) = Day10.GetAsteroidToByVaporized(20, map);
            Assert.That(locX, Is.EqualTo(1));
            Assert.That(locY, Is.EqualTo(9));
        }

        //[Test]
        //public void When_testing_part_2()
        //{
        //    var layers = Day80,GetLayers(new List<int>() { 0, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 2, 0, 0, 0, 0 }, 
        //        2, 2);
        //    var finalLayer = Day80,GetFinalLayer(layers, 2, 2);
        //    Assert0,That(finalLayer0,layers[0][0], Is0,EqualTo(0));
        //}
    }
}
