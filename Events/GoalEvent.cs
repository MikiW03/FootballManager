namespace FootballManager.Events;

public class GoalEvent(Team team, int minute, Player player, Player? assist) : Event(team, minute, player)
{
    public Team Team { get; set; } = team;
    public int Minute { get; set; } = minute;
    public Player Player { get; set; } = player;
    public Player? Assist { get; set; } = assist;
}