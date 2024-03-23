using FluentAssertions;
using Moq;
using Sportradar.LiveOddsService.Data;
using Sportradar.LiveOddsService.Domain.Services;
using Match = Sportradar.LiveOddsService.Domain.Models.Match;

namespace Sportradar.LiveOddsService.Business.Tests {
    public class MatchServiceTest {
        [Fact]
        public async Task StartAsync_NewMatchShouldSave() {
            // Arrange
            Mock<IMatchRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Match>())).Returns(Task.CompletedTask);

            IMatchService matchService = new MatchService(repositoryMock.Object);
            string homeTeam = "Home team";
            string awayTeam = "Away Team";

            // Act
            Match result = await matchService.StartAsync(homeTeam, awayTeam);

            // Assert
            result.Should().NotBeNull();
            result.HomeTeam.Should().Be(homeTeam);
            result.AwayTeam.Should().Be(awayTeam);
            result.HomeTeamScore.Should().Be(0);
            result.AwayTeamScore.Should().Be(0);
            repositoryMock.Verify(r => r.AddAsync(result), Times.Once());
        }
    }
}
