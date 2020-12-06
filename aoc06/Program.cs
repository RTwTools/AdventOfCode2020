using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc06
{
  public class Program
  {
    public static void Main()
    {
      var forms = DeclarationForm.ParseMultiple(File.ReadAllLines("input.txt"));

      Console.WriteLine($"Part 1: the sum is {forms.Sum(f => f.Answers.Values.Count)}.");
    }
  }

  public record DeclarationForm(Dictionary<char, int> Answers)
  {

    public static DeclarationForm Parse(IEnumerable<string> data)
    {
      var answers = new Dictionary<char, int>();

      foreach (var form in data)
      {
        foreach (var letter in form)
        {
            if (answers.ContainsKey(letter))
            {
              answers[letter]++;
            }
            else
            {
              answers.Add(letter, 1);
            }
        }
      }

      return new DeclarationForm(answers);
    }

    public static IEnumerable<DeclarationForm> ParseMultiple(string[] data)
    {
      var forms = new List<DeclarationForm>();
      var group = new List<string>();

      foreach (var line in data)
      {
        if (string.IsNullOrEmpty(line) && group.Any())
        {
          forms.Add(Parse(group));
          group.Clear();
        }
        else
        {
          group.Add(line);
        }
      }

      if (group.Any())
      {
        forms.Add(Parse(group));
      }

      return forms;
    }
  }
}
