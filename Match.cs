using System;
using System.Net.WebSockets;
using System.Numerics;
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
        HomeTeam.MatchesPlayed++;
        AwayTeam.MatchesPlayed++;
        for (int i = 0; i < HomeLineup.Count; i++)
        {
            var player = HomeLineup[i];
            if (i < 11)
            {
                HomeLineup[i] = Tuple.Create(player.Item1, player.Item2 <= 0 ? player.Item2 + 90 : player.Item2);
            }
            var playerIndex = HomeTeam.Players.IndexOf(player.Item1);
            HomeTeam.Players[playerIndex].MinutesPlayed += HomeLineup[i].Item2;
            HomeTeam.Players[playerIndex].MatchesPlayed++;
            HomeTeam.Players[playerIndex].Absence = Math.Max(0, HomeTeam.Players[playerIndex].Absence - 1);
        }
        for (int i = 0; i < AwayLineup.Count; i++)
        {
            var player = AwayLineup[i];
            if (i < 11)
            {
                AwayLineup[i] = Tuple.Create(player.Item1, player.Item2 <= 0 ? player.Item2 + 90 : player.Item2);
            }
            var playerIndex = AwayTeam.Players.IndexOf(player.Item1);
            AwayTeam.Players[playerIndex].MinutesPlayed += AwayLineup[i].Item2;
            AwayTeam.Players[playerIndex].MatchesPlayed++;
            AwayTeam.Players[playerIndex].Absence = Math.Max(0, AwayTeam.Players[playerIndex].Absence - 1);
        }
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

        // subs
        var homeLineupPositions = Enumerable.Range(0, HomeLineup.Count).ToList();
        var awayLineupPositions = Enumerable.Range(0, AwayLineup.Count).ToList();

        var sub2 = new SubstitutionEvent(HomeTeam, 43, HomeLineup[homeLineupPositions[9]].Item1, HomeLineup[homeLineupPositions[12]].Item1);
        (homeLineupPositions[12], homeLineupPositions[9]) = (homeLineupPositions[9], homeLineupPositions[12]);
        var sub = new SubstitutionEvent(HomeTeam, 70, HomeLineup[homeLineupPositions[9]].Item1, HomeLineup[homeLineupPositions[11]].Item1);
        (homeLineupPositions[11], homeLineupPositions[9]) = (homeLineupPositions[9], homeLineupPositions[11]);
        events.Add(sub2);
        events.Add(sub);

        return events;
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
                    if (goalEvent.Team == HomeTeam)
                    {
                        HomeGoals++;
                        AwayTeam.GoalsConceded++;
                    }
                    else
                    {
                        AwayGoals++;
                        HomeTeam.GoalsConceded++;
                    }
                    goalEvent.Team.GoalsScored++;
                    goalEvent.Player.Goals++;
                    if (goalEvent.Assist != null)
                    {
                        goalEvent.Assist.Assists++;
                    }
                    break;
                case YellowCardEvent yellowCardEvent:
                    yellowCardEvent.Player.YellowCards++;
                    yellowCardEvent.Team.YellowCards++;
                    break;
                case RedCardEvent redCardEvent:
                    redCardEvent.Player.RedCards++;
                    redCardEvent.Team.RedCards++;
                    redCardEvent.Player.Absence = 2;
                    break;
                case SubstitutionEvent substitutionEvent:
                    if (substitutionEvent.Team == HomeTeam)
                    {
                        var substitutedPlayerIndex = HomeLineup.FindIndex(x => x.Item1 == substitutionEvent.Player);
                        var substituteIndex = HomeLineup.FindIndex(x => x.Item1 == substitutionEvent.Substitute);

                        var substitutedPlayerMinutes = HomeLineup[substitutedPlayerIndex].Item2;
                        HomeLineup[substitutedPlayerIndex] = Tuple.Create(substitutionEvent.Substitute, -substitutionEvent.Minute);
                        HomeLineup[substituteIndex] = Tuple.Create(substitutionEvent.Player, substitutionEvent.Minute + substitutedPlayerMinutes);
                    }
                    else
                    {
                        var substitutedPlayerIndex = AwayLineup.FindIndex(x => x.Item1 == substitutionEvent.Player);
                        var substituteIndex = AwayLineup.FindIndex(x => x.Item1 == substitutionEvent.Substitute);

                        var substitutedPlayerMinutes = AwayLineup[substitutedPlayerIndex].Item2;
                        AwayLineup[substitutedPlayerIndex] = Tuple.Create(substitutionEvent.Substitute, -substitutionEvent.Minute);
                        AwayLineup[substituteIndex] = Tuple.Create(substitutionEvent.Player, substitutionEvent.Minute + substitutedPlayerMinutes);
                    }
                    break;
                case InjuryEvent injuryEvent:
                    injuryEvent.Player.Absence = injuryEvent.Duration;
                    injuryEvent.Player.Injuries++;
                    injuryEvent.Team.Injuries++;
                    break;
            }
        }
    }
}