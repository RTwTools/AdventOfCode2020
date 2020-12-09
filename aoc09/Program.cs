using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc09
{
  public class Program
  {
    public static void Main()
    {
      var numbers = File.ReadAllLines("input.txt").Select(long.Parse).ToList();
      var counter = new Counter(25, numbers);

      Console.WriteLine($"Part 1: the first number that does not have this property is {counter.GetError()}.");
    }
  }

  public record Counter(int PreambleSize, IList<long> Numbers)
  {
    public long GetError()
    {
      int index = PreambleSize;

      while (true)
      {
        bool found = false;

        for (int i = index - PreambleSize; i < index; i++)
        {
          for (int j = i + 1; j < index; j++)
          {
            if (Numbers[i] + Numbers[j] == Numbers[index])
            {
              found = true;
              break;
            }
          }

          if (found == true) break;
        }

        if (found == false)
        {
          return Numbers[index];
        }
        else
        {
          index++;
        }
      }
    }
  }
}
