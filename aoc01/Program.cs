using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

void part1(IReadOnlyList<int> numbers)
{
  for (int i = 0; i < numbers.Count; i++)
  {
    for (int j = i + 1; j < numbers.Count; j++)
    {
      if (numbers[i] + numbers[j] == 2020)
      {
        Console.WriteLine($"Part 1: the answer is: {numbers[i]} * {numbers[j]} = {numbers[i] * numbers[j]}.");
      }
    }
  }
}

void part2(IReadOnlyList<int> numbers)
{
  for (int i = 0; i < numbers.Count; i++)
  {
    for (int j = i + 1; j < numbers.Count; j++)
    {
      for (int k = j + 1; k < numbers.Count; k++)
      {
        if (numbers[i] + numbers[j] + numbers[k] == 2020)
        {
          Console.WriteLine($"Part 2: the answer is: {numbers[i]} * {numbers[j]} * {numbers[k]} = {numbers[i] * numbers[j] * numbers[k]}.");
        }
      }
    }
  }
}

var numbers = File.ReadLines("input.txt").Select(int.Parse).ToList();

part1(numbers);
part2(numbers);
