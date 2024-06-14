﻿using System.Text;
using System.Text.RegularExpressions;

namespace FootballManager;

/// <summary>   Represents a round of matches in a football league. </summary>
public class Round
{
    /// <summary>   Gets or sets the matches for this round. </summary>
    ///
    /// <value> A list of matches. </value>
    public List<Match> Matches { get; set; }

    /// <summary>   Simulates all the matches in this round, updates the points, and displays the results. </summary>
    public void Simulate()
    {
        foreach (var match in Matches)
        {
            match.Simulate();
            if (match.HomeGoals > match.AwayGoals)
            {
                match.HomeTeam.Points += 3;
                match.HomeTeam.Wins++;
                match.AwayTeam.Losses++;
            } else if (match.HomeGoals < match.AwayGoals)
            {
                match.AwayTeam.Points += 3;
                match.AwayTeam.Wins++;
                match.HomeTeam.Losses++;
            } else
            {
                match.HomeTeam.Points++;
                match.AwayTeam.Points++;
                match.HomeTeam.Draws++;
                match.AwayTeam.Draws++;
            }
            Console.WriteLine("{0,-25} {3,-25} {1,2}:{2,-2}", match.HomeTeam.Name, match.HomeGoals, match.AwayGoals, match.AwayTeam.Name);

            if (Matches.IndexOf(match) != 0) continue;
            Console.WriteLine();
            Console.WriteLine($"{match.HomeTeam.Name}'s lineup: ");
            PrintLineup(match.HomeLineup.ConvertAll(x=>x.Item1), match.HomeTeam);

            Console.WriteLine($"{match.AwayTeam.Name}'s lineup: ");
            PrintLineup(match.AwayLineup.ConvertAll(x => x.Item1), match.AwayTeam);

            Console.WriteLine("Match events:");
            match.Events.OrderBy(eventItem => eventItem.Minute).ToList().ForEach(eventItem => eventItem.PrintDetails());

            Console.WriteLine();
            Console.WriteLine("Other matches: ");
        }
    }

    /// <summary>   Prints the lineup of players for the specified team. </summary>
    ///
    /// <param name="lineup">   The list of players in the lineup.  </param>
    /// <param name="team">     The team whose lineup is being printed. </param>
    private static void PrintLineup(IReadOnlyList<Player> lineup, Team team)
    {
        var playerIndex = 1;
        var defence = "";
        for (var i = 0; i < team.Formation.Defenders; i++)
        {
            defence += $"{lineup[playerIndex].Name}({lineup[playerIndex++].Overall}) - ";
        }
        defence = defence[..^3];
        var midfield = "";
        for (var i = 0; i < team.Formation.Midfielders; i++)
        {
            midfield += $"{lineup[playerIndex].Name}({lineup[playerIndex++].Overall}) - ";
        }
        midfield = midfield[..^3];
        var attack = "";
        for (var i = 0; i < team.Formation.Forwards; i++)
        {
            attack += $"{lineup[playerIndex].Name}({lineup[playerIndex++].Overall}) - ";
        }
        attack = attack[..^3];

        Console.WriteLine($"Goalkeeper: {lineup[0].Name}({lineup[0].Overall})");
        Console.WriteLine($"Defence: {defence}");
        Console.WriteLine($"Midfield: {midfield}");
        Console.WriteLine($"Attack: {attack}");
        Console.WriteLine();
    }
}