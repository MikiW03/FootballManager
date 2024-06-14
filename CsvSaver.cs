using System.Text;
using FootballManager.Events;

namespace FootballManager;

/// <summary>   Provides functionality to save league data to CSV files. </summary>
///
/// <param name="path"> The full path where the CSV files will be saved. </param>
public class CsvSaver(string path)
{
    /// <summary>   Gets the full path of the data output directory. </summary>
    ///
    /// <value> The full path of the data output directory. </value>
    private string DataOutputPath { get; } = path;
    /// <summary>   Saves league data, user-chosen attack, and defense values to CSV files. </summary>
    ///
    /// <param name="league">               The league data to be saved. </param>
    /// <param name="userChosenAttack">     The user-chosen attack value. </param>
    /// <param name="userChosenDefence">    The user-chosen defense value. </param>
    public void SaveData(League league, int userChosenAttack, int userChosenDefence)
    {
        var name = $"{DateTime.Now:yyMMddHHmmss}_simulation_{userChosenAttack}a_{userChosenDefence}d";
        Directory.CreateDirectory(DataOutputPath);
        Directory.CreateDirectory($@"{DataOutputPath}\{name}");

        var eventsLogs = new StringBuilder();
        eventsLogs.AppendLine("round;match;team;minute;player;event_type");

        var roundTable = new StringBuilder();
        roundTable.AppendLine("no.;team_name;matches;W;D;L;GS;GC;points");

        var playersStats = new StringBuilder();
        playersStats.AppendLine("player_name;team_name;nationality;age;matches_played;minutes_played;minutes_per_match;goals;goals_per_match;assists;assist_per_match;yellow_cards;red_cards;injuries");


        var rounds = league.Rounds;
        foreach (var round in rounds)
        {
            var roundNumber = rounds.IndexOf(round);
            foreach (var match in round.Matches)
            {
                foreach (var e in match.Events)
                {
                    var eventType = e.GetType().Name;
                    eventsLogs.AppendLine(
                        $"{roundNumber}; {round.Matches.IndexOf(match)}; {e.Team.Name}; {e.Minute}; {e.Player.Name}; {eventType}");

                    switch (e)
                    {
                        case GoalEvent goalEvent:
                            if (goalEvent.Assist is not null)
                            {
                                eventsLogs.AppendLine(
                                    $"{roundNumber}; {round.Matches.IndexOf(match)}; {goalEvent.Team.Name}; {goalEvent.Minute}; {goalEvent.Assist.Name}; Assist");
                            }

                            break;
                        case SubstitutionEvent substitutionEvent:
                            eventsLogs.AppendLine(
                                $"{roundNumber}; {round.Matches.IndexOf(match)}; {substitutionEvent.Team.Name}; {substitutionEvent.Minute}; {substitutionEvent.Substitute.Name}; Substitution_In");
                            break;
                    }
                }

            }
        }

        var teams = league.Teams.OrderByDescending(team => team.Value.Points)
            .ThenByDescending(team => team.Value.GoalsScored - team.Value.GoalsScored).ToList();
        foreach (var team in teams)
        {
            roundTable.AppendLine(
                $"{teams.ToList().IndexOf(team) + 1};{team.Key};{team.Value.MatchesPlayed};{team.Value.Wins};{team.Value.Draws};{team.Value.Losses};{team.Value.GoalsScored};{team.Value.GoalsConceded};{team.Value.Points}");
        }

        var players = teams
            .SelectMany(team => team.Value.Players, (team, player) => new { Player = player, TeamName = team.Value.Name })
            .ToDictionary(x => x.Player, x => x.TeamName);

        foreach (var (player, team) in players)
        {
            if(player.MinutesPlayed < 0)
            {
                player.MinutesPlayed = Math.Max(0, player.MinutesPlayed + 90 * player.MatchesPlayed);
            }
            var minutesPerMatch = player.MatchesPlayed != 0 ? (double)player.MinutesPlayed / player.MatchesPlayed : 0;
            var goalsPerMatch = player.MinutesPlayed != 0 ? (double)player.Goals / player.MatchesPlayed : 0;
            var assistsPerMatch = player.MinutesPlayed != 0 ? (double)player.Assists / player.MatchesPlayed : 0;
            var playerStatsRow = $@"{player.Name};{team};{player.Nationality};{player.Age};{player.MatchesPlayed};{player.MinutesPlayed};{Math.Round(minutesPerMatch,2)};{player.Goals};{Math.Round(goalsPerMatch,2)};{player.Assists};{Math.Round(assistsPerMatch,2)};{player.YellowCards};{player.RedCards};{player.Injuries}";
            playersStats.AppendLine(playerStatsRow);
        }

        using (var writer = new StreamWriter($@"{DataOutputPath}\{name}\{name}_events_logs.csv", append: true))
        {
            writer.WriteLine(eventsLogs);
        }

        using (var writer = new StreamWriter($@"{DataOutputPath}\{name}\{name}_table.csv", append: true))
        {
            writer.WriteLine(roundTable);
        }

        using (var writer = new StreamWriter($@"{DataOutputPath}\{name}\{name}_players_stats.csv", append: true))
        {
            writer.WriteLine(playersStats);
        }
    }
}