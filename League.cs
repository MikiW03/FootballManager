namespace FootballManager;

public class League(Dictionary<string, Team> teams, List<Round> rounds, ISavable dataSaver)
{
    public Dictionary<string, Team> Teams { get; set; } = teams;
    public List<Round> Rounds { get; set; } = rounds;
    private ISavable DataSaver { get; set; } = dataSaver;

    public void StartLeague()
    {
        foreach (var round in Rounds)
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"Round {Rounds.IndexOf(round) + 1}");
            Console.WriteLine("-----------------------------------------");

            round.Simulate();
            DataSaver.SaveData(round, Rounds.IndexOf(round) + 1);

            Console.WriteLine();
            Console.WriteLine($"Table after round {Rounds.IndexOf(round) + 1}:");

            Teams = Teams.OrderByDescending(x => x.Value.Points)
                        .ThenByDescending(x => x.Value.GoalsScored - x.Value.GoalsConceded)
                        .ToDictionary();
            PrintTable();

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private void PrintTable()
    {
        const string formattingString = "{8,3} {0,-25} {1,-8} {2,-3} {3,-3} {4,-3} {5,-4} {6,-4} {7,-10}";
        Console.WriteLine(formattingString, "Team Name", "Matches", "W", "D", "L", "GS", "GC", "Points", "No");
        foreach (var (teamName, team) in Teams)
        {
            if (teamName == "Manchester City") Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(formattingString, teamName, team.MatchesPlayed, team.Wins, team.Draws, team.Losses, team.GoalsScored, team.GoalsConceded, team.Points, Teams.Values.ToList().IndexOf(team) + 1);
            Console.ResetColor();
        }
    }
}