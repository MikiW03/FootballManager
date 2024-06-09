namespace FootballManager;

public class Team()
{
    public string Name { get; set; }
    public List<Player> Players { get; set; }
    public Formation Formation { get; set; } 
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Overall { get; set; }
    public Coach Coach { get; set; }
    public int Points { get; set; }
    public int GoalsScored { get; set; }
    public int GoalsConceded { get; set; }
    public int RedCards { get; set; }
    public int YellowCards { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int MatchesPlayed { get; set; }
    public int Injuries { get; set; }

    public List<Tuple<Player,int>> GetSquad()
    {
        var availablePlayers = Players.OrderByDescending(player => player.Overall).Where(player => player.Absence == 0).ToList();
        var defenders = availablePlayers.FindAll(player => player.Position == Position.Defender);
        var midfielders = availablePlayers.FindAll(player => player.Position == Position.Midfielder);
        var forwards = availablePlayers.FindAll(player => player.Position == Position.Forward);
        var goalkeepers = availablePlayers.FindAll(player => player.Position == Position.Goalkeeper);

        var players = goalkeepers.Take(1)
                .Concat(defenders.Take(Formation.Defenders))
                .Concat(midfielders.Take(Formation.Midfielders))
                .Concat(forwards.Take(Formation.Forwards))
                .Concat(defenders.Skip(Formation.Defenders).Take(3))
                .Concat(midfielders.Skip(Formation.Midfielders).Take(3))
                .Concat(forwards.Skip(Formation.Forwards).Take(3))
                .ToList();

        return players.Select(player => new Tuple<Player, int>(player, 0)).ToList();
    }
}