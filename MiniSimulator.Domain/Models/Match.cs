namespace MiniSimulator.Domain.Models;

public record Match(
    Guid HomeTeamId, 
    Guid AwayTeamId, 
    int HomeTeamGoals, 
    int AwayTeamGoals);