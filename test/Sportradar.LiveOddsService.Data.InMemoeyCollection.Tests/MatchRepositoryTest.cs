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
            // Arrange
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
                .And.BeEquivalentTo(new Match[] { match1, match2 });
        }

        [Fact]
        public async Task GetAsync_EmptyCollection_ShouldReturnNull() {
            // Arrange
            DbContext dbContext = new();
            IMatchRepository matchRepository = new MatchRepository(dbContext);
            string homeTeam = "Home Team";
            string awayTeam = "Away Team";

            // Act
            var result = await matchRepository.GetAsync(homeTeam, awayTeam);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_NotFound_ShouldReturnNull() {
            // Arrange
            DbContext dbContext = new();
            dbContext.Matches.Add("Home Team 1-Away Team 1", new Match() {
                HomeTeam = "Home Team 1",
                AwayTeam = "Away Team 1"
            });
            IMatchRepository matchRepository = new MatchRepository(dbContext);
            string homeTeam = "Home Team 2";
            string awayTeam = "Away Team 2";

            // Act
            var result = await matchRepository.GetAsync(homeTeam, awayTeam);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_ContainsOnlyTheItem_ShouldReturnItem() {
            // Arrange
            DbContext dbContext = new();
            dbContext.Matches.Add("Home Team 1-Away Team 1", new Match() {
                HomeTeam = "Home Team 1",
                AwayTeam = "Away Team 1"
            });
            IMatchRepository matchRepository = new MatchRepository(dbContext);
            string homeTeam = "Home Team 1";
            string awayTeam = "Away Team 1";

            // Act
            var result = await matchRepository.GetAsync(homeTeam, awayTeam);

            // Assert
            result.Should().NotBeNull();
            result!.HomeTeam.Should().Be(homeTeam);
            result.AwayTeam.Should().Be(awayTeam);
        }

        [Fact]
        public async Task GetAsync_ContainsSomeItem_ShouldReturnItem() {
            // Arrange
            DbContext dbContext = new();
            dbContext.Matches.Add("Home Team 1-Away Team 1", new Match() {
                HomeTeam = "Home Team 1",
                AwayTeam = "Away Team 1"
            });
            dbContext.Matches.Add("Home Team 2-Away Team 2", new Match() {
                HomeTeam = "Home Team 2",
                AwayTeam = "Away Team 2"
            });
            IMatchRepository matchRepository = new MatchRepository(dbContext);
            string homeTeam = "Home Team 2";
            string awayTeam = "Away Team 2";

            // Act
            var result = await matchRepository.GetAsync(homeTeam, awayTeam);

            // Assert
            result.Should().NotBeNull();
            result!.HomeTeam.Should().Be(homeTeam);
            result.AwayTeam.Should().Be(awayTeam);
        }

        [Fact]
        public async Task UpdateAsync_UpdateValue_ValueShouldBeUpdated() {
            // Arrange
            DbContext dbContext = new();
            dbContext.Matches.Add("Home Team-Away Team", new Match() {
                HomeTeam = "Home Team",
                AwayTeam = "Away Team",
                HomeTeamScore = 0,
                AwayTeamScore = 0
            });
            IMatchRepository matchRepository = new MatchRepository(dbContext);
            Match match = new() {
                HomeTeam = "Home Team",
                AwayTeam = "Away Team",
                HomeTeamScore = 1,
                AwayTeamScore = 2
            };

            // Act
            await matchRepository.UpdateAsync(match);

            // Assert
            dbContext.Matches.Should().ContainKey("Home Team-Away Team");
            dbContext.Matches["Home Team-Away Team"].Should().Be(match);
            dbContext.Matches["Home Team-Away Team"].HomeTeamScore.Should().Be(1);
            dbContext.Matches["Home Team-Away Team"].AwayTeamScore.Should().Be(2);
        }

        [Fact]
        public async Task RemoveAsync_RemoveOnlyItem_ShouldRemoveValue() {
            // Arrange
            DbContext dbContext = new();
            Match match = new() {
                HomeTeam = "Home Team",
                AwayTeam = "Away Team",
            };
            dbContext.Matches.Add("Home Team-Away Team", match);
            IMatchRepository matchRepository = new MatchRepository(dbContext);

            // Act
            await matchRepository.RemoveAsync(match);

            // Assert
            dbContext.Matches.Should().BeEmpty();
        }

        [Fact]
        public async Task RemoveAsync_RemoveItem_ShouldRemoveValue() {
            // Arrange
            DbContext dbContext = new();
            Match match = new() {
                HomeTeam = "Home Team 1",
                AwayTeam = "Away Team 1",
            };
            dbContext.Matches.Add("Home Team 1-Away Team 1", match);
            dbContext.Matches.Add("Home Team 2-Away Team 2", new Match {
                HomeTeam = "Home Team 2",
                AwayTeam = "Away Team 2",
            });
            IMatchRepository matchRepository = new MatchRepository(dbContext);

            // Act
            await matchRepository.RemoveAsync(match);

            // Assert
            dbContext.Matches.Should().HaveCount(1)
                .And.NotContainValue(match)
                .And.NotContainKey($"Home Team 1-Away Team 1")
                .And.OnlyContain(m => m.Value.HomeTeam == "Home Team 2" && m.Value.AwayTeam == "Away Team 2");
        }
    }
}
