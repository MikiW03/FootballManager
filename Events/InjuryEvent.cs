namespace FootballManager.Events;

public class InjuryEvent(Team team, int minute, Player player, int duration) : Event(team, minute, player)
{
    public Team Team { get; set; } = team;
    public int Minute { get; set; } = minute;
    public Player Player { get; set; } = player;
    public int Duration { get; set; } = duration;
}