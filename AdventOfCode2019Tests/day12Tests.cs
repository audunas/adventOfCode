using AdventOfCode2019;
using NUnit.Framework;
using System.Collections.Generic;

namespace AdventOfCode2019Tests
{
    class day12Tests
    {

        [Test]
        public void When_testing_part_1()
        {
            var moons = new List<Moon>()
            {
               new Moon("Io", -1, 0, 2, new Velocity(0, 0, 0)),
               new Moon("Europa", 2, -10, -7, new Velocity(0, 0, 0)),
               new Moon("Ganymede", 4, -8, 8, new Velocity(0, 0, 0)),
               new Moon("Callisto", 3, 5, -1, new Velocity(0, 0, 0))
            };
            Day12.SimulateMoons(moons, 10);
            var totalEnergy = Day12.FindTotalEnergy(moons);
            Assert.That(totalEnergy, Is.EqualTo(179));
        }

        [Test]
        public void When_testing_part_2()
        {
            var moons = new List<Moon>()
            {
               new Moon("Io", -1, 0, 2, new Velocity(0, 0, 0)),
               new Moon("Europa", 2, -10, -7, new Velocity(0, 0, 0)),
               new Moon("Ganymede", 4, -8, 8, new Velocity(0, 0, 0)),
               new Moon("Callisto", 3, 5, -1, new Velocity(0, 0, 0))
            };
            var numberOfSteps = Day12.GetNumberOfStepsBeforeRepeat(moons);
            Assert.That(numberOfSteps, Is.EqualTo(2772));
        }

        [Test]
        public void When_testing_part_2_ex2()
        {
            var moons = new List<Moon>()
            {
               new Moon("Io", -8, -10, 0, new Velocity(0, 0, 0)),
               new Moon("Europa", 5, 5, 10, new Velocity(0, 0, 0)),
               new Moon("Ganymede", 2, -7, 3, new Velocity(0, 0, 0)),
               new Moon("Callisto", 9, -8, -3, new Velocity(0, 0, 0))
            };
            var numberOfSteps = Day12.GetNumberOfStepsBeforeRepeat(moons);
            Assert.That(numberOfSteps, Is.EqualTo(4686774924));
        }
    }
}
