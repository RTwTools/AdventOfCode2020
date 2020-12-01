using System;
using System.IO;
using System.Linq;

namespace aoc01
{
  class Program
  {
    static void Main(string[] args)
    {
      var numbers = File.ReadLines("input.txt").Select(int.Parse).ToList();

      for (int i = 0; i < numbers.Count; i++)
      {
        for (int j = i+1; j < numbers.Count; j++)
        {
          if (numbers[i] + numbers[j] == 2020)
          {
            Console.WriteLine($"The answer is: {numbers[i]} * {numbers[j]} = {numbers[i] * numbers[j]}.");
          }
        }
      }
    }
  }
}
