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
            string homeTeam = "Home Team";
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

        [Fact]
        public async Task UpdateAsync_ShouldBeSaved() {
            // Arrange
            var match = new Match() {
                HomeTeam = "Home Team",
                AwayTeam = "Away Team",
                HomeTeamScore = 2,
                AwayTeamScore = 3
            };

            Mock<IMatchRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetAsync(match.HomeTeam, match.AwayTeam)).ReturnsAsync(match);
            repositoryMock.Setup(r=>r.UpdateAsync(match)).Returns(Task.CompletedTask);

            IMatchService matchService = new MatchService(repositoryMock.Object);

            // Act
            await matchService.UpdateAsync(match);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(match), Times.Once());

        }
    }
}
