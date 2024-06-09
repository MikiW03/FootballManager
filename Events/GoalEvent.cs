namespace FootballManager.Events;

public class GoalEvent(Team team, int minute, Player player, Player? assist=null) : Event(team, minute, player)
{
    public Team Team { get; } = team;
    public int Minute { get; } = minute;
    public Player Player { get; } = player;
    public Player? Assist { get; } = assist;

    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Goal -  {Player.Name}{(Assist != null ? string.Concat("(", Assist?.Name, ")"):"")} ({Team.Name})");
    }
}