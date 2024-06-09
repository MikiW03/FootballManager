namespace FootballManager;

public interface ISavable
{
    void SaveData(League league, int userChosenAttack, int userChosenDefence);
}