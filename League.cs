namespace FootballManager;

public class League(Dictionary<string, Team> teams, List<Round> rounds)
{
    public Dictionary<string, Team> Teams { get; set; } = teams;
    public List<Round> Rounds { get; set; } = rounds;

    private string DataOutputPath { get; set; } = "data_output";

    public void StartLeague()
    {
        foreach (var round in Rounds)
        {
            round.Simulate();
        }
    }
}