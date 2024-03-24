using FluentAssertions;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Data.InMemoeyCollection.Tests {
    public class MatchRepositoryTest {
        [Fact]
        public async Task AddAsync_ToEmptyCollection_ShouldAddToCollection() {
            // Arrange
            DbContext dbContext = new();
            IMatchRepository matchRepository = new MatchRepository(dbContext);
            Match match = new() {
                HomeTeam = "Home Team",
                AwayTeam = "Away Team"
            };

            // Act
            await matchRepository.AddAsync(match);

            // Assert
            dbContext.Matches.Should().HaveCount(1)
                .And.ContainValue(match)
                .And.ContainKey($"{match.HomeTeam}-{match.AwayTeam}");
        }

        [Fact]
        public async Task AddAsync_ToNotEmptyCollection_ShouldAddToCollection() {
            // Arrange
            DbContext dbContext = new();
            dbContext.Matches.Add("Home Team 1-Away Team 1", new Match() {
                HomeTeam = "Home Team 1",
                AwayTeam = "Away Team 1"
            });
            IMatchRepository matchRepository = new MatchRepository(dbContext);
            Match match = new() {
                HomeTeam = "Home Team 2",
                AwayTeam = "Away Team 2"
            };

            // Act
            await matchRepository.AddAsync(match);

            // Assert
            dbContext.Matches.Should().HaveCount(2)
                .And.ContainValue(match)
                .And.ContainKey($"{match.HomeTeam}-{match.AwayTeam}");
        }

        [Fact]
        public async Task GetAllAsync_EmptyCollection_ShouldReturnEmptyCollection() {
            // Arrange
            DbContext dbContext = new();
            IMatchRepository matchRepository = new MatchRepository(dbContext);

            // Act
            var result = await matchRepository.GetAllAsync();

            // Assert
            result.Should().NotBeNull()
                .And.BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WithData_ShouldReturnAllData() {
            Match match1 = new() {
                HomeTeam = "Home Team 1",
                AwayTeam = "Away Team 1"
            };
            Match match2 = new() {
                HomeTeam = "Home Team 2",
                AwayTeam = "Away Team 2"
            };
            DbContext dbContext = new();
            dbContext.Matches.Add($"{match1.HomeTeam}-{match1.AwayTeam}", match1);
            dbContext.Matches.Add($"{match2.HomeTeam}-{match2.AwayTeam}", match2);
            IMatchRepository matchRepository = new MatchRepository(dbContext);

            // Act
            var result = await matchRepository.GetAllAsync();

            // Assert
            result.Should().NotBeNull()
                .And.HaveCount(2)
                .And.AllBeEquivalentTo(new Match[] { match1, match2 });
        }
    }
}
