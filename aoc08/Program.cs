using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc08
{
  public class Program
  {
    public static void Main()
    {
      var instructions = File.ReadAllLines("input.txt").Select(Instruction.Parse).ToList();

      Console.WriteLine($"Part 1: the value of the accumulator is {HandHeld.Execute(instructions)}.");
    }
  }

  public class HandHeld
  {
    public static int Execute(IList<Instruction> instructions)
    {
      var Instructions = new List<Instruction>(instructions);

      int currentInstruction = 0;
      int accumulator = 0;

      while (!Instructions[currentInstruction].Executed)
      {
        accumulator += Instructions[currentInstruction].Value;
        Instructions[currentInstruction].Executed = true;
        currentInstruction += Instructions[currentInstruction].NextInstruction;
      }

      return accumulator;
    }
  }

  public record Instruction(int Value, int NextInstruction = 1)
  {
    public bool Executed { get; set; } = false;

    public static Instruction Parse(string data)
    {
      var splitData = data.Split(" ");
      var value = int.Parse(splitData[1]);

      return (splitData[0]) switch
      {
        "acc" => new Instruction(value),
        "jmp" => new Instruction(0, value),
        "nop" => new Instruction(0),
        _ => throw new InvalidDataException(),
      };
    }
  }
}
