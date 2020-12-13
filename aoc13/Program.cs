using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc13
{
  public class Program
  {
    public static void Main()
    {
      var data = File.ReadAllLines("input.txt");

      int earliestDepartmentTime = int.Parse(data[0]);
      var busIds = data[1].Split(new[] { ',', 'x' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

      var busDepartData = busIds.Select(busId => (busId, ((((earliestDepartmentTime - 1) / busId) + 1) * busId) - earliestDepartmentTime)).OrderBy(b => b.Item2);

      Console.WriteLine($"Part 1: the busId multiplied by the waiting time is {busDepartData.First().busId * busDepartData.First().Item2}");

      var result = CalculateEarliestTimeStamp(data[1].Replace("x", "0").Split(',').Select(long.Parse).ToList());

      Console.WriteLine($"Part 2: the earliest timestamp is {result}");
    }

    public static long CalculateEarliestTimeStamp(IList<long> busIds)
    {
      long time = 0;
      long delta = busIds.First();
      int busIndex = 1;

      while (busIndex < busIds.Count)
      {
        if (busIds[busIndex] == 0)
        {
          busIndex++;
          continue;
        }

        time += delta;
        if ((time + busIndex) % busIds[busIndex] == 0)
        {
          delta *= busIds[busIndex];
          busIndex++;
        }
      }

      return time;
    }
  }
}