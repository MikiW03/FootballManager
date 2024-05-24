namespace FootballManager;

public class League(List<Team> teams, List<Round> rounds)
{
    public List<Team> Teams { get; set; } = teams;
    public List<Round> Rounds { get; set; } = rounds;

    public void StartLeague()
    {
        foreach (var round in Rounds)
        {
            round.Simulate();
        }
    }
}