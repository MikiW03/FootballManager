namespace FootballManager.Events;

/// <summary>   Represents a substitution event during a football match. </summary>
///
/// <param name="team">         The team making the substitution. </param>
/// <param name="minute">       The minute when the substitution occurs. </param>
/// <param name="player">       The player being substituted. </param>
/// <param name="substitute">   The player coming on as a substitute. </param>
public class SubstitutionEvent(Team team, int minute, Player player, Player? substitute) : Event(team, minute, player)
{
    /// <summary>   Gets the making the substitution. </summary>
    ///
    /// <value> The team making the substitution. </value>
    public Team Team { get; } = team;
    /// <summary>   Gets the minute when the substitution occurs. </summary>
    ///
    /// <value> The minute when the substitution occurs. </value>
    public int Minute { get; } = minute;
    /// <summary>   Gets the player being substituted. </summary>
    ///
    /// <value> The player being substituted. </value>
    public Player Player { get; } = player;
    /// <summary>   Gets the player coming on as a substitute. </summary>
    ///
    /// <value> The player coming on as a substitute. </value>
    public Player Substitute { get; } = substitute;

    /// <summary>   Print details of the substitution event. </summary>
    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Substitution - {Player.Name}({Substitute.Name}) ({Team.Name})");
    }
}