using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualBasic.FileIO;

namespace FootballManager;

public class LeagueInitializer
{
    public string DataOutputPath { get; set; } = "data_output";

    private Dictionary<string, Team> LoadTeams()
    {
        var teams = new Dictionary<string, Team>();

        using (var parser = new TextFieldParser("data_input\\teams.csv"))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            parser.ReadFields();
            while (!parser.EndOfData)
            {
                var random = new Random();
                var defendersCount = random.Next(4, 6);
                var midfieldersCount = random.Next(3, 5);
                var forwardsCount = 10 - defendersCount - midfieldersCount;

                var fields = parser.ReadFields();
                var team = new Team
                {
                    Name = fields[0],
                    Overall = short.Parse(fields[1]),
                    Attack = short.Parse(fields[2]),
                    Defense = short.Parse(fields[3]),
                    Players = [],
                    Formation = new Formation(defendersCount, midfieldersCount, forwardsCount),
                };

                teams.Add(team.Name, team);
            }
        }

        using (var parser = new TextFieldParser("data_input\\coaches.csv"))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            parser.ReadFields();
            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                var coach = new Coach
                {
                    Name = fields[0],
                    Nationality = fields[1],
                    Age = short.Parse(fields[2]),
                };

                teams[fields[3]].Coach = coach;
            }
        }

        using (var parser = new TextFieldParser("data_input\\players.csv"))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            parser.ReadFields();
            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                var player = new Player
                {
                    Name = fields[0],
                    Age = short.Parse(fields[1]),
                    Position = (Position)Enum.Parse(typeof(Position), fields[2]),
                    Nationality = fields[4],
                    Overall = short.Parse(fields[8]),
                    Attack = short.Parse(fields[9]),
                    Defense = short.Parse(fields[10]),
                };

                teams[fields[11]].Players.Add(player);
            }
        }

        return teams;
    }

    private List<Round> DrawRounds(Dictionary<string, Team> teams)
    {
        var rounds = new List<Round>();
        
        var teamsNames = teams.Keys.ToList();
        int numRounds = teamsNames.Count - 1;
        int half = teamsNames.Count / 2;


        for (int _ = 0; _ < numRounds; _++)
        {
            var round = new Round()
            {
                Matches = []
            };

            for (int matchNum = 0; matchNum < half; matchNum++)
            {
                var match = new Match(teams[teamsNames[matchNum]], teams[teamsNames[teamsNames.Count - matchNum - 1]]);
                round.Matches.Add(match);
            }
            rounds.Add(round);

            teamsNames.Insert(1, teamsNames.Last());
            teamsNames.RemoveAt(teamsNames.Count - 1);
        }

        for (int roundNum = 0; roundNum < numRounds; roundNum++)
        {
            var round = new Round
            {
                Matches = []
            };

            for (int matchNum = 0; matchNum < half; matchNum++)
            {
                var match = new Match(rounds[roundNum].Matches[matchNum].AwayTeam, rounds[roundNum].Matches[matchNum].HomeTeam);
                round.Matches.Add(match);
            }
            rounds.Add(round);
        }

        return rounds;
    }

    public void Init()
    {
        var teams = LoadTeams();
        var rounds = DrawRounds(teams);

        var csvSaver = new CsvSaver();
        League league = new(teams, rounds, csvSaver);
        league.StartLeague();
    }
}