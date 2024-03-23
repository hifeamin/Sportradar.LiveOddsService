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

        [Fact]
        public async Task FinishAsync_ShouldRemoveFromDatabase() {
            // Arrange
            var savedMatch = new Match() {
                HomeTeam = "Home Team",
                AwayTeam = "Away Team"
            };
            Mock<IMatchRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetAsync(savedMatch.HomeTeam, savedMatch.AwayTeam)).ReturnsAsync(savedMatch);
            repositoryMock.Setup(r => r.RemoveAsync(It.IsAny<Match>())).Returns(Task.CompletedTask);

            IMatchService matchService = new MatchService(repositoryMock.Object);

            // Act
            await matchService.FinishAsync(savedMatch.HomeTeam, savedMatch.AwayTeam);

            // Assert
            repositoryMock.Verify(r => r.RemoveAsync(It.Is<Match>(m => m == savedMatch)), Times.Once());
        }

        [Fact]
        public async Task GetSummeryAsync_ShouldReturnDataInTotalScoreAndMostRecentOrder() {
            // Arrange
            IEnumerable<Match> matches = [
                new Match(){
                    HomeTeam = "Home Team 1",
                    AwayTeam = "Away Team 1",
                    HomeTeamScore = 4,
                    AwayTeamScore = 0,
                    StartTime = DateTime.Now.AddMinutes(-10),
                },
               new Match(){
                    HomeTeam = "Home Team 2",
                    AwayTeam = "Away Team 2",
                    HomeTeamScore = 10,
                    AwayTeamScore = 2,
                    StartTime = DateTime.Now.AddMinutes(-15),
                },
                new Match(){
                    HomeTeam = "Home Team 3",
                    AwayTeam = "Away Team 3",
                    HomeTeamScore = 6,
                    AwayTeamScore = 6,
                    StartTime = DateTime.Now.AddMinutes(-5),
                },
                new Match(){
                    HomeTeam = "Home Team 4",
                    AwayTeam = "Away Team 4",
                    HomeTeamScore = 1,
                    AwayTeamScore = 7,
                    StartTime = DateTime.Now.AddMinutes(-20),
                }
            ];
            Mock<IMatchRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(matches);

            IMatchService matchService = new MatchService(repositoryMock.Object);

            // Act
            IEnumerable<Match> result = await matchService.GetSummeryAsync();

            // Assert
            result.Should().HaveSameCount(matches)
                .And.NotContainNulls()
                .And.Equal(matches.ElementAt(2), matches.ElementAt(1), matches.ElementAt(3), matches.ElementAt(0));
        }

        [Fact]
        public async Task GetSummeryAsync_EmptyData_ShouldReturnEmptyCollection() {
            // Arrange
            Mock<IMatchRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

            IMatchService matchService = new MatchService(repositoryMock.Object);

            // Act
            IEnumerable<Match> result = await matchService.GetSummeryAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
