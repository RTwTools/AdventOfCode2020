using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc04
{
  public class Program
  {
    public static void Main()
    {
      var passports = Passport.parseList(File.ReadAllLines("input.txt"));

      Console.WriteLine($"Part 1: the number of valid passports is {passports.Count(p => p.IsValid(new Part1Validation()))}.");
      Console.WriteLine($"Part 2: the number of valid passports is {passports.Count(p => p.IsValid(new Part2Validation()))}.");
    }
  }

  public interface IPassportValidation
  {
    public bool Check(Passport passport);
  }

  public class Part1Validation : IPassportValidation
  {
    public static IEnumerable<string> RequiredFields => new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

    public virtual bool Check(Passport passport)
    {
      return RequiredFields.All(r => passport.Data.Keys.Contains(r));
    }
  }

  public class Part2Validation : Part1Validation
  {
    public override bool Check(Passport passport)
    {
      bool valid = base.Check(passport);

      if (valid)
      {
        if (int.TryParse(passport.Data["byr"], out int birthYear) &&
          int.TryParse(passport.Data["iyr"], out int issueYear) &&
          int.TryParse(passport.Data["eyr"], out int expirationYear))
        {
          valid &= (birthYear >= 1920) && (birthYear <= 2002);
          valid &= (issueYear >= 2010) && (issueYear <= 2020);
          valid &= (expirationYear >= 2020) && (expirationYear <= 2030);
        }
        else
        {
          valid = false;
        }

        string height = passport.Data["hgt"];
        if (height.EndsWith("cm"))
        {
          int heightCm = int.Parse(height[0..^2]);
          valid &= (heightCm >= 150) && (heightCm <= 193);
        }
        else if (height.EndsWith("in"))
        {
          int heighIn = int.Parse(height[0..^2]);
          valid &= (heighIn >= 59) && (heighIn <= 76);
        }
        else
        {
          valid = false;
        }

        valid &= new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }
          .Any(x => x == passport.Data["ecl"]);

        valid &= Regex.IsMatch(passport.Data["hcl"], @"^#(\d|[a-f]){6}$");
        valid &= Regex.IsMatch(passport.Data["pid"], @"^\d{9}$");
      }

      return valid;
    }
  }

  public class Passport
  {
    public Dictionary<string, string> Data { get; set; }

    public Passport(Dictionary<string, string> data)
    {
      Data = data;
    }

    public bool IsValid(IPassportValidation validation)
    {
      return validation.Check(this);
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
