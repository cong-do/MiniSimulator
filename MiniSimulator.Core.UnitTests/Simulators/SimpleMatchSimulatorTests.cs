using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using MiniSimulator.Core.Simulators;
using MiniSimulator.Domain.Settings;

namespace MiniSimulator.Core.UnitTests.Simulators
{
    public static class SimpleMatchSimulatorTests
    {
        public abstract class SimpleMatchSimulatorTestsBase
        {
            protected readonly Fixture _fixture;

            protected readonly SimpleMatchSimulator _simulator;

            protected SimpleMatchSimulatorTestsBase()
            {
                _fixture = new Fixture();

                _simulator = new SimpleMatchSimulator();
            }
        }

        public class RunMatch : SimpleMatchSimulatorTestsBase
        {
            [Theory, AutoData]
            public async Task WhenValidInputGiven_ShouldReturnValidResult(Team homeTeam, Team awayTeam)
            {
                // Act
                var result = await _simulator.RunMatch(homeTeam, awayTeam);

                // Assert
                result.Should().NotBeNull();
                result.HomeTeamId.Should().Be(homeTeam.Id);
                result.AwayTeamId.Should().Be(awayTeam.Id);
                result.HomeTeamGoals.Should().BeGreaterThanOrEqualTo(0);
                result.AwayTeamGoals.Should().BeGreaterThanOrEqualTo(0);
            }

            [Fact]
            public async Task WhenHomeTeamStrengthIs100AndAwayTeamStrengthIs0_ShouldBeWonByHomeTeam()
            {
                // Arrange
                var homeTeam = _fixture.Create<Team>() with { Strength = 100 };
                var awayTeam = _fixture.Create<Team>() with { Strength = 0 };

                // Act
                var result = await _simulator.RunMatch(homeTeam, awayTeam);

                // Assert
                result.Should().NotBeNull();

                result.HomeTeamGoals.Should().BeGreaterThanOrEqualTo(result.AwayTeamGoals);
            }

            [Fact]
            public async Task WhenAwayTeamStrengthIs100AndHomeTeamStrengthIs0_ShouldBeWonByAwayTeam()
            {
                // Arrange
                var homeTeam = _fixture.Create<Team>() with { Strength = 0 };
                var awayTeam = _fixture.Create<Team>() with { Strength = 100 };

                // Act
                var result = await _simulator.RunMatch(homeTeam, awayTeam);

                // Assert
                result.Should().NotBeNull();

                result.AwayTeamGoals.Should().BeGreaterThanOrEqualTo(result.HomeTeamGoals);
            }
        }
    }
}