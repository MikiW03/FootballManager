namespace FootballManager.Events;

public class InjuryEvent(Team team, int minute, Player player, int duration) : Event(team, minute, player)
{
    public Team Team { get; } = team;
    public int Minute { get; } = minute;
    public Player Player { get; } = player;
    public int Duration { get; } = duration;

    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Injury -  {Player.Name}(Matches: {Duration}) ({Team.Name})");
    }
}