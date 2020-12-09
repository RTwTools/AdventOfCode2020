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
      var xmas = new Xmas(25, File.ReadAllLines("input.txt").Select(long.Parse).ToList());
      var error = xmas.GetError();

      Console.WriteLine($"Part 1: the first number that does not have this property is {error}.");
      Console.WriteLine($"Part 2: the encryption weaknessd in the XMAS-encrypted list is {xmas.GetWeakness(error)}.");
    }
  }

  public record Xmas(int PreambleSize, IList<long> Numbers)
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

    public long GetWeakness(long number)
    {
      int index = 0;

      while (true)
      {
        
        long count = 0;

        for (int i = index; i < Numbers.Count; i++)
        {
          count += Numbers[i];

          if (count == number)
          {
            var range = Numbers.ToList().GetRange(index, i - index);
            return  range.Min() + range.Max();
          }
          else if (count > number)
          {
            index++;
            break;
          }
        }
      }
    }
  }
}