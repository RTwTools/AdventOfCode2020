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

      Console.WriteLine($"Part 1: the number of valid passwords is {passwords.Count(p => p.Valid)}.");
    }

    class Password
    {
      public string Data { get; set; }
      public char RepeatedCharacter { get; set; }
      public int MinimalOccurences { get; set; }
      public int MaximalOccurences { get; set; }
      public bool Valid
      {
        get
        {
          int count = Data?.Count(x => x == RepeatedCharacter) ?? 0;
          return (Data != null) && (count >= MinimalOccurences) && (count <= MaximalOccurences);
        }
      }

      public Password(int min, int max, char @char, string data)
      {
        MinimalOccurences = min;
        MaximalOccurences = max;
        RepeatedCharacter = @char;
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


