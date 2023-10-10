using MiniSimulator.Domain.Interfaces;
using MiniSimulator.Domain.Models;
using MiniSimulator.Domain.Settings;

namespace MiniSimulator.Core.Simulators;

public class SimpleMatchSimulator : IMatchSimulator
{
    private const int MaxGoals = 5;

    private readonly Random _random;

    public SimpleMatchSimulator()
    {
        _random = new Random();
    }

    public Task<Match> RunMatch(Team homeTeam, Team awayTeam)
    {
        var homeTeamGoals = GetTeamScore(homeTeam.Strength);
        var awayTeamGoals = GetTeamScore(awayTeam.Strength);

        var match = new Match(homeTeam.Id, awayTeam.Id, homeTeamGoals, awayTeamGoals);

        return Task.FromResult(match);
    }

    private int GetTeamScore(int teamStrength)
    {
        // Generate probable goal amount for the team to be able to score
        var probableGoals = _random.Next(0, MaxGoals);

        // Simulate each goal
        var scoredGoals = 0;
        for (var index = 0; index <= probableGoals; index++)
        {
            // Generate a probability chance percentage
            var probability = _random.Next(0, 100);

            // If the team strength/scoring probability is higher than the probability, the team scores a goal
            if (teamStrength >= probability)
            {
                scoredGoals++;
            }
        }

        return scoredGoals;
    }
}