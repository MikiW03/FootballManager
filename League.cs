namespace FootballManager;

public class League(Dictionary<string, Team> teams, List<Round> rounds, ISavable dataSaver)
{
    public Dictionary<string, Team> Teams { get; set; } = teams;
    public List<Round> Rounds { get; set; } = rounds;
    private ISavable DataSaver { get; set; } = dataSaver;

    public void StartLeague()
    {
        foreach (var round in Rounds)
        {
            round.Simulate();
            DataSaver.SaveData(round, Rounds.IndexOf(round) + 1);
        }
    }
}