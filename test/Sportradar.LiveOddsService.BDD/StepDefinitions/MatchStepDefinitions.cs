using System;
using Microsoft.Extensions.DependencyInjection;
using Sportradar.LiveOddsService.Business;
using Sportradar.LiveOddsService.Business.Extensions;
using Sportradar.LiveOddsService.Data.InMemoeyCollection;
using Sportradar.LiveOddsService.Data.InMemoeyCollection.Extensions;
using Sportradar.LiveOddsService.Domain.Models;
using Sportradar.LiveOddsService.Domain.Services;
using TechTalk.SpecFlow;

namespace Sportradar.LiveOddsService.BDD.StepDefinitions
{
    [Binding]
    public class MatchStepDefinitions
    {
        private readonly IServiceProvider _serviceProvider;

        public MatchStepDefinitions() {
            var services = new ServiceCollection();
            services.AddMatchServices()
                .AddMatchRepositories();
            
            _serviceProvider = services.BuildServiceProvider();
        }
        private Match? Result { get; set; }
        private IEnumerable<Match>? MatchSummery { get; set; }

        [Given(@"start new match for home team ""([^""]*)"" and away team ""([^""]*)""")]
        [When(@"start new match for home team ""([^""]*)"" and away team ""([^""]*)""")]
        public async Task WhenStartNewMatchForHomeTeamAndAwayTeam(string homeTeam, string awayTeam) {
            using var scope = _serviceProvider.CreateScope();
            var matchService = scope.ServiceProvider.GetService<IMatchService>();
            Result = await matchService!.StartAsync(homeTeam, awayTeam);
        }

        [Then(@"the result should not be null")]
        public void ThenTheResultShouldNotBeNull()
        {
            Result.Should().NotBeNull();
        }

        [Then(@"the result home team should be ""([^""]*)""")]
        public void ThenTheResultHomeTeamShouldBe(string homeTeam)
        {
            Result!.HomeTeam.Should().Be(homeTeam);
        }

        [Then(@"the result away team should be ""([^""]*)""")]
        public void ThenTheResultAwayTeamShouldBe(string awayTeam)
        {
            Result!.AwayTeam.Should().Be(awayTeam);
        }

        [Given(@"wait for (.*) second")]
        public void GivenWaitForSecond(int second) {
            Thread.Sleep(second * 1000);
        }

        [When(@"get match summery")]
        public async void WhenGetMatchSummery() {
            using var scope = _serviceProvider.CreateScope();
            var matchService = scope.ServiceProvider.GetService<IMatchService>();
            MatchSummery = await matchService!.GetSummeryAsync();
        }

        [Then(@"the summery should have match with home team ""([^""]*)"" and away team ""([^""]*)"" and consider result")]
        public void ThenTheSummeryShouldHaveMatchWithHomeTeamAndAwayTeamAndConsiderResult(string homeTeam, string awayTeam) {
            MatchSummery.Should().Contain(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam);
            Result = MatchSummery!.First(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam);
        }

        [Then(@"start date of result should be older than (.*) second ago")]
        public void ThenStartDateOfResultShouldBeOlderThanSecondAgo(int second) {
            Result!.StartTime.Should().BeBefore(DateTime.Now.AddSeconds(-1 * second));
        }

        [When(@"update home team ""([^""]*)"" score to (.*) and away team ""([^""]*)"" to (.*)")]
        public async Task WhenUpdateHomeTeamScoreToAndAwayTeamTo(string homeTeam, int homeTeamScore, string awayTeam, int awayTeamScore) {
            using var scope = _serviceProvider.CreateScope();
            var matchService = scope.ServiceProvider.GetService<IMatchService>();
            await matchService!.UpdateAsync(new Match {
                HomeTeam = homeTeam,
                HomeTeamScore = homeTeamScore,
                AwayTeam = awayTeam,
                AwayTeamScore = awayTeamScore
            });
        }

        [Then(@"the result should have home team score (.*)")]
        public void ThenTheResultShouldHaveHomeTeamScore(int homeTeamScore) {
            Result!.HomeTeamScore.Should().Be(homeTeamScore);
        }

        [Then(@"the result should have away team score (.*)")]
        public void ThenTheResultShouldHaveAwayTeamScore(int awayTeamScore) {
            Result!.AwayTeamScore.Should().Be(awayTeamScore);
        }

    }
}
