using NUnit.Framework;
using AdventOfCode2019;

namespace AdventOfCode2019Tests
{
    public class day2Tests
    {
        [Test]
        public void When_testing_part_1()
        {
            var endState = Day2.FindEndState(new[] { 1, 0, 0, 0, 99 });
            Assert.That(endState, Is.EqualTo(new[] { 2, 0, 0, 0, 99 }));

            endState = Day2.FindEndState(new[] { 2, 4, 4, 5, 99, 0 });
            Assert.That(endState, Is.EqualTo(new[] { 2, 4, 4, 5, 99, 9801 }));

            endState = Day2.FindEndState(new[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 });
            Assert.That(endState, Is.EqualTo(new[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 }));
        }
    }
}
