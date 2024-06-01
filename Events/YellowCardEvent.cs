﻿namespace FootballManager.Events;

public class YellowCardEvent(Team team, int minute, Player player) : Event(team, minute, player)
{
    public Team Team { get; set; } = team;
    public int Minute { get; set; } = minute;
    public Player Player { get; set; } = player;

    public override void PrintDetails()
    {
        Console.WriteLine($"{Minute}' Yellow Card -  {Player.Name} ({Team.Name})");
    }
}