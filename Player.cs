namespace FootballManager;

/// <summary>   Represents a football player. </summary>
public class Player()
{
    /// <summary>   Gets or sets the name of the player. </summary>
    ///
    /// <value> The name of the player. </value>
    public string Name { get; set; }
    /// <summary>   Gets or sets the age of the player. </summary>
    ///
    /// <value> The age of the player. </value>
    public int Age { get; set; }
    /// <summary>   Gets or sets the nationality of the player. </summary>
    ///
    /// <value> The nationality of the player. </value>
    public string Nationality { get; set; }
    /// <summary>   Gets or sets the position of the player. </summary>
    ///
    /// <value> The position of the player. </value>
    public Position Position { get; set; }
    /// <summary>   Gets or sets the attack rating of the player. </summary>
    ///
    /// <value> The attack rating of the player. </value>
    public int Attack { get; set; }
    /// <summary>   Gets or sets the defense rating of the player. </summary>
    ///
    /// <value> The defense rating of the player. </value>
    public int Defense { get; set; }
    /// <summary>   Gets or sets the overall rating of the player. </summary>
    ///
    /// <value> The overall rating of the player. </value>
    public int Overall { get; set; }
    /// <summary>   Gets or sets the number of matches the player has been absent. </summary>
    ///
    /// <value> The number of matches the player has been absent. </value>
    public int Absence { get; set; }
    /// <summary>   Gets or sets the number of goals scored by the player. </summary>
    ///
    /// <value> The number of goals scored by the player. </value>
    public int Goals { get; set;  }
    /// <summary>   Gets or sets the number of assists made by the player. </summary>
    ///
    /// <value> The number of assists made by the player. </value>
    public int Assists { get; set; }
    /// <summary>   Gets or sets the number of yellow cards received by the player. </summary>
    ///
    /// <value> The number of yellow cards received by the player. </value>
    public int YellowCards { get; set; }
    /// <summary>   Gets or sets the number of red cards received by the player. </summary>
    ///
    /// <value> The number of red cards received by the player. </value>
    public int RedCards { get; set; }
    /// <summary>   Gets or sets the number of injuries suffered by the player. </summary>
    ///
    /// <value> The number of injuries suffered by the player. </value>
    public int Injuries { get; set; }
    /// <summary>   Gets or sets the number of matches played by the player. </summary>
    ///
    /// <value> The number of matches played by the player. </value>
    public int MatchesPlayed { get; set; }
    /// <summary>   Gets or sets the number of minutes played by the player. </summary>
    ///
    /// <value> The number of minutes played by the player. </value>
    public int MinutesPlayed { get; set; }
}