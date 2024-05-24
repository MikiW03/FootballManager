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
        //UpdateStats();
    }

    private List<Event> DrawEvents()
    {
        // TODO: Implement this method (drawing match events)
        return new List<Event>();
    }

    private void UpdateStats()
    {
        // TODO: Implement this method (updating players and teams stats)
        foreach (var matchEvent in Events)
        {
            if (matchEvent is GoalEvent goalEvent)
            {

            }
            else if (matchEvent is YellowCardEvent yellowCardEvent)
            {

            }
            else if (matchEvent is RedCardEvent redCardEvent)
            {
                
            } 
            else if (matchEvent is SubstitutionEvent substitutionEvent)
            {
                
            } 
            else if (matchEvent is InjuryEvent injuryEvent)
            {
                
            }
        }
    }
}