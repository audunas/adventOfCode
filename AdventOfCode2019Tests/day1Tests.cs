using AdventOfCode2019;
using NUnit.Framework;

namespace Tests
{
    public class Day1Tests
    {

        [Test]
        public void When_testing_part_2()
        {
            var fuelToAdd = Day1.GetFuelToAdd(100756);
            Assert.That(fuelToAdd, Is.EqualTo(50346));
        }
    }
}