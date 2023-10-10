using MiniSimulator.Domain.Models;
using MiniSimulator.Domain.Settings;

namespace MiniSimulator.Domain.Interfaces;

public interface IMatchSimulator
{
    Task<Match> RunMatch(Team homeTeam, Team awayTeam);
}