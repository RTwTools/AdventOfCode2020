using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace aoc11
{
  public class Program
  {
    public static void Main()
    {
      var ferry = new Ferry(File.ReadAllLines("input.txt"));
      while (ferry.UpdateLayoutPart1()) ;

      Console.WriteLine($"Part 1: the number of occupied seats is {ferry.OccupiedSeats}");

      ferry = new Ferry(File.ReadAllLines("input.txt"));
      while (ferry.UpdateLayoutPart2()) ;

      Console.WriteLine($"Part 2: the number of occupied seats is {ferry.OccupiedSeats}");
    }

    public class Ferry
    {
      private string[] Grid { get; set; }

      private Size[] Neighbors { get; } = new[]
      {
        new Size(-1, -1),
        new Size(-1, 0),
        new Size(-1, 1),
        new Size(0, -1),
        new Size(0, 1),
        new Size(1, -1),
        new Size(1, 0),
        new Size(1, 1)
      };

      public int OccupiedSeats
      {
        get
        {
          return Grid.Sum(row => row.Count(location => location == '#'));
        }
      }

      public Ferry(string[] seats)
      {
        Grid = seats;
      }

      public bool UpdateLayoutPart1()
      {
        return UpdateLayout(4, (Point current, Size direction) => IsOccupied(current + direction));
      }

      public bool UpdateLayoutPart2()
      {
        return UpdateLayout(5, (Point current, Size direction) => SeesOccupied(current, direction));
      }

      private bool UpdateLayout(int tolerance, Func<Point, Size, bool> neighborIsOccupied)
      {
        var newSeats = new List<string>();

        for (int y = 0; y < Grid.Length; y++)
        {
          string row = string.Empty;

          for (int x = 0; x < Grid[y].Length; x++)
          {
            var location = new Point(x, y);
            if (IsSeat(location))
            {
              var count = CountOccupiedNeighbors(location, neighborIsOccupied);

              if (IsOccupied(location))
              {
                if (count >= tolerance)
                {
                  row += "L";
                }
                else
                {
                  row += Grid[y][x];
                }
              }
              else
              {
                if (count == 0)
                {
                  row += "#";
                }
                else
                {
                  row += Grid[y][x];
                }
              }
            }
            else
            {
              row += ".";
            }
          }

          newSeats.Add(row);
        }

        var changed = (newSeats.SequenceEqual(Grid) == false);
        Grid = newSeats.ToArray();
        return changed;
      }


      private bool IsValid(Point location)
      {
        return (location.X >= 0) && (location.X < Grid[0].Length) &&
          (location.Y >= 0) && (location.Y < Grid.Length);
      }

      private int CountOccupiedNeighbors(Point seat, Func<Point, Size, bool> neighborIsOccupied)
      {
        return Neighbors.Count(n => neighborIsOccupied(seat, n));
      }

      private bool IsFree(Point location)
      {
        return IsValid(location) && GetStatus(location) == 'L';
      }

      private bool IsOccupied(Point location)
      {
        return IsValid(location) && GetStatus(location) == '#';
      }

      private bool SeesOccupied(Point seat, Size direction)
      {
        var location = seat + direction;

        while (IsValid(location))
        {
          if (IsFree(location))
          {
            return false;
          }
          else if (IsOccupied(location))
          {
            return true;
          }
          else
          {
            location += direction;
          }
        }

        return false;
      }

      private bool IsSeat(Point location)
      {
        var status = GetStatus(location);
        return (status == 'L' || status == '#');
      }

      private char GetStatus(Point location)
      {
        var row = Grid[location.Y];
        return row[location.X];
      }
    }
  }
}