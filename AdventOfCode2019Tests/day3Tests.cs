using AdventOfCode2019;
using NUnit.Framework;

namespace AdventOfCode2019Tests
{
    class day3Tests
    {
        [Test]
        public void When_testing_part_1()
        {
            var result = Day3.GetDistance(new[] { "R8", "U5", "L5", "D3" }, new[] { "U7", "R6", "D4", "L4" });
            Assert.That(result, Is.EqualTo(6));

            result = Day3.GetDistance(new[] { "R75", "D30", "R83", "U83", "L12", "D49", "R71", "U7", "L72" }, new[] { "U62", "R66", "U55", "R34", "D71", "R55", "D58", "R83"});
            Assert.That(result, Is.EqualTo(159));
        }

        [Test]
        public void When_testing_part_2()
        {
            var result = Day3.GetNumberOfSteps(new[] { "R8", "U5", "L5", "D3" }, new[] { "U7", "R6", "D4", "L4" });
            Assert.That(result, Is.EqualTo(30));

            result = Day3.GetNumberOfSteps(new[] { "R75", "D30", "R83", "U83", "L12", "D49", "R71", "U7", "L72" }, new[] { "U62", "R66", "U55", "R34", "D71", "R55", "D58", "R83" });
            Assert.That(result, Is.EqualTo(610));
        }
    }
}
