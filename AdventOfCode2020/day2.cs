using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day2
    {
        public Day2()
        {
            var lines = File.ReadLines(@"..\..\input\day2.txt").Select(l => new Password {
                firstNumber = int.Parse(l.Split('-')[0]),
                secondNumber = int.Parse(l.Split('-')[1].Split(' ')[0]),
                letter = l.Split(':')[0].Last(),
                password = l.Split(':')[1].Trim()
            });

            var numberOfValidPasswords = CheckPasswords(lines.ToList());
            numberOfValidPasswords = CheckPasswordsPart2(lines.ToList());

            Console.WriteLine(numberOfValidPasswords);
        }

        private int CheckPasswords(List<Password> passwords)
        {
            var validPasswords = passwords.Where(p => {
                var letterCount = p.password.Count(l => l == p.letter);
                return letterCount >= p.firstNumber && letterCount <= p.secondNumber;
            });
            return validPasswords.Count();
        }

        private int CheckPasswordsPart2(List<Password> passwords)
        {
            var validPasswords = passwords.Where(p => {
                var firstPositionChar = p.password[p.firstNumber-1];
                var secondPositionChar = p.password[p.secondNumber-1];
                return (firstPositionChar == p.letter || secondPositionChar == p.letter) && firstPositionChar != secondPositionChar;
            });
            return validPasswords.Count();
        }

        internal struct Password
        {
            public int firstNumber;
            public int secondNumber;
            public char letter;
            public string password;
        }
    }
}
