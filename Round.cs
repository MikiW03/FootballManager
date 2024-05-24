namespace FootballManager;

public class Round
{
    public List<Match> Matches { get; set; }

    public void Simulate()
    {
        foreach (var match in Matches)
        {
            match.Simulate();
        }
    }
}