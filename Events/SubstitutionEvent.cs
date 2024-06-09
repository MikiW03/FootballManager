namespace FootballManager.Events;

public class SubstitutionEvent(Team team, int minute, Player player, Player? substitute) : Event(team, minute, player)
{
    public Team Team { get; } = team;
    public int Minute { get; } = minute;
    public Player Player { get; } = player;
    public Player Substitute { get; } = substitute;

    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Substitution - {Player.Name}({Substitute.Name}) ({Team.Name})");
    }
}