namespace FootballManager;

public class League(Dictionary<string, Team> teams, List<Round> rounds, ISavable dataSaver)
{
    public Dictionary<string, Team> Teams { get; set; } = teams;
    public List<Round> Rounds { get; set; } = rounds;
    private ISavable DataSaver { get; set; } = dataSaver;

    private string DataOutputPath { get; set; } = "data_output";

    public void StartLeague()
    {
        foreach (var round in Rounds)
        {
            round.Simulate();
            DataSaver.SaveData(DataOutputPath, round);
        }
    }
}