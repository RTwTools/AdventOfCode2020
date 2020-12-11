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

      while (ferry.UpdateLayout()) ;

      Console.WriteLine($"Part 1: the number of occupied seats is {ferry.OccupiedSeats}");
    }

    public class Ferry
    {
      public string[] Seats { get; set; }

      public int OccupiedSeats 
      {
        get
        {
          return Seats.Sum(row => row.Count(location => location == '#'));
        }
      }

      public Ferry(string[] seats)
      {
        Seats = seats;
      }

      public bool UpdateLayout()
      {
        var newSeats = new List<string>();

        for (int y = 0; y < Seats.Length; y++)
        {
          string row = string.Empty;

          for (int x = 0; x < Seats[y].Length; x++)
          {
            var location = new Point(x, y);
            if (IsSeat(location))
            {
              var count = CountOccupiedNeighbors(location);

              if (IsOccupied(location))
              {
                if (count >= 4)
                {
                  row += "L";
                }
                else
                {
                  row += Seats[y][x];
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
                  row += Seats[y][x];
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

        var changed = (newSeats.SequenceEqual(Seats) == false);
        Seats = newSeats.ToArray();
        return changed;
      }


        public int CountOccupiedNeighbors(Point seat)
        {
          var neighbors = new List<bool>();
          (int x, int y) = (seat.X, seat.Y);

          if (x - 1 >= 0)
          {
            if (y - 1 >= 0)
            {
              neighbors.Add(IsOccupied(new Point(x - 1, y - 1)));
            }

            neighbors.Add(IsOccupied(new Point(x - 1, y)));

            if (y + 1 < Seats.Length)
            {
              neighbors.Add(IsOccupied(new Point(x - 1, y + 1)));
            }
          }

          if (y - 1 >= 0)
          {
            neighbors.Add(IsOccupied(new Point(x, y - 1)));
          }

          if (y + 1 < Seats.Length)
          {
            neighbors.Add(IsOccupied(new Point(x, y + 1)));
          }

          if (x + 1 < Seats[y].Length)
          {
            if (y - 1 >= 0)
            {
              neighbors.Add(IsOccupied(new Point(x + 1, y - 1)));
            }

            neighbors.Add(IsOccupied(new Point(x + 1, y)));

            if (y + 1 < Seats.Length)
            {
              neighbors.Add(IsOccupied(new Point(x + 1, y + 1)));
            }
          }

          return neighbors.Count(s => s);
        }

        public bool IsOccupied(Point seat)
        {
          return GetLocation(seat) == '#';
        }

        public bool IsSeat(Point seat)
        {
          var location = GetLocation(seat);
          return (location == 'L' || location == '#');
        }

        private char GetLocation(Point point)
        {
          var row = Seats[point.Y];
          return row[point.X];
        }
      }
    }
  }