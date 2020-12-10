using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc10
{
  public class Program
  {
    public static void Main()
    {
      var joltAdapter = JoltAdapter.Parse(File.ReadAllLines("input.txt").Select(int.Parse));

      Console.WriteLine($"Part 1: the counts (offsets of 1 and 3) multiplied is: " +
        $"{joltAdapter.AdaptersWithXOffsetCount(1) * joltAdapter.AdaptersWithXOffsetCount(3)}.");
    }

    public record JoltAdapter(IList<int> Adapters)
    {
      public int AdaptersWithXOffsetCount(int offset)
      {
        int count = 0;
        int lastJolt = 0;

        foreach (var adapter in Adapters)
        {
          if (lastJolt + offset == adapter)
          {
            count++;
          }
          lastJolt = adapter;
        }

        if (offset == 3)
        {
          // Add the built-in adapter to the count.
          count++;
        }

        return count;
      }

      public static JoltAdapter Parse(IEnumerable<int> data)
      {
        IList<int> adapters = new List<int>();

        int jolt = 0;

        foreach (var adapter in data.OrderBy(i => i))
        {
          if (jolt + 3 >= adapter)
          {
            adapters.Add(adapter);
            jolt += adapter;
          }
        }

        return new JoltAdapter(adapters);
      }
    }
  }
}