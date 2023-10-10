using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using MiniSimulator.Domain.Interfaces;
using MiniSimulator.Domain.Models;
using MiniSimulator.Domain.Settings;
using NSubstitute;

namespace MiniSimulator.Core.UnitTests
{
    public static class GameServiceTests
    {
        public abstract class GameServiceTestsBase
        {
            protected readonly Fixture _fixture;
            protected IMatchSimulator _matchSimulator;

            protected readonly GameService _gameService;

            protected GameServiceTestsBase()
            {
                _fixture = new Fixture();
                _matchSimulator = Substitute.For<IMatchSimulator>();

                _gameService = new GameService(_matchSimulator);
            }
        }

        public class SimulateGroupStage : GameServiceTestsBase
        {
            [Theory, AutoData]
            public async Task WhenValidParametersAreGiven_ShouldReturnExpectedResult(IReadOnlyList<Team> teams)
            {
                // Arrange
                const int roundsToSimulate = 3;
                var expectedMatchAmount = roundsToSimulate * (teams.Count / 2);

                _matchSimulator.RunMatch(Arg.Any<Team>(), Arg.Any<Team>()).Returns(_fixture.Create<Match>());

                // Act
                var result = await _gameService.SimulateGroupStage(roundsToSimulate, teams);

                // Assert
                result.Should().NotBeNull();

                var rounds = result.Rounds;
                rounds.Count.Should().Be(roundsToSimulate);

                var matches = result.Matches;
                matches.Count.Should().Be(expectedMatchAmount);

                var teamScores = result.TeamScores.ToList();
                var sortedTeamScores = teamScores
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalDifference)
                    .ThenByDescending(t => t.GoalsFor)
                    .ThenBy(t => t.GoalsAgainst);

                teamScores.Should().BeEquivalentTo(sortedTeamScores);
            }

            [Fact]
            public void WhenTeamsParamIsNull_ShouldThrowArgumentNullException()
            {
                // Arrange
                var roundsToSimulate = _fixture.Create<int>();
                var teams = default(IReadOnlyList<Team>);

                // Act
                var result = () => _gameService.SimulateGroupStage(roundsToSimulate, teams!);

                // Assert
                result.Should().ThrowAsync<ArgumentNullException>();
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-25)]
            public void WhenRoundsToSimulateIsInsufficient_ShouldThrowArgumentException(int roundsToSimulate)
            {
                // Arrange
                var teams = _fixture.Create<IReadOnlyList<Team>>();

                // Act
                var result = () => _gameService.SimulateGroupStage(roundsToSimulate, teams!);

                // Assert
                result.Should().ThrowAsync<ArgumentException>();
            }

            [Theory]
            [InlineData(1)]
            [InlineData(0)]
            public void WhenTeamsAmountIsInsufficient_ShouldThrowArgumentException(int teamsAmount)
            {
                // Arrange
                var roundsToSimulate = 2;
                var teams = _fixture.CreateMany<Team>(teamsAmount);

                // Act
                var result = () => _gameService.SimulateGroupStage(roundsToSimulate, teams.ToList());

                // Assert
                result.Should().ThrowAsync<ArgumentException>();
            }
        }

        public class SimulateKnockoutStage : GameServiceTestsBase
        {
            // TODO
        }
    }
}