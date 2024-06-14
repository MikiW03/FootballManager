namespace FootballManager
{
    /// <summary>   Represents the entry point of the FootballManager application. </summary>
    internal class Program
    {
        /// <summary>   Main entry point for launching and initializing the application. </summary>
        static void Main()
        {
            LeagueInitializer leagueInitializer = new();
            leagueInitializer.Init();
        }
    }
}
