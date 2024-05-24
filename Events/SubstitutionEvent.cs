namespace FootballManager.Events;

public class SubstitutionEvent(Team team, int minute, Player player, Player? substitute) : Event(team, minute, player)
{
    public Team Team { get; set; } = team;
    public int Minute { get; set; } = minute;
    public Player Player { get; set; } = player;
    public Player Substitute { get; set; } = substitute;
}