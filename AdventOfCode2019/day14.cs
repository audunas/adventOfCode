using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day14
    {

        public Day14()
        {
            var lines = File.ReadLines(@"..\..\..\input\day14.txt");
            
            var ore = GetOreFor1Fuel(lines);

            Console.WriteLine(ore);
        }

        public static List<Reaction> reactions;

        public static int GetOreFor1Fuel(IEnumerable<string> lines)
        {
            reactions = GetReactions(lines);
            var fuelReaction = reactions.First(r => r.producent.name.Equals("FUEL"));
            var oreReactions = reactions.Where(r => r.consuments.Count() == 1
                                && r.consuments.First().name.Equals("ORE"));

            var chemicalsToFindNeededOf = oreReactions.Select(o => o.producent.name);
            needForEach = new Dictionary<string, decimal>();
            remainders = new Dictionary<string, decimal>();

            var distinctElements = reactions.SelectMany(r => r.consuments.Select(s => s.name)).Distinct();

            foreach (var chemical in distinctElements)
            {
                needForEach.Add(chemical, 0);
                remainders.Add(chemical, 0);
            }

            SetNeedForChemical(fuelReaction.consuments.ToList(), 1);

            var totalOREneed = 0;

            var orePerChemical = new Dictionary<string, int>();

            foreach (var OREReaction in oreReactions)
            {
                var need = needForEach[OREReaction.producent.name];
                var ore = (int)Math.Ceiling(need / (decimal)OREReaction.producent.units) * OREReaction.consuments.First().units;
                orePerChemical.Add(OREReaction.producent.name, ore);
                totalOREneed += ore;
            }
            return totalOREneed;
        }

        public static Dictionary<string, decimal> needForEach;
        public static Dictionary<string, decimal> remainders;

        public static void SetNeedForChemical(List<Chemical> chemicals, decimal multiplier)
        {
            foreach (var chemical in chemicals)
            {
                var multi = multiplier;
                var reactionForChemical = reactions.First(r => r.producent.name.Equals(chemical.name));
                if (reactionForChemical.consuments.Count == 1 && 
                    reactionForChemical.consuments.First().name.Equals("ORE"))
                {
                    needForEach[chemical.name] += multiplier * chemical.units;
                }
                else
                {
                    var pUnits = reactionForChemical.producent.units;
                    var cUnits = chemical.units;
                    var currentRemainder = remainders[chemical.name];
                    var need = 1m;
                    if (currentRemainder > cUnits)
                    {
                        remainders[chemical.name] -= cUnits;
                        //need = Math.Ceiling(chemical.units / ((decimal)reactionForChemical.producent.units) + currentRemainder);
                    }
                    else if (pUnits > cUnits)
                    {
                        remainders[chemical.name] += (pUnits - cUnits);
                    }
                    else
                    {
                         need = Math.Ceiling((chemical.units - currentRemainder) / (decimal)reactionForChemical.producent.units);
                    }

                    multi *= need;
                    SetNeedForChemical(reactionForChemical.consuments, multi);
                }
            }
        }

        public static List<Reaction> GetReactions(IEnumerable<string> lines)
        {
            var reactions = new List<Reaction>();

            foreach (var line in lines)
            {
                var splittedLine = line.Split("=>");
                var consumentsArray = splittedLine[0].Split(",");
                var consuments = new List<Chemical>();
                foreach (var consument in consumentsArray)
                {
                    var con = consument.Trim().Split(" ");
                    consuments.Add(new Chemical(int.Parse(con[0].Trim()), con[1].Trim()));
                }

                var producent = splittedLine[1].Trim().Split(" ");
                reactions.Add(new Reaction(consuments,
                    new Chemical(int.Parse(producent[0].Trim()), producent[1].Trim())));
            }
            return reactions;
        }

        public class Reaction
        {
            public List<Chemical> consuments;
            public Chemical producent;

            public Reaction(List<Chemical> consuments, Chemical producent)
            {
                this.consuments = consuments;
                this.producent = producent;
            }
        }

        public class Chemical
        {
            public int units;
            public string name;

            public Chemical(int units, string name)
            {
                this.units = units;
                this.name = name;
            }
        }

        
    }
}
