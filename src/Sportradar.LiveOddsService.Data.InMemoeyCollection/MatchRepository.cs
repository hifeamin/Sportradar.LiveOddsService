using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Data.InMemoeyCollection {
    public class MatchRepository: IMatchRepository {
        private readonly DbContext _dbContext;

        public MatchRepository(DbContext dbContext) => _dbContext = dbContext;

        public Task AddAsync(Match match) => throw new NotImplementedException();
        public Task<IEnumerable<Match>> GetAllAsync() => throw new NotImplementedException();
        public Task<Match?> GetAsync(string homeTeam, string awayTeam) => throw new NotImplementedException();
        public Task RemoveAsync(Match match) => throw new NotImplementedException();
        public Task UpdateAsync(Match match) => throw new NotImplementedException();
    }
}
