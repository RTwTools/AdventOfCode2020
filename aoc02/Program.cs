using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc02
{
  public class Program
  {
    public static void Main()
    {
      var passwords = File.ReadAllLines("input.txt").Select(Password.Parse);

      Console.WriteLine($"Part 1: the number of valid passwords is {passwords.Count(p => p.ValidPart1)}.");
      Console.WriteLine($"Part 2: the number of valid passwords is {passwords.Count(p => p.ValidPart2)}.");
    }

    class Password
    {
      public string Data { get; set; }
      public char Character { get; set; }
      public int First { get; set; }
      public int Second { get; set; }

      public bool ValidPart1
      {
        get
        {
          int count = Data?.Count(x => x == Character) ?? 0;
          return (Data != null) && (count >= First) && (count <= Second);
        }
      }

      public bool ValidPart2
      {
        get
        {
          var bools = new[]
          {
            (Data[First - 1] == Character),
            (Data[Second - 1] == Character)
          };

          return bools.Count(b => b) == 1;
        }
      }

      public Password(int min, int max, char @char, string data)
      {
        First = min;
        Second = max;
        Character = @char;
        Data = data;
      }

      static public Password Parse(string input)
      {
        var match = Regex.Match(input, @"^(\d+)-(\d+)\s+(\w):\s+(\w+)$");

        if (match.Success)
        {
          var min = int.Parse(match.Groups[1].Value);
          var max = int.Parse(match.Groups[2].Value);
          var @char = match.Groups[3].Value[0];
          var data = match.Groups[4].Value;

          return new Password(min, max, @char, data);
        }

        return null;
      }
    }
  }
}


