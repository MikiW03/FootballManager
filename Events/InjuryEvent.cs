namespace FootballManager.Events;

/// <summary>   Represents an injury event during a football match. </summary>
///
/// <param name="team">     The team affected by the injury. </param>
/// <param name="minute">   The minute in which the injury occurred. </param>
/// <param name="player">   The player who got injured. </param>
/// <param name="duration"> The duration of the injury, in number of matches. </param>
public class InjuryEvent(Team team, int minute, Player player, int duration) : Event(team, minute, player)
{
    /// <summary>   Gets the team affected by the injury. </summary>
    ///
    /// <value> The team affected by the injury. </value>
    public Team Team { get; } = team;
    /// <summary>   Gets the minute in which the injury occurred. </summary>
    ///
    /// <value> The minute in which the injury occurred. </value>
    public int Minute { get; } = minute;
    /// <summary>   Gets the player who got injured. </summary>
    ///
    /// <value> The player who got injured. </value>
    public Player Player { get; } = player;
    /// <summary>   Gets the duration of the injury, in number of matches. </summary>
    ///
    /// <value> The duration of the injury, in number of matches. </value>
    public int Duration { get; } = duration;

    /// <summary>   Prints details of the injury event. </summary>
    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Injury -  {Player.Name}(Matches: {Duration}) ({Team.Name})");
    }
}