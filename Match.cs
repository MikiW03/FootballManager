using FootballManager.Events;
namespace FootballManager;

public class Match(Team homeTeam, Team awayTeam)
{
    public Team HomeTeam { get; set; } = homeTeam;
    public Team AwayTeam { get; set; } = awayTeam;
    public int HomeGoals { get; set; } = 0;
    public int AwayGoals { get; set; } = 0;
    public List<Player> HomeLineup { get; set; }
    public List<Player> AwayLineup { get; set; }
    public List<Event> Events { get; set; }

    public void Simulate()
    {
        HomeLineup = HomeTeam.GetSquad();
        AwayLineup = AwayTeam.GetSquad();
        Events = DrawEvents();
        UpdateStats();
    }

    private List<Event> DrawEvents()
    {
        // TODO: Implement this method (drawing match events)
        
        // For testing purposes
        var goal1 = new GoalEvent(HomeTeam, 20, HomeTeam.Players[0]);
        var goal2 = new GoalEvent(AwayTeam, 88, AwayTeam.Players[0], AwayTeam.Players[1]);
        var yellow = new YellowCardEvent(HomeTeam, 40, HomeTeam.Players[0]);
        var red = new RedCardEvent(HomeTeam, 30, HomeTeam.Players[4]);
        var sub = new SubstitutionEvent(HomeTeam, 70, HomeTeam.Players[4], HomeTeam.Players[9]);
        var injury = new InjuryEvent(AwayTeam, 31, AwayTeam.Players[5], 4);
        var injury2 = new InjuryEvent(HomeTeam, 1, HomeTeam.Players[2], 1);

        return [goal1, goal2, yellow, red, sub, injury, injury2];
    }

    private void UpdateStats()
    {
        // TODO: Implement this method (updating players and teams stats)
        foreach (var matchEvent in Events)
        {
            switch (matchEvent)
            {
                case GoalEvent goalEvent:
                    break;
                case YellowCardEvent yellowCardEvent:
                    break;
                case RedCardEvent redCardEvent:
                    break;
                case SubstitutionEvent substitutionEvent:
                    break;
                case InjuryEvent injuryEvent:
                    break;
            }
        }
    }
}