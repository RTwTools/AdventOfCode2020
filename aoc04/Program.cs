using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc04
{
  public class Program
  {
    public static void Main()
    {
      var passports = Passport.parseList(File.ReadAllLines("input.txt"));

      var requiredFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

      Console.WriteLine($"Part 1: the number of valid passports is {passports.Count(p => p.IsValid(requiredFields))}.");
    }
  }

  public class Passport
  {
    public Dictionary<string, string> Data { get; set; }

    public Passport(Dictionary<string, string> data)
    {
      Data = data;
    }

    public bool IsValid(IEnumerable<string> requiredFields)
    {
      return requiredFields.All(r => Data.Keys.Contains(r));
    }

    public static Passport Parse(IEnumerable<string> data)
    {
      var passportData = new Dictionary<string, string>();

      foreach (var line in data)
      {
        var keyValues = line.Split(' ')
          .Select(l => l.Split(':'))
          .ToDictionary(l => l[0], l => l[1]);

        foreach (var keyValue in keyValues)
        {
          passportData.Add(keyValue.Key, keyValue.Value);
        }
      }

      return new Passport(passportData);
    }

    public static IEnumerable<Passport> parseList(IEnumerable<string> rawData)
    {
      var passports = new List<Passport>();
      var data = new List<string>();

      foreach (var line in rawData)
      {
        if (string.IsNullOrEmpty(line) && data.Any())
        {
          passports.Add(Parse(data));
          data.Clear();
        }
        else
        {
          data.Add(line);
        }
      }

      if (data.Any())
      {
        passports.Add(Parse(data));
      }

      return passports;
    }
  }
}


