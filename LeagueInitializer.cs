using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.VisualBasic.FileIO;

namespace FootballManager;

public class LeagueInitializer
{
    public string DataOutputPath { get; set; } = "data_output";

    private Dictionary<string, Team> LoadTeams(int userChosenAttack, int userChosenDefence)
    {
        var teams = new Dictionary<string, Team>();

        using (var parser = new TextFieldParser("data_input\\teams.csv"))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            parser.ReadFields();

            var rand = new Random();
            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                var overall = short.Parse(fields[1]);
                var team = new Team
                {
                    Name = fields[0],
                    Overall = overall,
                    Attack = GenerateNormal(overall),
                    Defense = GenerateNormal(overall),
                    Players = [],
                    Formation = new Formation(short.Parse(fields[2].Split("-")[0]), short.Parse(fields[2].Split("-")[1]), short.Parse(fields[2].Split("-")[2])),
                };

                teams.Add(team.Name, team);
            }
        }

        teams["Manchester City"].Attack = GenerateNormal(userChosenAttack);
        teams["Manchester City"].Defense = GenerateNormal(userChosenDefence);
        teams["Manchester City"].Overall = (teams["Manchester City"].Attack + teams["Manchester City"].Defense)/2;
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
                var playerTeam = teams[fields[4]];
                var playerPosition = (Position)Enum.Parse(typeof(Position), fields[2]);
                var playerOverall = GenerateNormal(playerTeam.Overall);
                var player = new Player
                {
                    Name = fields[0],
                    Age = short.Parse(fields[1]),
                    Position = playerPosition,
                    Nationality = fields[3],
                    Overall = playerOverall,
                    Attack = playerPosition switch
                    {
                        Position.Goalkeeper => 0,
                        Position.Defender => GenerateNormal(50),
                        Position.Midfielder => GenerateNormal(playerOverall),
                        Position.Forward => playerOverall,
                        _ => 0
                    },
                    Defense = playerPosition switch
                    {
                        Position.Goalkeeper => playerOverall,
                        Position.Defender => playerOverall,
                        Position.Midfielder => GenerateNormal(playerOverall),
                        Position.Forward => GenerateNormal(50),
                        _ => 0
                    },
                };

                playerTeam.Players.Add(player);
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

    private static int GenerateNormal(int mean)
    {
        var random = new Random();
        const int stdDev = 15;
        double result;
        var u1 = 1.0 - random.NextDouble();
        var u2 = 1.0 - random.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        result = mean + stdDev * randStdNormal;

        return Math.Min(Math.Max((int)Math.Round(result), 1), 99);
    }

    public void Init()
    {
        Console.WriteLine("The main character of this simulation is Manchester City Football Club.\nChoose their power rates to simulate where it will place them in the table at the end of the season");
        short userChosenAttack;
        short userChosenDefence;
        bool flag;
        do
        {
            Console.Write("Attack (1-99): ");
            flag = short.TryParse(Console.ReadLine(), out userChosenAttack);
        } while(userChosenAttack < 1 || userChosenAttack > 99 || !flag);

        do
        {
            Console.Write("Defence (1-99): ");
            flag = short.TryParse(Console.ReadLine(), out userChosenDefence);
        } while(userChosenDefence < 1 || userChosenDefence > 99 || !flag);

        Console.WriteLine();
        Console.WriteLine("Simulation parameters: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Attack: {userChosenAttack}");
        Console.WriteLine($"Defence: {userChosenDefence}");
        Console.ResetColor();
        var teams = LoadTeams(userChosenAttack, userChosenDefence);
        var rounds = DrawRounds(teams);

        League league = new(teams, rounds);
        league.StartLeague();
        var csvSaver = new CsvSaver(DataOutputPath);
        csvSaver.SaveData(league, userChosenAttack, userChosenDefence);
        Console.WriteLine("Simulation finished. Check the results in the bin\\Debug\\net8.0\\data_output folder.");
    }
}