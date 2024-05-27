using System;
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
        // TODO: Implement this method (drawing rounds)
        return new List<Round>();
    }

    public void Init()
    {
        var teams = LoadTeams();
        var rounds = DrawRounds(teams);

        League league = new(teams, rounds);
        league.StartLeague();
    }
}