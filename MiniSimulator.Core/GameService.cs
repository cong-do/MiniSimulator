using MiniSimulator.Domain.Interfaces;
using MiniSimulator.Domain.Models;
using MiniSimulator.Domain.Settings;

namespace MiniSimulator.Core;

public class GameService : IGameService
{
    private readonly IMatchSimulator _simulator;

    public GameService(IMatchSimulator simulator)
    {
        _simulator = simulator;
    }

    public async Task<GroupStageResult> SimulateGroupStage(int roundsToSimulate, IReadOnlyList<Team> teams)
    {
        ArgumentNullException.ThrowIfNull(teams, nameof(teams));

        if (roundsToSimulate < 1)
        {
            throw new ArgumentException("There needs to be at least 1 round to simulate the game");
        }

        if (teams.Count < 2)
        {
            throw new ArgumentException("There needs to be at least 2 teams to simulate the game");
        }

        // Determine the matches per round based on a Round Robin algorithm
        var rounds = GetRounds(roundsToSimulate, teams);

        // Simulate the matches
        var runMatchTasks = rounds
            .SelectMany(round => round)
            .Select(tuple => _simulator.RunMatch(tuple.Item1, tuple.Item2));

        var matchResults = await Task.WhenAll(runMatchTasks);

        // Get the team scores based on the simulated matches
        var teamScores = teams
            .Select(team => GetTeamScore(team, matchResults.Where(match => match.HomeTeamId == team.Id || match.AwayTeamId == team.Id)));

        // Sort and rank the team scores
        var sortedTeamScores = teamScores
            .OrderByDescending(t => t.Points)
            .ThenByDescending(t => t.GoalDifference)
            .ThenByDescending(t => t.GoalsFor)
            .ThenBy(t => t.GoalsAgainst);

        return new GroupStageResult(rounds, matchResults, sortedTeamScores);
    }

    public Task<GroupStageResult> SimulateKnockoutStage(int teamsAmountToAdvance, IReadOnlyList<Team> teams)
    {
        // TODO: Did not have enough time to implement this :(
        throw new NotImplementedException();
    }

    public Match GetMatch(Guid homeTeamId, Guid awayTeamId, IReadOnlyList<Match> matches)
    {
        return matches.First(match => match.HomeTeamId.Equals(homeTeamId) && match.AwayTeamId.Equals(awayTeamId));
    }

    private static List<List<(Team, Team)>> GetRounds(int roundsToSimulate, IReadOnlyList<Team> teams)
    {
        var numberOfTeams = teams.Count;

        // Determine the ideal amount of matches per round
        var matchesPerRound = numberOfTeams / 2;

        var rounds = new List<List<(Team, Team)>>();
        for (var round = 0; round < roundsToSimulate; round++)
        {
            var roundMatches = new List<(Team, Team)>();

            for (var index = 0; index < matchesPerRound; index++)
            {
                // Calculate the index of the home team for the current match
                var homeTeamIndex = (round + index) % (numberOfTeams - 1);
                
                // Calculate the index of the away team for the current match
                var awayTeamIndex = (numberOfTeams - 1 - index + round) % (numberOfTeams - 1);

                // Ensure that in the first match of each round, the away team is set to the last team
                if (index == 0)
                {
                    awayTeamIndex = numberOfTeams - 1;
                }

                roundMatches.Add((teams[homeTeamIndex], teams[awayTeamIndex]));
            }

            rounds.Add(roundMatches);
        }

        return rounds;
    }

    private static TeamScore GetTeamScore(Team team, IEnumerable<Match> matches)
    {
        ArgumentNullException.ThrowIfNull(team, nameof(team));
        ArgumentNullException.ThrowIfNull(matches, nameof(matches));

        int played = 0, won = 0, draw = 0, lost = 0, goalsFor = 0, goalsAgainst = 0;
        foreach (var match in matches)
        {
            played++;
            
            // Check whether current team is home or away and determine the goals from there
            var isHomeTeam = match.HomeTeamId == team.Id;
            var teamGoalsFor = isHomeTeam ? match.HomeTeamGoals : match.AwayTeamGoals;
            var teamGoalsAgainst = isHomeTeam ? match.AwayTeamGoals : match.HomeTeamGoals;
            
            goalsFor += teamGoalsFor;
            goalsAgainst += teamGoalsAgainst;

            if (teamGoalsFor > teamGoalsAgainst)
            {
                won++;
            }
            else if (teamGoalsFor == teamGoalsAgainst)
            {
                draw++;
            }
            else
            {
                lost++;
            }
        }

        return new TeamScore(team.Id, team.Name, played, won, draw, lost, goalsFor, goalsAgainst);
    }
}