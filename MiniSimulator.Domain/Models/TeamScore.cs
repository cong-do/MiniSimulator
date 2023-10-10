namespace MiniSimulator.Domain.Models;

public record TeamScore(
    Guid TeamId, 
    string Name, 
    int Played, 
    int Won, 
    int Drawn, 
    int Lost, 
    int GoalsFor, 
    int GoalsAgainst)
{
    private const int PointsWeight = 3;

    public int GoalDifference => GoalsFor - GoalsAgainst;

    public int Points => (Won * PointsWeight) + Drawn;
}