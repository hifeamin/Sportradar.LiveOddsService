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

        [When(@"Start new match for home team ""([^""]*)"" and away team ""([^""]*)""")]
        public async Task StartNewMatchForHomeTeamAndAwayTeam(string homeTeam, string awayTeam)
        {
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
    }
}
