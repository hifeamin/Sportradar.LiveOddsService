using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Data.InMemoeyCollection {
    public class MatchRepository: IMatchRepository {
        private readonly DbContext _dbContext;

        public MatchRepository(DbContext dbContext) => _dbContext = dbContext;

        public Task AddAsync(Match match) {
            _dbContext.Matches.Add($"{match.HomeTeam}-{match.AwayTeam}", match);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Match>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Match>>(_dbContext.Matches.Values);

        public Task<Match?> GetAsync(string homeTeam, string awayTeam) {
            if(_dbContext.Matches.TryGetValue($"{homeTeam}-{awayTeam}", out Match data))
                return Task.FromResult<Match?>(data);
            return Task.FromResult<Match?>(null);
        }

        public Task RemoveAsync(Match match) => throw new NotImplementedException();
        public Task UpdateAsync(Match match) {
            _dbContext.Matches[$"{match.HomeTeam}-{match.AwayTeam}"] = match;
            return Task.CompletedTask;
        }
    }
}
