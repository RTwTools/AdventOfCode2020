using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc16
{
  public class Program
  {
    public static void Main()
    {
      var data = File.ReadAllLines("input.txt").ToList();
      var ticketValidator = TicketValidator.Parse(data);
      var nearbyTickets = Ticket.Parse(data.Skip(data.IndexOf("nearby tickets:") + 1));

      foreach (var ticket in nearbyTickets)
      {
        ticketValidator.Validate(ticket);
      }

      Console.WriteLine($"Part 1: the ticket scanning error rate is {nearbyTickets.Sum(t => t.Fields.Where(f => !f.Item2).Sum(t => t.Item1))}");

      var validTickets = nearbyTickets.Where(t => t.Fields.All(f => f.Item2));
      var myTicket = Ticket.Parse(data[(data.IndexOf("your ticket:") + 1)]);
      var fieldNames = ticketValidator.FindFieldNames(validTickets);
      var indexes = fieldNames.Where(f => f.Value.StartsWith("departure")).Select(f => f.Key);

      Console.WriteLine($"Part 2: the values is  {indexes.Select(i => (long)myTicket.Fields[i].Item1).Aggregate((a, x) => a * x)}");
    }
  }

  public class TicketValidator
  {
    public Dictionary<string, List<(int start, int end)>> Rules { get; set; } = new Dictionary<string, List<(int start, int end)>>();

    public Dictionary<int, string> FindFieldNames(IEnumerable<Ticket> tickets)
    {
      var fieldNames = new Dictionary<int, string>();

      while (Rules.Any())
      {
        foreach (var rule in Rules)
        {
          int validId = -1;

          for (int i = 0; i < tickets.First().Fields.Count; i++)
          {
            if (fieldNames.ContainsKey(i)) continue;

            var values = tickets.Select(t => t.Fields[i].Item1);

            var valid = true;

            var rule1 = rule.Value.First();
            var rule2 = rule.Value.Last();
            valid &= values.All(v => ((rule1.start <= v && rule1.end >= v) || (rule2.start <= v && rule2.end >= v)));

            if (valid)
            {
              if (validId != -1)
              {
                validId = -1;
                break;
              }
              else
              {
                validId = i;
              }
            }
          }

          if (validId != -1)
          {
            fieldNames.Add(validId, rule.Key);
          }
        }

        foreach (var name in fieldNames.Values)
        {
          Rules.Remove(name);
        }
      }

      

      return fieldNames;
    }

    public void Validate(Ticket ticket)
    {
      for (int i = 0; i < ticket.Fields.Count; i++)
      {
        foreach (var rule in Rules.Values)
        {
          foreach (var (start, end) in rule)
          {
            if (start <= ticket.Fields[i].Item1 && end >= ticket.Fields[i].Item1)
            {
              ticket.Fields[i] = (ticket.Fields[i].Item1, true);
              break;
            }
          }

          if (ticket.Fields[i].Item2) break;
        }
      }
    }

    public static TicketValidator Parse(IEnumerable<string> data)
    {
      var ticketValidator = new TicketValidator();

      foreach (var line in data)
      {
        if (string.IsNullOrEmpty(line))
        {
          break;
        }

        var parsedRule = ParseRule(line);
        ticketValidator.Rules.Add(parsedRule.Key, parsedRule.Value);
      }

      return ticketValidator;
    }

    private static KeyValuePair<string, List<(int start, int end)>> ParseRule(string rule)
    {
      var ranges = new List<(int start, int end)>();
      var name = rule.Split(":")[0];

      var splitData = rule.Split(new[] { '-', ' ' });
      ranges.Add((int.Parse(splitData[^5]), int.Parse(splitData[^4])));
      ranges.Add((int.Parse(splitData[^2]), int.Parse(splitData[^1])));

      return new KeyValuePair<string, List<(int start, int end)>>(name, ranges);
    }

  }

  public record Ticket(IList<(int, bool)> Fields)
  {
    public static Ticket Parse(string data)
    {
      return new Ticket(data.Split(',').Select(d => (int.Parse(d), false)).ToList());
    }

    public static IList<Ticket> Parse(IEnumerable<string> data)
    {
      return data.Select(Parse).ToList();
    }
  }
}