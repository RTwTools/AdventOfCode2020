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

      Console.WriteLine($"Part 1: the 2020th spoken number is {MemoryGame.Turn(data, 2020)}");
      Console.WriteLine($"Part 2: the 30000000th spoken number is {MemoryGame.Turn(data, 30000000)}");
    }
  }

  public class MemoryGame
  {
    public static int Turn(IList<int> input, int turnNr)
    {
      var numbers = new Dictionary<int, int>();

      for (int i = 0; i < input.Count; i++)
      {
        numbers.Add(input[i], i);
      }

      var lastNumber = input.Last();

      for (int i = numbers.Count; i < turnNr; i++)
      {
        var lastIndex = i - 1;
        var nextNumber = numbers.ContainsKey(lastNumber)
          ? lastIndex - numbers[lastNumber]
          : 0;

        numbers[lastNumber] = lastIndex;
        lastNumber = nextNumber;
      }

      return lastNumber;
    }
  }
}