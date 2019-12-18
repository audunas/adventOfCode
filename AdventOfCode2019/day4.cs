using System;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day4
    {
        public Day4()
        {
            int start = 307237;
            int end = 769058;

            int fitsTheCriteria = 0;

            for (int i = start; i<end; i++)
            {
                var stringVersion = i.ToString();
                if (stringVersion.Length == 6 &&
                    neverDecrease(stringVersion) &&
                    (stringVersion.Distinct().Count() != stringVersion.Length)
                    //Part2
                    && onlyTwoInARow(stringVersion)
                    )
                {
                    fitsTheCriteria++;
                }
            }

            Console.WriteLine(fitsTheCriteria);
        }

        public static bool onlyTwoInARow(string number)
        {
            for (int index = 1; index < number.Length; index++)
            {
                var isEqual = number[index] == number[index - 1];
                var beforeIsUnequal = index > 1 ? number[index - 1] != number[index-2] : true;
                var afterIsUnequal = index + 1 < number.Length ? number[index] != number[index + 1] : true;

                if (isEqual && beforeIsUnequal && afterIsUnequal)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool neverDecrease(string number)
        {
            for (int index = 1; index < number.Length; index++)
            {
                if (number[index] < number[index-1])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
