using System;
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
    }
  }
}