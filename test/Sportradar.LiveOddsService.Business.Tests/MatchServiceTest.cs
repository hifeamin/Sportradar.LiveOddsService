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
            var newMatchData = new Match() {
                HomeTeam = "Home Team",
                AwayTeam = "Away Team",
                HomeTeamScore = 2,
                AwayTeamScore = 3
            };
            var savedMatch = new Match() {
                HomeTeam = newMatchData.HomeTeam,
                AwayTeam = newMatchData.AwayTeam,
                HomeTeamScore = 1,
                AwayTeamScore = 2
            };

            Mock<IMatchRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetAsync(newMatchData.HomeTeam, newMatchData.AwayTeam)).ReturnsAsync(savedMatch);
            repositoryMock.Setup(r => r.UpdateAsync(savedMatch)).Returns(Task.CompletedTask);

            IMatchService matchService = new MatchService(repositoryMock.Object);

            // Act
            await matchService.UpdateAsync(newMatchData);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(It.Is<Match>(m =>
                    m.HomeTeam == savedMatch.HomeTeam &&
                    m.AwayTeam == savedMatch.AwayTeam &&
                    m.HomeTeamScore == newMatchData.HomeTeamScore &&
                    m.AwayTeamScore == newMatchData.AwayTeamScore
                )), Times.Once());
        }
    }
}
