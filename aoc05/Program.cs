using System;
using System.IO;
using System.Linq;

namespace aoc05
{
  public class Program
  {
    public static void Main()
    {
      var seats = File.ReadAllLines("input.txt").Select(Seat.Parse).OrderBy(s => s.Id).ToList();

      Console.WriteLine($"Part 1: the highest seat ID is {seats.Last().Id}.");

      var seatId = seats[0].Id;

      for (int i = 1; i < seats.Count; i++)
      {
        if (seats[i].Id == seatId + 2)
        {
          Console.WriteLine($"Part 2: the ID of my seat is {seats[i].Id - 1}.");
          break;
        }
        seatId = seats[i].Id;
      }
    }
  }

  public record Seat(int Row, int Column)
  {
    public int Id { get => (Row * 8) + Column; }

    public static Seat Parse(string data)
    {
      (int, int) rowRange = (0, 127);

      foreach (var direction in data[0..7])
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

    private static (int start, int end) Split((int start, int end) range, PartToKeep partToKeep)
    {
      var size = (range.end - range.start) + 1;

      if (PartToKeep.First == partToKeep)
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
