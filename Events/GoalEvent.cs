namespace FootballManager.Events;

/// <summary>   Represents a goal event during a football match. </summary>
///
/// <param name="team">     The team whose player scored the goal. </param>
/// <param name="minute">   The minute in which the goal was scored. </param>
/// <param name="player">   The player who scored the goal. </param>
/// <param name="assist">   Optional: The player who assisted in scoring the goal. </param>
public class GoalEvent(Team team, int minute, Player player, Player? assist=null) : Event(team, minute, player)
{
    /// <summary>   Gets the team whose player scored the goal. </summary>
    ///
    /// <value> The team whose player scored the goal. </value>
    public Team Team { get; } = team;
    /// <summary>   Gets the minute in which the goal was scored. </summary>
    ///
    /// <value> The minute in which the goal was scored. </value>
    public int Minute { get; } = minute;
    /// <summary>   Gets the player who scored the goal. </summary>
    ///
    /// <value> The player who scored the goal. </value>
    public Player Player { get; } = player;
    /// <summary>   Gets the player who assisted in scoring the goal (optional). </summary>
    ///
    /// <value> The player who assisted in scoring the goal or null if no assist. </value>
    public Player? Assist { get; } = assist;

    /// <summary>   Prints the details of the goal event. </summary>
    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Goal -  {Player.Name}{(Assist != null ? string.Concat("(", Assist?.Name, ")"):"")} ({Team.Name})");
    }
}