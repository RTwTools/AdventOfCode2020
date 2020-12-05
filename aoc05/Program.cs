using System;
using System.IO;
using System.Linq;

namespace aoc05
{
  public class Program
  {
    public static void Main()
    {
      var seats = File.ReadAllLines("input.txt").Select(Seat.Parse);

      Console.WriteLine($"Part 1: the highest seat ID is {seats.Max(s => s.Id)}.");
    }
  }

  public record Seat(int Row, int Column)
  {
    public int Id { get => (Row * 8) + Column; }

    public static Seat Parse(string data)
    {
      (int, int) rowRange = (0, 127);

      foreach (var direction in data[0..6])
      {
        rowRange = Split(rowRange, (direction == 'F') ? PartToKeep.First : PartToKeep.Last);
      }

      (int, int) columnRange = (0, 8);

      foreach (var direction in data[7..])
      {
        columnRange = Split(columnRange, (direction == 'L') ? PartToKeep.First : PartToKeep.Last);
      }

      return new Seat(rowRange.Item1, columnRange.Item1);
    }

    enum PartToKeep
    {
      First,
      Last
    }

    private static (int start, int end) Split((int start, int end) range, PartToKeep halfToKeep)
    {
      var size = (range.end - range.start) + 1;

      if (PartToKeep.First == halfToKeep)
      {
        return (range.start, range.end - (size / 2));
      }
      else
      {
        return (range.start + (size / 2), range.end);
      }
    }
  }
}
