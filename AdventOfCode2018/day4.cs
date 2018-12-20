//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var lines = File.ReadLines(@"..\..\..\input\day4.txt").ToList();

//            lines.Sort((l1, l2) => DateTime.Compare(GetTime(l1), GetTime(l2)));

//            Dictionary<int, Dictionary<int, int>> guardAsleep = new Dictionary<int, Dictionary<int, int>>();

//            var currentGuard = int.Parse(GetRest(lines[0]).Split(' ')[1].Trim('#'));
//            var currentFallsAsleepMinute = 0;

//            foreach (var line in lines)
//            {
//                var time = GetTime(line);
//                var rest = GetRest(line);

//                if (rest.StartsWith("Guard"))
//                {
//                    var guard = int.Parse(rest.Split(' ')[1].Trim('#'));
//                    currentGuard = guard;
//                }

//                if (rest.Equals(FALLS_ASLEEP))
//                {
//                    currentFallsAsleepMinute = time.Minute;
//                }
//                else if (rest.Equals(WAKES_UP))
//                {
//                    var seq = Enumerable.Range(currentFallsAsleepMinute, time.Minute - currentFallsAsleepMinute).ToList();
//                    foreach (int min in seq)
//                    {
//                        if (!guardAsleep.ContainsKey(currentGuard)) guardAsleep.Add(currentGuard, new Dictionary<int, int>());
//                        if (!guardAsleep[currentGuard].ContainsKey(min)) guardAsleep[currentGuard].Add(min, 0);
//                        guardAsleep[currentGuard][min] = guardAsleep[currentGuard][min] += 1;
//                    }

//                }
//            }

//            //Part 1

//            var maxMinutes = 0;
//            var maxGuard = -1;

//            foreach (var guard in guardAsleep.Keys)
//            {
//                var minutes = guardAsleep[guard].Sum(x => x.Value);
//                if (minutes > maxMinutes)
//                {
//                    maxMinutes = minutes;
//                    maxGuard = guard;
//                }
//            }

//            var minutesMostAwake = guardAsleep[maxGuard].OrderByDescending(x => x.Value).Take(1).First().Key;
//            Console.WriteLine($"Guard: {maxGuard}");
//            Console.WriteLine($"Minute: {minutesMostAwake}");
//            Console.WriteLine($"Result: {maxGuard * minutesMostAwake}");

//            //Part 2
//            var maxMinuteMostAsleep = -1;
//            var theMinuteMostAsleep = -1;
//            var guardId = -1;

//            foreach (var guard in guardAsleep.Keys)
//            {
//                var mostAsleepPair = guardAsleep[guard].OrderByDescending(x => x.Value).Take(1).First();
//                var minuteMostAsleep = mostAsleepPair.Key;
//                var minuteMostAsleepMinutes = mostAsleepPair.Value;
//                if (minuteMostAsleepMinutes > maxMinuteMostAsleep)
//                {
//                    maxMinuteMostAsleep = minuteMostAsleepMinutes;
//                    theMinuteMostAsleep = minuteMostAsleep;
//                    guardId = guard;
//                }
//            }
//            Console.WriteLine($"Guard: {guardId}");
//            Console.WriteLine($"Minute: {theMinuteMostAsleep}");
//            Console.WriteLine($"Result: {guardId * theMinuteMostAsleep}");

//            Console.ReadLine();
//        }

//        public static DateTime GetTime(string line)
//        {
//            return DateTime.Parse(line.Split(']')[0].Trim(new char[] { '[' }));
//        }

//        public static string GetRest(string line)
//        {
//            return line.Split(']')[1].Trim(' ');
//        }

//        public static string GetGuardId(string line)
//        {
//            return line.Split(']')[0].Trim(new char[] { '[' });
//        }

//        public const string BEGINS_SHIFT = "begins shift";
//        public const string WAKES_UP = "wakes up";
//        public const string FALLS_ASLEEP = "falls asleep";
//    }
//}
