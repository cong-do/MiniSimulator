using MiniSimulator.Domain.Settings;

namespace MiniSimulator.Domain.Models;

public record GroupStageResult(
    IReadOnlyList<IReadOnlyList<(Team, Team)>> Rounds,
    IReadOnlyList<Match> Matches, 
    IEnumerable<TeamScore> TeamScores);