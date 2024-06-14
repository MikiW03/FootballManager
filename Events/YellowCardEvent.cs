namespace FootballManager.Events;

/// <summary>   Represents a yellow card event during a football match. </summary>
///
/// <param name="team">     The team whose player is given the yellow card. </param>
/// <param name="minute">   The minute when the yellow card is given. </param>
/// <param name="player">   The player who is given the yellow card. </param>
public class YellowCardEvent(Team team, int minute, Player player) : Event(team, minute, player)
{
    /// <summary>   Gets the team whose player is given the yellow card. </summary>
    ///
    /// <value> The team whose player is given the yellow card. </value>
    public Team Team { get; } = team;
    /// <summary>   Gets the minute when the yellow card is given. </summary>
    ///
    /// <value> The minute when the yellow card is given. </value>
    public int Minute { get; } = minute;
    /// <summary>   Gets the player who is given the yellow card. </summary>
    ///
    /// <value> The player who is given the yellow card. </value>
    public Player Player { get; } = player;

    /// <summary>   Print details of the yellow card event. </summary>
    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Yellow Card -  {Player.Name} ({Team.Name})");
    }
}