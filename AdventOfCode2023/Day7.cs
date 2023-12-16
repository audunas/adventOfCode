using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day7
    {
        public string part = "Part1";

        public Day7()
        {
            Console.WriteLine("Day7");

            var lines = File.ReadLines(@"..\..\..\input\day7.txt");

            var handBids = new List<HandBid>();

            foreach(var line in lines)
            {
                var split = line.Split(' ').Where(c => c.Trim() != "").ToArray();
                handBids.Add(new HandBid()
                {
                    Hand = split[0].Trim(),
                    Bid = int.Parse(split[1].Trim())
                });
            }

            var ordered = handBids.OrderBy(c => c).Reverse().ToArray();

            var sum = 0l;

            for (var i = 1; i<=ordered.Count(); i++)
            {
                var h = ordered[i-1];
                sum += (h.Bid * i);
            }

            Console.WriteLine(sum);

        }

        public class HandBid : IComparable<HandBid>
        {
            public string Hand;
            public int Bid;

            public Type FindType()
            {
                
                var handType = GetTypeFromString(Hand);

                //Part 2
                if (Hand.Contains('J') && handType != Type.Five)
                {
                    switch(handType)
                    {
                        case Type.Four:
                            //Either one of the two is J, so can just turn that one to the other and get same of all
                            handType = Type.Five;
                            break;
                        case Type.FullHouse:
                            //Either one of the two is J, so can just turn that one to the other and get same of all
                            handType = Type.Five;
                            break;
                        case Type.Three:
                            //Three Js can be changed to one of the oher letter to get a Four,
                            //or 1 J can be changed to the letter already three of
                            handType = Type.Four;
                            break;
                        case Type.TwoPair:
                            if (Hand.Where(c => c == 'J').Count() == 2)
                            {
                                handType = Type.Four;
                            }
                            else if (Hand.Where(c => c == 'J').Count() == 1)
                            {
                                handType = Type.FullHouse;
                            }
                            break;
                        case Type.OnePair:
                            handType = Type.Three;
                            break;
                        case Type.HighCard:
                            handType = Type.OnePair;
                            break;
                        default:
                            break;
                    }
                }

                return handType;
            }

            private Type GetTypeFromString(string hand)
            {
                if (hand.Distinct().Count() == 1)
                {
                    return Type.Five;
                }
                else if (hand.Distinct().Count() == 5)
                {
                    return Type.HighCard;
                }

                else if (hand.Distinct().Count() == 2)
                {
                    if (hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 4)
                    {
                        return Type.Four;
                    }
                    else
                    {
                        return Type.FullHouse;
                    }
                }
                else if (hand.Distinct().Count() == 3)
                {
                    if (hand.GroupBy(h => h).OrderByDescending(h => h.Count()).First().Count() == 3)
                    {
                        return Type.Three;
                    }
                    else
                    {
                        return Type.TwoPair;
                    }
                }
                else
                {
                    return Type.OnePair;
                }
            }

            //part 1
            public List<string> LetterOrder => new List<string>()
            {
                "A", "K", "Q", "J", "T", "9", "8", "7", "6", "5", "4", "3", "2"
            };

            //part 2
            public List<string> JokerLetterOrder => new List<string>()
            {
                "A", "K", "Q", "T", "9", "8", "7", "6", "5", "4", "3", "2", "J"
            };

            int IComparable<HandBid>.CompareTo(HandBid? other)
            {
                var thisType = FindType();
                var otherType = other.FindType();
                if (thisType != otherType)
                {
                    return thisType > otherType ? 1 : -1;
                }
                else
                {
                    for (var c = 0; c < Hand.Length; c++)
                    {
                        var lettera = Hand[c];
                        var letterb = other.Hand[c];
                        if (lettera != letterb)
                        {
                            return JokerLetterOrder.IndexOf(lettera.ToString().ToUpper()) > JokerLetterOrder.IndexOf(letterb.ToString().ToUpper()) ? 1 : -1;
                        }
                    }
                }
                return 0;
            }
        }

        public enum Type
        {
            Five,
            Four,
            FullHouse,
            Three,
            TwoPair,
            OnePair,
            HighCard
        }
    }
}
