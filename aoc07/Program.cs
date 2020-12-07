using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc07
{
  public class Program
  {
    public static void Main()
    {
      var bags = Bag.ParseMultiple(File.ReadAllLines("input.txt"));
      var bagName = "shiny gold";

      Console.WriteLine($"Part 1: the number of bags is {bags[bagName].UniqueParents(new HashSet<string>())}.");
      Console.WriteLine($"Part 2: the number of bags is {bags[bagName].TotalChildren}.");
    }

    public record Bag(string Name)
    {
      public IList<Bag> Parents { get; set; } = new List<Bag>();
      public IList<Bag> Children { get; set; } = new List<Bag>();

      public int TotalChildren
      {
        get
        {
          int count = 0;
          count += Children.Count;

          foreach (var child in Children)
          {
            count += child.TotalChildren;
          }

          return count;
        }
      }

      public int UniqueParents(HashSet<string> parents)
      {
        foreach (var parent in Parents)
        {
          parents.Add(parent.Name);
          parent.UniqueParents(parents);
        }

        return parents.Count;
      }

      public static IDictionary<string, Bag> ParseMultiple(IEnumerable<string> Data)
      {
        var bags = Data.Select(d => d.Split(" bags contain ")[0]).Select(d => new Bag(d));

        var bagDictionary = new Dictionary<string, Bag>();
        foreach (var bag in bags)
        {
          bagDictionary.Add(bag.Name, bag);
        }

        // Link all bags.
        var splitItems = new[] { " bags contain ", "bags", "bag", ".", ",", "no other bags." };
        foreach (var item in Data)
        {
          var bagData = item.Split(splitItems, StringSplitOptions.RemoveEmptyEntries).Select(b => b.Trim()).ToList();

          var currentBag = bagDictionary[bagData[0]];
          foreach (var bag in bagData.Skip(1))
          {
            int numberOfBags = int.Parse(bag.Split(' ')[0]);
            string bagName = bag.Substring(bag.IndexOf(' ') + 1);
            for (int i = 0; i < numberOfBags; i++)
            {
              if (!bagDictionary.ContainsKey(bagName))
              {
                bagDictionary.Add(bagName, new Bag(bagName));
              }

              currentBag.Children.Add(bagDictionary[bagName]);
              bagDictionary[bagName].Parents.Add(currentBag);
            }
          }
        }

        return bagDictionary;
      }
    }
  }
}
