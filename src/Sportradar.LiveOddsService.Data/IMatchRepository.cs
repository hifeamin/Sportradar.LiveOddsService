﻿using System.Threading.Tasks;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Data {
    public interface IMatchRepository {
        Task AddAsync(Match match);
    }
}
