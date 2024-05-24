namespace FootballManager;

public class Player(string name, int age, string nationality, Position position, int attack, int defense, int overall)
{
    public string Name { get; set; } = name;
    public int Age { get; set; } = age;
    public string Nationality { get; set; } = nationality;
    public Position Position { get; set; } = position;
    public int Attack { get; set; } = attack;
    public int Defense { get; set; } = defense;
    public int Overall { get; set; } = overall;
    public int Absence { get; set; }
    public int Goals { get; set;  }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int Injuries { get; set; }
    public int MatchesPlayed { get; set; }
    public int MinutesPlayed { get; set; }
}