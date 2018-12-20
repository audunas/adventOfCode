//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var line = File.ReadLines(@"..\..\..\input\day9.txt").ToList()[0];
//            //line = "30 players; last marble is worth 5807 points";

//            var noOfPlayers = int.Parse(line.Split(' ')[0]);
//            var lastMarbleWorth = int.Parse(line.Split(' ')[6]) * 100;

//            var currentMarble = 0;
//            var currentUser = 1;

//            LinkedList<int> circle = new LinkedList<int>();
//            long[] userScores = new long[noOfPlayers];
//            var current = circle.AddFirst(currentMarble);


//            for (int i = 1; i <= lastMarbleWorth; i++)
//            {
//                var listLength = circle.Count;
//                if (i % 23 == 0)
//                {
//                    current = get7Before(current, circle);
//                    userScores[currentUser - 1] = userScores[currentUser - 1] + i + current.Value;

//                    var tmp = current;
//                    current = current.Next ?? circle.First;
//                    circle.Remove(tmp);
//                }
//                else
//                {
//                    current = current.Next ?? circle.First;
//                    current = circle.AddAfter(current, i);
//                }
//                currentUser++;
//                if (currentUser > noOfPlayers)
//                {
//                    currentUser = 1;
//                }
//            }


//            Console.WriteLine(userScores.Max());

//            Console.ReadLine();

//        }

//        public static LinkedListNode<int> get7Before(LinkedListNode<int> node, LinkedList<int> circle, int counter = 7)
//        {
//            if (counter == 0)
//            {
//                return node;
//            }
//            return get7Before(node.Previous ?? circle.Last, circle, counter - 1);
//        }

//    }
//}
