namespace MiniSimulator.Domain.Settings;

public record SimulationSettings
{
    public int RoundsToSimulate { get; set; }

    public IEnumerable<Team> Teams { get; init; } = default!;
}