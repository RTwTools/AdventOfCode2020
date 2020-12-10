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
      var joltAdapter = JoltAdapter.Parse(File.ReadAllLines("input.txt").Select(long.Parse));

      Console.WriteLine($"Part 1: the counts (offsets of 1 and 3) multiplied is: " +
        $"{joltAdapter.AdaptersWithXOffsetCount(1) * joltAdapter.AdaptersWithXOffsetCount(3)}.");
      Console.WriteLine($"Part 2: the number of options is {joltAdapter.NumberOfConnectionOptions}");
    }

    public record JoltAdapter(IList<long> Adapters)
    {
      public long AdaptersWithXOffsetCount(long offset)
      {
        long count = 0;
        long lastJolt = 0;

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

      public long NumberOfConnectionOptions
      {
        get
        {
          long count = 0;
          long lastAdapter = Adapters.Last();
          var options = new Dictionary<long, long> { { 0, 1 } };
          var newOptions = new Dictionary<long, long>();

          do
          {
            foreach (var option in options)
            {
              foreach (var adapter in Adapters)
              {
                if (adapter > option.Key && adapter <= option.Key + 3)
                {
                  if (newOptions.ContainsKey(adapter))
                  {
                    newOptions[adapter] += option.Value;
                  }
                  else
                  {
                    newOptions.Add(adapter, option.Value);
                  }
                }
                else if (adapter > option.Key + 3)
                {
                  break;
                }
              }
            }

            if (newOptions.Any() == true)
            {
              if (newOptions.ContainsKey(lastAdapter))
              {
                count += newOptions[lastAdapter];
                newOptions.Remove(lastAdapter);
              }

              options = newOptions.ToDictionary(i => i.Key, i => i.Value);
              newOptions.Clear();
            }
          }
          while (options.Any());

          return count;
        }
      }

      public static JoltAdapter Parse(IEnumerable<long> data)
      {
        var adapters = new List<long>();

        long jolt = 0;

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