namespace FootballManager;

public class Formation(int defenders, int midfielders, int forwards)
{
    public int Defenders { get; set; } = defenders;
    public int Midfielders { get; set; } = midfielders;
    public int Forwards { get; set; } = forwards;
}