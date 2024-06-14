namespace FootballManager.Events;

/// <summary>   Represents a red card event during a football match. </summary>
///
/// <param name="team">     The team whose player is given the red card. </param>
/// <param name="minute">   The minute when the red card is given. </param>
/// <param name="player">   The player who is given the red card. </param>
public class RedCardEvent(Team team, int minute, Player player) : Event(team, minute, player)
{
    /// <summary>   Gets the team whose player is given the red card. </summary>
    ///
    /// <value> The team receiving the red card. </value>
    public Team Team { get; } = team;
    /// <summary>   Gets the minute when the red card is given. </summary>
    ///
    /// <value> The minute when the red card is given. </value>
    public int Minute { get; } = minute;
    /// <summary>   Gets the player who is given the red card. </summary>
    ///
    /// <value> The player who is given the red card. </value>
    public Player Player { get; } = player;

    /// <summary>   Print details of the red card event. </summary>
    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Red Card -  {Player.Name} ({Team.Name})");
    }
}