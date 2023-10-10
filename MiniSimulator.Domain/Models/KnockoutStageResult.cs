namespace MiniSimulator.Domain.Models;

public record KnockoutStageResult(
    IEnumerable<Match> Matches, 
    IEnumerable<TeamScore> TeamScores);