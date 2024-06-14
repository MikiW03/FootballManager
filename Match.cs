﻿using System;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using FootballManager.Events;
namespace FootballManager;

/// <summary> Represents a football match between two teams. </summary>
///
/// <param name="homeTeam"> The home team participating in the match. </param>
/// <param name="awayTeam"> The away team participating in the match. </param>

public class Match(Team homeTeam, Team awayTeam)
{
    /// <summary>   Gets the home team participating in the match. </summary>
    ///
    /// <value> The home team participating in the match. </value>
    public Team HomeTeam { get; } = homeTeam;
    /// <summary>   Gets the away team participating in the match. </summary>
    ///
    /// <value> The away team participating in the match. </value>
    public Team AwayTeam { get; } = awayTeam;
    /// <summary>   Gets or sets the number of goals scored by the home team. </summary>
    ///
    /// <value> The number of goals scored by the home team. </value>
    public int HomeGoals { get; set; }
    /// <summary>   Gets or sets the number of goals scored by the away team. </summary>
    ///
    /// <value> The number of goals scored by the away team. </value>
    public int AwayGoals { get; set; }
    /// <summary>   Gets or sets the lineup of the home team for the match. </summary>
    ///
    /// <value> The lineup of the home team for the match. </value>
    public List<Tuple<Player, int>> HomeLineup { get; set; }
    /// <summary>   Gets or sets the lineup of the away team for the match. </summary>
    ///
    /// <value> The lineup of the away team for the match. </value>
    public List<Tuple<Player, int>> AwayLineup { get; set; }
    /// <summary>   Gets or sets the list of events that occurred during the match. </summary>
    ///
    /// <value> The list of events that occurred during the match. </value>
    public List<Event> Events { get; set; }

    /// <summary>   Simulates the match by processing player lineups, events, and updating statistics. </summary>
    public void Simulate()
    {
        HomeLineup = HomeTeam.GetSquad();
        AwayLineup = AwayTeam.GetSquad();
        Events = DrawEvents();
        UpdateStats();
        HomeTeam.MatchesPlayed++;
        AwayTeam.MatchesPlayed++;
        for (var i = 0; i < HomeLineup.Count; i++)
        {
            var player = HomeLineup[i];
            if (i < 11)
            {
                HomeLineup[i] = Tuple.Create(player.Item1, player.Item2 <= 0 ? player.Item2 + 90 : player.Item2);
            }
            var playerIndex = HomeTeam.Players.IndexOf(player.Item1);
            HomeTeam.Players[playerIndex].MinutesPlayed += HomeLineup[i].Item2;
            if(HomeLineup[i].Item2 > 0)
            {
                HomeTeam.Players[playerIndex].MatchesPlayed++;
            }
            HomeTeam.Players[playerIndex].Absence = Math.Max(0, HomeTeam.Players[playerIndex].Absence - 1);
        }
        for (var i = 0; i < AwayLineup.Count; i++)
        {
            var player = AwayLineup[i];
            if (i < 11)
            {
                AwayLineup[i] = Tuple.Create(player.Item1, player.Item2 <= 0 ? player.Item2 + 90 : player.Item2);
            }
            var playerIndex = AwayTeam.Players.IndexOf(player.Item1);
            AwayTeam.Players[playerIndex].MinutesPlayed += AwayLineup[i].Item2;
            if (AwayLineup[i].Item2 > 0)
            {
                AwayTeam.Players[playerIndex].MatchesPlayed++;
            }
            AwayTeam.Players[playerIndex].Absence = Math.Max(0, AwayTeam.Players[playerIndex].Absence - 1);
        }
    }

    /// <summary>   Draws events (substitutions, injuries, cards, goals) that occur during the match. </summary>
    ///
    /// <returns>   A list of events that occurred during the match.; </returns>

