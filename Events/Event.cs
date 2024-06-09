namespace FootballManager.Events;

public abstract class Event(Team team, int minute, Player player)
{
    public Team Team { get; } = team;
    public int Minute { get; } = minute;
    public Player Player { get; } = player;

    public abstract void PrintDetails();
}