//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var lines = File.ReadLines(@"..\..\..\input\day7.txt").ToList();

//            //lines = new List<string>
//            //{
//            //    "Step C must be finished before step A can begin.",
//            //    "Step C must be finished before step F can begin.",
//            //    "Step A must be finished before step B can begin.",
//            //    "Step A must be finished before step D can begin.",
//            //    "Step B must be finished before step E can begin.",
//            //    "Step D must be finished before step E can begin.",
//            //    "Step F must be finished before step E can begin."
//            //};

//            List<Step> steps = lines.Select(x => new Step { step = x.Split(' ')[1], stepAfter = x.Split(' ')[7] }).ToList();
//            List<string> availableSteps = steps.Select(x => x.step).ToList();

//            //Part 2
//            var numOfWorkers = 5;

//            List<string> stepsInOrder = new List<string>();

//            var stepsWithoutStepsBefore = availableSteps.Except(steps.Select(x => x.stepAfter).ToList()).OrderBy(x => x);

//            Dictionary<string, int> downCounter = new Dictionary<string, int>();
//            var stepsInProgress = new HashSet<string>();
//            foreach (var startStep in stepsWithoutStepsBefore)
//            {
//                downCounter.Add(startStep, 60 + startStep[0] % 32);
//                stepsInProgress.Add(startStep);
//            }

//            var counter = 0;
//            var nextSteps = new HashSet<string>();
//            while (downCounter.Count() > 0)
//            {
//                var stepsToStart = new HashSet<string>();
//                var stepsToRemove = new HashSet<string>();
//                foreach (var step in stepsInProgress)
//                {
//                    downCounter[step]--;
//                    if (downCounter[step] == 0)
//                    {
//                        stepsInOrder.Add(step);
//                        stepsToRemove.Add(step);
//                        nextSteps.UnionWith(steps.Where(x => x.step.Equals(step)).Select(x => x.stepAfter));
//                        for (int i = downCounter.Count(); i <= numOfWorkers; i++)
//                        {
//                            var next = getNextStep(nextSteps, steps, stepsInOrder, stepsToStart);
//                            if (next != null) stepsToStart.Add(next);
//                        }
//                    }
//                }
//                foreach (var next in stepsToStart)
//                {
//                    downCounter.Add(next, 60 + next[0] % 32);
//                    stepsInProgress.Add(next);
//                    nextSteps.Remove(next);
//                }
//                foreach (var remove in stepsToRemove)
//                {
//                    downCounter.Remove(remove);
//                    stepsInProgress.Remove(remove);
//                }

//                counter++;
//            }

//            Console.WriteLine(counter);
//            Console.ReadLine();

//            //Part 1
//            stepsInOrder = new List<string>();

//            stepsWithoutStepsBefore = availableSteps.Except(steps.Select(x => x.stepAfter).ToList()).OrderBy(x => x);
//            stepsInOrder.Add(stepsWithoutStepsBefore.ElementAt(0));
//            nextSteps = steps.Where(x => stepsInOrder.Contains(x.step)).Select(x => x.stepAfter).ToHashSet();
//            for (int i = 1; i < stepsWithoutStepsBefore.Count(); i++)
//            {
//                nextSteps.Add(stepsWithoutStepsBefore.ElementAt(i));
//            }

//            while (nextSteps.Any())
//            {
//                var next = getNextStep(nextSteps, steps, stepsInOrder);
//                nextSteps.Remove(next);
//                stepsInOrder.Add(next);
//                nextSteps.UnionWith(steps.Where(x => x.step == next).Select(x => x.stepAfter));
//            }

//            Console.WriteLine(string.Join("", stepsInOrder));
//            Console.ReadLine();

//        }

//        public struct Counter
//        {
//            public Counter(int v) { iValue = v; }
//            public int iValue;
//        }

//        public static string getNextStep(HashSet<string> possibleNextStep, List<Step> steps, List<string> stepsInOrder, HashSet<string> stepsToStart = null)
//        {
//            if (!possibleNextStep.Any())
//            {
//                return null;
//            }
//            if (stepsToStart == null)
//            {
//                stepsToStart = new HashSet<string>();
//            }
//            var first = possibleNextStep.OrderBy(x => x).First();
//            int i = 0;
//            var nextFound = false;
//            while (!nextFound)
//            {
//                if (i > possibleNextStep.Count() - 1)
//                {
//                    return null;
//                }
//                first = possibleNextStep.OrderBy(x => x).ElementAt(i);
//                var getAllPreReqs = steps.Where(x => x.stepAfter.Equals(first)).Select(x => x.step);
//                if (getAllPreReqs.Intersect(stepsInOrder).Count() == getAllPreReqs.Count() && !stepsToStart.Contains(first))
//                {
//                    nextFound = true;
//                }
//                i++;
//            }
//            return first;
//        }

//        public struct Step
//        {
//            public string step;
//            public string stepAfter;
//        }
//    }
//}