    private List<Event> DrawEvents()
    {
        var rand = new Random();
        var teams = new List<Team>{HomeTeam, AwayTeam};
        var lineups = new List<List<Tuple<Player, int>>>{HomeLineup, AwayLineup};
        var events = new List<Event>();

        // subs
        var homeSubs = new List<List<int>>();
        var awaySubs = new List<List<int>>();

        var homeSubsAvailable = HomeLineup[11..];
        var awaySubsAvailable = AwayLineup[11..];

        var homeLineupPositions = Enumerable.Range(0, HomeLineup.Count).ToList();
        var awayLineupPositions = Enumerable.Range(0, AwayLineup.Count).ToList();
        for (var i = 0; i < 3; i++)
        {
            if (homeSubsAvailable.Count != 0)
            {
                var homeSub = new List<int>
                {
                    rand.Next(60 + i * 10, 60 + (i + 1) * 10),
                    homeLineupPositions[rand.Next(0, 11)],
                    homeLineupPositions[HomeLineup.IndexOf(homeSubsAvailable[0])]
                };

                var sub = new SubstitutionEvent(HomeTeam, homeSub[0], HomeLineup[homeSub[1]].Item1, HomeLineup[homeSub[2]].Item1);
                (homeLineupPositions[homeSub[2]], homeLineupPositions[homeSub[1]]) = (homeLineupPositions[homeSub[1]], homeLineupPositions[homeSub[2]]);

                homeSubsAvailable.RemoveAt(0);
                homeSubs.Add(homeSub);

                events.Add(sub);
            }

            if (awaySubsAvailable.Count != 0)
            {
                var awaySub = new List<int>
                {
                    rand.Next(60 + i * 10, 60 + (i + 1) * 10),
                    awayLineupPositions[rand.Next(0, 11)],
                    awayLineupPositions[AwayLineup.IndexOf(awaySubsAvailable[0])]
                };

                var sub = new SubstitutionEvent(AwayTeam, awaySub[0], AwayLineup[awaySub[1]].Item1, AwayLineup[awaySub[2]].Item1);
                (awayLineupPositions[awaySub[2]], awayLineupPositions[awaySub[1]]) = (awayLineupPositions[awaySub[1]], awayLineupPositions[awaySub[2]]);

                awaySubsAvailable.RemoveAt(0);
                awaySubs.Add(awaySub);

                events.Add(sub);
            }
        }

        // injuries
        var injuries = DrawWithProbability([0, 1], [95, 5]);
        for (var i = 0; i < injuries; i++)
        {
            var teamIndex = rand.Next(2);
            var minute = rand.Next(1, 90);
            var playerIndex = rand.Next(0, 11);
            playerIndex = NewPlayerIndex(teamIndex==0?homeSubs:awaySubs, minute, playerIndex);

            events.Add(new InjuryEvent(
                teams[teamIndex],
                minute,
                lineups[teamIndex][playerIndex].Item1,
                DrawWithProbability([0, 1, 2], [10, 80, 10]))
            );

            if (teamIndex == 0 && homeSubsAvailable.Count != 0)
            {
                var homeSub = new List<int>
                {
                    minute,
                    playerIndex,
                    homeLineupPositions[HomeLineup.IndexOf(homeSubsAvailable[0])]
                };

                var sub = new SubstitutionEvent(HomeTeam, homeSub[0], HomeLineup[homeSub[1]].Item1, HomeLineup[homeSub[2]].Item1);
                (homeLineupPositions[homeSub[2]], homeLineupPositions[homeSub[1]]) = (homeLineupPositions[homeSub[1]], homeLineupPositions[homeSub[2]]);

                homeSubsAvailable.RemoveAt(0);
                homeSubs.Add(homeSub);

                events.Add(sub);
            }

            if (teamIndex == 1 && awaySubsAvailable.Count != 0)
            {
                var awaySub = new List<int>
                {
                    minute,
                    playerIndex,
                    awayLineupPositions[AwayLineup.IndexOf(awaySubsAvailable[0])]
                };

                var sub = new SubstitutionEvent(AwayTeam, awaySub[0], AwayLineup[awaySub[1]].Item1, AwayLineup[awaySub[2]].Item1);
                (awayLineupPositions[awaySub[2]], awayLineupPositions[awaySub[1]]) = (awayLineupPositions[awaySub[1]], awayLineupPositions[awaySub[2]]);

                awaySubsAvailable.RemoveAt(0);
                awaySubs.Add(awaySub);

                events.Add(sub);
            }
        }

        // red cards
        var redCards = DrawWithProbability([0, 1], [95, 5]);
        for (var i = 0; i < redCards; i++)
        {
            var teamIndex = rand.Next(2);
            var minute = rand.Next(1, 90);
            var playersWithoutRedCard = teams[teamIndex].Players.Where(player => !(events.Any(e => e is RedCardEvent && e.Player == player))).ToList().ConvertAll(p => teams[teamIndex].Players.IndexOf(p)).ToList();
            var playerIndex = playersWithoutRedCard[rand.Next(0, playersWithoutRedCard.Where(x => x < 11).ToList().Count)];
            playerIndex = NewPlayerIndex(teamIndex == 0 ? homeSubs : awaySubs, minute, playerIndex);
            var playerIndexAndMinute = NewRedCardsPlayerIndexAndMinutes(teamIndex == 0 ? homeSubs : awaySubs, minute, playerIndex);
            playerIndex = playerIndexAndMinute[0];
            minute = playerIndexAndMinute[1];

            events.Add(new RedCardEvent(
                teams[teamIndex],
                minute,
                (teamIndex == 0 ? HomeLineup : AwayLineup)[playerIndex].Item1)
            );
        }

        // yellow cards
        var yellowCards = DrawWithProbability([0, 1, 2, 3, 4, 5], [5, 35, 30, 10, 10, 10]);
        for (var i = 0; i < yellowCards; i++)
        {
            var teamIndex = rand.Next(2);
            var minute = rand.Next(1, 90);


            var playersWithoutRedCard = teams[teamIndex].Players.Where(player => !(events.Any(e => e is RedCardEvent && e.Player == player))).ToList().ConvertAll(p => teams[teamIndex].Players.IndexOf(p)).ToList();
            var playerIndex = playersWithoutRedCard[rand.Next(0, playersWithoutRedCard.Where(x=>x<11).ToList().Count)];

            var playerYellowCards = events.Where(x => x is YellowCardEvent && x.Player == (teamIndex == 0 ? HomeLineup : AwayLineup)[playerIndex].Item1).ToList();
            if (playerYellowCards.Count >= 1)
            {
                if(playerIndex == NewPlayerIndex(teamIndex == 0 ? homeSubs : awaySubs, minute, playerIndex) && playerIndex == NewRedCardsPlayerIndexAndMinutes(teamIndex == 0 ? homeSubs : awaySubs, minute, playerIndex)[0])
                {
                    events.Add(new YellowCardEvent(
                         teams[teamIndex],
                         minute,
                         (teamIndex == 0 ? HomeLineup : AwayLineup)[playerIndex].Item1)
                    );

                    playerYellowCards = events.Where(x => x is YellowCardEvent && x.Player == (teamIndex == 0 ? HomeLineup : AwayLineup)[playerIndex].Item1).ToList();
                    events.Add(new RedCardEvent(
                        teams[teamIndex],
                        Math.Max(playerYellowCards[0].Minute, playerYellowCards[1].Minute),
                        (teamIndex == 0 ? HomeLineup : AwayLineup)[playerIndex].Item1)
                    );
                }
                else
                {
                    playerIndex = NewPlayerIndex(teamIndex==0?homeSubs:awaySubs, minute, playerIndex);
                    var playerIndexAndMinute = NewRedCardsPlayerIndexAndMinutes(teamIndex == 0 ? homeSubs : awaySubs, minute, playerIndex);
                    playerIndex = playerIndexAndMinute[0];
                    minute = playerIndexAndMinute[1];
                    events.Add(new YellowCardEvent(
                        teams[teamIndex],
                        minute,
                        (teamIndex == 0 ? HomeLineup : AwayLineup)[playerIndex].Item1)
                    );
                }
            }
            else
            {
                playerIndex = NewPlayerIndex(teamIndex == 0 ? homeSubs : awaySubs, minute, playerIndex);
                events.Add(new YellowCardEvent(
                    teams[teamIndex],
                    minute,
                    (teamIndex == 0 ? HomeLineup : AwayLineup)[playerIndex].Item1)
                );
            }
        }
        
        //goals
        var homeTeamLineupOverall = Math.Round(HomeLineup.Average(player => player.Item1.Overall));
        var awayTeamLineupOverall = Math.Round(AwayLineup.Average(player => player.Item1.Overall));

        var homeExpectedGoals = ((double)HomeTeam.Attack / AwayTeam.Defense) * homeTeamLineupOverall / 100.0;
        var awayExpectedGoals = ((double)AwayTeam.Attack / HomeTeam.Defense) * awayTeamLineupOverall / 100.0;

        var homeGoals = Poisson(homeExpectedGoals);
        var awayGoals = Poisson(awayExpectedGoals);

        for (var i = 0; i < homeGoals; i++)
        {
            var minute = rand.Next(1, 90);
            var player = DrawWithProbability(HomeLineup[..11].ConvertAll(player => player.Item1), HomeLineup[..11].ConvertAll(player => (double)player.Item1.Attack));
            var playersWithoutRedCard = teams[0].Players.Where(player => !(events.Any(e => e is RedCardEvent && e.Player == player))).ToList().ConvertAll(p => teams[0].Players.IndexOf(p)).ToList();
            var playerIndex = playersWithoutRedCard[rand.Next(0, playersWithoutRedCard.Where(x=>x<11).ToList().Count)];
            playerIndex = NewPlayerIndex(homeSubs, minute, playerIndex);
            var assist = DrawWithProbability([true, false], [60, 40]);
            if (assist)
            {
                var assistPlayer = DrawWithProbability(HomeLineup[..11].ConvertAll(player => player.Item1), HomeLineup[..11].ConvertAll(player => (double)player.Item1.Attack));
                var assistPlayerIndex = HomeLineup.IndexOf(HomeLineup.Find(x => x.Item1 == assistPlayer));
                assistPlayerIndex = NewPlayerIndex(homeSubs, minute, assistPlayerIndex);
                events.Add(new GoalEvent(
                    HomeTeam,
                    minute,
                    HomeLineup[playerIndex].Item1,
                    HomeLineup[assistPlayerIndex].Item1
                    )
                );
            } else
            {
                events.Add(new GoalEvent(
                    HomeTeam,
                    minute,
                    HomeLineup[playerIndex].Item1
                    )
                );
            }
        }

        for (var i = 0; i < awayGoals; i++)
        {
            var minute = rand.Next(1, 90);
            var player = DrawWithProbability(AwayLineup[..11].ConvertAll(player => player.Item1), AwayLineup[..11].ConvertAll(player => (double)player.Item1.Attack));
            var playersWithoutRedCard = teams[1].Players.Where(player => !(events.Any(e => e is RedCardEvent && e.Player == player))).ToList().ConvertAll(p => teams[1].Players.IndexOf(p)).ToList();
            var playerIndex = playersWithoutRedCard[rand.Next(0, playersWithoutRedCard.Where(x => x < 11).ToList().Count)]; ;
            playerIndex = NewPlayerIndex(awaySubs, minute, playerIndex);
            var assist = DrawWithProbability([true, false], [60, 40]);
            if (assist)
            {
                var assistPlayer = DrawWithProbability(HomeLineup[..11].ConvertAll(player => player.Item1), HomeLineup[..11].ConvertAll(player => (double)player.Item1.Attack));
                var assistPlayerIndex = HomeLineup.IndexOf(HomeLineup.Find(x => x.Item1 == assistPlayer));
                assistPlayerIndex = NewPlayerIndex(awaySubs, minute, assistPlayerIndex);
                events.Add(new GoalEvent(
                    AwayTeam,
                    minute,
                    AwayLineup[playerIndex].Item1,
                    AwayLineup[assistPlayerIndex].Item1
                    )
                );
            }
            else
            {
                events.Add(new GoalEvent(
                    AwayTeam,
                    minute,
                    AwayLineup[playerIndex].Item1
                    )
                );
            }
        }

        return events;
    }

