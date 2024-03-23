using System;
using System.Threading.Tasks;
using Sportradar.LiveOddsService.Data;
using Sportradar.LiveOddsService.Domain.Models;
using Sportradar.LiveOddsService.Domain.Services;

namespace Sportradar.LiveOddsService.Business {
    public class MatchService: IMatchService {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository) => this._matchRepository = matchRepository;

        public async Task<Match> StartAsync(string homeTeam, string awayTeam) {
            var match = new Match() {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            await _matchRepository.AddAsync(match);
            return match;
        }

        public async Task UpdateAsync(Match match) {
            var savedMatch = await _matchRepository.GetAsync(match.HomeTeam, match.AwayTeam);

            savedMatch.HomeTeamScore = match.HomeTeamScore;
            savedMatch.AwayTeamScore = match.AwayTeamScore;
            await _matchRepository.UpdateAsync(savedMatch);
        }
    }
}
