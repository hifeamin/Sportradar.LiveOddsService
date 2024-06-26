﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Data {
    public interface IMatchRepository {
        Task AddAsync(Match match);
        Task<Match?> GetAsync(string homeTeam, string awayTeam);
        Task RemoveAsync(Match match);
        Task<IEnumerable<Match>> GetAllAsync();
        Task UpdateAsync(Match match);
    }
}
