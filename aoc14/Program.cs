using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc14
{
  public class Program
  {
    public static void Main()
    {
      var data = File.ReadAllLines("input.txt");

      Console.WriteLine($"Part 1: the sum of the data in all the registers is {new Computer().Initialize(data).Registers.Values.Sum()}");
      Console.WriteLine($"Part 1: the sum of the data in all the registers is {new Computer().Decode(data).Registers.Values.Sum()}");
    }

    public class Computer
    {
      public Dictionary<long, long> Registers { get; set; } = new Dictionary<long, long>();
      public BitMask Mask { get; set; }

      public Computer Initialize(string[] instructions)
      {
        foreach (var instruction in instructions)
        {
          var splitData = instruction.Split(new char[] { '[', ']', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);

          if (instruction.StartsWith("mask"))
          {
            Mask = BitMask.Parse(splitData[1]);
          }
          else if (instruction.StartsWith("mem"))
          {
            var address = long.Parse(splitData[1]);
            var value = Mask.Apply(long.Parse(splitData[2]));

            SaveToRegister(address, value);
          }
        }

        return this;
      }

      public Computer Decode(string[] instructions)
      {
        foreach (var instruction in instructions)
        {
          var splitData = instruction.Split(new char[] { '[', ']', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);

          if (instruction.StartsWith("mask"))
          {
            Mask = BitMask.Parse(splitData[1]);
          }
          else if (instruction.StartsWith("mem"))
          {
            var address = Mask.SetBits(long.Parse(splitData[1]));
            var value = long.Parse(splitData[2]);

            for (int floatNr = 0; floatNr < Math.Pow(2, Mask.Float.Count); floatNr++)
            {
              var tempAddress = address;

              for (int bitIndex = 0; bitIndex < Mask.Float.Count; bitIndex++)
              {
                var bitValue = ((floatNr >> bitIndex) & 0x1);
                tempAddress = UpdateBit(tempAddress, Mask.Float[bitIndex], bitValue);
              }

              SaveToRegister(tempAddress, value);
            }
          }
        }

        return this;
      }

      private static long UpdateBit(long input, int bitNr, int bitValue)
      {
        if (bitValue >= 1)
        {
          return input | ((long)1 << bitNr);
        }
        else
        {
          return input & (~((long)1 << bitNr) & 0xFFFFFFFFF);
        }
      }

      private void SaveToRegister(long address, long value)
      {
        if (Registers.ContainsKey(address))
        {
          Registers[address] = value;
        }
        else
        {
          Registers.Add(address, value);
        }
      }
    }

    public record BitMask(long Clear, long Set, IList<int> Float)
    {
      public long SetBits(long input)
      {
        return input | Set;
      }

      public long Apply(long input)
      {
        return input & Clear | Set;
      }

      public static BitMask Parse(string data)
      {
        long set = 0;
        long clear = 0xFFFFFFFFF;
        var @float = new List<int>();

        for (int i = 0; i < data.Length; i++)
        {
          int index = data.Length - (i + 1);

          if (data[index] == '1')
          {
            set += ((long)1 << i);
          }
          if (data[index] == '0')
          {
            clear &= ~((long)1 << i);
          }
          if (data[index] == 'X')
          {
            @float.Add(i);
          }
        }

        return new BitMask(clear, set, @float);
      }
    }
  }
}