    /// <summary>   Determines the updated player index considering substitution history. </summary>
    ///
    /// <param name="subs">         The sequence of substitution events or data. </param>
    /// <param name="minute">       The minute of the match when the event occurred. </param>
    /// <param name="playerIndex">  Index of the player in the lineup. </param>
    ///
    /// <returns>   The updated index of the player. </returns>

    private static int NewPlayerIndex(IEnumerable<List<int>> subs, int minute, int playerIndex)
    {
        var newPlayer = subs.FirstOrDefault(sub => (sub[1] == playerIndex) && (sub[0] <=
        minute));

        if (newPlayer == null) return playerIndex;
        playerIndex = newPlayer[2];
        return playerIndex;

    }

    /// <summary>   Determines the updated player index and minute for cards events considering substitution history. </summary>
    ///
    /// <param name="subs">         The sequence of substitution events or data. </param>
    /// <param name="minute">       The minute of the match when the event occurred. </param>
    /// <param name="playerIndex">  Index of the player in the lineup. </param>
    ///
    /// <returns>   A list containing updated player index and adjusted minute. </returns>

    private static List<int> NewRedCardsPlayerIndexAndMinutes(IEnumerable<List<int>> subs, int minute, int playerIndex)
    {
        var newPlayer = subs.FirstOrDefault(sub => (sub[1] == playerIndex) && (sub[0] >=
        minute));
        if (newPlayer == null) return [playerIndex, minute];
        var rand = new Random();
        minute = newPlayer[0] + rand.Next(0, 90 - newPlayer[0]);
        playerIndex = newPlayer[2];
        return [playerIndex, minute];

    }

