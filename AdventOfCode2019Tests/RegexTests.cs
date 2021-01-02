using NUnit.Framework;
using System.Text.RegularExpressions;

namespace AdventOfCode2019Tests
{
    class RegexTests
    {

        [Test]
        public void When_testing_regex()
        {
            var word = "bbabbbbaabaabbabbabbbbaabaabba";
            var regex = new Regex("bbabbbbaabaabba");
            Assert.True(regex.IsMatch(word));
        }
    }
}
