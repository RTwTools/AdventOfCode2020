using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace aoc12
{
  public class Program
  {
    public static void Main()
    {
      var instructions = File.ReadAllLines("input.txt").Select(i => (i[0], int.Parse(i[1..])));

      var location = new Ship().ExecuteInstructions(instructions).Location;
      Console.WriteLine($"Part 1: the Manhatten distance is {Math.Abs(location.X) + Math.Abs(location.Y)}");
    }

    public enum Direction
    {
      North = 0,
      East = 90,
      South = 180,
      West = 270,
      Max = 360
    }

    public class Ship
    {
      public Point Location { get; set; }
      public Direction Direction { get; set; } = Direction.East;
      public IEnumerable<(string, int)> Directions { get; set; }

      public static Size GetMovement(Direction direction, int value)
      {
        return direction switch
        {
          Direction.North => new Size(0, value),
          Direction.East => new Size(value, 0),
          Direction.South => new Size(0, -value),
          Direction.West => new Size(-value, 0),
          _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };
      }

      public Ship ExecuteInstructions(IEnumerable<(char, int)> instructions)
      {
        foreach (var (action, value) in instructions)
        {
          System.Console.WriteLine($"Location: {Location.X} {Location.Y} \t {Direction} \t Next instruction {action}{value}");

          if (action == 'F')
          {
            Location += GetMovement(Direction, value);
          }
          else if (action == 'R')
          {
            Direction = (Direction)mod(((int)Direction + value), ((int)Direction.Max));
          }
          else if (action == 'L')
          {
            Direction = (Direction)mod(((int)Direction - value), ((int)Direction.Max));
          }
          else
          {
            Location += GetMovement(ParseDirection(action), value);
          }
        }

        return this;
      }

      public static Direction ParseDirection(char action)
      {
        return action switch
        {
          'N' => Direction.North,
          'E' => Direction.East,
          'S' => Direction.South,
          'W' => Direction.West,
          _ => throw new ArgumentOutOfRangeException(nameof(action)),
        };
      }

      static int mod(int k, int n)
      {
        return ((k %= n) < 0) ? k + n : k;
      }
    }
  }
}