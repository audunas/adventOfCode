using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    class Day17
    {
        public Day17()
        {
            var lines = File.ReadLines(@"..\..\input\day17.txt").ToList();
            var cubes = new List<Cube>();
            cubes = lines.SelectMany((l, index) => l.Select((c, cIndex) =>
                new Cube { x = index, y = cIndex, z = 1, state = c == '.' ? State.Inactive : State.Active })).ToList();

            // Part 1
            //Part1(cubes);

            // Part 2
            var perms = CreatePermutationsPart2();
            var fourDCubes = new HashSet<FourDCube>(lines.SelectMany((l, index) => l.Select((c, cIndex) =>
                new FourDCube { x = index, y = cIndex, z = 1, w = 1, state = c == '.' ? State.Inactive : State.Active })));

            var cubeState = new Dictionary<Tuple<int, int, int, int>, State>();
            var neighbourDict = new Dictionary<Tuple<int, int, int, int>, List<FourDCube>>();
            foreach (var cube in fourDCubes)
            {
                cubeState[cube.GetTuple()] = cube.state;
                neighbourDict[cube.GetTuple()] = GetFourDNeighbours(cube, perms, neighbourDict, cubeState);
            }

            Part2(fourDCubes, perms, neighbourDict, cubeState);
        }
        private void Part2(HashSet<FourDCube> cubes, List<Tuple<int, int, int, int>> perms, Dictionary<Tuple<int, int, int, int>, List<FourDCube>> neighbourDict,
            Dictionary<Tuple<int, int, int, int>, State> cubeState)
        {
            for (var i = 0; i < 6; i++)
            {
                var startNeighbours = cubes.SelectMany(c => GetFourDNeighbours(c, perms, neighbourDict, cubeState)).ToList();
                cubes = new HashSet<FourDCube>(cubes.Concat(new HashSet<FourDCube>(startNeighbours)));
                var tempCubes = new HashSet<FourDCube>();
                var tempCubeState = new Dictionary<Tuple<int, int, int, int>, State>(cubeState);
                foreach (var cube in cubes)
                {
                    var neighbours = GetFourDNeighbours(cube, perms, neighbourDict, tempCubeState);
                    var activeNeighbours = neighbours.Select(n => n.GetTuple()).Where(c => cubeState.ContainsKey(c))
                        .Select(c => cubeState[c]).Where(n => n == State.Active);
                    var activeNeighbourCount = activeNeighbours.Count();
                    if (cube.state == State.Active)
                    {
                        if (activeNeighbourCount == 2 || activeNeighbourCount == 3)
                        {
                            tempCubes.Add(new FourDCube { x = cube.x, y = cube.y, z = cube.z, w = cube.w, state = State.Active});
                            tempCubeState[cube.GetTuple()] = State.Active;
                        }
                        else
                        {
                            tempCubes.Add(new FourDCube { x = cube.x, y = cube.y, z = cube.z, w = cube.w, state = State.Inactive});
                            tempCubeState[cube.GetTuple()] = State.Inactive;
                        }
                    }
                    else
                    {
                        if (activeNeighbourCount == 3)
                        {
                            tempCubes.Add(new FourDCube { x = cube.x, y = cube.y, z = cube.z, w = cube.w, state = State.Active});
                            tempCubeState[cube.GetTuple()] = State.Active;
                        }
                        else
                        {
                            tempCubes.Add(new FourDCube { x = cube.x, y = cube.y, z = cube.z, w = cube.w, state = State.Inactive});
                            tempCubeState[cube.GetTuple()] = State.Inactive;
                        }
                    }
                }
                cubes = tempCubes;
                cubeState = tempCubeState;
            }

            Console.WriteLine(cubeState.Where(c => c.Value == State.Active).Count());
        }

        private void Part1(List<Cube> cubes)
        {
            for (var i = 0; i < 6; i++)
            {
                var startNeighbours = cubes.SelectMany(c => GetNeighbours(c, cubes)).ToList();
                var neighboursOutside = startNeighbours.Except(cubes);
                cubes.AddRange(neighboursOutside);
                var tempCubes = new List<Cube>();
                foreach (var cube in cubes)
                {
                    var neighbours = GetNeighbours(cube, cubes);
                    var activeNeighbours = neighbours.Where(n => n.state == State.Active);
                    var activeNeighbourCount = activeNeighbours.Count();
                    if (cube.state == State.Active)
                    {
                        if (activeNeighbourCount == 2 || activeNeighbourCount == 3)
                        {
                            tempCubes.Add(new Cube { x = cube.x, y = cube.y, z = cube.z, state = State.Active });
                        }
                        else
                        {
                            tempCubes.Add(new Cube { x = cube.x, y = cube.y, z = cube.z, state = State.Inactive });
                        }
                    }
                    else
                    {
                        if (activeNeighbourCount == 3)
                        {
                            tempCubes.Add(new Cube { x = cube.x, y = cube.y, z = cube.z, state = State.Active });
                        }
                        else
                        {
                            tempCubes.Add(new Cube { x = cube.x, y = cube.y, z = cube.z, state = State.Inactive });
                        }
                    }
                }
                cubes = tempCubes;
            }

            Console.WriteLine(cubes.Where(c => c.state == State.Active).Count());
        }

        public List<FourDCube> GetFourDNeighbours(FourDCube cube, List<Tuple<int, int, int, int>> perms, 
            Dictionary<Tuple<int, int, int, int>, List<FourDCube>> neighbourDict, Dictionary<Tuple<int, int, int, int>, State> cubeState)
        {
            var neighbours = new List<FourDCube>();
            var key = cube.GetTuple();
            var alreadyAdded = neighbourDict.ContainsKey(key);
            if (alreadyAdded)
            {
                return neighbourDict[key];
            }
            neighbours = perms.Select(p => addNeighbour(p, cube, cubeState)).ToList();
            //foreach (var perm in perms)
            //{
            //    var newX = cube.x + perm.Item1;
            //    var newY = cube.y + perm.Item2;
            //    var newZ = cube.z + perm.Item3;
            //    var newW = cube.w + perm.Item4;
            //    var neighbour = cubes.Where(c => c.x == newX &&
            //             c.y == newY && c.z == newZ && c.w == newW);
            //    if (!neighbour.Any())
            //    {
            //        var fourDCube = new FourDCube { x = newX, y = newY, z = newZ, w = newW, state = State.Inactive };
            //        neighbours.Add(fourDCube);
            //        cubeState[fourDCube.GetTuple()] = State.Inactive;
            //    }
            //    else
            //    {
            //        neighbours.Add(neighbour.First());
            //    }
            //}
            neighbourDict[key] = neighbours;
            return neighbours;
        }

        public FourDCube addNeighbour(Tuple<int, int, int, int> perm, FourDCube cube, Dictionary<Tuple<int, int, int, int>, State> cubeState)
        {
            var newX = cube.x + perm.Item1;
            var newY = cube.y + perm.Item2;
            var newZ = cube.z + perm.Item3;
            var newW = cube.w + perm.Item4;
            var newCube = new FourDCube { x = newX, y = newY, z = newZ, w = newW, state = State.Inactive };
            var cubeAlreadyFound = cubeState.ContainsKey(newCube.GetTuple());
            if (!cubeAlreadyFound)
            {
                cubeState.Add(newCube.GetTuple(), State.Inactive);
                return newCube;
            }
            else
            {
                newCube.state = cubeState[newCube.GetTuple()];
                return newCube;
            }
        }


        public List<Cube> GetNeighbours(Cube c, List<Cube> cubes)
        {
            var neighbours = new List<Cube>();
            foreach (var perm in CreatePermutations())
            {
                var newX = c.x + perm.Item1;
                var newY = c.y + perm.Item2;
                var newZ = c.z + perm.Item3;
                var neighbour = cubes.Where(cube => cube.x == newX &&
                        cube.y == newY && cube.z == newZ);
                if (!neighbour.Any())
                {
                    neighbours.Add(new Cube { x = newX, y = newY, z = newZ, state = State.Inactive });
                }
                else
                {
                    neighbours.Add(neighbour.First());
                }
            }
            return neighbours;
        }

        private List<Tuple<int, int, int>> CreatePermutations()
        {
            var perms = new List<Tuple<int, int, int>>();
            for (var x = -1; x<2; x++)
            {
                for (var y = -1; y<2; y++)
                {
                    for (var z = -1; z<2; z++)
                    {
                        if (!(x == 0 && y == 0 && z == 0))
                        {
                            perms.Add(Tuple.Create(x, y, z));
                        }
                    }
                }
            }
            
            return perms;
        }

        private List<Tuple<int, int, int, int>> CreatePermutationsPart2()
        {
            var perms = new List<Tuple<int, int, int, int>>();
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    for (var z = -1; z < 2; z++)
                    {
                        for (var w = -1; w < 2; w++)
                        {
                            if (!(x == 0 && y == 0 && z == 0 && w == 0))
                            {
                                perms.Add(Tuple.Create(x, y, z, w));
                            }
                        }
                    }
                }
            }

            return perms;
        }
    }

    internal struct Cube
    {
        public int x;
        public int y;
        public int z;
        public State state;
    }

    internal class FourDCube
    {
        public int x;
        public int y;
        public int z;
        public int w;
        public State state;

        public Tuple<int, int, int, int> GetTuple()
        {
            return Tuple.Create(x, y, z, w);
        }

        public override bool Equals(Object obj)
        {
            var cube = (FourDCube)obj;
            return x == cube.x && y == cube.y && z == cube.z && w == cube.w;
        }

        public override int GetHashCode()
        {
            return (x << 2) ^ (y << 2 ^ (z << 2 ^ w << 2));
        }
    }

    public enum State
    {
        Active,
        Inactive
    }
}
