using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sportradar.LiveOddsService.Business.Extensions;
using Sportradar.LiveOddsService.Data.InMemoeyCollection.Extensions;
using Sportradar.LiveOddsService.Domain.Services;
using Match = Sportradar.LiveOddsService.Domain.Models.Match;

namespace Sportradar.LiveOddsService.IntegrationTests {
    public class MatchServiceTest {
        private IServiceProvider GetServiceProvider() =>
            new ServiceCollection()
                .AddMatchServices()
                .AddMatchRepositories()
                .BuildServiceProvider();

        private IMatchService GetMatchService(IServiceProvider serviceProvider) =>
            serviceProvider.CreateScope().ServiceProvider.GetService<IMatchService>()!;

        [Fact]
        public async Task StartNewMatch_GetSummery_ShouldContainTheMatch() {
            // Arrange
            IServiceProvider serviceProvider = GetServiceProvider();
            string homeTeam = "Home Team";
            string awayTeam = "Away Team";

            // Act
            await GetMatchService(serviceProvider).StartAsync(homeTeam, awayTeam);
            var result = await GetMatchService(serviceProvider).GetSummeryAsync();

            // Arrange
            result.Should().HaveCount(1)
                .And.Contain(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam);
        }

        [Fact]
        public async Task StartNewMatch_UpdateTheMatch_GetSummery_ShouldContainFinalUpdateMatch() {
            // Arrange
            IServiceProvider serviceProvider = GetServiceProvider();
            string homeTeam = "Home Team";
            string awayTeam = "Away Team";

            // Act
            await GetMatchService(serviceProvider).StartAsync(homeTeam, awayTeam);
            await GetMatchService(serviceProvider).UpdateAsync(new Match() {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeTeamScore = 3,
                AwayTeamScore = 2,
            });
            var result = await GetMatchService(serviceProvider).GetSummeryAsync();

            // Arrange
            result.Should().HaveCount(1);
            var data = result.First();
            data.HomeTeam.Should().Be(homeTeam);
            data.AwayTeam.Should().Be(awayTeam);
            data.HomeTeamScore.Should().Be(3);
            data.AwayTeamScore.Should().Be(2);
        }

        [Fact]
        public async Task StartNewMatch_UpdateTheMatch_FinishTheMatch_GetSummery_ShouldnotHaveTheMatch() {
            // Arrange
            IServiceProvider serviceProvider = GetServiceProvider();
            string homeTeam = "Home Team";
            string awayTeam = "Away Team";

            // Act
            await GetMatchService(serviceProvider).StartAsync(homeTeam, awayTeam);
            await GetMatchService(serviceProvider).UpdateAsync(new Match() {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeTeamScore = 3,
                AwayTeamScore = 2,
            });
            await GetMatchService(serviceProvider).FinishAsync(homeTeam, awayTeam);
            var result = await GetMatchService(serviceProvider).GetSummeryAsync();

            // Arrange
            result.Should().HaveCount(0);
        }
    }
}
