using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day1
    {

        public Day1()
        {
            var lines = File.ReadLines(@"..\..\..\input\day1.txt").ToList();

            //Part1
            int totalFuelRequirement = 0;
            foreach (var line in lines)
            {
                var mass = int.Parse(line);

                var fuelRequired = getFuelRequired(mass);
                totalFuelRequirement += fuelRequired;
            }
            Console.WriteLine("Part 1:" + totalFuelRequirement);

            //Part2
            totalFuelRequirement = 0;
            foreach (var line in lines)
            {
                var mass = int.Parse(line);

                int fuelToAdd = GetFuelToAdd(mass);
                totalFuelRequirement += fuelToAdd;
            }

            Console.WriteLine("Part 2:" + totalFuelRequirement);
        }

        public static int GetFuelToAdd(int mass)
        {

            int fuelRequired = getFuelRequired(mass);
            int fuelToAdd = 0;

            while (fuelRequired > 0)
            {
                fuelToAdd += fuelRequired;
                fuelRequired = getFuelRequired(fuelRequired);
            }
            return fuelToAdd;
        }

        private static int getFuelRequired(int mass) => (int)Math.Floor(new decimal(mass / 3)) - 2;
    }
}
