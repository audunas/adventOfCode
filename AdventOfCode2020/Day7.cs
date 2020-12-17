using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day7
    {
        private static string TargetBagName => "shiny gold";

        public Day7()
        {
            var lines = File.ReadLines(@"..\..\input\day7.txt").ToList();

            var bags = ParseToBags(lines);

            // Part 1
            var bagsThatCanContainBag = FindBagsContainingBag(bags).Select(b => b.Name).Distinct();

            Console.WriteLine(bagsThatCanContainBag.Count());

            // Part 2

            var costOfBags = GetCostOfBags(bags, bags.First(r => r.Name == TargetBagName), 0);

            Console.WriteLine(costOfBags);
        }

        private static int GetCostOfBags(List<Bag> allBags, Bag containingBag, int numberOfBags)
        {
            var bag = allBags.First(r => r.Name == containingBag.Name);
            if (!bag.ContainedBags.Any())
            {
                return numberOfBags;
            }

            var s = bag.ContainedBags.Sum(b => GetCostOfBags(allBags, b, b.NumberOfBags));

            if (containingBag.Name == TargetBagName)
            {
                return s;
            }

            return numberOfBags + (numberOfBags * s);
        }

        private static List<Bag> FindBagsContainingBag(List<Bag> bags)
        {
            var foundBags = new List<Bag>();
            var directBags = bags.Where(b => b.ContainedBags.Select(c => c.Name).Contains(TargetBagName)).ToList();
            foundBags.AddRange(directBags);

            var bagsToSearch = directBags.Select(c => c.Name).ToList();
            var alreadySearchFor = new List<string>() { TargetBagName};

            while (bagsToSearch.Any())
            {
                var found = new List<Bag>();
                foreach (var bagToSearch in bagsToSearch)
                {
                    found.AddRange(bags.Where(b => b.ContainedBags.Select(c => c.Name).Contains(bagToSearch)));
                }
                foundBags.AddRange(found);
                alreadySearchFor.AddRange(bagsToSearch.Distinct());
                bagsToSearch.AddRange(found.Select(h => h.Name));
                bagsToSearch = bagsToSearch.Distinct().Where(r => !alreadySearchFor.Contains(r)).ToList();
            }

            return foundBags;
        }

        private static List<Bag> ParseToBags(List<string> lines)
        {
            var bags = new List<Bag>();

            foreach (var line in lines)
            {
                var splitByContain = line.Split(new string[] { "contain" }, StringSplitOptions.None);
                var bagName = splitByContain[0].Split(new string[] { "bags" }, StringSplitOptions.None)[0].Trim();
                if (splitByContain[1].Trim() == "no other bags.")
                {
                    bags.Add(new Bag { Name = bagName, NumberOfBags = 1, ContainedBags = new List<Bag>() });
                }
                else
                {
                    var contained = splitByContain[1].Trim().Split(',');
                    if (!contained.Any())
                    {
                        bags.Add(new Bag
                        {
                            Name = bagName,
                            NumberOfBags = 1,
                            ContainedBags = new List<Bag>()
                        { GetBagFromString(splitByContain[1]) }
                        });
                    }
                    else
                    {
                        var containedBags = new List<Bag>();
                        foreach (var bag in contained)
                        {
                            containedBags.Add(GetBagFromString(bag));
                        }
                        bags.Add(new Bag
                        {
                            Name = bagName,
                            NumberOfBags = 1,
                            ContainedBags = containedBags
                        });
                    }
                }

            }
            return bags;
        }


        private static Bag GetBagFromString(string input)
        {
            var numberOfOccurences = GetNumbersFromStart(input);
            var words = new string(input.Trim().SkipWhile(s => char.IsDigit(s)).ToArray()).Trim();
            var wordsBeforeBag = words.Split(new string[] { "bag" }, StringSplitOptions.None)[0].Trim();
            return new Bag { Name = wordsBeforeBag, NumberOfBags = numberOfOccurences, ContainedBags = new List<Bag>() };
        }

        private static int GetNumbersFromStart(string input) => int.Parse(new string(input.Trim().TakeWhile(char.IsDigit).ToArray()));

        [DebuggerDisplay("Name = {Name}, NumberOfBags = {NumberOfBags}")]
        internal struct Bag
        {
            public string Name;
            public int NumberOfBags;
            public List<Bag> ContainedBags;
        }

    }


}
