using System;
using System.Runtime.InteropServices;
using FootballManager.Events;
namespace FootballManager;

public class Match(Team homeTeam, Team awayTeam)
{
    public Team HomeTeam { get; set; } = homeTeam;
    public Team AwayTeam { get; set; } = awayTeam;
    public int HomeGoals { get; set; } = 0;
    public int AwayGoals { get; set; } = 0;
    public List<Tuple<Player, int>> HomeLineup { get; set; }
    public List<Tuple<Player, int>> AwayLineup { get; set; }
    public List<Event> Events { get; set; }

    public void Simulate()
    {
        HomeLineup = HomeTeam.GetSquad();
        AwayLineup = AwayTeam.GetSquad();
        Events = DrawEvents();
        UpdateStats();
    }

    private List<Event> DrawEvents()
    {
        // TODO: Implement this method (drawing match events)
        var rand = new Random();
        var teams = new List<Team>{HomeTeam, AwayTeam};
        var lineups = new List<List<Tuple<Player, int>>>{HomeLineup, AwayLineup};

        var events = new List<Event>();

        var yellowCards = DrawWithProbability([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [5, 5, 20, 20, 20, 10, 10, 3, 3, 2, 2]);
        for (var i = 0; i < yellowCards; i++)
        {
            var teamIndex = rand.Next(2);
            events.Add(new YellowCardEvent(
                teams[teamIndex], 
                rand.Next(1, 90), 
                lineups[teamIndex][rand.Next(0, 11)].Item1)
            );
        }
        var redCards = DrawWithProbability([0, 1, 2], [90, 9, 1]);
        for (var i = 0; i < redCards; i++)
        {
            var teamIndex = rand.Next(2);
            events.Add(new RedCardEvent(
                teams[teamIndex], 
                rand.Next(1, 90), 
                lineups[teamIndex][rand.Next(0, 11)].Item1)
            );
        }

        var injuries = DrawWithProbability([0, 1, 2], [95, 3, 2]);
        for (var i = 0; i < injuries; i++)
        {
            events.Add(new InjuryEvent(
                teams[rand.Next(2)], 
                rand.Next(1, 90), 
                lineups[rand.Next(2)][rand.Next(0, 11)].Item1,
                DrawWithProbability([0, 1, 2, 3, 4, 5], [2, 25, 30, 15, 10, 8]))
            );
        }

        return events;
        
        // For testing purposes
        //var goal1 = new GoalEvent(HomeTeam, 20, HomeLineup[0].Item1);
        //var goal2 = new GoalEvent(AwayTeam, 88, AwayLineup[0].Item1, AwayLineup[1].Item1);
        //var yellow = new YellowCardEvent(HomeTeam, 40, HomeLineup[0].Item1);
        //var red = new RedCardEvent(HomeTeam, 30, HomeLineup[4].Item1);
        //var sub = new SubstitutionEvent(HomeTeam, 70, HomeLineup[4].Item1, HomeLineup[9].Item1);
        //var injury = new InjuryEvent(AwayTeam, 31, AwayLineup[5].Item1, 4);
        //var injury2 = new InjuryEvent(HomeTeam, 1, HomeLineup[2].Item1, 1);

        //return [goal1, goal2, yellow, red, sub, injury, injury2];
    }

    private T DrawWithProbability<T>(List<T> items, List<double> probabilities)
    {
        var rand = new Random();

        if (items.Count != probabilities.Count)
        {
            throw new ArgumentException("List of items and list of probabilities must have the same length.");
        }

        var total = probabilities.Sum();

        var randomValue = rand.NextDouble() * total;

        double cumulative = 0;
        for (var i = 0; i < items.Count; i++)
        {
            cumulative += probabilities[i];
            if (randomValue <= cumulative)
            {
                return items[i];
            }
        }

        return items[^1];
    }

    private void UpdateStats()
    {
        // TODO: Implement this method (updating players and teams stats)
        foreach (var matchEvent in Events)
        {
            switch (matchEvent)
            {
                case GoalEvent goalEvent:
                    break;
                case YellowCardEvent yellowCardEvent:
                    break;
                case RedCardEvent redCardEvent:
                    break;
                case SubstitutionEvent substitutionEvent:
                    break;
                case InjuryEvent injuryEvent:
                    break;
            }
        }
    }
}