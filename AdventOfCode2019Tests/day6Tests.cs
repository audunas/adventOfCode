using AdventOfCode2019;
using NUnit.Framework;
using System.Collections.Generic;
using static AdventOfCode2019.Day6;

namespace AdventOfCode2019Tests
{
    class day6Tests
    {

        [Test]
        public void When_testing_part_1()
        {
            var numOfOrbits = FindNumberOfOrbits(new List<Orbit> { new Orbit("D", "I"), new Orbit("B", "C"),new Orbit("G", "H"), new Orbit("C", "D"), new Orbit("D", "E"),
            new Orbit("E", "F"), new Orbit("B", "G"), new Orbit("COM", "B"), new Orbit("E", "J"), new Orbit("J", "K"), new Orbit("K", "L")});
            Assert.That(numOfOrbits, Is.EqualTo(42));
        }

        [Test]
        public void When_testing_part_2()
        {
            var numOfOrbits = FindNumberOfOrbitTranfers(new List<Orbit> { new Orbit("D", "I"), new Orbit("B", "C"),new Orbit("G", "H"), new Orbit("C", "D"), new Orbit("D", "E"),
            new Orbit("E", "F"), new Orbit("B", "G"), new Orbit("COM", "B"), new Orbit("E", "J"),
                new Orbit("J", "K"), new Orbit("K", "L"),
                new Orbit("K", "YOU"), new Orbit("I", "SAN")});
            Assert.That(numOfOrbits, Is.EqualTo(4));
        }
    }
}
