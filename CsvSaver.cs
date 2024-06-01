namespace FootballManager;

public class CsvSaver(string path) : ISavable
{
    public string DataOutputPath { get; set; } = path;
    public void SaveData(Round round, int roundNumber)
    {
        // TODO: Implement the logic of saving the data to a CSV file
    }
}