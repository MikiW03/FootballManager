namespace FootballManager;

public interface ISavable
{
    void SaveData(string path, Round round);
}