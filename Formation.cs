namespace FootballManager;

public class Formation(int defenders, int midfielders, int forwards)
{
    public int Defenders { get; } = defenders;
    public int Midfielders { get; } = midfielders;
    public int Forwards { get; } = forwards;
}