    /// <summary>   Generates a random number of goals based on the Poisson distribution and the given lambda. </summary>
    ///
    /// <param name="lambda">   The expected rate of events per match simulation. </param>
    ///
    /// <returns>   The number of goal events to draw. </returns>

    private static int Poisson(double lambda)
    {
        var random = new Random();
        var k = 0;
        var p = 1.0;
        var l = Math.Exp(-lambda);
        do
        {
            k++;
            p *= random.NextDouble();
        }
        while (p > l && k < 20);

        if (k >= 20) return k - random.Next(1, 10);
        return k - 1;
    }

    /// <summary>   Draws an item randomly based on given probabilities.
    /// Used to select an item from a list based on specified probabilities, such as selecting players, events, or outcomes during a match simulation </summary>
    ///
    /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
    ///                                         illegal values. </exception>
    ///
    /// <typeparam name="T">    The type of items in the list. </typeparam>
    /// <param name="items">            The list of items to choose from. </param>
    /// <param name="probabilities">    The probabilities corresponding to each item in the list. </param>
    ///
    /// <returns>   The selected item based on the probabilities. </returns>

    private static T DrawWithProbability<T>(List<T> items, IReadOnlyList<double> probabilities)
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

    /// <summary>   Updates team and player statistics based on match events. </summary>
    private void UpdateStats()
    {
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
                    redCardEvent.Player.Absence++;
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