namespace FootballManager
{
    internal class Program
    {
        static void Main()
        {
            LeagueInitializer leagueInitializer = new()
            {
                // TODO: Change the paths to the correct ones
                DataInputPath = "data.txt",
                DataOutputPath = "output.txt"
            };
            leagueInitializer.Init();
        }
    }
}
