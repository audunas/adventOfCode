//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AdventOfCode2018
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var line = File.ReadLines(@"..\..\..\input\day8.txt").ToList()[0];
//            //line = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";

//            var numbers = line.Split(' ').Select(int.Parse).ToList();

//            //Part 2

//            var metadataSum = 0;

//            var stack = new Stack<Node>();
//            var rootNode = new Node { id = 0, noOfChildren = numbers[0], noOfMetadataEntries = numbers[1] };
//            stack.Push(rootNode);

//            var currentIndex = 2;

//            Dictionary<int, List<int>> nodeValues = new Dictionary<int, List<int>>();
//            Dictionary<int, List<Node>> nodeChildren = new Dictionary<int, List<Node>>();
//            while (stack.Any())
//            {
//                var next = stack.Pop();
//                if (next.noOfChildren == 0)
//                {
//                    var metadata = numbers.GetRange(currentIndex, next.noOfMetadataEntries).Sum(x => x);
//                    if (!nodeValues.ContainsKey(next.id))
//                    {
//                        nodeValues.Add(next.id, numbers.GetRange(currentIndex, next.noOfMetadataEntries));
//                    }
//                    metadataSum += metadata;
//                    currentIndex += next.noOfMetadataEntries;
//                }
//                else
//                {
//                    var updatedNode = new Node { id = next.id, noOfChildren = next.noOfChildren - 1, noOfMetadataEntries = next.noOfMetadataEntries };
//                    stack.Push(updatedNode);
//                    var newNode = new Node
//                    {
//                        id = currentIndex,
//                        noOfChildren = numbers[currentIndex],
//                        noOfMetadataEntries = numbers[currentIndex + 1]
//                    };
//                    stack.Push(newNode);

//                    if (!nodeChildren.ContainsKey(next.id))
//                    {
//                        nodeChildren.Add(next.id, new List<Node> { newNode });
//                    }
//                    else
//                    {
//                        var newList = nodeChildren[next.id];
//                        newList.Add(newNode);
//                        newList.Sort((x1, x2) => x1.id - x2.id);
//                        nodeChildren[next.id] = newList;
//                    }
//                    currentIndex += 2;
//                }
//            }

//            var rootValue = GetNodeValue(rootNode, nodeValues, nodeChildren);

//            Console.WriteLine(rootValue);
//            Console.ReadLine();



//            //Part 1
//            metadataSum = 0;

//            stack = new Stack<Node>();
//            stack.Push(new Node { noOfChildren = numbers[0], noOfMetadataEntries = numbers[1] });

//            currentIndex = 2;

//            while (stack.Any())
//            {
//                var next = stack.Pop();
//                if (next.noOfChildren == 0)
//                {
//                    metadataSum += numbers.GetRange(currentIndex, next.noOfMetadataEntries).Sum(x => x);
//                    currentIndex += next.noOfMetadataEntries;
//                }
//                else
//                {
//                    var updatedNode = new Node { noOfChildren = next.noOfChildren - 1, noOfMetadataEntries = next.noOfMetadataEntries };
//                    stack.Push(updatedNode);
//                    stack.Push(new Node { noOfChildren = numbers[currentIndex], noOfMetadataEntries = numbers[currentIndex + 1] });
//                    currentIndex += 2;
//                }
//            }

//            Console.WriteLine(metadataSum);
//            Console.ReadLine();
//        }

//        public static int GetNodeValue(Node node, Dictionary<int, List<int>> nodeValues,
//            Dictionary<int, List<Node>> nodeChildren)
//        {
//            if (node.noOfChildren == 0)
//            {
//                return nodeValues[node.id].Sum(x => x);
//            }
//            var children = new List<Node>();
//            if (nodeChildren.Keys.Contains(node.id))
//            {
//                children = nodeChildren[node.id];
//            }
//            var childrenSum = 0;
//            foreach (var metadataEntry in nodeValues[node.id])
//            {
//                if (children.Count > (metadataEntry - 1))
//                {
//                    var child = children.ElementAt(metadataEntry - 1);
//                    childrenSum += GetNodeValue(child, nodeValues, nodeChildren);
//                }
//            }
//            return childrenSum;
//        }

//        public struct Node
//        {
//            public int id;
//            public int noOfChildren;
//            public int noOfMetadataEntries;
//        }
//    }
//}
