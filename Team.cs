namespace FootballManager;

public class Team()
{
    public string Name { get; set; }
    public List<Player> Players { get; set; }
    public Formation Formation { get; set; } 
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Overall { get; set; }
    public Coach Coach { get; set; }
    public int Form { get; set; }
    public int Points { get; set; }
    public int GoalsScored { get; set; }
    public int GoalsConceded { get; set; }
    public int RedCards { get; set; }
    public int YellowCards { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int MatchesPlayed { get; set; }
    public int Injuries { get; set; }

    public List<Player> GetSquad()
    {
        // TODO: Implement this method (setting lineup that is supposed to play in match)
        return [];
    }
}