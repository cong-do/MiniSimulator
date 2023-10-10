namespace MiniSimulator.Domain.Settings;

public record Team
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; init; } = default!;

    public int Strength { get; init; }

    public string Logo { get; init; } = default!;
}