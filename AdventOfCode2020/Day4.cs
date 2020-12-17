using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day4
    {
        public Day4()
        {
            var lines = File.ReadLines(@"..\..\input\day4.txt").ToList();

            var passports = new List<List<string>>();
            var passport = new List<string>();
            foreach (var line in lines)
            {
                passport.Add(line);
                if (line == string.Empty)
                {
                    passport.RemoveAt(passport.Count() - 1);
                    passports.Add(passport);
                    passport = new List<string>();
                }
            }
            passports.Add(passport);

            var numberOfValidPassports = GetNumberOfValidPassports(passports);
            Console.WriteLine(numberOfValidPassports);

            var numberOfValidPassports2 = GetNumberOfValidPassportsPart2(passports);
            Console.WriteLine(numberOfValidPassports2);
        }

        //Part2
        public int GetNumberOfValidPassportsPart2(List<List<string>> passports)
        {
            var requiredFields = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            var numberOfValidPassports = 0;

            foreach (var pass in passports)
            {
                var fields = pass.SelectMany(p => p.Split(' ')).Select(s => new Field { Key = s.Split(':')[0], Value = s.Split(':')[1] });
                var fieldKeys = fields.Select(f => f.Key);
                var hasAllRequiredFields = requiredFields.All(r => fieldKeys.Contains(r));
                if (hasAllRequiredFields)
                {
                    var allFieldsIsValid = true;
                    foreach (var field in fields)
                    {
                        switch (field.Key)
                        {
                            case "byr":
                                if (!(field.Value.Count() == 4 && 
                                        field.Value.Where(s => char.IsDigit(s)).Count() == 4 &&
                                        int.Parse(field.Value) >= 1920 && int.Parse(field.Value) <= 2002
                                    ))
                                {
                                    allFieldsIsValid = false;
                                }
                                break;
                            case "iyr":
                                if (!(field.Value.Count() == 4 &&
                                       field.Value.Where(s => char.IsDigit(s)).Count() == 4 &&
                                       int.Parse(field.Value) >= 2010 && int.Parse(field.Value) <= 2020
                                   ))
                                {
                                    allFieldsIsValid = false;
                                }
                                break;
                            case "eyr":
                                if (!(field.Value.Count() == 4 &&
                                      field.Value.Where(s => char.IsDigit(s)).Count() == 4 &&
                                      int.Parse(field.Value) >= 2020 && int.Parse(field.Value) <= 2030
                                  ))
                                {
                                    allFieldsIsValid = false;
                                }
                                break;
                            case "hgt":
                                if (!field.Value.Any(r => !char.IsDigit(r)))
                                {
                                    allFieldsIsValid = false;
                                    break;
                                }
                                var firstCharNotNumber = field.Value.First(r => !char.IsDigit(r));
                                var index = field.Value.IndexOf(firstCharNotNumber);
                                var numbersFromStart = int.Parse(string.Join("",field.Value.Where((v, i) => i < index && char.IsDigit(v)).Select(c => c.ToString())));
                                var stringAtEnd = string.Join("", field.Value.Where((v, i) => i >= index && !char.IsDigit(v)).Select(c => c.ToString()));

                                if(stringAtEnd == "cm")
                                {
                                    if (!(numbersFromStart >= 150 && numbersFromStart <= 193))
                                    {
                                        allFieldsIsValid = false;
                                    }
                                }
                                else if (stringAtEnd == "in")
                                {
                                    if (!(numbersFromStart >= 59 && numbersFromStart <= 76))
                                    {
                                        allFieldsIsValid = false;
                                    }
                                }
                                else
                                {
                                    allFieldsIsValid = false;
                                }

                                break;
                            case "hcl":
                                if (!(field.Value.StartsWith("#") &&
                                    field.Value.Count() == 7 &&
                                    field.Value.Skip(1).All(v => char.IsDigit(v) || new char[]{'a', 'b', 'c', 'd', 'e', 'f' }.Contains(v))
                                    ))
                                {
                                    allFieldsIsValid = false;
                                }
                                break;
                            case "ecl":
                                if (!(field.Value.Count() == 3 &&
                                    new string[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(field.Value)))
                                {
                                    allFieldsIsValid = false;
                                }
                                break;
                            case "pid":
                                if (!(field.Value.Count() == 9 && 
                                    field.Value.All(v => char.IsDigit(v))))
                                {
                                    allFieldsIsValid = false;
                                }
                                break;
                        }
                        if (!allFieldsIsValid)
                        {
                            continue;
                        }
                    }

                    if (allFieldsIsValid)
                    {
                        numberOfValidPassports++;
                    }
                }
            }

            return numberOfValidPassports;
        }


        //Part 1
        public int GetNumberOfValidPassports(List<List<string>> passports)
        {
            var requiredFields = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            var numberOfValidPassports = 0;

            foreach (var pass in passports)
            {
                //File.AppendAllText(@"..\..\input\day4Output.txt", string.Join(",", pass) + "\n");
                var fields = pass.SelectMany(p => p.Split(' ')).Select(s => new Field { Key = s.Split(':')[0], Value = s.Split(':')[1] });
                var fieldKeys = fields.Select(f => f.Key);
                //File.AppendAllText(@"..\..\input\day4Output.txt", string.Join(",", fields) + "\n");
                var isValidPassport = requiredFields.All(r => fieldKeys.Contains(r));
                if (isValidPassport)
                {
                    //File.AppendAllText(@"..\..\input\day4Output.txt", $"isValidPassport: {isValidPassport} \n");
                    numberOfValidPassports++;
                }
                //File.AppendAllText(@"..\..\input\day4Output.txt", "****************************************************** \n");
            }

            return numberOfValidPassports;
        }

        [DebuggerDisplay("Key = {Key}, Value = {Value}")]
        internal struct Field
        {
            public string Key;
            public string Value;
        }
    }
}
