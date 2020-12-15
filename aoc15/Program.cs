using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc15
{
  public class Program
  {
    public static void Main()
    {
      var data = File.ReadAllLines("input.txt")[0].Split(",").Select(int.Parse).ToList();

      Console.WriteLine($"Part 1: the 2020th spoken number is {new MemoryGame(data).Turn(2020)}");
    }
  }

  public record MemoryGame(List<int> Numbers)
  {
    public int Turn(int turnNr)
    {
      while (Numbers.Count < turnNr)
      {
        var currentNumber = Numbers.Last();
        var location = Numbers.LastIndexOf(currentNumber);
        var olderLocation = Numbers.LastIndexOf(currentNumber, location - 1);

        var newNumber = (olderLocation == -1) ? 0 : location - olderLocation;
        Numbers.Add(newNumber);
      }

      return Numbers.Last();
    }
  }
}