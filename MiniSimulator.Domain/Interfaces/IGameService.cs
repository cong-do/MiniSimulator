using MiniSimulator.Domain.Models;
using MiniSimulator.Domain.Settings;

namespace MiniSimulator.Domain.Interfaces;

public interface IGameService
{
    Task<GroupStageResult> SimulateGroupStage(int roundsToSimulate, IReadOnlyList<Team> teams);

    Task<GroupStageResult> SimulateKnockoutStage(int teamsAmountToAdvance, IReadOnlyList<Team> teams);

    Match GetMatch(Guid homeTeamId, Guid awayTeamId, IReadOnlyList<Match> matches);
}