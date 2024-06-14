
namespace FootballManager;

/// <summary>   Represents a football team. </summary>
public class Team()
{
    /// <summary>   Gets or sets the name of the team. </summary>
    ///
    /// <value> The name of the team. </value>
    public string Name { get; set; }
    /// <summary>   Gets or sets the list of players in the team. </summary>
    ///
    /// <value> The list of players in the team.  </value>
    public List<Player> Players { get; set; }
    /// <summary>   Gets or sets the formation of the team. </summary>
    ///
    /// <value> The formation of the team. </value>
    public Formation Formation { get; set; }
    /// <summary>   Gets or sets the attack rating of the team. </summary>
    ///
    /// <value> The attack rating of the team. </value>
    public int Attack { get; set; }
    /// <summary>   Gets or sets the defense rating of the team. </summary>
    ///
    /// <value> The defense rating of the team. </value>
    public int Defense { get; set; }
    /// <summary>   Gets or sets the overall rating of the team. </summary>
    ///
    /// <value> The overall rating of the team. </value>
    public int Overall { get; set; }
    /// <summary>   Gets or sets the team's coach. </summary>
    ///
    /// <value> The team's coach. </value>
    public Coach Coach { get; set; }
    /// <summary>   Gets or sets the points accumulated by the team. </summary>
    ///
    /// <value> The points accumulated by the team. </value>
    public int Points { get; set; }
    /// <summary>   Gets or sets the number of goals scored by the team. </summary>
    ///
    /// <value> The number of goals scored by the team. </value>
    public int GoalsScored { get; set; }
    /// <summary>   Gets or sets the number of goals conceded by the team. </summary>
    ///
    /// <value> The number of goals conceded by the team. </value>
    public int GoalsConceded { get; set; }
    /// <summary>   Gets or sets the number of red cards received by the team. </summary>
    ///
    /// <value> The number of red cards received by the team. </value>
    public int RedCards { get; set; }
    /// <summary>   Gets or sets the number of yellow cards received by the team. </summary>
    ///
    /// <value> The number of yellow cards received by the team. </value>
    public int YellowCards { get; set; }
    /// <summary>   Gets or sets the number of matches won by the team. </summary>
    ///
    /// <value> The number of matches won by the team. </value>
    public int Wins { get; set; }
    /// <summary>   Gets or sets the number of matches drawn by the team. </summary>
    ///
    /// <value> The number of matches drawn by the team. </value>
    public int Draws { get; set; }
    /// <summary>   Gets or sets the number of matches lost by the team. </summary>
    ///
    /// <value> The number of matches lost by the team. </value>
    public int Losses { get; set; }
    /// <summary>   Gets or sets the total number of matches played by the team. </summary>
    ///
    /// <value> The total number of matches played by the team. </value>
    public int MatchesPlayed { get; set; }
    /// <summary>   Gets or sets the number of injuries suffered by the team. </summary>
    ///
    /// <value> The number of injuries suffered by the team. </value>
    public int Injuries { get; set; }

    /// <summary>   Gets the squad lineup for a match based on the team's formation and player availability. </summary>
    ///
    /// <returns>   The squad lineup. </returns>
    public List<Tuple<Player,int>> GetSquad()
    {
        var availablePlayers = Players.OrderByDescending(player => player.Overall).Where(player => player.Absence == 0).ToList();
        var defenders = availablePlayers.FindAll(player => player.Position == Position.Defender);
        var midfielders = availablePlayers.FindAll(player => player.Position == Position.Midfielder);
        var forwards = availablePlayers.FindAll(player => player.Position == Position.Forward);
        var goalkeepers = availablePlayers.FindAll(player => player.Position == Position.Goalkeeper);

        var players = goalkeepers.Take(1)
                .Concat(defenders.Take(Formation.Defenders))
                .Concat(midfielders.Take(Formation.Midfielders))
                .Concat(forwards.Take(Formation.Forwards))
                .Concat(defenders.Skip(Formation.Defenders).Take(3))
                .Concat(midfielders.Skip(Formation.Midfielders).Take(3))
                .Concat(forwards.Skip(Formation.Forwards).Take(3))
                .ToList();

        return players.Select(player => new Tuple<Player, int>(player, 0)).ToList();
    }
}