﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace aoc03
{
  public class Program
  {
    public static void Main()
    {
      var grid = new Grid(File.ReadAllLines("input.txt"));

      Console.WriteLine($"Part 1: the number of encountered trees is {grid.TravelGetNumberOfTrees(new Point(), new Size(3,1))}.");

      long numberOfTrees = new long[]
      {
        grid.TravelGetNumberOfTrees(new Point(), new Size(1,1)),
        grid.TravelGetNumberOfTrees(new Point(), new Size(3,1)),
        grid.TravelGetNumberOfTrees(new Point(), new Size(5,1)),
        grid.TravelGetNumberOfTrees(new Point(), new Size(7,1)),
        grid.TravelGetNumberOfTrees(new Point(), new Size(1,2))
      }.Aggregate((a, x) => a * x);

      Console.WriteLine($"Part 2: the number of encountered trees is {numberOfTrees}.");
    }

    public class Grid
    {
      public int Heigth { get { return Map.Length; } }
      public int Width { get { return Map[0].Length; } }

      private string[] Map { get; set; }

      public Grid(string[] grid)
      {
        Map = grid;
      }

      public bool IsOpen(Point point)
      {
        return (GetLocation(point) == '.');
      }

      public bool IsTree(Point point)
      {
        return (GetLocation(point) == '#');
      }

      public int TravelGetNumberOfTrees(Point start, Size move)
      {
        var myLocation = start;

        int numberOfTrees = 0;

        while (myLocation.Y < Heigth)
        {
          if (IsTree(myLocation))
          {
            numberOfTrees++;
          }

          myLocation += move;
        }

        return numberOfTrees;
      }

      private char GetLocation(Point point)
      {
        var row = Map[point.Y];
        return row[point.X % row.Length];
      }
    }
  }
}


