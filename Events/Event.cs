namespace FootballManager.Events;

public abstract class Event(Team team, int minute, Player player)
{
    public Team Team { get; set; } = team;
    public int Minute { get; set; } = minute;
    public Player Player { get; set; } = player;

    public void PrintDetails()
    {
        Console.WriteLine($"{Minute} - {Player.Name} ({Team.Name})");
    }
}