namespace FootballManager.Events;

/// <summary>   Represents an event that occurs during a football match. </summary>
///
/// <param name="team">     The team affected by the event. </param>
/// <param name="minute">   The minute when the event occurs. </param>
/// <param name="player">   The player involved in the event. </param>
public abstract class Event(Team team, int minute, Player player)
{
    /// <summary>   Gets the team affected by the event. </summary>
    ///
    /// <value> The team affected by the event. </value>
    public Team Team { get; } = team;
    /// <summary>   Gets the minute when the event occurs. </summary>
    ///
    /// <value> The minute when the event occurs. </value>
    public int Minute { get; } = minute;
    /// <summary>   Gets the player involved in the event. </summary>
    ///
    /// <value> The player involved in the event. </value>
    public Player Player { get; } = player;

    /// <summary>   Print details of the event. </summary>
    public abstract void PrintDetails();
}