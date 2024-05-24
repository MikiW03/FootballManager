namespace FootballManager;

public class Team(string name, Coach coach, List<Player> players, Formation formation, int attack, int defense, int overall)
{
    public string Name { get; set; } = name;
    public Coach Coach { get; set; } = coach;
    public List<Player> Players { get; set; } = players;
    public Formation Formation { get; set; } = formation;
    public int Attack { get; set; } = attack;
    public int Defense { get; set; } = defense;
    public int Overall { get; set; } = overall;
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
        return new List<Player>();
    }
}