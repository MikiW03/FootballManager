namespace FootballManager;

public class LeagueInitializer
{
    public string DataInputPath { get; set; }
    public string DataOutputPath { get; set; }

    private List<Team> LoadTeams(string path)
    {
        // TODO: Implement this method (loading teams from external file)
        return new List<Team>();
    }

    private List<Round> DrawRounds(List<Team> teams)
    {
        // TODO: Implement this method (drawing rounds)
        return new List<Round>();
    }

    public void Init()
    {
        var teams = LoadTeams(this.DataInputPath);
        var rounds = DrawRounds(teams);

        League league = new(teams, rounds);
        league.StartLeague();
    }
}