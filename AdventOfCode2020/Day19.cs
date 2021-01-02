using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    class Day19
    {
        public Day19()
        {
            var lines = File.ReadLines(@"..\..\input\day19.txt").ToList();
            var rules = lines
                .TakeWhile(l => l != string.Empty)
                .Select(l => CreateRule(l)).ToList()
                .ToDictionary(key => key.RuleId, value => new Rule { Rules = value.Rules, Character = value.Character});
            var messages = lines.Skip(rules.Count()+1);
            var zeroRule = rules[0];
            var validMessages = 0;
            var foundRules = new Dictionary<int, List<string>>();

            // Part 1
            var zeroRulesPossiblities = new HashSet<string>(GetZeroRulePossibilities(0, zeroRule, rules, foundRules));

            foreach (var message in messages)
            {
                if (zeroRulesPossiblities.Contains(message))
                {
                    validMessages++;
                }
            }

            Console.WriteLine(validMessages);

            // Part 2
            rules[8] = new Rule { Rules = new List<List<int>> {
                   new List<int> {42}, new List<int> { 42, 8}
            }, Character = string.Empty }; 
            rules[11] = new Rule
            {
                Rules = new List<List<int>> {
                   new List<int> {42, 31}, new List<int> { 42, 11, 31}
            },
                Character = string.Empty
            };

            validMessages = 0;
            foundRules = new Dictionary<int, List<string>>();

            zeroRulesPossiblities = new HashSet<string>(GetZeroRulePossibilities(0, zeroRule, rules, foundRules));

            var loopPossibilities = zeroRulesPossiblities.Where(p => p.Contains('s')).ToList()  ;

            var matchingMessages = new List<string>();

            var counter = 0;
            var rule42 = foundRules[42];
            var rule31 = foundRules[31];
            var length = rule42.First().Length;
            Console.WriteLine($"Total number of messages: {messages.Count()}");

            foreach (var message in messages)
            {
                Console.WriteLine($"Counter: {counter}");
                counter++;
                if (zeroRulesPossiblities.Contains(message))
                {
                    validMessages++;
                    matchingMessages.Add(message);
                }
                else
                {
                    if (message.Length % length == 0)
                    {
                        var numberOfPieces = (message.Length / length);
                        var split = Enumerable.Range(0, numberOfPieces)
                            .Select(i => message.Substring(i * length, length));
                        var matchedBy42 = split.TakeWhile(s => rule42.Contains(s));
                        var allMatched = split.Skip(matchedBy42.Count()).All(r => rule31.Contains(r));
                        if (allMatched && matchedBy42.Count() < split.Count()
                            && matchedBy42.Count() > (split.Count() - matchedBy42.Count()))
                        {
                            validMessages++;
                            matchingMessages.Add(message);
                        }
                    }
                }
            }

            Console.WriteLine(validMessages);

        }

        private List<string> GetZeroRulePossibilities(int ruleId, Rule currentRule, Dictionary<int, Rule> rules, 
            Dictionary<int, List<string>> foundRules )
        {
            if (!currentRule.Rules.Any())
            {
                return new List<string> { currentRule.Character };
            }

            var possibliities = new List<string>();

            foreach (var ruleSet in currentRule.Rules)
            {
                var possibility = new List<List<string>>();
                foreach (var id in ruleSet)
                {
                    if (foundRules.ContainsKey(id))
                    {
                        possibility.Add(foundRules[id]);
                    }
                    else if (id == ruleId)
                    {
                        possibility.Add(new List<string> { $"s{id}e" });
                    }
                   
                    else
                    {
                        possibility.Add(GetZeroRulePossibilities(id, rules[id], rules, foundRules));
                    }
                }

                IEnumerable<string> combos = new[] { "" };

                foreach (var inner in possibility)
                    combos = from c in combos
                             from i in inner
                             select c + i;

                possibliities.AddRange(combos);
            }

            foundRules[ruleId] = possibliities;

            return possibliities;
        }

        private RuleWithId CreateRule(string l)
        {
            var split = l.Split(':');
            var rules = new List<List<int>>();
            var character = string.Empty;
            var rule = split[1].Trim();

            if (!rule.Any(r => char.IsDigit(r)))
            {
                character = split[1].Trim();
            }
            else
            {
                var ruleBuilder = new List<int>();
                var ruleIdString = string.Empty;
                foreach (var ruleChar in rule)
                {
                    if (ruleChar == ' ')
                    {
                        if (ruleIdString != string.Empty)
                        {
                            ruleBuilder.Add(int.Parse(ruleIdString));
                            ruleIdString = string.Empty;
                        }
                       
                        continue;
                    }
                    if (ruleChar == '|')
                    {
                        rules.Add(ruleBuilder);
                        ruleBuilder = new List<int>();
                    }
                    else
                    {
                        ruleIdString += ruleChar;   
                    }
                }
                ruleBuilder.Add(int.Parse(ruleIdString));
                rules.Add(ruleBuilder);
            }

            return new RuleWithId { RuleId = int.Parse(split[0].Trim()), Rules = rules, Character = character.Replace("\"", "") };
        }

        internal struct Rule
        {
            public List<List<int>> Rules;
            public string Character;
        }

        internal struct RuleWithId
        {
            public int RuleId;
            public List<List<int>> Rules;
            public string Character;
        }
    }
}
