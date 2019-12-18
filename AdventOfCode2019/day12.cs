using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day12
    {
        public Day12()
        {
            var lines = File.ReadLines(@"..\..\..\input\day12.txt");

            var split = lines.Select(m => m.Trim(new char[] {'<', '>' }))
                            .Select(l => l.Split(','));

            var moons = new List<Moon>();
            var moonNames = new string[] {"Io", "Europa", "Ganymede", "Callisto" };
            var nameCounter = 0;
            foreach (var s in split)
            {
                var x = int.Parse(s[0].Trim().Split("=")[1]);
                var y = int.Parse(s[1].Trim().Split("=")[1]);
                var z = int.Parse(s[2].Trim().Split("=")[1]);

                moons.Add(new Moon(moonNames[nameCounter], x, y, z, new Velocity(0,0,0)));
                nameCounter++;
            }

            //Part1
            SimulateMoons(moons, 1000);
            var totalEnergy = FindTotalEnergy(moons); 

            Console.WriteLine(totalEnergy);

            //Part2
            var numberOfSteps = GetNumberOfStepsBeforeRepeat(moons);

            Console.WriteLine(numberOfSteps);
        }

        public static void SimulateMoons(List<Moon> moons, int numberOfSteps)
        {
            List<Tuple<Moon, Moon>> allPairsOfMoons = GetAllPairsOfMoons(moons);

            for (int i = 0; i < numberOfSteps; i++)
            {
                //Update velocity by applying gravity
                UpdateVelocity(moons, allPairsOfMoons);

                //Update position by applying velocity
                UpdatePosition(moons);
            }
        }

        public static int GetNumberOfStepsBeforeRepeat(List<Moon> moons)
        {
            var counter = 0;
            
            var firstMoonList = new List<Moon>();
            firstMoonList.AddRange(moons
                .Select(m => new Moon(m.name, m.posX, m.posY, m.posZ, 
                new Velocity(m.velocity.x, m.velocity.y, m.velocity.z))));
            var moonList = new MoonList(firstMoonList);
            var initialState = moonList.ToString();
            HashSet<string> listOfStates = new HashSet<string>() {initialState };
            List<Tuple<Moon, Moon>> allPairsOfMoons = GetAllPairsOfMoons(moons);
            var foundRepeatedState = false;

            while (!foundRepeatedState)
            {
                //Update velocity by applying gravity
                UpdateVelocity(moons, allPairsOfMoons);

                //Update position by applying velocity
                UpdatePosition(moons);

                firstMoonList = new List<Moon>();
                firstMoonList.AddRange(moons
                    .Select(m => new Moon(m.name, m.posX, m.posY, m.posZ,
                    new Velocity(m.velocity.x, m.velocity.y, m.velocity.z))));
                moonList = new MoonList(firstMoonList);
                foundRepeatedState = moonList.ToString() == initialState;
                counter++;
            }

            return counter;
        }

        private static List<Tuple<Moon, Moon>> GetAllPairsOfMoons(List<Moon> moons)
        {
            var allPairsOfMoons = new List<Tuple<Moon, Moon>>();

            for (int i = 0; i < moons.Count(); i++)
            {
                for (int j = i + 1; j < moons.Count(); j++)
                {
                    allPairsOfMoons.Add(new Tuple<Moon, Moon>(moons.ElementAt(i), moons.ElementAt(j)));
                }
            }

            return allPairsOfMoons;
        }

        private static void UpdatePosition(List<Moon> moons)
        {
            foreach (var moon in moons)
            {
                moon.posX += moon.velocity.x;
                moon.posY += moon.velocity.y;
                moon.posZ += moon.velocity.z;
            }
        }

        private static void UpdateVelocity(List<Moon> moons, List<Tuple<Moon, Moon>> allPairsOfMoons)
        {
            foreach (var moonPair in allPairsOfMoons)
            {
                var moonA = moons.First(m => m.name == moonPair.Item1.name);
                var moonB = moons.First(m => m.name == moonPair.Item2.name);

                if (moonA.posX > moonB.posX)
                {
                    moonA.velocity.x--;
                    moonB.velocity.x++;
                }
                else if (moonA.posX < moonB.posX)
                {
                    moonA.velocity.x++;
                    moonB.velocity.x--;
                }

                if (moonA.posY > moonB.posY)
                {
                    moonA.velocity.y--;
                    moonB.velocity.y++;
                }
                else if (moonA.posY < moonB.posY)
                {
                    moonA.velocity.y++;
                    moonB.velocity.y--;
                }

                if (moonA.posZ > moonB.posZ)
                {
                    moonA.velocity.z--;
                    moonB.velocity.z++;
                }
                else if (moonA.posZ < moonB.posZ)
                {
                    moonA.velocity.z++;
                    moonB.velocity.z--;
                }

            }
        }

        public static int FindTotalEnergy(List<Moon> moons)
        {
            var totalEnergy = 0;

            foreach (var moon in moons)
            {
                var potentialEnergy = Math.Abs(moon.posX) + Math.Abs(moon.posY) + Math.Abs(moon.posZ);
                var kineticEnergy = Math.Abs(moon.velocity.x) + Math.Abs(moon.velocity.y) + Math.Abs(moon.velocity.z);
                var total = potentialEnergy * kineticEnergy;
                totalEnergy += total;
            }


            return totalEnergy;
        }
    }

    public class MoonList
    {
        public List<Moon> moons;

        public MoonList(List<Moon> moons)
        {
            this.moons = moons;
        }

        public override string ToString()
        {
            var stringBuild = "";
            foreach (var moon in moons)
            {
                stringBuild += moon.name + moon.posX + moon.posY + moon.posZ;
                stringBuild += moon.velocity.x + moon.velocity.y + moon.velocity.z;
            }
            return stringBuild;
        }
    }

    public class Moon
    {
        public string name;
        public int posX;
        public int posY;
        public int posZ;
        public Velocity velocity;

        public Moon(string name, int posX, int posY, int posZ, Velocity velocity)
        {
            this.name = name;
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;
            this.velocity = velocity;
        }
    }

    public class Velocity
    {
        public int x;
        public int y;
        public int z;

        public Velocity(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
