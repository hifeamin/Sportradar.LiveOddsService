using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sportradar.LiveOddsService.Business.Extensions;
using Sportradar.LiveOddsService.Data.InMemoeyCollection.Extensions;
using Sportradar.LiveOddsService.Domain.Services;
using Match = Sportradar.LiveOddsService.Domain.Models.Match;

namespace Sportradar.LiveOddsService.IntegrationTests {
    public class MatchServiceTest {
        [Fact]
        public async Task StartNewMatch_GetSummery_ShouldContainTheMatch() {
            // Arrange
            IServiceCollection serviceCollection = new ServiceCollection();
            IServiceProvider serviceProvider = serviceCollection
                .AddMatchServices()
                .AddMatchRepositories()
                .BuildServiceProvider();
            IServiceScope serviceScope = serviceProvider.CreateScope();
            IMatchService matchService = serviceScope.ServiceProvider.GetService<IMatchService>()!;
            string homeTeam = "Home Team";
            string awayTeam = "Away Team";

            // Act
            await matchService.StartAsync(homeTeam, awayTeam);
            var result = await matchService.GetSummeryAsync();

            // Arrange
            result.Should().HaveCount(1)
                .And.Contain(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam);
        }

        [Fact]
        public async Task StartNewMatch_UpdateTheMatch_GetSummery_ShouldContainFinalUpdateMatch() {
            // Arrange
            IServiceCollection serviceCollection = new ServiceCollection();
            IServiceProvider serviceProvider = serviceCollection
                .AddMatchServices()
                .AddMatchRepositories()
                .BuildServiceProvider();
            IServiceScope serviceScope = serviceProvider.CreateScope();
            IMatchService matchService = serviceScope.ServiceProvider.GetService<IMatchService>()!;
            string homeTeam = "Home Team";
            string awayTeam = "Away Team";

            // Act
            await matchService.StartAsync(homeTeam, awayTeam);
            await matchService.UpdateAsync(new Match() {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeTeamScore = 3,
                AwayTeamScore = 2,
            });
            var result = await matchService.GetSummeryAsync();

            // Arrange
            result.Should().HaveCount(1);
            var data = result.First();
            data.HomeTeam.Should().Be(homeTeam);
            data.AwayTeam.Should().Be(awayTeam);
            data.HomeTeamScore.Should().Be(3);
            data.AwayTeamScore.Should().Be(2);
        }
    }
}
