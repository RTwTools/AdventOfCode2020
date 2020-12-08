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
      IReadOnlyList<Instruction> instructions = File.ReadAllLines("input.txt").Select(Instruction.Parse).ToList();

      Console.WriteLine($"Part 1: the value of the accumulator is {HandHeld.Execute(instructions).accumulator}.");

      int instruction = 1;
      while (instruction <= instructions.Count)
      {
        var newInstructions = instructions.Select(i => new Instruction(i.Command, i.Value)).ToList();
        var counter = 0;

        for (int i = 0; i < newInstructions.Count; i++)
        {
          if (newInstructions[i].Command == "jmp" || newInstructions[i].Command == "nop")
          {
            counter++;

            if (counter >= instruction)
            {
              if (newInstructions[i].Command == "nop" && newInstructions[i].Value == 0)
              {
                instruction++;
                continue;
              }

              newInstructions[i].Command = (newInstructions[i].Command == "jmp")
                ? "nop"
                : "jmp";

              break;
            }
          }
        }

        var result = HandHeld.Execute(newInstructions);

        if (result.ok)
        {
          Console.WriteLine($"Part 2: the value of the accumulator is {result.accumulator}.");

          break;
        }
        else
        {
          instruction++;
        }
      }

    }
  }

  public class HandHeld
  {
    public static (int accumulator, bool ok) Execute(IReadOnlyList<Instruction> instructions)
    {
      var Instructions = instructions.Select(i => new Instruction(i.Command, i.Value)).ToList();

      int currentInstruction = 0;
      int accumulator = 0;

      while (currentInstruction < Instructions.Count && !Instructions[currentInstruction].Executed)
      {
        Instructions[currentInstruction].Executed = true;

        switch (Instructions[currentInstruction].Command)
        {
          case "acc":
            accumulator += Instructions[currentInstruction].Value;
            currentInstruction++;
            break;
          case "jmp":
            currentInstruction += Instructions[currentInstruction].Value;
            break;
          default:
            currentInstruction++;
            break;
        }
      }

      var ok = currentInstruction == Instructions.Count;

      return (accumulator, ok);
    }
  }

  public class Instruction
  {
    public string Command { get; set; }
    public int Value { get; set; }
    public bool Executed { get; set; } = false;

    public Instruction(string command, int value)
    {
      Command = command;
      Value = value;
    }

    public static Instruction Parse(string data)
    {
      var splitData = data.Split(" ");
      return new Instruction(splitData[0], int.Parse(splitData[1]));
    }

    public override string ToString()
    {
      return $"{Command}:{Value}:{Executed}";
    }
  }
}
