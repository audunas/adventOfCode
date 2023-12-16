using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day4
    {

        public Day4()
        {
            Console.WriteLine("Day4");

            var lines = File.ReadLines(@"..\..\..\input\day4.txt");


            //Part 1
            var sum = 0;

            foreach (var line in lines)
            {
                var winningNumbers = line.Split('|')[0].Split(':')[1].Trim().Split(' ').Where(c => c.Trim() != "").Select(c => int.Parse(c.Trim()));
                var numbersHave = line.Split('|')[1].Trim().Split(' ').Where(c => c.Trim() != "").Select(c => int.Parse(c.Trim()));

                var matches = winningNumbers.Intersect(numbersHave);
                
                
                if (matches.Any())
                {
                    var value = Enumerable.Range(1, matches.Count()).Aggregate((a, b) => a * 2);
                    sum += value;
                }
            }
            Console.WriteLine(sum);

            //Part 2
            sum = 0;

            var cards = new Dictionary<int, int>();
            for(var i = 1; i<=lines.Count(); i++)
            {
                cards.Add(i, 1);
            }

            foreach (var line in lines)
            {
                var cardNumber = int.Parse(line.Split('|')[0].Split(':')[0].Split(' ').Where(c => c.Trim() != "").Last().Trim());

                var winningNumbers = line.Split('|')[0].Split(':')[1].Trim().Split(' ').Where(c => c.Trim() != "").Select(c => int.Parse(c.Trim()));
                var numbersHave = line.Split('|')[1].Trim().Split(' ').Where(c => c.Trim() != "").Select(c => int.Parse(c.Trim()));

                var matches = winningNumbers.Intersect(numbersHave);

                var copyOfNextCards = Enumerable.Range(cardNumber + 1, matches.Count()).Select(c => c).ToList();

                var instances = cards[cardNumber];
                
                foreach (var card in copyOfNextCards) 
                {
                    if (cards.ContainsKey(card))
                    {
                        cards[card] = cards[card] + instances;
                    }
                }
            }

            Console.WriteLine(cards.Values.Sum());
        }
    }
}
